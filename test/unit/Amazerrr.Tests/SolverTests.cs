using System.Collections.Generic;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Amazerrr.Tests
{
    public class SolverTests
    {
        private readonly Solver _solver;

        public SolverTests()
        {
            _solver = new Solver(NullLogger.Instance);
        }

        [Fact]
        public void Solve_Test1_File()
        {
            // Arrange
            var board = new Board()
            {
                TotalCount = 9,
                StartPosition = new Position(1, 3),
                Moves = new Dictionary<int, List<Move>>()
            };

            var position1 = new Position(1, 3);
            var position2 = new Position(4, 3);
            var position3 = new Position(4, 1);
            var position4 = new Position(1, 1);
            board.Moves.Add(position1.ToKey(), new List<Move>()
            {
                new Move()
                {
                    To = position2,
                    Swipe = Swipe.Right,
                    Locations = new HashSet<int>()
                    {
                        1, 2, 3, 4
                    }
                }
            });

            board.Moves.Add(position2.ToKey(), new List<Move>()
            {
                new Move()
                {
                    To = position1,
                    Swipe = Swipe.Left,
                    Locations = new HashSet<int>()
                    {
                        1, 2, 3, 4
                    }
                },
                new Move()
                {
                    To = position3,
                    Swipe = Swipe.Up,
                    Locations = new HashSet<int>()
                    {
                        4, 5, 6
                    }
                }
            });

            board.Moves.Add(position3.ToKey(), new List<Move>()
            {
                new Move()
                {
                    To = position4,
                    Swipe = Swipe.Left,
                    Locations = new HashSet<int>()
                    {
                        6, 7, 8, 9
                    }
                },
                new Move()
                {
                    To = position2,
                    Swipe = Swipe.Down,
                    Locations = new HashSet<int>()
                    {
                        4, 5, 6
                    }
                }
            });

            var expected = new List<Swipe>()
            {
                Swipe.Right, Swipe.Up, Swipe.Left
            };

            // Act
            var actual = _solver.Solve(board);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
