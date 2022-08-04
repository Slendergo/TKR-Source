using common;
using common.resources;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        private readonly ConcurrentDictionary<int, TickThreadSingle> RealmThreads = new ConcurrentDictionary<int, TickThreadSingle>();

        public ConcurrentDictionary<int, int> WorldMapId { get; set; } = new ConcurrentDictionary<int, int>();

        public NexusWorld Nexus => (Worlds[-2] as NexusWorld);
        public TestWorld Test => (Worlds[-6] as TestWorld);

        private readonly ConcurrentDictionary<int, World> Worlds = new ConcurrentDictionary<int, World>();
        private readonly ConcurrentDictionary<int, VaultWorld> Vaults = new ConcurrentDictionary<int, VaultWorld>();
        private readonly ConcurrentDictionary<int, World> Guilds = new ConcurrentDictionary<int, World>();
        private readonly ConcurrentDictionary<int, int> WorldToGuildId = new ConcurrentDictionary<int, int>();

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
        }

        public void SetPreviousWorld(int accountId, int worldId)
        {
            WorldMapId[accountId] = worldId;
        }

        public World GetPreviousRealmWorld(int accountId) => WorldMapId.TryGetValue(accountId, out var id) ? GetWorld(id) : null;

        public void Initialize()
        {
            var nexus = CreateNewWorld("Nexus", -2, null);
            CreateNewTest();
            NexusThread.Attach(nexus);

            // todo async creation system
            Nexus.PortalMonitor.CreateNewRealm();
        }

        public Task<RealmWorld> CreateNewRealmAsync()
        {
            return Task<RealmWorld>.Factory.StartNew(() =>
            {
                using (var t = new TimedProfiler("CreateNewRealm()"))
                {
                    var worldResource = CoreServerManager.Resources.GameData.GetWorld("Realm of the Mad God");
                    if (worldResource == null)
                        return null;

                    var nextId = Interlocked.Increment(ref NextWorldId);

                    var world = new RealmWorld(nextId, worldResource);
                    world.Manager = CoreServerManager; // todo add to ctor
                    var success = world.LoadMapFromData(worldResource);
                    if (!success)
                        return null;

                    world.Init();
                    _ = Worlds.TryAdd(world.Id, world);

                    var thread = new TickThreadSingle(this);
                    thread.Attach(world);
                    RealmThreads.TryAdd(world.Id, thread);
                    return world;
                }
            });
        }

        public World CreateNewWorld(string dungeonName, int? id = null, World parent = null)
        {
            //Console.WriteLine($"CreateNewWorld({dungeonName}, {id ?? null}, {(parent == null ? "null" : parent.IdName)}");
            var worldResource = CoreServerManager.Resources.GameData.GetWorld(dungeonName);
            if (worldResource == null)
                return null;

            var nextId = id ?? Interlocked.Increment(ref NextWorldId);

            World world;
            switch (worldResource.Instance)
            {
                case WorldResourceInstanceType.Nexus:
                    world = new NexusWorld(nextId, worldResource);
                    break;
                case WorldResourceInstanceType.Vault:
                    world = new VaultWorld(nextId, worldResource);
                    break;
                default:
                    world = new World(nextId, worldResource);
                    break;
            }

            world.Manager = CoreServerManager; // todo add to ctor
            var success = world.LoadMapFromData(worldResource);
            if (!success)
                return null;
            world.Init();
            Worlds.TryAdd(world.Id, world);
            parent?.WorldBranch.AddBranch(world);
            return world;
        }

        public void CreateNewTest()
        {
            Console.WriteLine($"CreateNewTest");

            var worldResource = CoreServerManager.Resources.GameData.GetWorld("Testing");
            if (worldResource == null)
            {
                Console.WriteLine("Testing couldnt be made");
                return;
            }
            var world = new TestWorld(-6, worldResource);

            world.Manager = CoreServerManager; // todo add to ctor
            
            world.Init();
            Worlds[world.Id] = world;
            Nexus.WorldBranch.AddBranch(world);
        }

        public void AddVaultInstance(int accountId, VaultWorld world)
        {
            Vaults.TryAdd(accountId, world);
        }

        public void AddGuildInstance(int guildId, World world)
        {
            Guilds.TryAdd(guildId, world);
            WorldToGuildId.TryAdd(world.Id, guildId);
        }

        public World GetWorld(int id) => Worlds.TryGetValue(id, out World ret) ? ret : null;
        public VaultWorld GetVault(int accountId) => Vaults.TryGetValue(accountId, out var ret) ? ret : null;
        public World GetGuild(int guildId) => Guilds.TryGetValue(guildId, out var ret) ? ret : null;
        public int GetGuildId(int gameId) => WorldToGuildId.TryGetValue(gameId, out var ret) ? ret : -1;

        public bool RemoveWorld(World world)
        {
            if(world is RealmWorld)
                RealmThreads.TryRemove(world.Id, out _);

            if (Worlds.TryRemove(world.Id, out _))
            {
                switch (world.InstanceType)
                {
                    case WorldResourceInstanceType.Vault:
                        _ = Vaults.TryRemove((world as VaultWorld).AccountId, out _);
                        break;
                    case WorldResourceInstanceType.Guild:
                        var guildId = GetGuildId(world.Id);
                        _ = Guilds.TryRemove(guildId, out _);
                        break;
                }
                return true;
            }
            return false;
        }

        public void Shutdown()
        {
            foreach (var realm in RealmThreads.Values)
                realm.Stop();
            NexusThread.Stop();
        }
    }
}
