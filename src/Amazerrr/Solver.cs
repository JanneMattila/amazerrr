using System;
using System.Collections.Generic;
using System.Linq;

namespace Amazerrr
{
    public class Solver
    {
        class SolveSnapshot
        {
            public int Level { get; set; }
            public int Count { get; set; }
            public List<Move> Moves { get; set; }

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
                Moves = new List<Move>();
            }
        }

        private int _minimumMoveCount = int.MaxValue;
        private int _totalCount;

        public List<Move> Solve(string input)
        {
            return ScanSolution();
        }

        private List<Move> ScanSolution()
        {
            var recursionLevel = 1;
            var checkpoints = new List<SolveSnapshot>()
            {
                new SolveSnapshot() { Level = recursionLevel }
            };

            var moves = new List<Move>()
            {
                Move.Up, Move.Down, Move.Left, Move.Right
            };

            List<Move> solution = null;
            while (true)
            {
                var solve = checkpoints.FirstOrDefault(c => c.Level == recursionLevel);
                if (solve == null)
                {
                    var begin = checkpoints.Count;
                    var groups = checkpoints
                      .GroupBy(c => c.UniqueString)
                      .ToList();
                    checkpoints.Clear();

                    var end = checkpoints.Count;
                    if (end == 0)
                    {
                        break;
                    }

                    if (begin > end)
                    {
                        Console.WriteLine($"Reduced calculation from {begin} to {end}.");
                    }

                    recursionLevel++;
                    continue;
                }

                checkpoints.Remove(solve);
                if (solve.Moves.Count > _minimumMoveCount)
                {
                    continue;
                }

                if (solve.Count == _totalCount)
                {
                    if (solve.Moves.Count < _minimumMoveCount)
                    {
                        _minimumMoveCount = solve.Moves.Count;
                        solution = solve.Moves.ToList();
                    }
                    continue;
                }

                foreach (var move in moves)
                {
                    // TODO: Make move
                }
            }

            return solution;
        }
    }
}
