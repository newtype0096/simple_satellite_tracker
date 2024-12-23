﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelesTrakLib.Dictionaries
{
    public static class Sources
    {
        private static Dictionary<string, string> _sources;

        static Sources()
        {
            _sources = new Dictionary<string, string>()
            {
                {"AB", "Arab Satellite Communications Organization"},
                {"ABS", "Asia Broadcast Satellite"},
                {"AC", "Asia Satellite Telecommunications Company (ASIASAT)"},
                {"ALG", "Algeria"},
                {"ANG", "Angola"},
                {"ARGN", "Argentina"},
                {"ARM", "Republic of Armenia"},
                {"ASRA", "Austria"},
                {"AUS", "Australia"},
                {"AZER", "Azerbaijan"},
                {"BEL", "Belgium"},
                {"BELA", "Belarus"},
                {"BERM", "Bermuda"},
                {"BGD", "Peoples Republic of Bangladesh"},
                {"BHUT", "Kingdom of Bhutan"},
                {"BOL", "Bolivia"},
                {"BRAZ", "Brazil"},
                {"BUL", "Bulgaria"},
                {"CA", "Canada"},
                {"CHBZ", "China/Brazil"},
                {"CHTU", "China/Turkey"},
                {"CHLE", "Chile"},
                {"CIS", "Commonwealth of Independent States (former USSR)"},
                {"COL", "Colombia"},
                {"CRI", "Republic of Costa Rica"},
                {"CZCH", "Czech Republic (former Czechoslovakia)"},
                {"DEN", "Denmark"},
                {"DJI", "Republic of Djibouti"},
                {"ECU", "Ecuador"},
                {"EGYP", "Egypt"},
                {"ESA", "European Space Agency"},
                {"ESRO", "European Space Research Organization"},
                {"EST", "Estonia"},
                {"ETH", "Ethiopia"},
                {"EUME", "European Organization for the Exploitation of Meteorological Satellites (EUMETSAT)"},
                {"EUTE", "European Telecommunications Satellite Organization (EUTELSAT)"},
                {"FGER", "France/Germany"},
                {"FIN", "Finland"},
                {"FR", "France"},
                {"FRIT", "France/Italy"},
                {"GER", "Germany"},
                {"GHA", "Republic of Ghana"},
                {"GLOB", "Globalstar"},
                {"GREC", "Greece"},
                {"GRSA", "Greece/Saudi Arabia"},
                {"GUAT", "Guatemala"},
                {"HUN", "Hungary"},
                {"IM", "International Mobile Satellite Organization (INMARSAT)"},
                {"IND", "India"},
                {"INDO", "Indonesia"},
                {"IRAN", "Iran"},
                {"IRAQ", "Iraq"},
                {"IRID", "Iridium"},
                {"IRL", "Ireland"},
                {"ISRA", "Israel"},
                {"ISRO", "Indian Space Research Organisation"},
                {"ISS", "International Space Station"},
                {"IT", "Italy"},
                {"ITSO", "International Telecommunications Satellite Organization (INTELSAT)"},
                {"JPN", "Japan"},
                {"KAZ", "Kazakhstan"},
                {"KEN", "Republic of Kenya"},
                {"LAOS", "Laos"},
                {"LKA", "Democratic Socialist Republic of Sri Lanka"},
                {"LTU", "Lithuania"},
                {"LUXE", "Luxembourg"},
                {"MA", "Morocco"},
                {"MALA", "Malaysia"},
                {"MCO", "Principality of Monaco"},
                {"MDA", "Republic of Moldova"},
                {"MEX", "Mexico"},
                {"MMR", "Republic of the Union of Myanmar"},
                {"MNG", "Mongolia"},
                {"MUS", "Mauritius"},
                {"NATO", "North Atlantic Treaty Organization"},
                {"NETH", "Netherlands"},
                {"NICO", "New ICO"},
                {"NIG", "Nigeria"},
                {"NKOR", "Democratic People's Republic of Korea"},
                {"NOR", "Norway"},
                {"NPL", "Federal Democratic Republic of Nepal"},
                {"NZ", "New Zealand"},
                {"O3B", "O3b Networks"},
                {"ORB", "ORBCOMM"},
                {"PAKI", "Pakistan"},
                {"PERU", "Peru"},
                {"POL", "Poland"},
                {"POR", "Portugal"},
                {"PRC", "People's Republic of China"},
                {"PRY", "Republic of Paraguay"},
                {"PRES", "People's Republic of China/European Space Agency"},
                {"QAT", "State of Qatar"},
                {"RASC", "RascomStar-QAF"},
                {"ROC", "Taiwan (Republic of China)"},
                {"ROM", "Romania"},
                {"RP", "Philippines (Republic of the Philippines)"},
                {"RWA", "Republic of Rwanda"},
                {"SAFR", "South Africa"},
                {"SAUD", "Saudi Arabia"},
                {"SDN", "Republic of Sudan"},
                {"SEAL", "Sea Launch"},
                {"SEN", "Republic of Senegal"},
                {"SES", "SES"},
                {"SGJP", "Singapore/Japan"},
                {"SING", "Singapore"},
                {"SKOR", "Republic of Korea"},
                {"SPN", "Spain"},
                {"STCT", "Singapore/Taiwan"},
                {"SVN", "Slovenia"},
                {"SWED", "Sweden"},
                {"SWTZ", "Switzerland"},
                {"TBD", "To Be Determined"},
                {"THAI", "Thailand"},
                {"TMMC", "Turkmenistan/Monaco"},
                {"TUN", "Republic of Tunisia"},
                {"TURK", "Turkey"},
                {"UAE", "United Arab Emirates"},
                {"UK", "United Kingdom"},
                {"UKR", "Ukraine"},
                {"UNK", "Unknown"},
                {"URY", "Uruguay"},
                {"US", "United States"},
                {"USBZ", "United States/Brazil"},
                {"VAT", "Vatican City State"},
                {"VENZ", "Venezuela"},
                {"VTNM", "Vietnam"},
                {"ZWE", "Republic of Zimbabwe"}
            };
        }

        public static string GetSourceFull(string source)
        {
            if (!_sources.ContainsKey(source))
            {
                return string.Empty;
            }

            return $"{_sources[source]} ({source})";
        }
    }
}
