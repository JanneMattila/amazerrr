using System;
using System.Collections.Generic;
using System.Linq;

namespace Amazerrr
{
    public class Solver
    {
        private int _minimumMoveCount = int.MaxValue;
        private int _totalCount;

        public List<Swipe> Solve(string input)
        {
            return ScanSolution();
        }

        private List<Swipe> ScanSolution()
        {
            var recursionLevel = 1;
            var checkpoints = new List<SolveSnapshot>()
            {
                new SolveSnapshot() { Level = recursionLevel }
            };

            var moves = new List<Swipe>()
            {
                Swipe.Up, Swipe.Down, Swipe.Left, Swipe.Right
            };

            List<Swipe> solution = null;
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
                if (solve.Swipes.Count > _minimumMoveCount)
                {
                    continue;
                }

                if (solve.Count == _totalCount)
                {
                    if (solve.Swipes.Count < _minimumMoveCount)
                    {
                        _minimumMoveCount = solve.Swipes.Count;
                        solution = solve.Swipes.ToList();
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
