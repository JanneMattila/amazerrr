using System.Collections.Generic;
using Xunit;

namespace Amazerrr.Tests
{
    public class ParserTests
    {
        private readonly Parser _parser;

        public ParserTests()
        {
            _parser = new Parser();
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
            var expected = 9;

            // Act
            var actual = _parser.Parse(input);

            // Assert
            Assert.Equal(expected, actual.TotalCount);
        }
    }
}
