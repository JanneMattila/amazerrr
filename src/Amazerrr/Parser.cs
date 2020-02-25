using System;

namespace Amazerrr
{
    public class Parser
    {
        private Board _board = new Board();
        private char[,] _map;
        private int _height;
        private int _width;

        public Board Parse(string input)
        {
            var lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            (int x, int y) = FindPlayer(lines);

            FindMoves(x, y);
            return _board;
        }

        private void FindMoves(int x, int y)
        {
            var c = _map[x, y];

            // TODO: Add available moves to board.
        }

        private (int, int) FindPlayer(string[] lines)
        {
            _height = lines.Length;
            _width = lines[0].Length;
            var playerx = -1;
            var playery = -1;

            _map = new char[_width, _height];
            for (int y = 0; y < _height; y++)
            {
                var line = lines[y];
                for (int x = 0; x < _width; x++)
                {
                    _map[x, y] = line[x];
                    if (line[x] == Constants.Player)
                    {
                        playerx = x;
                        playery = y;
                    }
                }
            }

            if (playerx == -1 || playery == -1)
            {
                throw new ParserException("No player found.");
            }

            return (playerx, playery);
        }
    }
}
