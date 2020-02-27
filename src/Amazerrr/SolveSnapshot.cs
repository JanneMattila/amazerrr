using System.Linq;
using System.Collections.Generic;

namespace Amazerrr
{
    class SolveSnapshot
    {
        public int Level { get; set; }
        public Position Position { get; set; }
        public HashSet<int> VisitedLocations { get; set; }
        public List<Swipe> Swipes { get; set; }

        public string UniqueString
        {
            get
            {
                var s = Position.ToKey() + "," + string.Join(",", VisitedLocations.Select(s => s).OrderBy(s => s));
                return s;
            }
        }

        public SolveSnapshot()
        {
            Swipes = new List<Swipe>();
            VisitedLocations = new HashSet<int>();
        }
    }
}
