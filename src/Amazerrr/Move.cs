using System.Collections.Generic;

namespace Amazerrr
{
    public class Move
    {
        public Position To { get; set; }
        public Swipe Swipe { get; set; }
        public HashSet<int> Locations { get; set; }

        public Move()
        {
            Locations = new HashSet<int>();
        }
    }
}
