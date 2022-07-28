using dungeonGen.definitions;
using RotMG.Common.Rasterizer;
using System;

namespace dungeonGen.templates.Lab
{
    public class StartRoom : FixedRoom
    {
        private static readonly Tuple<Direction, int>[] connections = { Tuple.Create(Direction.North, 11) };
        private static readonly Rect template = new Rect(0, 96, 26, 128);

        public override Tuple<Direction, int>[] ConnectionPoints => connections;
        public override int Height => template.MaxY - template.Y;
        public override RoomType Type => RoomType.Start;
        public override int Width => template.MaxX - template.X;

        public override void Rasterize(BitmapRasterizer<DungeonTile> rasterizer, Random rand)
        {
            rasterizer.Copy(LabTemplate.MapTemplate, template, Pos);

            LabTemplate.DrawSpiderWeb(rasterizer, Bounds, rand);
        }
    }
}
