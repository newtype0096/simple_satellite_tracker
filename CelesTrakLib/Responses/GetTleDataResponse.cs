using CelesTrakLib.Datas;

namespace CelesTrakLib.Responses
{
    public class GetTleDataResponse : CelesTrackResponse
    {
        public TleData Data { get; set; }
    }
}