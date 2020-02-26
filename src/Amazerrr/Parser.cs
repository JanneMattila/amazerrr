using System;
using System.Collections.Generic;

namespace Amazerrr
{
    public class Parser
    {
        private Board _board = new Board();
        private HashSet<int> _positions = new HashSet<int>();
        private HashSet<string> _scannedPaths = new HashSet<string>();
        private char[,] _map;
        private int _height;
        private int _width;

        public Board Parse(string input)
        {
            var lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            (int x, int y) = FindPlayer(lines);
            _board.StartPosition = new Position(x, y);

            FindMoves(new Position(x, y), -1, 0, Swipe.Left);
            FindMoves(new Position(x, y), 1, 0, Swipe.Right);
            FindMoves(new Position(x, y), 0, -1, Swipe.Up);
            FindMoves(new Position(x, y), 0, 1, Swipe.Down);

            _board.TotalCount = _positions.Count;
            return _board;
        }

        private void FindMoves(Position position, int xdelta, int ydelta, Swipe swipe)
        {
            var startPosition = position.Clone();
            var startkey = position.ToKey();
            var scankey = $"{startkey},{xdelta},{ydelta}";
            if (_scannedPaths.Contains(scankey))
            {
                // We have previously already scanned to this direction.
                return;
            }

            _scannedPaths.Add(scankey);
            _positions.Add(startkey);
            var locations = new HashSet<int>
            {
                startkey
            };

            while (true)
            {
                var newx = position.X + xdelta;
                var newy = position.Y + ydelta;

                var c = _map[newx, newy];
                if (c == Constants.Wall)
                {
                    if (startPosition == position)
                    {
                        // No move available in this direction.
                        return;
                    }

                    var move = new Move()
                    {
                        To = position,
                        Swipe = swipe,
                        Locations = locations
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

                    FindMoves(position.Clone(), -1, 0, Swipe.Left);
                    FindMoves(position.Clone(), 1, 0, Swipe.Right);
                    FindMoves(position.Clone(), 0, -1, Swipe.Up);
                    FindMoves(position.Clone(), 0, 1, Swipe.Down);
                    return;
                }

                position.X = newx;
                position.Y = newy;

                var key = position.ToKey();
                _positions.Add(key);
                locations.Add(key);
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
