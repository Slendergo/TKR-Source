using dungeonGen.definitions;
using RotMG.Common;
using System;
using System.IO;

namespace dungeonGen.templates
{
    public abstract class DungeonTemplate
    {
        public abstract int CorridorWidth { get; }
        public abstract int MaxDepth { get; }

        public virtual Range NumRoomRate => new Range(3, 5);

        public abstract Range RoomSeparation { get; }
        public abstract NormDist SpecialRmCount { get; }
        public abstract NormDist SpecialRmDepthDist { get; }
        public abstract NormDist TargetDepth { get; }

        protected Random Rand { get; private set; }

        public virtual MapRender CreateBackground() => new MapRender();

        public virtual MapCorridor CreateCorridor() => new MapCorridor();

        public abstract Room CreateNormal(int depth, Room prev);

        public virtual MapRender CreateOverlay() => new MapRender();

        public abstract Room CreateSpecial(int depth, Room prev);

        public abstract Room CreateStart(int depth);

        public abstract Room CreateTarget(int depth, Room prev);

        public virtual void Initialize()
        { }

        public virtual void InitializeRasterization(DungeonGraph graph)
        { }

        public void SetRandom(Random rand) => Rand = rand;

        protected static DungeonTile[,] ReadTemplate(Type templateType)
        {
            var templateName = templateType.Namespace + ".template.jm";
            var stream = templateType.Assembly.GetManifestResourceStream(templateName);

            using (var reader = new StreamReader(stream))
                return JsonMap.Load(reader.ReadToEnd());
        }
    }
}
