using System.Collections.Generic;

namespace ARngryBirdsBackend.Models
{
    public class State
    {
        public int PlanetRotation { get; set; }
        public List<Zone> Zones { get; set; } = new List<Zone>();
        public List<Bird> Birds { get; set; } = new List<Bird>();
        public List<Hint> Hints { get; set; } = new List<Hint>();
        public int Score { get; set; }
    }
}