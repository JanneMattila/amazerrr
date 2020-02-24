using System.Collections.Generic;

namespace Amazerrr
{
    public class Board
    {
        public int TotalCount { get; set; }
        public Dictionary<string, List<Move>> Moves { get; set; }

        public Board()
        {
            Moves = new Dictionary<string, List<Move>>();
        }
    }
}
