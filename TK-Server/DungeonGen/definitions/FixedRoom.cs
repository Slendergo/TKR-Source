using System;

namespace dungeonGen.definitions
{
    public abstract class FixedRoom : Room
    {
        public abstract Tuple<Direction, int>[] ConnectionPoints { get; }

        public override Range NumBranches => new Range(1, ConnectionPoints.Length);
    }
}
