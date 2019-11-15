using System.Collections.Generic;

namespace ARngryBirdsBackend.Models
{
    public class State
    {
        private const int TotalHealth = 1000;

        public int PlanetRotation { get; set; }
        public List<Zone> Zones { get; } = new List<Zone>();
        public List<Bird> Birds { get; } = new List<Bird>();
        public List<Hint> Hints { get; } = new List<Hint>();
        public int Score { get; set; }
        public int Health { get; set; } = TotalHealth;
        public int MaxHealth { get; } = TotalHealth;

        public void Reset()
        {
            Zones.Clear();
            Birds.Clear();
            Hints.Clear();
            Score = 0;
            Health = TotalHealth;
        }
    }
}