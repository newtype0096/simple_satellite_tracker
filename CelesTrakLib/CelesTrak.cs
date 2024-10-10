using CelesTrakLib.Data;
using CelesTrakLib.Response;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;

namespace CelesTrakLib
{
    public static class CelesTrak
    {
        public static bool get_stations(out GetStationsResponse response)
        {
            response = new GetStationsResponse();

            using (HttpClient client = new HttpClient())
            {
                string url = $"{_baseUrl}?GROUP=stations&FORMAT=json";

                try
                {
                    var http_response = client.GetAsync(url).Result;
                    response.error_code = (int)http_response.StatusCode;

                    if (http_response.IsSuccessStatusCode)
                    {
                        string jsonString = http_response.Content.ReadAsStringAsync().Result;
                        response.Stations = JsonConvert.DeserializeObject<List<Satellite>>(jsonString);

                        return true;
                    }
                    else
                    {
                        response.error_message = http_response.Content.ReadAsStringAsync().Result;
                    }
                }
                catch (HttpRequestException ex)
                {
                    response.error_code = null;
                    response.error_message = $"Request error: {ex.Message}";
                }

                return false;
            }
        }

        private static readonly string _baseUrl = "https://celestrak.org/NORAD/elements/gp.php";
    }
}