using common.resources;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using wServer.core.worlds;
using wServer.core.worlds.logic;

namespace wServer.core
{
    public sealed class WorldManager
    {
        private readonly Random _random;

        public CoreServerManager CoreServerManager;

        private int NextWorldId = 0;

        public WorldManager(CoreServerManager coreServerManager)
        {
            CoreServerManager = coreServerManager;
            _random = new Random();
        }

        public TickThreadSingle Dungeons { get; private set; }
        public TickThreadBatch NonEssentialWorlds { get; private set; }
        public PortalMonitor PortalMonitor { get; private set; }
        public TickThreadSingle Realms { get; private set; }
        public ConcurrentDictionary<int, World> Worlds { get; private set; }

        public World AddWorld(World world)
        {
            if (world.Manager != null)
                throw new InvalidOperationException("World already added.");

            world.Id = Interlocked.Increment(ref NextWorldId);
            Worlds[world.Id] = world;

            if (CoreServerManager.Initialized)
                OnWorldAdded(world);

            return world;
        }

        public World GetRandomGameWorld()
        {
            var realms = GetRealms().ToArray();
            return realms.Length == 0
                ? Worlds[World.Nexus]
                : realms[_random.Next(0, realms.Length)];
        }

        public World GetWorld(int id)
        {
            if (!Worlds.TryGetValue(id, out World ret))
                return null;

            if (ret.Id == 0)
                return null;

            return ret;
        }

        public IEnumerable<World> GetWorlds()
        {
            foreach (var world in Worlds)
                if (world.Value != null && !world.Value.Deleted)
                    yield return world.Value;
        }

        public IEnumerable<Realm> GetRealms()
        {
            foreach (var world in Worlds)
                if (world.Value != null && world.Value.IsRealm)
                {
                    var realm = world.Value as Realm;
                    if (!realm.Closed)
                        yield return realm;
                }
        }

        public void WorldsBroadcastAsParallel(Action<World> action)
        {
            var worlds = GetWorlds();
            worlds.AsParallel().Select(_ =>
            {
                action.Invoke(_);
                return _;
            }).ToArray();
        }

        public void Initialize()
        {
            Worlds = new ConcurrentDictionary<int, World>();

            Dungeons = new TickThreadSingle(this);
            Realms = new TickThreadSingle(this);
            NonEssentialWorlds = new TickThreadBatch(this);

            foreach (var wData in CoreServerManager.Resources.Worlds.Data.Values)
                if (wData.id < 0)
                    AddWorld(wData);

            AddWorld("Realm");
            AddWorld("Realm");
            AddWorld("Poseidon's Domain");

            // add portal monitor to nexus and initialize worlds
            if (Worlds.ContainsKey(World.Nexus))
                PortalMonitor = new PortalMonitor(CoreServerManager, Worlds[World.Nexus]);

            var worlds = GetWorlds();
            foreach (var world in worlds)
                OnWorldAdded(world);
        }

        public bool RemoveWorld(World world)
        {
            if (world.Manager == null)
                throw new InvalidOperationException("World is not added.");

            if (Worlds.TryRemove(world.Id, out world))
            {
                OnWorldRemoved(world);
                return true;
            }

            return false;
        }

        public void Shutdown()
        {
            Dungeons.Stop();
            Realms.Stop();
            NonEssentialWorlds.Stop();
        }

        private void AddWorld(string name, bool actAsNexus = false) => AddWorld(CoreServerManager.Resources.Worlds.Data[name], actAsNexus);

        private void AddWorld(ProtoWorld proto, bool actAsNexus = false)
        {
            int id;
            if (actAsNexus)
                id = World.Nexus;
            else
                id = proto.id < 0 ? proto.id : Interlocked.Increment(ref NextWorldId);

            DynamicWorld.TryGetWorld(proto, null, out World world);

            if (world != null)
            {
                AddWorld(id, world);
                return;
            }

            AddWorld(id, new World(proto));
        }

        private void AddWorld(int id, World world)
        {
            if (world.Manager != null)
                throw new InvalidOperationException("World already added.");

            world.Id = id;
            Worlds[id] = world;

            if (CoreServerManager.Initialized)
                OnWorldAdded(world);
        }

        private void OnWorldAdded(World world)
        {
            world.Manager = CoreServerManager;

            if (world.IsDungeon)
                Dungeons.Attach(world);
            else if (world.IsRealm)
                Realms.Attach(world);
            else
                NonEssentialWorlds.Attach(world);
        }

        private void OnWorldRemoved(World world)
        {
            if (world.IsDungeon)
                Dungeons.Detatch(world);
            else if (world.IsRealm)
                Realms.Detatch(world);
            else
                NonEssentialWorlds.Detatch(world);

            PortalMonitor.RemovePortal(world.Id);
        }
    }
}
