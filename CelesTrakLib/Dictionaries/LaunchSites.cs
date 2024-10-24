﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelesTrakLib.Dictionaries
{
    public static class LaunchSites
    {
        private static Dictionary<string, string> _launchSites;

        static LaunchSites()
        {
            _launchSites = new Dictionary<string, string>
            {
                {"AFETR", "Air Force Eastern Test Range, Florida, USA"},
                {"AFWTR", "Air Force Western Test Range, California, USA"},
                {"CAS", "Canaries Airspace"},
                {"DLS", "Dombarovskiy Launch Site, Russia"},
                {"ERAS", "Eastern Range Airspace"},
                {"FRGUI", "Europe's Spaceport, Kourou, French Guiana"},
                {"HGSTR", "Hammaguira Space Track Range, Algeria"},
                {"JJSLA", "Jeju Island Sea Launch Area, Republic of Korea"},
                {"JSC", "Jiuquan Space Center, PRC"},
                {"KODAK", "Kodiak Launch Complex, Alaska, USA"},
                {"KSCUT", "Uchinoura Space Center (Formerly Kagoshima Space Center—University of Tokyo, Japan)"},
                {"KWAJ", "US Army Kwajalein Atoll (USAKA)"},
                {"KYMSC", "Kapustin Yar Missile and Space Complex, Russia"},
                {"NSC", "Naro Space Complex, Republic of Korea"},
                {"PLMSC", "Plesetsk Missile and Space Complex, Russia"},
                {"RLLB", "Rocket Lab Launch Base, Mahia Peninsula, New Zealand"},
                {"SCSLA", "South China Sea Launch Area, PRC"},
                {"SEAL", "Sea Launch Platform (mobile)"},
                {"SEMLS", "Semnan Satellite Launch Site, Iran"},
                {"SMTS", "Shahrud Missile Test Site, Iran"},
                {"SNMLP", "San Marco Launch Platform, Indian Ocean (Kenya)"},
                {"SPKII", "Space Port Kii, Japan"},
                {"SRILR", "Satish Dhawan Space Centre, India (Formerly Sriharikota Launching Range)"},
                {"SUBL", "Submarine Launch Platform (mobile)"},
                {"SVOBO", "Svobodnyy Launch Complex, Russia"},
                {"TAISC", "Taiyuan Space Center, PRC"},
                {"TANSC", "Tanegashima Space Center, Japan"},
                {"TYMSC", "Tyuratam Missile and Space Center, Kazakhstan (Also known as Baikonur Cosmodrome)"},
                {"UNK", "Unknown"},
                {"VOSTO", "Vostochny Cosmodrome, Russia"},
                {"WLPIS", "Wallops Island, Virginia, USA"},
                {"WOMRA", "Woomera, Australia"},
                {"WRAS", "Western Range Airspace"},
                {"WSC", "Wenchang Satellite Launch Site, PRC"},
                {"XICLF", "Xichang Launch Facility, PRC"},
                {"YAVNE", "Yavne Launch Facility, Israel"},
                {"YSLA", "Yellow Sea Launch Area, PRC"},
                {"YUN", "Yunsong Launch Site (Sohae Satellite Launching Station), Democratic People's Republic of Korea (North Korea)"}
            };
        }

        public static string GetLaunchSiteFull(string launchSiteCode)
        {
            if (!_launchSites.ContainsKey(launchSiteCode))
            {
                return string.Empty;
            }

            return $"{_launchSites[launchSiteCode]} ({launchSiteCode})";
        }
    }
}
