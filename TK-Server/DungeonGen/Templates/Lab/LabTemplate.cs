using dungeonGen.definitions;
using RotMG.Common;
using RotMG.Common.Rasterizer;
using System;

namespace dungeonGen.templates.Lab
{
    public class LabTemplate : DungeonTemplate
    {
        public static readonly ObjectType[] Big = new[]
        {
            new ObjectType(0x0981, "Escaped Experiment"),
            new ObjectType(0x0982, "Enforcer Bot 3000"),
            new ObjectType(0x0983, "Crusher Abomination")
        };

        public static readonly ObjectType DestructibleWall = new ObjectType(0x18c3, "Lab Destructible Wall");
        public static readonly TileType LabFloor = new TileType(0x00d3, "Lab Floor");
        public static readonly ObjectType LabWall = new ObjectType(0x188c, "Lab Wall");
        public static readonly DungeonTile[,] MapTemplate;

        public static readonly ObjectType[] Small = new[]
        {
            new ObjectType(0x0979, "Mini Bot"),
            new ObjectType(0x0980, "Rampage Cyborg")
        };

        public static readonly TileType Space = new TileType(0x00fe, "Space");
        public static readonly ObjectType Web = new ObjectType(0x0732, "Spider Web");

        private static readonly DungeonObject web = new DungeonObject { ObjectType = Web };
        private bool generatedEvilRoom;

        private NormDist targetDepth;

        static LabTemplate() => MapTemplate = ReadTemplate(typeof(LabTemplate));

        public override int CorridorWidth => 4;
        public override int MaxDepth => 20;
        public override Range NumRoomRate => new Range(2, 3);
        public override Range RoomSeparation => new Range(6, 8);
        public override NormDist SpecialRmCount => null;
        public override NormDist SpecialRmDepthDist => null;
        public override NormDist TargetDepth => targetDepth;

        public static void CreateEnemies(BitmapRasterizer<DungeonTile> rasterizer, Rect bounds, Random rand)
        {
            var numBig = new Range(0, 3).Random(rand);
            var numSmall = new Range(4, 10).Random(rand);
            var buf = rasterizer.Bitmap;

            while (numBig > 0 || numSmall > 0)
            {
                var x = rand.Next(bounds.X, bounds.MaxX);
                var y = rand.Next(bounds.Y, bounds.MaxY);

                if (buf[x, y].TileType == Space || buf[x, y].Object != null)
                    continue;

                switch (rand.Next(2))
                {
                    case 0:
                        if (numBig > 0)
                        {
                            buf[x, y].Object = new DungeonObject { ObjectType = Big[rand.Next(Big.Length)] };
                            numBig--;
                        }
                        break;

                    case 1:
                        if (numSmall > 0)
                        {
                            buf[x, y].Object = new DungeonObject { ObjectType = Small[rand.Next(Small.Length)] };
                            numSmall--;
                        }
                        break;
                }
            }
        }

        public static void DrawSpiderWeb(BitmapRasterizer<DungeonTile> rasterizer, Rect bounds, Random rand)
        {
            var w = rasterizer.Width;
            var h = rasterizer.Height;
            var buf = rasterizer.Bitmap;

            for (var x = bounds.X; x < bounds.MaxX; x++)
                for (var y = bounds.Y; y < bounds.MaxY; y++)
                {
                    if (buf[x, y].TileType == Space || buf[x, y].Object != null)
                        continue;

                    if (rand.NextDouble() > 0.99)
                        buf[x, y].Object = web;
                }
        }

        public override MapCorridor CreateCorridor() => new Corridor();

        public override Room CreateNormal(int depth, Room prev)
        {
            var rm = new NormalRoom(prev as NormalRoom, Rand, generatedEvilRoom);

            if ((rm.Flags & RoomFlags.Evil) != 0)
                generatedEvilRoom = true;

            return rm;
        }

        public override MapRender CreateOverlay() => new Overlay();

        public override Room CreateSpecial(int depth, Room prev) => throw new InvalidOperationException();

        public override Room CreateStart(int depth) => new StartRoom();

        public override Room CreateTarget(int depth, Room prev) => new BossRoom();

        public override void Initialize()
        {
            targetDepth = new NormDist(3, 10, 7, 15, Rand.Next());
            generatedEvilRoom = false;
        }
    }
}
