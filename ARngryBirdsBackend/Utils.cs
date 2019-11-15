using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ARngryBirdsBackend
{
    public static class Utils
    {
        private static Random _random = new Random();
        
        public static readonly JsonSerializerSettings ConverterSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            }
        };

        public static int ToAngle(this int a)
        {
            while (a > 360)
            {
                a -= 360;
            }

            while (a < 0)
            {
                a += 360;
            }

            return a;
        }

        public static bool AreWithinAngles(this int angle, int lower, int higher)
        {
            if (lower < higher)
            {
                // normal area
                return angle >= lower && angle <= higher;
            }
            
            // date line area
            return angle >= lower || angle <= higher;
        }

        public static int GenerateAngleWithin(int lower, int higher)
        {
            if (lower < higher)
            {
                // normal area
                return _random.Next(lower, higher + 1);
            }
            
            // date line area
            var upHigher = higher + 360;
            return _random.Next(lower, upHigher + 1).ToAngle();
        }

        public static double DistanceBetween2d(int lat1, int lng1, int lat2, int lng2)
        {
            var lngDiff = Math.Abs(lng1 - lng2);
            var latDiff = Math.Abs(lat1 - lat2);
            if (latDiff > 180)
            {
                latDiff = 360 - latDiff;
            }

            return Math.Sqrt(lngDiff * lngDiff + latDiff * latDiff);
        }
    }
}