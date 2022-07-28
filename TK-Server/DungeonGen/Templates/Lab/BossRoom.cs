using dungeonGen.definitions;
using RotMG.Common.Rasterizer;
using System;

namespace dungeonGen.templates.Lab
{
    internal class BossRoom : FixedRoom
    {
        private static readonly Tuple<Direction, int>[] connections = { Tuple.Create(Direction.South, 10) };
        private static readonly Rect template = new Rect(0, 0, 24, 50);

        public override Tuple<Direction, int>[] ConnectionPoints => connections;
        public override int Height => template.MaxY - template.Y;
        public override RoomType Type => RoomType.Target;
        public override int Width => template.MaxX - template.X;

        public override void Rasterize(BitmapRasterizer<DungeonTile> rasterizer, Random rand)
        {
            rasterizer.Copy(LabTemplate.MapTemplate, template, Pos);

            LabTemplate.DrawSpiderWeb(rasterizer, Bounds, rand);
        }
    }
}
