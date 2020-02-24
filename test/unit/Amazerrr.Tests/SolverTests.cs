using System.Collections.Generic;
using Xunit;

namespace Amazerrr.Tests
{
    public class SolverTests
    {
        private readonly Solver _solver;

        public SolverTests()
        {
            _solver = new Solver();
        }

        [Fact]
        public void Analyze_Test1_File()
        {
            // Arrange
            var board = new Board()
            {
                TotalCount = 9,
                Moves = new Dictionary<string, List<Move>>()
            };
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
