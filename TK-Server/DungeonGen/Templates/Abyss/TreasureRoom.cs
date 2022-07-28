using dungeonGen.definitions;
using RotMG.Common.Rasterizer;
using System;

namespace dungeonGen.templates.Abyss
{
    public class TreasureRoom : FixedRoom
    {
        private static readonly Tuple<Direction, int>[] connections = new[] { Tuple.Create(Direction.South, 6) };

        public override Tuple<Direction, int>[] ConnectionPoints => connections;
        public override int Height => 21;
        public override RoomType Type => RoomType.Special;
        public override int Width => 15;

        public override void Rasterize(BitmapRasterizer<DungeonTile> rasterizer, Random rand)
        {
            rasterizer.Copy(AbyssTemplate.MapTemplate, new Rect(70, 10, 85, 31), Pos, tile => tile.TileType.Name == "Space");

            var bounds = Bounds;
            var buf = rasterizer.Bitmap;

            for (var x = bounds.X; x < bounds.MaxX; x++)
                for (var y = bounds.Y; y < bounds.MaxY; y++)
                    if (buf[x, y].TileType != AbyssTemplate.Space)
                        buf[x, y].Region = "Treasure";
        }
    }
}
