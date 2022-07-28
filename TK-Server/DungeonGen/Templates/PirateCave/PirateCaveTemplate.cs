using dungeonGen.definitions;
using RotMG.Common;
using System;

namespace dungeonGen.templates.PirateCave
{
    public class PirateCaveTemplate : DungeonTemplate
    {
        public static readonly ObjectType[] Boss = new[]
        {
            new ObjectType(0x683, "Pirate Lieutenant"),
            new ObjectType(0x684, "Pirate Commander"),
            new ObjectType(0x685, "Pirate Captain"),
            new ObjectType(0x686, "Pirate Admiral")
        };

        public static readonly TileType BrownLines = new TileType(0x000c, "Brown Lines");
        public static readonly ObjectType CaveWall = new ObjectType(0x01ce, "Cave Wall");
        public static readonly TileType Composite = new TileType(0x00fd, "Composite");
        public static readonly ObjectType CowardicePortal = new ObjectType(0x0703, "Portal of Cowardice");
        public static readonly TileType LightSand = new TileType(0x00bd, "Light Sand");

        public static readonly ObjectType[] Minion = new[]
        {
            new ObjectType(0x687, "Cave Pirate Brawler"),
            new ObjectType(0x688, "Cave Pirate Sailor"),
            new ObjectType(0x689, "Cave Pirate Veteran")
        };

        public static readonly ObjectType PalmTree = new ObjectType(0x018e, "Palm Tree");

        public static readonly ObjectType[] Pet = new[]
        {
            new ObjectType(0x68a, "Cave Pirate Moll"),
            new ObjectType(0x68b, "Cave Pirate Parrot"),
            new ObjectType(0x68c, "Cave Pirate Macaw"),
            new ObjectType(0x68d, "Cave Pirate Monkey"),
            new ObjectType(0x68e, "Cave Pirate Hunchback"),
            new ObjectType(0x68f, "Cave Pirate Cabin Boy")
        };

        public static readonly ObjectType PirateKing = new ObjectType(0x0927, "Dreadstump the Pirate King");
        public static readonly TileType ShallowWater = new TileType(0x0073, "Shallow Water");
        public static readonly TileType Space = new TileType(0x00fe, "Space");

        private NormDist targetDepth;

        public override int CorridorWidth => 2;
        public override int MaxDepth => 10;
        public override Range RoomSeparation => new Range(3, 7);
        public override NormDist SpecialRmCount => null;
        public override NormDist SpecialRmDepthDist => null;
        public override NormDist TargetDepth => targetDepth;

        public override MapRender CreateBackground() => new Background();

        public override MapCorridor CreateCorridor() => new Corridor();

        public override Room CreateNormal(int depth, Room prev) => new NormalRoom(Rand.Next(8, 15), Rand.Next(8, 15));

        public override MapRender CreateOverlay() => new Overlay();

        public override Room CreateSpecial(int depth, Room prev) => throw new InvalidOperationException();

        public override Room CreateStart(int depth) => new StartRoom(10);

        public override Room CreateTarget(int depth, Room prev) => new BossRoom(10);

        public override void Initialize() => targetDepth = new NormDist(1, 5.5f, 4, 7, Rand.Next());
    }
}
