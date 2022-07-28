using common.resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using wServer.core.terrain;
using wServer.core.worlds;

namespace wServer.core.setpieces
{
    public static class SetPieces
    {
        private static readonly List<Tuple<ISetPiece, int, int, TerrainType[]>> setPieces = new List<Tuple<ISetPiece, int, int, TerrainType[]>>()
        {
            SetPiece(new KageKami(), 4, 5, TerrainType.HighForest, TerrainType.HighPlains),
            SetPiece(new Building(), 80, 100, TerrainType.LowForest, TerrainType.LowPlains, TerrainType.MidForest),
            SetPiece(new Graveyard(), 5, 10, TerrainType.LowSand, TerrainType.LowPlains),
            SetPiece(new Castle(), 4, 7, TerrainType.HighForest, TerrainType.HighPlains),
            SetPiece(new TempleA(), 10, 20, TerrainType.MidForest, TerrainType.MidPlains),
            SetPiece(new TempleB(), 10, 20, TerrainType.MidForest, TerrainType.MidPlains),
            SetPiece(new Avatar(), 1, 1, TerrainType.Mountains),
            SetPiece(new NamedEntitySetPiece("Spectral Sentry"), 1, 1, TerrainType.Mountains),
            SetPiece(new NamedEntitySetPiece("Julius Caesar"), 1, 1, TerrainType.Mountains),
            SetPiece(new NamedEntitySetPiece("Crystal Prisoner"), 1, 1, TerrainType.Mountains)
        };

        public static void ApplySetPieces(World world)
        {
            var map = world.Map;
            var w = map.Width;
            var h = map.Height;
            var rand = new Random();
            var rects = new HashSet<Rect>();

            foreach (var dat in setPieces)
            {
                var size = dat.Item1.Size;
                var count = rand.Next(dat.Item2, dat.Item3);

                for (var i = 0; i < count; i++)
                {
                    Rect rect;

                    var pt = new IntPoint();
                    var max = 1024;

                    do
                    {
                        pt.X = rand.Next(0, w);
                        pt.Y = rand.Next(0, h);
                        rect = new Rect() { x = pt.X, y = pt.Y, w = size, h = size };
                        max--;
                    } while ((Array.IndexOf(dat.Item4, map[pt.X, pt.Y].Terrain) == -1 || rects.Any(_ => Rect.Intersects(rect, _))) && max > 0);

                    if (max <= 0)
                        continue;

                    dat.Item1.RenderSetPiece(world, pt);
                    rects.Add(rect);
                }
            }
        }

        public static int[,] reflectHori(int[,] mat)
        {
            var M = mat.GetLength(0);
            var N = mat.GetLength(1);
            var ret = new int[M, N];

            for (var x = 0; x < M; x++)
                for (var y = 0; y < N; y++)
                    ret[M - x - 1, y] = mat[x, y];

            return ret;
        }

        public static int[,] reflectVert(int[,] mat)
        {
            var M = mat.GetLength(0);
            var N = mat.GetLength(1);
            var ret = new int[M, N];

            for (var x = 0; x < M; x++)
                for (var y = 0; y < N; y++)
                    ret[x, N - y - 1] = mat[x, y];

            return ret;
        }

        public static void RenderFromProto(World world, IntPoint pos, ProtoWorld proto)
        {
            var manager = world.Manager;
            var map = 0;

            if (proto.maps != null && proto.maps.Length > 1)
            {
                var rnd = new Random();
                map = rnd.Next(0, proto.maps.Length);
            }

            var ms = new MemoryStream(proto.wmap[map]);
            var sp = new Wmap(manager.Resources.GameData);

            sp.Load(ms, 0);
            sp.ProjectOntoWorld(world, pos);
        }

        public static int[,] rotateCW(int[,] mat)
        {
            var M = mat.GetLength(0);
            var N = mat.GetLength(1);
            var ret = new int[N, M];

            for (var r = 0; r < M; r++)
                for (var c = 0; c < N; c++)
                    ret[c, M - 1 - r] = mat[r, c];

            return ret;
        }

        private static int DistSqr(IntPoint a, IntPoint b) => (a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y);

        private static Tuple<ISetPiece, int, int, TerrainType[]> SetPiece(ISetPiece piece, int min, int max, params TerrainType[] terrains) => Tuple.Create(piece, min, max, terrains);

        private struct Rect
        {
            public int h;
            public int w;
            public int x;
            public int y;

            public static bool Intersects(Rect r1, Rect r2) => !(r2.x > r1.x + r1.w || r2.x + r2.w < r1.x || r2.y > r1.y + r1.h || r2.y + r2.h < r1.y);
        }
    }
}
