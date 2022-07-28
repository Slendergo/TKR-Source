using dungeonGen.definitions;
using RotMG.Common;

namespace dungeonGen.templates.Abyss
{
    public class AbyssTemplate : DungeonTemplate
    {
        public static readonly ObjectType AbyssBones = new ObjectType(0x01fa, "Abyss Bones");

        public static readonly ObjectType[] AbyssBrute = new[]
        {
            new ObjectType(0x671, "Brute of the Abyss"),
            new ObjectType(0x672, "Brute Warrior of the Abyss")
        };

        public static readonly ObjectType[] AbyssDemon = new[]
        {
            new ObjectType(0x66e, "Demon of the Abyss"),
            new ObjectType(0x66f, "Demon Warrior of the Abyss"),
            new ObjectType(0x670, "Demon Mage of the Abyss")
        };

        internal static readonly ObjectType AbyssImp = new ObjectType(0x66d, "Imp of the Abyss");
        internal static readonly ObjectType BrokenRedPillar = new ObjectType(0x0183, "Broken Red Pillar");
        internal static readonly ObjectType CowardicePortal = new ObjectType(0x0703, "Portal of Cowardice");
        internal static readonly TileType Lava = new TileType(0x0070, "Lava");
        internal static readonly DungeonTile[,] MapTemplate;
        internal static readonly ObjectType PartialRedFloor = new ObjectType(0x0153, "Partial Red Floor");
        internal static readonly ObjectType RedPillar = new ObjectType(0x017e, "Red Pillar");
        internal static readonly TileType RedSmallChecks = new TileType(0x003c, "Red Small Checks");
        internal static readonly ObjectType RedTorchWall = new ObjectType(0x0151, "Red Torch Wall");
        internal static readonly ObjectType RedWall = new ObjectType(0x0150, "Red Wall");
        internal static readonly TileType Space = new TileType(0x00fe, "Space");

        private NormDist specialRmCount;
        private NormDist specialRmDepthDist;
        private NormDist targetDepth;

        static AbyssTemplate() => MapTemplate = ReadTemplate(typeof(AbyssTemplate));

        public override int CorridorWidth => 3;
        public override int MaxDepth => 50;
        public override Range RoomSeparation => new Range(0, 1);
        public override NormDist SpecialRmCount => specialRmCount;
        public override NormDist SpecialRmDepthDist => specialRmDepthDist;
        public override NormDist TargetDepth => targetDepth;

        public override MapCorridor CreateCorridor() => new Corridor();

        public override Room CreateNormal(int depth, Room prev) => new NormalRoom(8, 8);

        public override MapRender CreateOverlay() => new Overlay();

        public override Room CreateSpecial(int depth, Room prev) => new TreasureRoom();

        public override Room CreateStart(int depth) => new StartRoom(16);

        public override Room CreateTarget(int depth, Room prev) => new BossRoom();

        public override void Initialize()
        {
            targetDepth = new NormDist(3, 20, 15, 35, Rand.Next());
            specialRmCount = new NormDist(1.5f, 0.5f, 0, 5, Rand.Next());
            specialRmDepthDist = new NormDist(5, 20, 10, 35, Rand.Next());
        }
    }
}
