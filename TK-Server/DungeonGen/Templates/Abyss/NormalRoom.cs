using dungeonGen.definitions;
using RotMG.Common.Rasterizer;
using System;

namespace dungeonGen.templates.Abyss
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
            rasterizer.FillRect(Bounds, new DungeonTile
            {
                TileType = AbyssTemplate.RedSmallChecks
            });

            var numImp = new Range(0, 2).Random(rand);
            var numDemon = new Range(2, 4).Random(rand);
            var numBrute = new Range(1, 4).Random(rand);
            var numSkull = new Range(1, 3).Random(rand);

            var buf = rasterizer.Bitmap;
            var bounds = Bounds;

            while (numImp > 0 || numDemon > 0 || numBrute > 0 || numSkull > 0)
            {
                int x = rand.Next(bounds.X, bounds.MaxX);
                int y = rand.Next(bounds.Y, bounds.MaxY);
                if (buf[x, y].Object != null)
                    continue;

                switch (rand.Next(4))
                {
                    case 0:
                        if (numImp > 0)
                        {
                            buf[x, y].Object = new DungeonObject { ObjectType = AbyssTemplate.AbyssImp };
                            numImp--;
                        }
                        break;

                    case 1:
                        if (numDemon > 0)
                        {
                            buf[x, y].Object = new DungeonObject { ObjectType = AbyssTemplate.AbyssDemon[rand.Next(AbyssTemplate.AbyssDemon.Length)] };
                            numDemon--;
                        }
                        break;

                    case 2:
                        if (numBrute > 0)
                        {
                            buf[x, y].Object = new DungeonObject { ObjectType = AbyssTemplate.AbyssBrute[rand.Next(AbyssTemplate.AbyssBrute.Length)] };
                            numBrute--;
                        }
                        break;

                    case 3:
                        if (numSkull > 0)
                        {
                            buf[x, y].Object = new DungeonObject { ObjectType = AbyssTemplate.AbyssBones };
                            numSkull--;
                        }
                        break;
                }
            }
        }
    }
}
