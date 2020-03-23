using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Amazerrr
{
    public class Solver
    {
        private int _minimumMoveCount = int.MaxValue;

        private readonly ILogger _log;

        public Solver(ILogger log)
        {
            _log = log;
        }

        public List<Swipe> Solve(Board board)
        {
            _log.LogInformation("Solving started");

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
                        _log.LogInformation("{RecursionLevel}: Optimized calculation from {Begin} to {End}.", recursionLevel, begin, end);
                    }
                    else
                    {
                        _log.LogInformation("{RecursionLevel}: Calculating {Begin}.", recursionLevel, begin);
                    }
                    recursionLevel++;
                    continue;
                }

                checkpoints.Remove(solve);
                if (solve.Swipes.Count >= _minimumMoveCount)
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
                var availableMovesCount = board.Moves[positionKey].Count;
                var blockSwipe = Swipe.None;

                if (solve.PreviousMoveBlocked)
                {
                    blockSwipe = solve.Swipes.Last() switch
                    {
                        Swipe.Up => Swipe.Down,
                        Swipe.Down => Swipe.Up,
                        Swipe.Left => Swipe.Right,
                        Swipe.Right => Swipe.Left,
                        _ => Swipe.None
                    };
                }

                foreach (var move in board.Moves[positionKey])
                {
                    if (blockSwipe == move.Swipe)
                    {
                        continue;
                    }

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
                        VisitedLocations = visitedLocations,
                        PreviousMoveBlocked = availableMovesCount <= 1
                    };

                    checkpoints.Add(newsolve);
                }
            }

            _log.LogInformation("Solving finished with {Moves} moves", solution.Count);

            return solution;
        }
    }
}
