using System.Collections.Generic;
using System.Drawing;

namespace Amazerrr
{
    public class Board
    {
        public int TotalCount { get; set; }
        public Point StartPosition { get; set; }
        public Dictionary<string, List<Move>> Moves { get; set; }

        public Board()
        {
            Moves = new Dictionary<string, List<Move>>();
        }
    }
}
