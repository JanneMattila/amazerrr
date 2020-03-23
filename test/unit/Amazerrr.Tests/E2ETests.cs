using System.Collections.Generic;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Amazerrr.Tests
{
    public class E2ETests
    {
        private readonly Parser _parser;
        private readonly Solver _solver;

        public E2ETests()
        {
            _parser = new Parser(NullLogger.Instance);
            _solver = new Solver(NullLogger.Instance);
        }

        [Fact]
        public void Large_Board_Test()
        {
            // Arrange
            var input =
                "WWWWWWWWWW\r\n" +
                "W.WWWWW..W\r\n" +
                "W.W...W..W\r\n" +
                "W.W.W.W..W\r\n" +
                "W.......oW\r\n" +
                "W.W.W.WW.W\r\n" +
                "W.W.W.WW.W\r\n" +
                "W.W.W....W\r\n" +
                "W...WWWWWW\r\n" +
                "WWWWWWWWWW";

            var expected = new List<Swipe>()
            {
                Swipe.Left, Swipe.Up, Swipe.Down, Swipe.Right, 
                Swipe.Up, Swipe.Right, Swipe.Down, Swipe.Right, 
                Swipe.Up, Swipe.Left, Swipe.Down
            };
            var board = _parser.Parse(input);

            // Act
            var actual = _solver.Solve(board);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Complex_Board_Test()
        {
            // Arrange
            var input =
                "WWWWWWWWWW\r\n" +
                "W........W\r\n" +
                "W.WW.W...W\r\n" +
                "W.WW.W.WWW\r\n" +
                "W........W\r\n" +
                "WWWW.W.W.W\r\n" +
                "WW.....W.W\r\n" +
                "WW.WoWWW.W\r\n" +
                "WW.W.WWW.W\r\n" +
                "W........W\r\n" +
                "W....WW.WW\r\n" +
                "W.WW.WW.WW\r\n" +
                "W........W\r\n" +
                "WWWWWWW..W\r\n" +
                "WWWWWWWWWW";

            var expected = 20;
            var board = _parser.Parse(input);

            // Act
            var actual = _solver.Solve(board);

            // Assert
            Assert.Equal(expected, actual.Count);
        }
    }
}
