using Xunit;

namespace Amazerrr.Tests
{
    public class ImageAnalyzerTests
    {
        private readonly ImageAnalyzer _imageAnalyzer;

        public ImageAnalyzerTests()
        {
            _imageAnalyzer = new ImageAnalyzer();
        }

        [Fact]
        public void Analyze_Test1_File()
        {
            // Arrange
            var expected =
                "######\r\n" +
                "#....#\r\n" +
                "####.#\r\n" +
                "#o...#\r\n" +
                "######";

            // Act
            var actual = _imageAnalyzer.Analyze(Resources.Test1);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
