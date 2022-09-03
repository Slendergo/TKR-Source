using System;
using System.Collections.Generic;

namespace wServer.core.worlds.impl
{
    public sealed class WorldBranch
    {
        private readonly World Branch;
        private readonly Dictionary<int, World> Branches = new Dictionary<int, World>();

        public WorldBranch(World branch)
        {
            Branch = branch;
        }

        public void AddBranch(World world)
        {
            Branches.Add(world.Id, world);
        }

        public void GetPlayerCount(ref int count)
        {
            count += Branch.Players.Values.Count;
            foreach (var branch in Branches.Values)
                branch.GetPlayerCount(ref count);
        }

        public bool HasBranches() => Branches.Count > 0;

        public void Update(ref TickTime time)
        {
            var toRemove = new List<World>();
            foreach (var world in Branches.Values)
                if (world.Update(ref time))
                    toRemove.Add(world);

            foreach (var world in toRemove)
            {
                world.ParentWorld = null;
                _ = Branch.GameServer.WorldManager.RemoveWorld(world);
                _ = Branches.Remove(world.Id);
            }
            toRemove.Clear();
        }
    }

}
