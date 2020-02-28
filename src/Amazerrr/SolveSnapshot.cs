using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Amazerrr
{
    class SolveSnapshot
    {
        public int Level { get; set; }
        public Position Position { get; set; }
        public HashSet<int> VisitedLocations { get; set; }
        public List<Swipe> Swipes { get; set; }
        public bool PreviousMoveBlocked { get; set; }

        public string UniqueString
        {
            get
            {
                return $"{Position.ToKey()},{string.Join(",", VisitedLocations.Select(s => s).OrderBy(s => s))}";
            }
        }

        public SolveSnapshot()
        {
            Swipes = new List<Swipe>();
            VisitedLocations = new HashSet<int>();
        }
    }
}
