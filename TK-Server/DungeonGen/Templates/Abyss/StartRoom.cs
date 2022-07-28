using dungeonGen.definitions;
using RotMG.Common.Rasterizer;
using System;

namespace dungeonGen.templates.Abyss
{
    internal class StartRoom : Room
    {
        public Point portalPos;

        private readonly int len;

        public StartRoom(int len) => this.len = len;

        public override int Height => len;
        public override RoomType Type => RoomType.Start;
        public override int Width => len;

        public override void Rasterize(BitmapRasterizer<DungeonTile> rasterizer, Random rand)
        {
            rasterizer.FillRect(Bounds, new DungeonTile { TileType = AbyssTemplate.RedSmallChecks });

            var buf = rasterizer.Bitmap;
            var bounds = Bounds;
            var portalPlaced = false;

            while (!portalPlaced)
            {
                var x = rand.Next(bounds.X + 2, bounds.MaxX - 4);
                var y = rand.Next(bounds.Y + 2, bounds.MaxY - 4);

                if (buf[x, y].Object != null)
                    continue;

                buf[x, y].Region = "Spawn";
                buf[x, y].Object = new DungeonObject { ObjectType = AbyssTemplate.CowardicePortal };
                portalPos = new Point(x, y);
                portalPlaced = true;
            }
        }
    }
}
