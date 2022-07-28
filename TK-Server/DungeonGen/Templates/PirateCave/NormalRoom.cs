using dungeonGen.definitions;
using RotMG.Common.Rasterizer;
using System;

namespace dungeonGen.templates.PirateCave
{
    public class NormalRoom : Room
    {
        private readonly int h;
        private readonly int w;

        public NormalRoom(int w, int h)
        {
            this.w = w;
            this.h = h;
        }

        public override int Height => h;
        public override RoomType Type => RoomType.Normal;
        public override int Width => w;

        public override void Rasterize(BitmapRasterizer<DungeonTile> rasterizer, Random rand)
        {
            rasterizer.FillRect(Bounds, new DungeonTile { TileType = PirateCaveTemplate.BrownLines });

            var numBoss = new Range(0, 1).Random(rand);
            var numMinion = new Range(3, 5).Random(rand);
            var numPet = new Range(0, 2).Random(rand);
            var buf = rasterizer.Bitmap;
            var bounds = Bounds;

            while (numBoss > 0 || numMinion > 0 || numPet > 0)
            {
                var x = rand.Next(bounds.X, bounds.MaxX);
                var y = rand.Next(bounds.Y, bounds.MaxY);

                if (buf[x, y].Object != null)
                    continue;

                switch (rand.Next(3))
                {
                    case 0:
                        if (numBoss > 0)
                        {
                            buf[x, y].Object = new DungeonObject { ObjectType = PirateCaveTemplate.Boss[rand.Next(PirateCaveTemplate.Boss.Length)] };
                            numBoss--;
                        }
                        break;

                    case 1:
                        if (numMinion > 0)
                        {
                            buf[x, y].Object = new DungeonObject { ObjectType = PirateCaveTemplate.Minion[rand.Next(PirateCaveTemplate.Minion.Length)] };
                            numMinion--;
                        }
                        break;

                    case 2:
                        if (numPet > 0)
                        {
                            buf[x, y].Object = new DungeonObject { ObjectType = PirateCaveTemplate.Pet[rand.Next(PirateCaveTemplate.Pet.Length)] };
                            numPet--;
                        }
                        break;
                }
            }
        }
    }
}
