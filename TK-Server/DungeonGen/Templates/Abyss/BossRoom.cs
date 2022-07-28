using dungeonGen.definitions;
using RotMG.Common.Rasterizer;
using System;

namespace dungeonGen.templates.Abyss
{
    public class BossRoom : Room
    {
        public override int Height => 42;
        public override RoomType Type => RoomType.Target;
        public override int Width => 42;

        public override void Rasterize(BitmapRasterizer<DungeonTile> rasterizer, Random rand)
        {
            var buf = rasterizer.Bitmap;
            var bounds = Bounds;

            rasterizer.Copy(AbyssTemplate.MapTemplate, new Rect(10, 10, 52, 52), Pos, tile => tile.TileType.Name == "Space");

            var numCorrupt = new Range(2, 10).Random(rand);

            while (numCorrupt > 0)
            {
                var x = rand.Next(bounds.X, bounds.MaxX);
                var y = rand.Next(bounds.Y, bounds.MaxY);

                if (buf[x, y].Object == null)
                    continue;

                if (buf[x, y].Object.ObjectType != AbyssTemplate.PartialRedFloor)
                    continue;

                buf[x, y].Object = null;
                numCorrupt--;
            }

            var numImp = new Range(1, 2).Random(rand);
            var numDemon = new Range(1, 3).Random(rand);
            var numBrute = new Range(1, 3).Random(rand);

            while (numImp > 0 || numDemon > 0 || numBrute > 0)
            {
                var x = rand.Next(bounds.X, bounds.MaxX);
                var y = rand.Next(bounds.Y, bounds.MaxY);

                if (buf[x, y].Object != null || buf[x, y].TileType == AbyssTemplate.Space)
                    continue;

                switch (rand.Next(3))
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
                }
            }
        }
    }
}
