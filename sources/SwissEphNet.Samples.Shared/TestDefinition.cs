using System;
using System.Collections.Generic;
using System.Text;

namespace SwissEphNet.Samples
{
    /// <summary>
    /// Definition of the test
    /// </summary>
    public class TestDefinition
    {
        /// <summary>
        /// Date of the test
        /// </summary>
        public DateTime Date { get; set; } = DateTime.Now;

        /// <summary>
        /// Ephemeris time or Universal Time of the date
        /// </summary>
        public TimeType EtUt { get; set; } = TimeType.UniversalTime;

        /// <summary>
        /// Geo position
        /// </summary>
        public uint LongitudeDeg { get; set; } = 8;
        public uint LongitudeMin { get; set; } = 33;
        public uint LongitudeSec { get; set; } = 0;
        public LongitudeType LongitudeType { get; set; } = LongitudeType.East;
        public uint LatitudeDeg { get; set; } = 47;
        public uint LatitudeMin { get; set; } = 23;
        public uint LatitudeSec { get; set; } = 0;
        public LatitudeType LatitudeType { get; set; } = LatitudeType.North;
        public int Altitude { get; set; } = 0;

        /// <summary>
        /// Ephemeris
        /// </summary>
        public EphemerisType Ephemeris { get; set; } = EphemerisType.SwissEphemeris;

        /// <summary>
        /// Planets displayed
        /// </summary>
        public PlanetsType Planets { get; set; } = PlanetsType.MainPlanets;

        /// <summary>
        /// Centric
        /// </summary>
        public CentricType Centric { get; set; } = CentricType.Geocentric;

        /// <summary>
        /// House system
        /// </summary>
        public HouseSystem HouseSystem { get; set; } = HouseSystem.Placidus;

        /// <summary>
        /// Extra asteroids
        /// </summary>
        public string Asteroids { get; set; } = "433, 3045, 7066";
    }

    /// <summary>
    /// Type of time
    /// </summary>
    public enum TimeType
    {
        /// <summary>
        /// UT
        /// </summary>
        UniversalTime,
        /// <summary>
        /// ET
        /// </summary>
        EphemerisTime
    }

    /// <summary>
    /// Longitude type
    /// </summary>
    public enum LongitudeType
    {
        East,
        West
    }

    /// <summary>
    /// Latitude type
    /// </summary>
    public enum LatitudeType
    {
        North,
        South
    }

    /// <summary>
    /// Type of ephemeris
    /// </summary>
    public enum EphemerisType
    {
        SwissEphemeris,
        JplEphemeris,
        MoshierEphemeris
    }

    /// <summary>
    /// Type of planets displayed
    /// </summary>
    public enum PlanetsType
    {
        MainPlanets,
        WithAsteroids,
        WithHypBodies
    }

    /// <summary>
    /// Type of centric
    /// </summary>
    public enum CentricType
    {
        Geocentric,
        Topocentric,
        Heliocentric,
        Barycentric,
        SiderealFagan,
        SiderealLahiri
    }

    /// <summary>
    /// House system
    /// </summary>
    public enum HouseSystem
    {
        Placidus,
        Campanus,
        Regionmontanus,
        Koch,
        Equal,
        VehlowEqual,
        Horizon,
        BAlcabitus
    }

}
