using System.Collections.Generic;

namespace ARngryBirdsBackend.Models
{
    public class Hint
    {
        public BirdType BirdType { get; set; }
        public List<int> Poses { get; set; }
    }
}