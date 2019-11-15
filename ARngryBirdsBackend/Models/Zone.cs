using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ARngryBirdsBackend.Models
{
    public class Zone
    {
        public int Latitude { get; set; }
        public int Longitude { get; set; }
        
        [JsonConverter(typeof(StringEnumConverter))]
        public ZoneType Type { get; set; }
        
        public double Strength { get; set; }
    }

    public enum ZoneType
    {
        Fire,
        Army,
        Flood
    }
}