using System;
using System.Collections.Generic;
using System.Drawing;

namespace Amazerrr
{
    public class Parser
    {
        private Board _board = new Board();
        private HashSet<string> _positions = new HashSet<string>();
        private HashSet<string> _scannedPaths = new HashSet<string>();
        private char[,] _map;
        private int _height;
        private int _width;

        public Board Parse(string input)
        {
            var lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            (int x, int y) = FindPlayer(lines);
            _board.StartPosition = new Point(x, y);

            FindMoves(x, y, -1, 0);
            FindMoves(x, y, 1, 0);
            FindMoves(x, y, 0, -1);
            FindMoves(x, y, 0, 1);

            _board.TotalCount = _positions.Count;
            return _board;
        }

        private void FindMoves(int x, int y, int xdelta, int ydelta)
        {
            var startx = x;
            var starty = y;
            var startkey = $"{x},{y}";
            var scankey = $"{x},{y},{xdelta},{ydelta}";
            if (_scannedPaths.Contains(scankey))
            {
                // We have previously already scanned to this direction.
                return;
            }

            _scannedPaths.Add(scankey);
            while (true)
            {
                var newx = x + xdelta;
                var newy = y + ydelta;

                var c = _map[newx, newy];
                if (c == Constants.Wall)
                {
                    if (startx == x && starty == y)
                    {
                        // No move available in this direction.
                        return;
                    }

                    var move = new Move()
                    {
                        From = new Point(startx, starty),
                        To = new Point(x, y)
                    };

                    if (_board.Moves.ContainsKey(startkey))
                    {
                        _board.Moves[startkey].Add(move);
                    }
                    else
                    {
                        _board.Moves.Add(startkey, new List<Move>()
                        {
                            move
                        });
                    }

                    FindMoves(x, y, -1, 0);
                    FindMoves(x, y, 1, 0);
                    FindMoves(x, y, 0, -1);
                    FindMoves(x, y, 0, 1);
                    return;
                }

                var key = $"{x},{y}";
                _positions.Add(key);

                x = newx;
                y = newy;
            }
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
