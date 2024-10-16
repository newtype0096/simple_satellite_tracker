using CelesTrakLib.Datas;
using CelesTrakLib.Responses;
using CsvHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;

namespace CelesTrakLib
{
    public sealed class CelesTrak
    {
        public static CelesTrak Default { get; } = new CelesTrak();

        public bool GetSatelliteCatalogs(out GetSatelliteCatalogsResponse response)
        {
            response = new GetSatelliteCatalogsResponse();

            string targetSatCatFileName = string.Empty;

            var satcatFiles = Directory.GetFiles(WorkingDirectory, $"*{_satcatFileName}");
            var mostRecentSatcatFile = satcatFiles.OrderByDescending(x => Path.GetFileName(x).Replace(_satcatFileName, string.Empty)).FirstOrDefault();

            bool needDownload = true;

            if (!string.IsNullOrEmpty(mostRecentSatcatFile))
            {
                string date = Path.GetFileName(mostRecentSatcatFile).Replace(_satcatFileName, string.Empty);
                string today = $"{DateTime.Now:yyyyMMdd}";

                if (string.Compare(date, today, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    targetSatCatFileName = mostRecentSatcatFile;
                    needDownload = false;
                }
            }

            if (needDownload)
            {
                using (HttpClient client = new HttpClient())
                {
                    string url = $"{_mainUrl}/pub/satcat.csv";

                    try
                    {
                        var http_response = client.GetAsync(url).Result;
                        response.ErrorCode = (int)http_response.StatusCode;

                        if (http_response.IsSuccessStatusCode)
                        {
                            string fileName = Path.Combine(WorkingDirectory, $"{DateTime.Now:yyyyMMdd}{_satcatFileName}");

                            using (var writer = new StreamWriter(fileName))
                            {
                                writer.Write(http_response.Content.ReadAsStringAsync().Result);
                            }

                            targetSatCatFileName = fileName;

                            if (!string.IsNullOrEmpty(mostRecentSatcatFile))
                            {
                                File.Delete(mostRecentSatcatFile);
                            }
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        response.ErrorCode = null;
                        response.ErrorMessage = ex.Message;
                    }
                }
            }

            if (!string.IsNullOrEmpty(targetSatCatFileName))
            {
                try
                {
                    using (var reader = new StreamReader(targetSatCatFileName))
                    {
                        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                        {
                            response.SatelliteCatalogs = csv.GetRecords<SatelliteCatalogData>().ToList();
                        }
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    response.ErrorCode = null;
                    response.ErrorMessage = ex.Message;
                }
            }

            return false;
        }

        public bool GetOribitalData(string norad_cat_id, out GetOrbitalDataResponse response)
        {
            response = new GetOrbitalDataResponse();

            using (HttpClient client = new HttpClient())
            {
                string url = $"{_gpUrl}?CATNR={norad_cat_id}&FORMAT=json";

                try
                {
                    var http_response = client.GetAsync(url).Result;
                    response.ErrorCode = (int)http_response.StatusCode;

                    if (http_response.IsSuccessStatusCode)
                    {
                        string jsonString = http_response.Content.ReadAsStringAsync().Result;
                        response.Data = JsonConvert.DeserializeObject<List<OrbitalData>>(jsonString).FirstOrDefault();

                        return true;
                    }
                    else
                    {
                        response.ErrorMessage = http_response.Content.ReadAsStringAsync().Result;
                    }
                }
                catch (Exception ex)
                {
                    response.ErrorCode = null;
                    response.ErrorMessage = ex.Message;
                }
            }

            return false;
        }

        public bool GetTleData(string norad_cat_id, out GetTleDataResponse response)
        {
            response = new GetTleDataResponse();

            using (HttpClient client = new HttpClient())
            {
                string url = $"{_gpUrl}?CATNR={norad_cat_id}&FORMAT=tle";

                try
                {
                    var http_response = client.GetAsync(url).Result;
                    response.ErrorCode = (int)http_response.StatusCode;

                    if (http_response.IsSuccessStatusCode)
                    {
                        string tleString = http_response.Content.ReadAsStringAsync().Result;
                        var tleLines = tleString.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

                        if (tleLines.Length <= 2)
                        {
                            return false;
                        }

                        response.Data = new TleData()
                        {
                            Line1 = tleLines[1],
                            Line2 = tleLines[2]
                        };

                        return true;
                    }
                    else
                    {
                        response.ErrorMessage = http_response.Content.ReadAsStringAsync().Result;
                    }
                }
                catch (Exception ex)
                {
                    response.ErrorCode = null;
                    response.ErrorMessage = ex.Message;
                }
            }

            return false;
        }

        public string WorkingDirectory
        {
            get => _workingDirectory;
            set
            {
                _workingDirectory = value;
                Directory.CreateDirectory(_workingDirectory);
            }
        }

        private string _workingDirectory = "celestrak";

        private static readonly string _mainUrl = "https://celestrak.org";
        private static readonly string _gpUrl = "https://celestrak.org/NORAD/elements/gp.php";
        private static readonly string _satcatFileName = "_satcat.csv";
    }
}