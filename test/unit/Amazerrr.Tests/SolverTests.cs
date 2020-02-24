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
            var input =
                "######\r\n" +
                "#....#\r\n" +
                "####.#\r\n" +
                "#o...#\r\n" +
                "######";
            var expected = new List<Move>()
            {
                Move.Right, Move.Up, Move.Left
            };

            // Act
            var actual = _solver.Solve(input);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
