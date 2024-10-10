using System.ComponentModel;

namespace CelesTrakLib.Query
{
    public enum Group
    {
        [Description("Last 30 Days' Launches")]
        last_30_days,

        [Description("Space Stations")]
        stations,

        [Description("100 (or so) Brightest")]
        visual,

        [Description("Active Satellites")]
        active,

        [Description("Analyst Satellites")]
        analyst,

        [Description("Russian ASAT Test Debris (COSMOS 1408)")]
        cosmos_1408_debris,

        [Description("Chinese ASAT Test Debris (FENGYUN 1C)")]
        fengyun_1c_debris,

        [Description("IRIDIUM 33 Debris")]
        iridium_33_debris,

        [Description("COSMOS 2251 Debris")]
        cosmos_2251_debris,

        [Description("Weather")]
        weather,

        [Description("NOAA")]
        noaa,

        [Description("GOES")]
        goes,

        [Description("Earth Resources")]
        resource,

        [Description("Search & Rescue (SARSAT)")]
        sarsat,

        [Description("Disaster Monitoring")]
        dmc,

        [Description("Tracking and Data Relay Satellite System (TDRSS)")]
        tdrss,

        [Description("ARGOS Data Collection System")]
        argos,

        [Description("Planet")]
        planet,

        [Description("Spire")]
        spire,

        // TODO Communications Satellites, Navigation Satellites, Scientific Satellites, Miscellaneous Satellites
    }
}