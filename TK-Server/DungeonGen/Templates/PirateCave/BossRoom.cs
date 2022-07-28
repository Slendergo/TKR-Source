using dungeonGen.definitions;
using RotMG.Common.Rasterizer;
using System;

namespace dungeonGen.templates.PirateCave
{
    public class BossRoom : Room
    {
        private readonly int radius;

        public BossRoom(int radius) => this.radius = radius;

        public override int Height => radius * 2 + 1;
        public override RoomType Type => RoomType.Target;
        public override int Width => radius * 2 + 1;

        public override void Rasterize(BitmapRasterizer<DungeonTile> rasterizer, Random rand)
        {
            var tile = new DungeonTile { TileType = PirateCaveTemplate.BrownLines };
            var cX = Pos.X + radius + 0.5;
            var cY = Pos.Y + radius + 0.5;
            var bounds = Bounds;
            var r2 = radius * radius;
            var buf = rasterizer.Bitmap;

            for (var x = bounds.X; x < bounds.MaxX; x++)
                for (var y = bounds.Y; y < bounds.MaxY; y++)
                    if ((x - cX) * (x - cX) + (y - cY) * (y - cY) <= r2)
                        buf[x, y] = tile;

            var numKing = 1;
            var numBoss = new Range(4, 7).Random(rand);
            var numMinion = new Range(4, 7).Random(rand);

            r2 = (radius - 2) * (radius - 2);

            while (numKing > 0 || numBoss > 0 || numMinion > 0)
            {
                var x = rand.Next(bounds.X, bounds.MaxX);
                var y = rand.Next(bounds.Y, bounds.MaxY);

                if ((x - cX) * (x - cX) + (y - cY) * (y - cY) > r2)
                    continue;

                if (buf[x, y].Object != null || buf[x, y].TileType != PirateCaveTemplate.BrownLines)
                    continue;

                switch (rand.Next(3))
                {
                    case 0:
                        if (numKing > 0)
                        {
                            buf[x, y].Object = new DungeonObject { ObjectType = PirateCaveTemplate.PirateKing };
                            numKing--;
                        }
                        break;

                    case 1:
                        if (numBoss > 0)
                        {
                            buf[x, y].Object = new DungeonObject { ObjectType = PirateCaveTemplate.Boss[rand.Next(PirateCaveTemplate.Boss.Length)] };
                            numBoss--;
                        }
                        break;

                    case 2:
                        if (numMinion > 0)
                        {
                            buf[x, y].Object = new DungeonObject { ObjectType = PirateCaveTemplate.Minion[rand.Next(PirateCaveTemplate.Minion.Length)] };
                            numMinion--;
                        }
                        break;
                }
            }
        }
    }
}
