using System;
using System.Collections.Generic;

namespace Amazerrr
{
    class SolveSnapshot
    {
        public int Level { get; set; }
        public int Count { get; set; }
        public List<Swipe> Swipes { get; set; }

        public string UniqueString
        {
            get
            {
                // TODO: Add consolidation identifier.
                return Guid.NewGuid().ToString();
            }
        }

        public SolveSnapshot()
        {
            Swipes = new List<Swipe>();
        }
    }
}
