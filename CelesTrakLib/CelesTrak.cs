using CelesTrakLib.Datas;
using CelesTrakLib.Responses;
using CsvHelper;
using System;
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
                            response.SatelliteCatalogs = csv.GetRecords<SatelliteCatalog>().ToList();
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

        //private static readonly string _baseUrl = "https://celestrak.org/NORAD/elements/gp.php";
        private static readonly string _satcatFileName = "_satcat.csv";
    }
}