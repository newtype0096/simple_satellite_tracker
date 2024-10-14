using CelesTrakLib.Datas;

namespace CelesTrakLib.Responses
{
    public class GetOrbitalDataResponse : CelesTrackResponse
    {
        public OrbitalData Data { get; set; }
    }
}