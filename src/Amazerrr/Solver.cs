using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Amazerrr
{
    public class Solver
    {
        private int _minimumMoveCount = int.MaxValue;

        public List<Swipe> Solve(Board board)
        {
            var recursionLevel = 1;
            var checkpoints = new List<SolveSnapshot>()
            {
                new SolveSnapshot() 
                { 
                    Level = recursionLevel, 
                    Position = board.StartPosition
                }
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
                    checkpoints = checkpoints
                      .GroupBy(c => c.UniqueString)
                      .Select(c => c.First())
                      .ToList();
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

                if (solve.VisitedLocations.Count == board.TotalCount)
                {
                    if (solve.Swipes.Count < _minimumMoveCount)
                    {
                        _minimumMoveCount = solve.Swipes.Count;
                        solution = solve.Swipes.ToList();
                    }
                    continue;
                }

                var positionKey = solve.Position.ToKey();
                foreach (var move in board.Moves[positionKey])
                {
                    var swipes = solve.Swipes.ToList();
                    swipes.Add(move.Swipe);

                    var visitedLocations = solve.VisitedLocations.ToHashSet();
                    foreach (var location in move.Locations)
                    {
                        visitedLocations.Add(location);
                    }

                    var newsolve = new SolveSnapshot()
                    {
                        Level = solve.Level + 1,
                        Position = move.To,
                        Swipes = swipes,
                        VisitedLocations = visitedLocations
                    };

                    checkpoints.Add(newsolve);
                }
            }

            return solution;
        }
    }
}
