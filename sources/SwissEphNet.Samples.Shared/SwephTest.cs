using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace SwissEphNet.Samples
{
    public class SwephTest : IDisposable
    {
        public SwephTest(ITestProvider provider)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            Provider = provider;
            SwissEph = new SwissEph();
            SwissEph.OnLoadFile += SwissEph_OnLoadFile;
        }

        public void Dispose()
        {
            SwissEph.Dispose();
        }

        private void SwissEph_OnLoadFile(object sender, LoadFileEventArgs e)
        {
            Encoding enc = e.Encoding;
            e.File = Provider.LoadFile(e.FileName, out enc);
            e.Encoding = enc;
            Provider.Debug.WriteLine($"Required file:{e.FileName} => {(e.File != null ? "OK" : "Not found")}");
        }

        const string MY_ODEGREE_STRING = "°";
        const int BIT_ROUND_SEC = 1;
        const int BIT_ROUND_MIN = 2;
        const int BIT_ZODIAC = 4;
        const string PLSEL_D = "0123456789mtABC";
        const string PLSEL_P = "0123456789mtABCDEFGHI";
        const string PLSEL_H = "JKLMNOPQRSTUVWX";
        const string PLSEL_A = "0123456789mtABCDEFGHIJKLMNOPQRSTUVWX";
        static string[] zod_nam = new string[] { "ar", "ta", "ge", "cn", "le", "vi", "li", "sc", "sa", "cp", "aq", "pi" };
        static double square_sum(double[] x) { return (x[0] * x[0] + x[1] * x[1] + x[2] * x[2]); }

        void RunTest(TestDefinition test, TestResult result)
        {
            string serr = String.Empty, serr_save = String.Empty, serr_warn = String.Empty;
            //string s, s1, s2;
            string star = String.Empty;
            string se_pname;
            string spnam, spnam2 = "";
            string fmt = "PZBRS";
            string plsel = String.Empty; //char psp;
            string gap = " ";
            double y_frac;
            double hpos = 0;
            int ipl, ipldiff = SwissEph.SE_SUN;
            double[] x = new double[6], xequ = new double[6], xcart = new double[6], xcartq = new double[6];
            double[] cusp = new double[12 + 1];    /* cusp[0] + 12 houses */
            double[] ascmc = new double[10];		/* asc, mc, vertex ...*/
            //double ar, sinp;
            //double a, sidt, armc, lon, lat;
            //double eps_true, eps_mean, nutl, nuto;
            string fname = String.Empty;
            //int nast, iast;
            int[] astno = new int[100];
            int iflag = 0, iflag2;              /* external flag: helio, geo... */
            //int iflgret;
            var whicheph = SwissEph.SEFLG_SWIEPH;
            bool universal_time = false;
            bool calc_house_pos = false;
            int gregflag;
            bool diff_mode = false;
            int round_flag = 0;
            string jul;
            char hsys = test.HouseSystem.ToString()[0];
            var buf = Provider.Output;

            switch (test.Ephemeris)
            {
                case EphemerisType.JplEphemeris:
                    whicheph = SwissEph.SEFLG_JPLEPH;
                    fname = SwissEph.SE_FNAME_DE406;
                    break;
                case EphemerisType.MoshierEphemeris:
                    whicheph = SwissEph.SEFLG_MOSEPH;
                    break;
                case EphemerisType.SwissEphemeris:
                default:
                    whicheph = SwissEph.SEFLG_SWIEPH;
                    break;
            }

            universal_time = test.EtUt == TimeType.UniversalTime;

            switch (test.Planets)
            {
                case PlanetsType.WithAsteroids:
                    plsel = PLSEL_P;
                    break;
                case PlanetsType.WithHypBodies:
                    plsel = PLSEL_A;
                    break;
                case PlanetsType.MainPlanets:
                default:
                    plsel = PLSEL_D;
                    break;
            }

            switch (test.Centric)
            {
                case CentricType.Topocentric:
                    iflag |= SwissEph.SEFLG_TOPOCTR;
                    calc_house_pos = true;
                    break;
                case CentricType.Heliocentric:
                    iflag |= SwissEph.SEFLG_HELCTR;
                    break;
                case CentricType.Barycentric:
                    iflag |= SwissEph.SEFLG_BARYCTR;
                    break;
                case CentricType.SiderealFagan:
                    iflag |= SwissEph.SEFLG_SIDEREAL;
                    SwissEph.swe_set_sid_mode(SwissEph.SE_SIDM_FAGAN_BRADLEY, 0, 0);
                    break;
                case CentricType.SiderealLahiri:
                    iflag |= SwissEph.SEFLG_SIDEREAL;
                    SwissEph.swe_set_sid_mode(SwissEph.SE_SIDM_LAHIRI, 0, 0);
                    break;
                case CentricType.Geocentric:
                default:
                    calc_house_pos = true;
                    break;
            }

            result.Longitude = test.LongitudeDeg + test.LongitudeMin / 60.0 + test.LongitudeSec / 3600.0;
            if (test.LongitudeType == LongitudeType.West)
                result.Longitude = -result.Longitude;
            result.Latitude = test.LatitudeDeg + test.LatitudeMin / 60.0 + test.LatitudeSec / 3600.0;
            if (test.LatitudeType == LatitudeType.South)
                result.Latitude = -result.Latitude;
            do_print(buf, C.sprintf("Planet Positions from %s \n\n", test.Ephemeris));
            if ((whicheph & SwissEph.SEFLG_JPLEPH) != 0)
                SwissEph.swe_set_jpl_file(fname);
            iflag = (iflag & ~SwissEph.SEFLG_EPHMASK) | whicheph;
            iflag |= SwissEph.SEFLG_SPEED;

            if (test.Date.Year * 10000 + test.Date.Month * 100 + test.Date.Day < 15821015)
                gregflag = SwissEph.SE_JUL_CAL;
            else
                gregflag = SwissEph.SE_GREG_CAL;
            result.UseGregorianDate = gregflag == SwissEph.SE_GREG_CAL;

            var jday = test.Date.Day;
            var jmon = test.Date.Month;
            var jyear = test.Date.Year;
            var jhour = test.Date.Hour;
            var jmin = test.Date.Minute;
            var jsec = test.Date.Second;
            var jut = jhour + (jmin / 60.0) + (jsec / 3600.0);
            result.JulianDateUT = SwissEph.swe_julday(jyear, jmon, jday, jut, gregflag);
            SwissEph.swe_revjul(result.JulianDateUT, gregflag, ref jyear, ref jmon, ref jday, ref jut);
            jut += 0.5 / 3600;
            jhour = (int)jut;
            jmin = (int)((jut * 60.0) % 60.0);
            jsec = (int)((jut * 3600.0) % 60.0);
            result.JulianDay = jday;
            result.JulianMonth = jmon;
            result.JulianYear = jyear;
            result.JulianHour = jhour;
            result.JulianMinute = jmin;
            result.JulianSecond = jsec;
            var bc = string.Empty;
            do_print(buf, $"Date: {test.Date}\n");
            if (test.Date.Year <= 0)
                bc = C.sprintf("(%d B.C.)", 1 - jyear);
            if (jyear * 10000L + jmon * 100L + jday <= 15821004)
                jul = "jul.";
            else
                jul = "";
            do_print(buf, C.sprintf("%d.%d.%d %s %s    %#02d:%#02d:%#02d %s\n",
                jday, jmon, jyear, bc, jul,
                jhour, jmin, jsec, test.EtUt == TimeType.UniversalTime ? "UT" : "ET"));
            jut = jhour + jmin / 60.0 + jsec / 3600.0;
            if (universal_time)
            {
                result.DeltaTime = SwissEph.swe_deltat(result.JulianDateUT);
                do_print(buf, C.sprintf(" delta t: %f sec", result.DeltaTime * 86400.0));
                result.JulianDateET = result.JulianDateUT + result.DeltaTime;
            }
            else
                result.JulianDateET = result.JulianDateUT;
            do_print(buf, C.sprintf(" jd (ET) = %f\n", result.JulianDateET));
            var iflgret = SwissEph.swe_calc(result.JulianDateET, SwissEph.SE_ECL_NUT, iflag, x, ref serr);
            var eps_true = x[0];
            var eps_mean = x[1];
            var s1 = dms(eps_true, round_flag);
            var s2 = dms(eps_mean, round_flag);
            do_print(buf, C.sprintf("\n%-15s %s%s%s    (true, mean)", "Ecl. obl.", s1, gap, s2));
            var nutl = x[2];
            var nuto = x[3];
            s1 = dms(nutl, round_flag);
            s2 = dms(nuto, round_flag);
            do_print(buf, C.sprintf("\n%-15s %s%s%s    (dpsi, deps)", "Nutation", s1, gap, s2));
            do_print(buf, "\n\n");
            do_print(buf, "               ecl. long.       ecl. lat.   ");
            do_print(buf, "    dist.          speed");
            if (calc_house_pos)
                do_print(buf, "          house");
            do_print(buf, "\n");
            if ((iflag & SwissEph.SEFLG_TOPOCTR) != 0)
                SwissEph.swe_set_topo(result.Longitude, result.Latitude, test.Altitude);
            result.SideralTime = SwissEph.swe_sidtime(result.JulianDateUT) + result.Longitude / 15;
            if (result.SideralTime >= 24)
                result.SideralTime -= 24;
            if (result.SideralTime < 0)
                result.SideralTime += 24;
            result.ARMC = result.SideralTime * 15;
            /* additional asteroids */
            //splan = plsel;
            if (String.Compare(plsel, PLSEL_P) == 0)
            {
                var cpos = test.Asteroids.Split(",;. \t".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                var j = cpos.Length;
                for (int i = 0, nast = 0; i < j; i++)
                {
                    if ((astno[nast] = int.Parse(cpos[i])) > 0)
                    {
                        nast++;
                        plsel += "+";
                    }
                }
            }
            for (int pspi = 0, iast = 0; pspi < plsel.Length; pspi++)
            {
                var psp = plsel[pspi];
                if (psp == '+')
                {
                    ipl = SwissEph.SE_AST_OFFSET + (int)astno[iast];
                    iast++;
                }
                else
                    ipl = letter_to_ipl(psp);
                if ((iflag & SwissEph.SEFLG_HELCTR) != 0)
                {
                    if (ipl == SwissEph.SE_SUN
                      || ipl == SwissEph.SE_MEAN_NODE || ipl == SwissEph.SE_TRUE_NODE
                      || ipl == SwissEph.SE_MEAN_APOG || ipl == SwissEph.SE_OSCU_APOG)
                        continue;
                }
                else if ((iflag & SwissEph.SEFLG_BARYCTR) != 0)
                {
                    if (ipl == SwissEph.SE_MEAN_NODE || ipl == SwissEph.SE_TRUE_NODE
                      || ipl == SwissEph.SE_MEAN_APOG || ipl == SwissEph.SE_OSCU_APOG)
                        continue;
                }
                else          /* geocentric */
                  if (ipl == SwissEph.SE_EARTH)
                    continue;
                /* ecliptic position */
                if (ipl == SwissEph.SE_FIXSTAR)
                {
                    iflgret = SwissEph.swe_fixstar(star, result.JulianDateET, iflag, x, ref serr);
                    se_pname = star;
                }
                else
                {
                    iflgret = SwissEph.swe_calc(result.JulianDateET, ipl, iflag, x, ref serr);
                    se_pname = SwissEph.swe_get_planet_name(ipl);
                    if (ipl > SwissEph.SE_AST_OFFSET)
                    {
                        var s = C.sprintf("#%d", (int)astno[iast - 1]);
                        se_pname += new String(' ', 11 - s.Length) + s;
                    }
                }
                if (iflgret >= 0)
                {
                    if (calc_house_pos)
                    {
                        hpos = SwissEph.swe_house_pos(result.ARMC, result.Latitude, eps_true, hsys, x, ref serr);
                        if (hpos == 0)
                            iflgret = SwissEph.ERR;
                    }
                }
                if (iflgret < 0)
                {
                    if (!String.IsNullOrEmpty(serr) && String.Compare(serr, serr_save) != 0)
                    {
                        serr_save = serr;
                        do_print(buf, "error: ");
                        do_print(buf, serr);
                        do_print(buf, "\n");
                    }
                }
                else if (!String.IsNullOrEmpty(serr) && String.IsNullOrEmpty(serr_warn))
                    serr_warn = serr;
                /* equator position */
                if (fmt.IndexOfAny("aADdQ".ToCharArray()) >= 0)
                {
                    iflag2 = iflag | SwissEph.SEFLG_EQUATORIAL;
                    if (ipl == SwissEph.SE_FIXSTAR)
                        iflgret = SwissEph.swe_fixstar(star, result.JulianDateET, iflag2, xequ, ref serr);
                    else
                        iflgret = SwissEph.swe_calc(result.JulianDateET, ipl, iflag2, xequ, ref serr);
                }
                /* ecliptic cartesian position */
                if (fmt.IndexOfAny("XU".ToCharArray()) >= 0)
                {
                    iflag2 = iflag | SwissEph.SEFLG_XYZ;
                    if (ipl == SwissEph.SE_FIXSTAR)
                        iflgret = SwissEph.swe_fixstar(star, result.JulianDateET, iflag2, xcart, ref serr);
                    else
                        iflgret = SwissEph.swe_calc(result.JulianDateET, ipl, iflag2, xcart, ref serr);
                }
                /* equator cartesian position */
                if (fmt.IndexOfAny("xu".ToCharArray()) >= 0)
                {
                    iflag2 = iflag | SwissEph.SEFLG_XYZ | SwissEph.SEFLG_EQUATORIAL;
                    if (ipl == SwissEph.SE_FIXSTAR)
                        iflgret = SwissEph.swe_fixstar(star, result.JulianDateET, iflag2, xcartq, ref serr);
                    else
                        iflgret = SwissEph.swe_calc(result.JulianDateET, ipl, iflag2, xcartq, ref serr);
                }
                double t2, ar, sinp;
                spnam = se_pname;
                /*
                 * The string fmt contains a sequence of format specifiers;
                 * each character in fmt creates a column, the columns are
                 * sparated by the gap string.
                 */
                int spi = 0;
                for (spi = 0; spi < fmt.Length; spi++)
                {
                    char sp = fmt[spi];
                    if (spi > 0)
                        do_print(buf, gap);
                    switch (sp)
                    {
                        case 'y':
                            do_print(buf, "%d", jyear);
                            break;
                        case 'Y':
                            jut = 0;
                            t2 = SwissEph.swe_julday(jyear, 1, 1, jut, gregflag);
                            y_frac = (result.JulianDateUT - t2) / 365.0;
                            do_print(buf, "%.2lf", jyear + y_frac);
                            break;
                        case 'p':
                            if (diff_mode)
                                do_print(buf, "%d-%d", ipl, ipldiff);
                            else
                                do_print(buf, "%d", ipl);
                            break;
                        case 'P':
                            if (diff_mode)
                                do_print(buf, "%.3s-%.3s", spnam, spnam2);
                            else
                                do_print(buf, "%-11s", spnam);
                            break;
                        case 'J':
                        case 'j':
                            do_print(buf, "%.2f", result.JulianDateUT);
                            break;
                        case 'T':
                            do_print(buf, "%02d.%02d.%d", jday, jmon, jyear);
                            break;
                        case 't':
                            do_print(buf, "%02d%02d%02d", jyear % 100, jmon, jday);
                            break;
                        case 'L':
                            do_print(buf, dms(x[0], round_flag));
                            break;
                        case 'l':
                            do_print(buf, "%# 11.7f", x[0]);
                            break;
                        case 'Z':
                            do_print(buf, dms(x[0], round_flag | BIT_ZODIAC));
                            break;
                        case 'S':
                        case 's':
                            var sp2i = spi + 1;
                            char sp2 = fmt.Length <= sp2i ? '\0' : fmt[sp2i];
                            if (sp2 == 'S' || sp2 == 's' || fmt.IndexOfAny("XUxu".ToCharArray()) >= 0)
                            {
                                for (sp2i = 0; sp2i < fmt.Length; sp2i++)
                                {
                                    sp2 = fmt[sp2i];
                                    if (sp2i > 0)
                                        do_print(buf, gap);
                                    switch (sp2)
                                    {
                                        case 'L':       /* speed! */
                                        case 'Z':       /* speed! */
                                            do_print(buf, dms(x[3], round_flag));
                                            break;
                                        case 'l':       /* speed! */
                                            do_print(buf, "%11.7f", x[3]);
                                            break;
                                        case 'B':       /* speed! */
                                            do_print(buf, dms(x[4], round_flag));
                                            break;
                                        case 'b':       /* speed! */
                                            do_print(buf, "%11.7f", x[4]);
                                            break;
                                        case 'A':       /* speed! */
                                            do_print(buf, dms(xequ[3] / 15, round_flag | SwissEph.SEFLG_EQUATORIAL));
                                            break;
                                        case 'a':       /* speed! */
                                            do_print(buf, "%11.7f", xequ[3]);
                                            break;
                                        case 'D':       /* speed! */
                                            do_print(buf, dms(xequ[4], round_flag));
                                            break;
                                        case 'd':       /* speed! */
                                            do_print(buf, "%11.7f", xequ[4]);
                                            break;
                                        case 'R':       /* speed! */
                                        case 'r':       /* speed! */
                                            do_print(buf, "%# 14.9f", x[5]);
                                            break;
                                        case 'U':       /* speed! */
                                        case 'X':       /* speed! */
                                            if (sp == 'U')
                                                ar = Math.Sqrt(square_sum(xcart));
                                            else
                                                ar = 1;
                                            do_print(buf, "%# 14.9f%s", xcart[3] / ar, gap);
                                            do_print(buf, "%# 14.9f%s", xcart[4] / ar, gap);
                                            do_print(buf, "%# 14.9f", xcart[5] / ar);
                                            break;
                                        case 'u':       /* speed! */
                                        case 'x':       /* speed! */
                                            if (sp == 'u')
                                                ar = Math.Sqrt(square_sum(xcartq));
                                            else
                                                ar = 1;
                                            do_print(buf, "%# 14.9f%s", xcartq[3] / ar, gap);
                                            do_print(buf, "%# 14.9f%s", xcartq[4] / ar, gap);
                                            do_print(buf, "%# 14.9f", xcartq[5] / ar);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                if (fmt.Length <= spi + 1 && (fmt[spi + 1] == 'S' || fmt[sp + 1] == 's'))
                                {
                                    spi++;
                                    sp = fmt[spi];
                                }
                            }
                            else
                            {
                                do_print(buf, dms(x[3], round_flag));
                            }
                            break;
                        case 'B':
                            do_print(buf, dms(x[1], round_flag));
                            break;
                        case 'b':
                            do_print(buf, "%# 11.7f", x[1]);
                            break;
                        case 'A': /* rectascensio */
                            do_print(buf, dms(xequ[0] / 15, round_flag | SwissEph.SEFLG_EQUATORIAL));
                            break;
                        case 'a': /* rectascensio */
                            do_print(buf, "%# 11.7f", xequ[0]);
                            break;
                        case 'D': /* declination */
                            do_print(buf, dms(xequ[1], round_flag));
                            break;
                        case 'd': /* declination */
                            do_print(buf, "%# 11.7f", xequ[1]);
                            break;
                        case 'R':
                            do_print(buf, "%# 14.9f", x[2]);
                            break;
                        case 'r':
                            if (ipl == SwissEph.SE_MOON)
                            { /* for moon print parallax */
                                sinp = 8.794 / x[2];        /* in seconds of arc */
                                ar = sinp * (1 + sinp * sinp * 3.917402e-12);
                                /* the factor is 1 / (3600^2 * (180/pi)^2 * 6) */
                                do_print(buf, "%# 13.5f\"", ar);
                            }
                            else
                            {
                                do_print(buf, "%# 14.9f", x[2]);
                            }
                            break;
                        case 'U':
                        case 'X':
                            if (sp == 'U')
                                ar = Math.Sqrt(square_sum(xcart));
                            else
                                ar = 1;
                            do_print(buf, "%# 14.9f%s", xcart[0] / ar, gap);
                            do_print(buf, "%# 14.9f%s", xcart[1] / ar, gap);
                            do_print(buf, "%# 14.9f", xcart[2] / ar);
                            break;
                        case 'u':
                        case 'x':
                            if (sp == 'u')
                                ar = Math.Sqrt(square_sum(xcartq));
                            else
                                ar = 1;
                            do_print(buf, "%# 14.9f%s", xcartq[0] / ar, gap);
                            do_print(buf, "%# 14.9f%s", xcartq[1] / ar, gap);
                            do_print(buf, "%# 14.9f", xcartq[2] / ar);
                            break;
                        case 'Q':
                            do_print(buf, "%-15s", spnam);
                            do_print(buf, dms(x[0], round_flag));
                            do_print(buf, dms(x[1], round_flag));
                            do_print(buf, "  %# 14.9f", x[2]);
                            do_print(buf, dms(x[3], round_flag));
                            do_print(buf, dms(x[4], round_flag));
                            do_print(buf, "  %# 14.9f\n", x[5]);
                            do_print(buf, "               %s", dms(xequ[0], round_flag));
                            do_print(buf, dms(xequ[1], round_flag));
                            do_print(buf, "                %s", dms(xequ[3], round_flag));
                            do_print(buf, dms(xequ[4], round_flag));
                            break;
                    } /* switch */
                }   /* for sp */
                if (calc_house_pos)
                {
                    //sprintf(s, "  %# 6.4f", hpos);
                    do_print(buf, "%# 9.4f", hpos);
                }
                do_print(buf, "\n");
            }     /* for psp */
            if (!String.IsNullOrEmpty(serr_warn))
            {
                do_print(buf, "\nwarning: ");
                do_print(buf, serr_warn);
                do_print(buf, "\n");
            }
            /* houses */
            do_print(buf, C.sprintf("\nHouse Cusps (%s)\n\n", test.HouseSystem.ToString()));
            var a = result.SideralTime + 0.5 / 3600;
            do_print(buf, C.sprintf("sid. time : %4d:%#02d:%#02d  ", (int)a,
                (int)((a * 60.0) % 60.0),
                (int)((a * 3600.0) % 60.0))
                );
            a = result.ARMC + 0.5 / 3600;
            do_print(buf, "armc      : %4d%s%#02d'%#02d\"\n",
                  (int)result.ARMC, MY_ODEGREE_STRING,
                  (int)((result.ARMC * 60.0) % 60.0),
                  (int)((a * 3600.0) % 60.0));
            do_print(buf, "geo. lat. : %4d%c%#02d'%#02d\" ",
                  test.LatitudeDeg, test.LatitudeType.ToString()[0], test.LatitudeMin, test.LatitudeSec);
            do_print(buf, "geo. long.: %4d%c%#02d'%#02d\"\n\n",
                  test.LongitudeDeg, test.LongitudeType.ToString()[0], test.LongitudeMin, test.LongitudeSec);
            SwissEph.swe_houses_ex(result.JulianDateUT, iflag, result.Latitude, result.Longitude, hsys, cusp, ascmc);
            round_flag |= BIT_ROUND_SEC;
            do_print(buf, C.sprintf("AC        : %s\n", dms(ascmc[0], round_flag | BIT_ZODIAC)));
            do_print(buf, C.sprintf("MC        : %s\n", dms(ascmc[1], round_flag | BIT_ZODIAC)));
            for (int i = 1; i <= 12; i++)
            {
                do_print(buf, C.sprintf("house   %2d: %s\n", i, dms(cusp[i], round_flag | BIT_ZODIAC)));
            }
            do_print(buf, C.sprintf("Vertex    : %s\n", dms(ascmc[3], round_flag | BIT_ZODIAC)));
        }

        static string dms(double x, long iflag)
        {
            int izod;
            long k, kdeg, kmin, ksec;
            string c = MY_ODEGREE_STRING, s1;
            //char *sp, s1[50];
            //static char s[50];
            int sgn;
            string s = String.Empty;
            if ((iflag & SwissEph.SEFLG_EQUATORIAL) != 0)
                c = "h";
            if (x < 0)
            {
                x = -x;
                sgn = -1;
            }
            else
                sgn = 1;
            if ((iflag & BIT_ROUND_MIN) != 0)
                x += 0.5 / 60;
            if ((iflag & BIT_ROUND_SEC) != 0)
                x += 0.5 / 3600;
            if ((iflag & BIT_ZODIAC) != 0)
            {
                izod = (int)(x / 30);
                x = (x % 30.0);
                kdeg = (long)x;
                s = C.sprintf(" %2ld %s ", kdeg, zod_nam[izod]);
            }
            else
            {
                kdeg = (long)x;
                s = C.sprintf("%3ld%s", kdeg, c);
            }
            x -= kdeg;
            x *= 60;
            kmin = (long)x;
            if ((iflag & BIT_ZODIAC) != 0 && (iflag & BIT_ROUND_MIN) != 0)
                s1 = C.sprintf("%2ld", kmin);
            else
                s1 = C.sprintf("%2ld'", kmin);
            s += s1;
            if ((iflag & BIT_ROUND_MIN) != 0)
                goto return_dms;
            x -= kmin;
            x *= 60;
            ksec = (long)x;
            if ((iflag & BIT_ROUND_SEC) != 0)
                s1 = C.sprintf("%2ld\"", ksec);
            else
                s1 = C.sprintf("%2ld", ksec);
            s += s1;
            if ((iflag & BIT_ROUND_SEC) != 0)
                goto return_dms;
            x -= ksec;
            k = (long)(x * 10000);
            s1 = C.sprintf(".%04ld", k);
            s += s1;
            return_dms:;
            if (sgn < 0)
            {
                int spi = s.IndexOfAny("0123456789".ToCharArray());
                s = string.Concat(s.Substring(0, spi - 1), "-", s.Substring(spi));
            }
            return (s);
        }

        void do_print(ref string target, string info)
        {
            if (string.IsNullOrWhiteSpace(target))
                target = " ";
            target += info.Replace("\n", "\r\n");
        }

        void do_print(TextWriter target, string info, params object[] args)
        {
            if (args != null)
                info = C.sprintf(info, args);
            target.Write(info.Replace("\n", "\r\n"));
        }

        int letter_to_ipl(char letter)
        {
            if (letter >= '0' && letter <= '9')
                return letter - '0' + SwissEph.SE_SUN;
            if (letter >= 'A' && letter <= 'I')
                return letter - 'A' + SwissEph.SE_MEAN_APOG;
            if (letter >= 'J' && letter <= 'X')
                return letter - 'J' + SwissEph.SE_CUPIDO;
            switch (letter)
            {
                case 'm': return SwissEph.SE_MEAN_NODE;
                case 'n':
                case 'o': return SwissEph.SE_ECL_NUT;
                case 't': return SwissEph.SE_TRUE_NODE;
                case 'f': return SwissEph.SE_FIXSTAR;
            }
            return -1;
        }

        public TestResult RunTest(TestDefinition test = null)
        {
            if (test == null) test = new TestDefinition();
            var result = new TestResult();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            RunTest(test, result);
            sw.Stop();
            result.TestDelay = sw.Elapsed;
            do_print(Provider.Output, $"\n\nCalculation time: {result.TestDelay}\n");
            Provider.Output.Flush();
            Provider.Debug.Flush();
            return result;
        }

        public SwissEph SwissEph { get; private set; }

        public ITestProvider Provider { get; set; }

    }
}
