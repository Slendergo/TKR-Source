using dungeonGen.definitions;
using dungeonGen.templates;
using RotMG.Common.Rasterizer;
using System;
using System.Linq;

namespace dungeonGen
{
    public class Rasterizer
    {
        private static readonly TileType Space = new TileType(0x00fe, "Space");

        private readonly DungeonGraph graph;
        private readonly Random rand;
        private readonly BitmapRasterizer<DungeonTile> rasterizer;

        public Rasterizer(int seed, DungeonGraph graph)
        {
            rand = new Random(seed);
            this.graph = graph;
            rasterizer = new BitmapRasterizer<DungeonTile>(graph.Width, graph.Height);
            Step = RasterizationStep.Initialize;
        }

        public RasterizationStep Step { get; set; }

        public DungeonTile[,] ExportMap() => rasterizer.Bitmap;

        public void Rasterize(RasterizationStep? targetStep = null)
        {
            while (Step != targetStep && Step != RasterizationStep.Finish)
                RunStep();
        }

        private void CreateCorridor(Room src, Room dst, out Point srcPos, out Point dstPos)
        {
            var edge = src.Edges.Single(ed => ed.RoomB == dst);
            var link = edge.Linkage;

            if (link.Direction == Direction.South || link.Direction == Direction.North)
            {
                srcPos = new Point(link.Offset, src.Pos.Y + src.Height / 2);
                dstPos = new Point(link.Offset, dst.Pos.Y + dst.Height / 2);
            }
            else if (link.Direction == Direction.East || link.Direction == Direction.West)
            {
                srcPos = new Point(src.Pos.X + src.Width / 2, link.Offset);
                dstPos = new Point(dst.Pos.X + dst.Width / 2, link.Offset);
            }
            else
                throw new ArgumentException();
        }

        private void RasterizeCorridor(MapCorridor corridor, Edge edge)
        {
            CreateCorridor(edge.RoomA, edge.RoomB, out Point srcPos, out Point dstPos);
            corridor.Rasterize(edge.RoomA, edge.RoomB, srcPos, dstPos);
        }

        private void RasterizeCorridors()
        {
            var corridor = graph.Template.CreateCorridor();
            corridor.Init(rasterizer, graph, rand);

            foreach (var room in graph.Rooms)
                foreach (var edge in room.Edges)
                {
                    if (edge.RoomA != room)
                        continue;

                    RasterizeCorridor(corridor, edge);
                }
        }

        private void RasterizeRooms()
        {
            foreach (var room in graph.Rooms)
                room.Rasterize(rasterizer, rand);
        }

        private void RunStep()
        {
            switch (Step)
            {
                case RasterizationStep.Initialize:
                    rasterizer.Clear(new DungeonTile { TileType = Space });
                    graph.Template.InitializeRasterization(graph);
                    break;

                case RasterizationStep.Background:
                    var bg = graph.Template.CreateBackground();
                    bg.Init(rasterizer, graph, rand);
                    bg.Rasterize();
                    break;

                case RasterizationStep.Corridor: RasterizeCorridors(); break;
                case RasterizationStep.Room: RasterizeRooms(); break;

                case RasterizationStep.Overlay:
                    var overlay = graph.Template.CreateOverlay();
                    overlay.Init(rasterizer, graph, rand);
                    overlay.Rasterize();
                    break;
            }

            Step++;
        }
    }
}
