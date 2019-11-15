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
    }
}