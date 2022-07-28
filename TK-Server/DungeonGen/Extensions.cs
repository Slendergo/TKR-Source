using dungeonGen.definitions;
using RotMG.Common.Rasterizer;
using System;

namespace dungeonGen
{
    public static class Extensions
    {
        public static void Copy<TPixel>(this BitmapRasterizer<TPixel> self, TPixel[,] src, Rect srcRect, Point dst, Func<TPixel, bool> transprent = null) where TPixel : struct
        {
            var w = srcRect.MaxX - srcRect.X;
            var h = srcRect.MaxY - srcRect.Y;
            var buf = self.Bitmap;

            if (transprent == null)
                transprent = pix => false;

            for (var x = 0; x < w; x++)
                for (var y = 0; y < h; y++)
                {
                    var pix = src[x + srcRect.X, y + srcRect.Y];

                    if (transprent(pix))
                        continue;

                    buf[x + dst.X, y + dst.Y] = src[x + srcRect.X, y + srcRect.Y];
                }
        }

        public static Direction Reverse(this Direction direction)
        {
            switch (direction)
            {
                default: throw new ArgumentException();
                case Direction.North: return Direction.South;
                case Direction.South: return Direction.North;
                case Direction.East: return Direction.West;
                case Direction.West: return Direction.East;
            }
        }
    }
}
