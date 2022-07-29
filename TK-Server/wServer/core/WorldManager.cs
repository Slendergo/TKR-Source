using common;
using common.resources;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using wServer.core.worlds;
using wServer.core.worlds.logic;

namespace wServer.core
{
    public sealed class WorldManager
    {
        public CoreServerManager CoreServerManager;

        private int NextWorldId = 0;

        public TickThreadSingle NexusThread { get; private set; }
        public List<TickThreadSingle> RealmThreads { get; private set; }

        public Nexus Nexus => (Worlds[-2] as Nexus);

        private readonly Dictionary<int, World> Worlds = new Dictionary<int, World>();

        public IEnumerable<World> GetWorlds() => Worlds.Values;

        public void WorldsBroadcastAsParallel(Action<World> action)
        {
            var worlds = GetWorlds();
            worlds.AsParallel().Select(_ =>
            {
                action.Invoke(_);
                return _;
            }).ToArray();
        }

        public WorldManager(CoreServerManager coreServerManager)
        {
            CoreServerManager = coreServerManager;
            NexusThread = new TickThreadSingle(this);
            RealmThreads = new List<TickThreadSingle>();
        }

        public void Initialize()
        {
            var nexus = CreateNewWorld("Nexus", -2, null);
            NexusThread.Attach(nexus);

            _ = CreateNewRealm();

            //(nexus as Nexus).RealmManager.CreateNewRealm();

            // add portal monitor to nexus and initialize worlds
        }

        public World CreateNewRealm()
        {
            Console.WriteLine("CreateNewRealm()");
            var worldResource = CoreServerManager.Resources.GameData.GetWorld("Realm of the Mad God");
            if (worldResource == null)
                return null;

            var nextId = Interlocked.Increment(ref NextWorldId);

            var world = new Realm(nextId, worldResource);
            world.Manager = CoreServerManager; // todo add to ctor
            var success = world.LoadMapFromData(worldResource);
            if (!success)
                return null;
            world.Init();
            Worlds.Add(world.Id, world);

            Nexus.PortalMonitor.AddPortal(world.Id);
            
            var thread = new TickThreadSingle(this);
            thread.Attach(world);
            RealmThreads.Add(thread);
            return world;
        }

        public World CreateNewWorld(string dungeonName, int? id = null, World parent = null)
        {
            Console.WriteLine($"CreateNewWorld({dungeonName}, {id ?? null}, {(parent == null ? "null" : parent.IdName)}");
            var worldResource = CoreServerManager.Resources.GameData.GetWorld(dungeonName);
            if (worldResource == null)
                return null;
            
            var nextId = id ?? Interlocked.Increment(ref NextWorldId);

            var world = id == -2 ? new Nexus(nextId, worldResource) : new World(nextId, worldResource);
            world.Manager = CoreServerManager; // todo add to ctor
            var success = world.LoadMapFromData(worldResource);
            if (!success)
                return null;
            world.Init();
            Worlds.Add(world.Id, world);
            parent?.WorldBranch.AddBranch(world);
            return world;
        }

        public World GetWorld(int id)
        {
            if (!Worlds.TryGetValue(id, out World ret))
                return null;
            return ret;
        }

        public bool RemoveWorld(World world)
        {
            if (Worlds.Remove(world.Id))
            {
                if(world is Realm)
                {
                    // remove the thread thingy here and recycle mabye
                }
                return true;
            }
            return false;
        }

        public void Shutdown()
        {
            foreach (var realm in RealmThreads)
                realm.Stop();
            NexusThread.Stop();
        }
    }
}
