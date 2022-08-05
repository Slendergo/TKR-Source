using System;
using System.Collections.Generic;

namespace wServer.core.worlds.impl
{
    public sealed class WorldBranch
    {
        private readonly World Branch;
        private readonly Dictionary<int, World> Branches = new Dictionary<int, World>();
        private readonly List<World> ToRemove = new List<World>();

        public WorldBranch(World branch)
        {
            Branch = branch;
        }

        public void AddBranch(World world)
        {
            Branches.Add(world.Id, world);
        }

        public bool HasBranches() => Branches.Count > 0;

        public void Update(ref TickTime time)
        {
            foreach (var world in Branches.Values)
                if (world.Update(ref time))
                    ToRemove.Add(world);

            if (ToRemove.Count > 0)
            {
                foreach (var world in ToRemove)
                {
                    world.ParentWorld = null;
                    _ = Branch.GameServer.WorldManager.RemoveWorld(world);
                    _ = Branches.Remove(world.Id);
                }
                ToRemove.Clear();
            }
        }
    }

}
