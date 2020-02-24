using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Amazerrr
{
    public class ImageAnalyzer
    {
        private const int WhiteColor = -1;
        private const int BoardOuterColor = -9735296;
        private const int BoardInnerColor = -13684945;

        private List<ImagePosition> _board;
        private int _width;
        private int _height;

        public string Analyze(byte[] imageData)
        {
            _board = new List<ImagePosition>();
            using var memoryStream = new MemoryStream(imageData);
            using var bitmap = new Bitmap(memoryStream);

            var topX = bitmap.Width / 2;
            var topY = 0;

            // Finding the inner board
            while (true)
            {
                var c = bitmap.GetPixel(topX, topY);
                if (c.ToArgb() == BoardInnerColor)
                {
                    break;
                }
                topY++;
            }

            // Finding the left side of board square
            var leftX = topX;
            while (true)
            {
                var c = bitmap.GetPixel(leftX, topY);
                if (c.ToArgb() != BoardInnerColor)
                {
                    break;
                }
                leftX--;
            }

            // Finding the right side of board square
            var rightX = topX;
            while (true)
            {
                var c = bitmap.GetPixel(rightX, topY);
                if (c.ToArgb() != BoardInnerColor)
                {
                    break;
                }
                rightX++;
            }

            // Finding the border width
            var border = 0;
            while (true)
            {
                var c = bitmap.GetPixel(rightX + border, topY);
                if (c.ToArgb() == BoardInnerColor)
                {
                    break;
                }
                border++;
            }

            // Finding the bottom side of board square
            var bottomY = topY;
            while (true)
            {
                var c = bitmap.GetPixel(topX, bottomY);
                if (c.ToArgb() != BoardInnerColor)
                {
                    break;
                }

                bottomY++;
            }

            _width = rightX - leftX + border;
            _height = bottomY - topY + border;

            FindBoard(bitmap, leftX + _width / 2, topY + _height / 3, 0, 0);

            return CreateGrid();
        }

        private string CreateGrid()
        {
            var sb = new StringBuilder();
            var smallestX = int.MaxValue;
            var smallestY = int.MaxValue;
            var largestX = int.MinValue;
            var largestY = int.MinValue;
            foreach (var item in _board)
            {
                if (item.X < smallestX)
                {
                    smallestX = item.X;
                }
                if (item.Y < smallestY)
                {
                    smallestY = item.Y;
                }
                if (item.X > largestX)
                {
                    largestX = item.X;
                }
                if (item.Y > largestY)
                {
                    largestY = item.Y;
                }
            }

            // Header line
            for (int x = smallestX; x <= largestX + 2; x++)
            {
                sb.Append(ImageAnalyzerConstants.Wall);
            }
            sb.AppendLine();

            for (int y = smallestY; y <= largestY; y++)
            {
                sb.Append(ImageAnalyzerConstants.Wall);
                for (int x = smallestX; x <= largestX; x++)
                {
                    var b = _board.FirstOrDefault(b => b.X == x && b.Y == y);
                    if (b == null)
                    {
                        sb.Append(ImageAnalyzerConstants.Wall);
                    }
                    else if (b.IsPlayer)
                    {
                        sb.Append(ImageAnalyzerConstants.Player);
                    }
                    else
                    {
                        sb.Append(ImageAnalyzerConstants.Corridor);
                    }
                }
                sb.AppendLine(ImageAnalyzerConstants.Wall);
            }

            // Footer line
            for (int x = smallestX; x <= largestX + 2; x++)
            {
                sb.Append(ImageAnalyzerConstants.Wall);
            }
            sb.AppendLine();
            return sb.ToString();
        }

        private void FindBoard(Bitmap bitmap, int pixelX, int pixelY, int x, int y)
        {
            if (_board.Any(b => b.X == x && b.Y == y))
            {
                return;
            }

            if (pixelX < 0 || pixelY < 0 ||
                pixelX >= bitmap.Width || pixelY >= bitmap.Height)
            {
                return;
            }

            var c = bitmap.GetPixel(pixelX, pixelY);
            var argb = c.ToArgb();
            if (argb == WhiteColor || argb == BoardOuterColor)
            {
                return;
            }

            _board.Add(new ImagePosition()
            {
                X = x, Y = y, IsPlayer = argb != BoardInnerColor
            });
            FindBoard(bitmap, pixelX + _width, pixelY, x + 1, y);
            FindBoard(bitmap, pixelX - _width, pixelY, x - 1, y);
            FindBoard(bitmap, pixelX, pixelY + _height, x, y + 1);
            FindBoard(bitmap, pixelX, pixelY - _height, x, y - 1);
        }
    }
}
