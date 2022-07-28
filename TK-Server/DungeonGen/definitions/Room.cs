using RotMG.Common.Rasterizer;
using System;
using System.Collections.Generic;

namespace dungeonGen.definitions
{
    public abstract class Room
    {
        protected Room() => Edges = new List<Edge>(4);

        public Rect Bounds => new Rect(Pos.X, Pos.Y, Pos.X + Width, Pos.Y + Height);
        public int Depth { get; internal set; }
        public IList<Edge> Edges { get; private set; }
        public abstract int Height { get; }
        public virtual Range NumBranches => new Range(1, 4);
        public Point Pos { get; set; }
        public abstract RoomType Type { get; }
        public abstract int Width { get; }

        public abstract void Rasterize(BitmapRasterizer<DungeonTile> rasterizer, Random rand);
    }
}
