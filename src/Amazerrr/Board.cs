using System.Collections.Generic;

namespace Amazerrr
{
    public class Board
    {
        public int TotalCount { get; set; }
        public Position StartPosition { get; set; }
        public Dictionary<int, List<Move>> Moves { get; set; }

        public Board()
        {
            Moves = new Dictionary<int, List<Move>>();
        }
    }
}
