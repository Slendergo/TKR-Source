using dungeonGen.definitions;
using RotMG.Common.Rasterizer;
using System;

namespace dungeonGen.templates
{
    public class MapRender
    {
        protected DungeonGraph Graph { get; private set; }
        protected Random Rand { get; private set; }
        protected BitmapRasterizer<DungeonTile> Rasterizer { get; private set; }

        public void Init(BitmapRasterizer<DungeonTile> rasterizer, DungeonGraph graph, Random rand)
        {
            Rasterizer = rasterizer;
            Graph = graph;
            Rand = rand;
        }

        public virtual void Rasterize()
        { }
    }
}
