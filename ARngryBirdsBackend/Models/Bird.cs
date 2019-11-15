using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ARngryBirdsBackend.Models
{
    public class Bird
    {
        public int Id { get; set; }
        
        [JsonConverter(typeof(StringEnumConverter))]
        public BirdType Type { get; set; }
        
        public int X { get; set; }
        public int Y { get; set; }
        public int Latitude { get; set; }
        public int Longitude { get; set; }
        
        public double Strength { get; set; }
    }

    public enum BirdType
    {
        Fireball,
        Waterball,
        Stone
    }
}