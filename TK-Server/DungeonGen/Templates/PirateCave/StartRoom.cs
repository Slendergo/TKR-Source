using dungeonGen.definitions;
using RotMG.Common.Rasterizer;
using System;

namespace dungeonGen.templates.PirateCave
{
    public class StartRoom : Room
    {
        private readonly int radius;

        public StartRoom(int radius) => this.radius = radius;

        public override int Height => radius * 2 + 1;
        public override RoomType Type => RoomType.Start;
        public override int Width => radius * 2 + 1;

        public override void Rasterize(BitmapRasterizer<DungeonTile> rasterizer, Random rand)
        {
            var tile = new DungeonTile { TileType = PirateCaveTemplate.LightSand };
            var cX = Pos.X + radius + 0.5;
            var cY = Pos.Y + radius + 0.5;
            var bounds = Bounds;
            var r2 = radius * radius;
            var buf = rasterizer.Bitmap;
            var pR = rand.NextDouble() * (radius - 2);
            var pA = rand.NextDouble() * 2 * Math.PI;
            var pX = (int)(cX + Math.Cos(pR) * pR);
            var pY = (int)(cY + Math.Sin(pR) * pR);

            for (var x = bounds.X; x < bounds.MaxX; x++)
                for (var y = bounds.Y; y < bounds.MaxY; y++)
                {
                    if ((x - cX) * (x - cX) + (y - cY) * (y - cY) <= r2)
                    {
                        buf[x, y] = tile;

                        if (rand.NextDouble() > 0.95)
                            buf[x, y].Object = new DungeonObject { ObjectType = PirateCaveTemplate.PalmTree };
                    }

                    if (x == pX && y == pY)
                    {
                        buf[x, y].Region = "Spawn";
                        buf[x, y].Object = new DungeonObject { ObjectType = PirateCaveTemplate.CowardicePortal };
                    }
                }
        }
    }
}
