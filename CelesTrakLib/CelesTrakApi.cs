using CelesTrakLib.Datas;
using CelesTrakLib.Responses;
using CsvHelper;
using One_Sgp4;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CelesTrakLib
{
    public class CelesTrakApi
    {
        private static readonly string _mainUrl = "https://celestrak.org";
        private static readonly string _gpUrl = $"{_mainUrl}/NORAD/elements/gp.php";

        public bool DownloadSatCats(out DownloadSatCatsResponse response)
        {
            response = new DownloadSatCatsResponse();

            using (HttpClient client = new HttpClient())
            {
                string url = $"{_mainUrl}/pub/satcat.csv";

                try
                {
                    var http_response = client.GetAsync(url).Result;
                    response.ErrorCode = (int)http_response.StatusCode;

                    if (http_response.IsSuccessStatusCode)
                    {
                        string csvString = http_response.Content.ReadAsStringAsync().Result;
                        using (var csv = new CsvReader(new StringReader(csvString), CultureInfo.InvariantCulture))
                        {
                            response.SatCats = csv.GetRecords<SatCat>().ToList();
                        }

                        return true;
                    }
                }
                catch (HttpRequestException ex)
                {
                    response.ErrorCode = null;
                    response.ErrorMessage = ex.Message;
                }
            }

            return false;
        }

        public bool DownloadGpDatas(out DownloadGpDatasResponse response)
        {
            response = new DownloadGpDatasResponse();

            using (HttpClient client = new HttpClient())
            {
                string url = $"{_gpUrl}?GROUP=active&FORMAT=tle";

                try
                {
                    var http_response = client.GetAsync(url).Result;
                    response.ErrorCode = (int)http_response.StatusCode;

                    if (http_response.IsSuccessStatusCode)
                    {
                        string tleString = http_response.Content.ReadAsStringAsync().Result;

                        var lines = tleString.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                        if (lines.Length % 3 != 0)
                        {
                            response.ErrorMessage = "Invalid Datas";
                            return false;
                        }

                        response.GpDatas = new List<GpData>();
                        for (int i = 0; i < lines.Length; i += 3)
                        {
                            string name = lines[i].Trim();
                            string line1 = lines[i + 1];
                            string line2 = lines[i + 2];

                            var tleItme = ParserTLE.parseTle(line1, line2, name);

                            var gpData = new GpData()
                            {
                                OBJECT_NAME = tleItme.getName(),
                                NORAD_CAT_ID = int.Parse(tleItme.getNoradID()).ToString(),
                                LINE1 = line1,
                                LINE2 = line2
                            };

                            response.GpDatas.Add(gpData);
                        }
                        return true;
                    }
                }
                catch (HttpRequestException ex)
                {
                    response.ErrorCode = null;
                    response.ErrorMessage = ex.Message;
                }
            }

            return false;
        }
    }
}
