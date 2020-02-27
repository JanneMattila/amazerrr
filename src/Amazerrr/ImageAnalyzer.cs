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
        private Rectangle _rectangle;

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
                var c = GetPixel(bitmap, topX, topY);
                if (!c.HasValue || c.Value.ToArgb() == BoardInnerColor)
                {
                    break;
                }
                topY++;
            }

            (_rectangle, _) = FindRectangle(bitmap, topX, topY);
            FindBoard(bitmap, topX, topY, 0, 0);

            return CreateGrid();
        }

        private static Color? GetPixel(Bitmap bitmap, int x, int y)
        {
            if (x < 0 || y < 0 || x >= bitmap.Width || y >= bitmap.Height)
            {
                return null;
            }
            return bitmap.GetPixel(x, y);
        }

        private static (Rectangle, Color) FindRectangle(Bitmap bitmap, int x, int y)
        {
            int leftX;
            int rightX;
            int bottomY;
            int topY;
            Color color = GetPixel(bitmap, x, y).GetValueOrDefault();

            // Finding the top side of board square
            topY = y;
            while (true)
            {
                var c = GetPixel(bitmap, x, topY);
                if (!c.HasValue || c.Value.ToArgb() != BoardInnerColor)
                {
                    break;
                }
                topY--;
            }

            // Finding the left side of board square
            leftX = x;
            while (true)
            {
                var c = GetPixel(bitmap, leftX, y);
                if (!c.HasValue || c.Value.ToArgb() != BoardInnerColor)
                {
                    break;
                }
                leftX--;
            }

            // Finding the right side of board square
            rightX = x;
            while (true)
            {
                var c = GetPixel(bitmap, rightX, y);
                if (!c.HasValue || c.Value.ToArgb() != BoardInnerColor)
                {
                    break;
                }
                rightX++;
            }

            // Finding the bottom side of board square
            bottomY = y;
            while (true)
            {
                var c = GetPixel(bitmap, x, bottomY);
                if (!c.HasValue || c.Value.ToArgb() != BoardInnerColor)
                {
                    break;
                }

                bottomY++;
            }

            var rectangle = new Rectangle(leftX + 1, topY + 1, rightX - leftX, bottomY - topY);
            return (rectangle, color);
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
                sb.Append(Constants.Wall);
            }
            sb.AppendLine();

            for (int y = smallestY; y <= largestY; y++)
            {
                sb.Append(Constants.Wall);
                for (int x = smallestX; x <= largestX; x++)
                {
                    var b = _board.FirstOrDefault(b => b.X == x && b.Y == y);
                    if (b == null)
                    {
                        sb.Append(Constants.Wall);
                    }
                    else if (b.IsPlayer)
                    {
                        sb.Append(Constants.Player);
                    }
                    else
                    {
                        sb.Append(Constants.Corridor);
                    }
                }
                sb.AppendLine(Constants.Wall.ToString());
            }

            // Footer line
            for (int x = smallestX; x <= largestX + 2; x++)
            {
                sb.Append(Constants.Wall);
            }
            sb.AppendLine();
            return sb.ToString();
        }

        private void FindBoard(Bitmap bitmap, int pixelX, int pixelY, int boardX, int boardY)
        {
            if (_board.Any(b => b.X == boardX && b.Y == boardY))
            {
                return;
            }

            const int minSize = 10;
            (var rectangle, var color) = FindRectangle(bitmap, pixelX, pixelY);
            if (rectangle.X < 0 || rectangle.Y < 0 ||
                rectangle.Right >= bitmap.Width || 
                rectangle.Bottom >= bitmap.Height ||
                rectangle.Width < minSize || rectangle.Height < minSize)
            {
                var centerColor = color.ToArgb();
                if (centerColor == WhiteColor || 
                    centerColor == BoardOuterColor ||
                    centerColor == 0)
                {
                    // Out from board
                    return;
                }
                else
                {
                    rectangle = new Rectangle(pixelX - _rectangle.Width / 2, pixelY - _rectangle.Height / 2, _rectangle.Width, _rectangle.Height);
                }
            }

            var c = GetPixel(bitmap, rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2);
            var argb = c.Value.ToArgb();
            if (argb == WhiteColor || argb == BoardOuterColor)
            {
                return;
            }

            _board.Add(new ImagePosition()
            {
                X = boardX,
                Y = boardY,
                Rectangle = rectangle,
                IsPlayer = argb != BoardInnerColor
            });

            var xcenter = rectangle.X + rectangle.Width / 2;
            var ycenter = rectangle.Y + rectangle.Height / 2;
            FindBoard(bitmap, xcenter + rectangle.Width, ycenter, boardX + 1, boardY);
            FindBoard(bitmap, xcenter - rectangle.Width, ycenter, boardX - 1, boardY);
            FindBoard(bitmap, xcenter, ycenter + rectangle.Height, boardX, boardY + 1);
            FindBoard(bitmap, xcenter, ycenter - rectangle.Height, boardX, boardY - 1);
        }
    }
}
