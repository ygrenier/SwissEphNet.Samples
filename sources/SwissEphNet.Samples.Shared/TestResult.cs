using System;
using System.Collections.Generic;
using System.Text;

namespace SwissEphNet.Samples
{
    public class TestResult
    {
        /// <summary>
        /// Longitude
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// Latitude
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Julian date in Universal Time
        /// </summary>
        public double JulianDateUT { get; set; }

        /// <summary>
        /// Julian year
        /// </summary>
        public int JulianYear { get; set; }

        /// <summary>
        /// Julian month
        /// </summary>
        public int JulianMonth { get; set; }

        /// <summary>
        /// Julian day
        /// </summary>
        public int JulianDay { get; set; }

        /// <summary>
        /// Julian hour
        /// </summary>
        public int JulianHour { get; set; }

        /// <summary>
        /// Julian minute
        /// </summary>
        public int JulianMinute { get; set; }

        /// <summary>
        /// Julian second
        /// </summary>
        public int JulianSecond { get; set; }

        /// <summary>
        /// Delta time
        /// </summary>
        public double DeltaTime { get; set; }

        /// <summary>
        /// Julian date in Ephemeris Time
        /// </summary>
        public double JulianDateET { get; set; }

        /// <summary>
        /// Use gregorian date
        /// </summary>
        public bool UseGregorianDate { get; set; }

        /// <summary>
        /// Sideral time
        /// </summary>
        public double SideralTime { get; set; }

        /// <summary>
        /// ARMC
        /// </summary>
        public double ARMC { get; set; }

        /// <summary>
        /// Delay of the test
        /// </summary>
        public TimeSpan TestDelay { get; set; }
    }
}
