using System.Drawing;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Amazerrr.Tests
{
    public class ParserTests
    {
        private readonly Parser _parser;

        public ParserTests()
        {
            _parser = new Parser(NullLogger.Instance);
        }

        [Fact]
        public void Parse_Test1_File()
        {
            // Arrange
            var input =
                "WWWWWW\r\n" +
                "W....W\r\n" +
                "WWWW.W\r\n" +
                "Wo...W\r\n" +
                "WWWWWW";
            var expected = 9;
            var expectedStartPosition = new Point(1, 3);

            // Act
            var actual = _parser.Parse(input);

            // Assert
            Assert.Equal(expected, actual.TotalCount);
            Assert.Equal(expectedStartPosition.X, actual.StartPosition.X);
            Assert.Equal(expectedStartPosition.Y, actual.StartPosition.Y);
        }
    }
}
