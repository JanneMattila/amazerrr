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
                "WWWWWW\r\n" +
                "W....W\r\n" +
                "WWWW.W\r\n" +
                "Wo...W\r\n" +
                "WWWWWW\r\n";

            // Act
            var actual = _imageAnalyzer.Analyze(Resources.Test1);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
