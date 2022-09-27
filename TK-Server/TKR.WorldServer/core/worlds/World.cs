using Pipelines.Sockets.Unofficial.Arenas;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using TKR.Shared.database;
using TKR.Shared.resources;
using TKR.WorldServer.core.miscfile.census;
using TKR.WorldServer.core.miscfile.structures;
using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.core.miscfile.world;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.core.objects.containers;
using TKR.WorldServer.core.objects.player;
using TKR.WorldServer.core.terrain;
using TKR.WorldServer.core.worlds.impl;
using TKR.WorldServer.core.worlds.logic;
using TKR.WorldServer.memory;
using TKR.WorldServer.networking;
using TKR.WorldServer.networking.packets.outgoing;
using TKR.WorldServer.utils;

namespace TKR.WorldServer.core.worlds
{
    public class World
    {
        public const int NEXUS_ID = -2;
        public const int TEST_ID = -6;

        private static int NextEntityId;

        public readonly Random Random = new Random();

        public int Id { get; }
        public string IdName { get; set; }
        public string DisplayName { get; set; }
        public WorldResourceInstanceType InstanceType { get; private set; }
        public bool Persist { get; private set; }
        public int MaxPlayers { get; protected set; }
        public bool CreateInstance { get; private set; }

        public bool IsRealm { get; set; }
        public bool AllowTeleport { get; protected set; }
        public int Background { get; protected set; }
        public byte Blocking { get; protected set; }
        public string Music { get; set; }
        public int Difficulty { get; protected set; }
        public bool Deleted { get; protected set; }
        public bool DisableShooting { get; set; }
        public bool DisableAbilities { get; set; }
        private long Lifetime { get; set; }

        public readonly Wmap Map;
        public readonly GameServer GameServer;
        public CollisionMap<Entity> EnemiesCollision { get; private set; }
        public CollisionMap<Entity> PlayersCollision { get; private set; }

        public bool ShowDisplays { get; protected set; }

        public ConcurrentDictionary<int, Player> Players { get; private set; } = new ConcurrentDictionary<int, Player>();
        public ConcurrentDictionary<int, Enemy> Enemies { get; private set; } = new ConcurrentDictionary<int, Enemy>();
        public ConcurrentDictionary<int, Enemy> Quests { get; private set; } = new ConcurrentDictionary<int, Enemy>();
        public ConcurrentDictionary<int, StaticObject> StaticObjects { get; private set; } = new ConcurrentDictionary<int, StaticObject>();
        public ConcurrentDictionary<int, Container> Containers { get; private set; } = new ConcurrentDictionary<int, Container>();
        public ConcurrentDictionary<int, Portal> Portals { get; private set; } = new ConcurrentDictionary<int, Portal>();
        public ConcurrentDictionary<int, Pet> Pets { get; private set; } = new ConcurrentDictionary<int, Pet>();
        public Dictionary<int, Dictionary<int, Projectile>> Projectiles { get; private set; } = new Dictionary<int, Dictionary<int, Projectile>>();
        private readonly List<WorldTimer> Timers = new List<WorldTimer>();

        public ObjectPools ObjectPools { get; private set; }

        public WorldBranch WorldBranch { get; private set; }
        public World ParentWorld { get; set; }

        public World(GameServer gameServer, int id, WorldResource resource, World parent = null)
        {
            GameServer = gameServer;
            Map = new Wmap(this);
            ObjectPools = new ObjectPools(this);

            Id = id;
            IdName = resource.DisplayName;
            DisplayName = resource.DisplayName;
            Difficulty = resource.Difficulty;
            Background = resource.Background;
            MaxPlayers = resource.Capacity;
            InstanceType = resource.Instance;
            Persist = resource.Persists;
            ShowDisplays = Id == -2 || resource.ShowDisplays;
            Blocking = resource.VisibilityType;
            AllowTeleport = resource.AllowTeleport;
            DisableShooting = resource.DisableShooting;
            DisableAbilities = resource.DisableAbilities;
            CreateInstance = resource.CreateInstance;

            IsRealm = false;

            if (resource.Music.Count > 0)
                Music = resource.Music[Random.Next(0, resource.Music.Count)];
            else
                Music = "sorc";

            WorldBranch = new WorldBranch(this);
            ParentWorld = parent;
        }

        public virtual bool AllowedAccess(Client client) => true;

        public void Broadcast(OutgoingMessage outgoingMessage)
        {
            foreach (var player in Players.Values)
                player.Client.SendPacket(outgoingMessage);
        }

        public void BroadcastIfVisible(OutgoingMessage outgoingMessage, ref Position worldPosData)
        {
            foreach (var player in Players.Values)
                if (player.SqDistTo(ref worldPosData) < PlayerUpdate.VISIBILITY_RADIUS_SQR)
                    player.Client.SendPacket(outgoingMessage);
        }

        public void BroadcastIfVisible(OutgoingMessage outgoingMessage, Entity host)
        {
            foreach (var player in Players.Values)
                if (player.SqDistTo(host) < PlayerUpdate.VISIBILITY_RADIUS_SQR)
                    player.Client.SendPacket(outgoingMessage);
        }

        public void BroadcastIfVisibleExclude(OutgoingMessage outgoingMessage, Entity broadcaster, Entity exclude)
        {
            foreach (var player in Players.Values)
                if (player.Id != exclude.Id && player.SqDistTo(broadcaster) <= PlayerUpdate.VISIBILITY_RADIUS_SQR)
                    player.Client.SendPacket(outgoingMessage);
        }

        public void BroadcastToPlayer(OutgoingMessage outgoingMessage, int playerId)
        {
            foreach (var player in Players.Values)
                if (player.Id == playerId)
                {
                    player.Client.SendPacket(outgoingMessage);
                    break;
                }
        }

        public void BroadcastToPlayers(OutgoingMessage outgoingMessage, List<int> playerIds)
        {
            foreach (var player in Players.Values)
                if (playerIds.Contains(player.Id))
                    player.Client.SendPacket(outgoingMessage);
        }

        public void ChatReceived(Player player, string text)
        {
            foreach (var en in Enemies)
                en.Value.OnChatTextReceived(player, text);
            foreach (var en in StaticObjects)
                en.Value.OnChatTextReceived(player, text);
        }

        public void AddProjectile(Projectile projectile)
        {
            if (!Projectiles.ContainsKey(projectile.Host.Id))
                Projectiles.Add(projectile.Host.Id, new Dictionary<int, Projectile>());
            Projectiles[projectile.Host.Id][projectile.ProjectileId] = projectile;
        }

        public Projectile GetProjectile(int objectId, int bulletId)
        {
            if (Projectiles.TryGetValue(objectId, out var projectiles))
                if (projectiles.TryGetValue(bulletId, out var ret))
                    return ret;
            return null;
        }

        public void RemoveProjectile(Projectile projectile)
        {
            if (Projectiles.ContainsKey(projectile.Host.Id))
                Projectiles[projectile.Host.Id].Remove(projectile.ProjectileId);
            ObjectPools.Projectiles.Return(projectile);
        }

        public virtual int EnterWorld(Entity entity)
        {
            entity.Id = GetNextEntityId();

            if (entity is Player)
            {
                Players.TryAdd(entity.Id, entity as Player);
                PlayersCollision.Insert(entity);
            }
            else if (entity is Enemy)
            {
                Enemies.TryAdd(entity.Id, entity as Enemy);
                EnemiesCollision.Insert(entity);
                if (entity.ObjectDesc.Quest)
                    Quests.TryAdd(entity.Id, entity as Enemy);
            }
            else if (entity is Container)
                Containers.TryAdd(entity.Id, entity as Container);
            else if (entity is Portal)
                Portals.TryAdd(entity.Id, entity as Portal);
            else if (entity is StaticObject)
            {
                StaticObjects.TryAdd(entity.Id, entity as StaticObject);
                if (entity is Decoy)
                    PlayersCollision.Insert(entity);
                else
                    EnemiesCollision.Insert(entity);
            }
            else if (entity is Pet)
            {
                Pets.TryAdd(entity.Id, entity as Pet);
                PlayersCollision.Insert(entity);
            }
            entity.Init(this);
            return entity.Id;
        }

        public string GetDisplayName() => DisplayName != null && DisplayName.Length > 0 ? DisplayName : IdName;

        public void GetPlayerCount(ref int count) => WorldBranch.GetPlayerCount(ref count);

        public Entity GetEntity(int id)
        {
            if (Players.TryGetValue(id, out var ret1))
                return ret1;

            if (Enemies.TryGetValue(id, out var ret2))
                return ret2;

            if (StaticObjects.TryGetValue(id, out var ret3))
                return ret3;

            if (Containers.TryGetValue(id, out var ret4))
                return ret4;

            if (Portals.TryGetValue(id, out var ret5))
                return ret5;

            return null;
        }

        public int GetNextEntityId() => Interlocked.Increment(ref NextEntityId);

        public IEnumerable<Player> GetPlayers() => Players.Values;

        public Position? GetRegionPosition(TileRegion region)
        {
            if (Map.Regions.All(t => t.Value != region))
                return null;

            var reg = Map.Regions.Single(t => t.Value == region);

            return new Position() { X = reg.Key.X, Y = reg.Key.Y };
        }

        public virtual KeyValuePair<IntPoint, TileRegion>[] GetSpawnPoints() => Map.Regions.Where(t => t.Value == TileRegion.Spawn).ToArray();
        public virtual KeyValuePair<IntPoint, TileRegion>[] GetRegionPoints(TileRegion region) => Map.Regions.Where(t => t.Value == region).ToArray();

        public Player GetUniqueNamedPlayer(string name)
        {
            if (Database.GuestNames.Contains(name))
                return null;

            foreach (var i in Players.Values)
                if (i.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (!i.NameChosen && !(this is TestWorld))
                        GameServer.Database.ReloadAccount(i.Client.Account);

                    if (i.Client.Account.NameChosen)
                        return i;
                    break;
                }

            return null;
        }

        public bool IsPassable(double x, double y, bool spawning = false)
        {
            var x_ = (int)x;
            var y_ = (int)y;

            if (!Map.Contains(x_, y_))
                return false;

            var tile = Map[x_, y_];

            var tileDesc = GameServer.Resources.GameData.Tiles[tile.TileId];
            if (tileDesc.NoWalk)
                return false;

            if (tile.ObjType != 0 && tile.ObjDesc != null)
                if (tile.ObjDesc.FullOccupy || tile.ObjDesc.EnemyOccupySquare || spawning && tile.ObjDesc.OccupySquare)
                    return false;

            return true;
        }

        public bool IsPlayersMax() => Players.Count >= MaxPlayers;

        public virtual void LeaveWorld(Entity entity)
        {
            if (entity is Player)
            {
                Players.TryRemove(entity.Id, out Player dummy);
                PlayersCollision.Remove(entity);

                // if in trade, cancel it...
                if (dummy != null && dummy.tradeTarget != null)
                    dummy.CancelTrade();

                if (dummy != null && dummy.Pet != null)
                    LeaveWorld(dummy.Pet);
            }
            else if (entity is Enemy)
            {
                Enemies.TryRemove(entity.Id, out Enemy dummy);
                EnemiesCollision.Remove(entity);
                if (entity.ObjectDesc.Quest)
                    Quests.TryRemove(entity.Id, out dummy);
            }
            else if (entity is Container)
                Containers.TryRemove(entity.Id, out Container dummy);
            else if (entity is Portal)
                Portals.TryRemove(entity.Id, out _);
            else if (entity is StaticObject)
            {
                StaticObjects.TryRemove(entity.Id, out StaticObject dummy);

                if (entity is Decoy)
                    PlayersCollision.Remove(entity);
                else
                    EnemiesCollision.Remove(entity);
            }
            else if (entity is Pet)
            {
                Pets.TryRemove(entity.Id, out Pet dummy);
                PlayersCollision.Remove(entity);
            }

            entity.Destroy();
        }

        public void ForeachPlayer(Action<Player> action)
        {
            foreach (var player in Players.Values)
                action?.Invoke(player);
        }

        public void ObjsWithin(Entity host, double radius, List<Entity> enemies)
        {
            foreach (var enemy in EnemiesCollision.HitTest(host.X, host.Y, radius))
            {
                if (enemy.SqDistTo(host.X, host.Y) >= radius * radius)
                    continue;
                enemies.Add(enemy);
            }
        }

        public void WorldAnnouncement(string msg)
        {
            var announcement = string.Concat("<ANNOUNCMENT> ", msg);
            foreach (var player in Players.Values)
                player.SendInfo(announcement);
        }

        protected void FromWorldMap(Stream dat)
        {
            Interlocked.Add(ref NextEntityId, Map.Load(dat, NextEntityId));
            InitMap();
        }

        public bool LoadMapFromData(WorldResource worldResource)
        {
            var data = GameServer.Resources.GameData.GetWorldData(worldResource.MapJM[Random.Next(0, worldResource.MapJM.Count)]);
            if (data == null)
                return false;
            FromWorldMap(new MemoryStream(data));
            return true;
        }

        public virtual void Init()
        {
        }

        private void InitMap()
        {
            var w = Map.Width;
            var h = Map.Height;

            EnemiesCollision = new CollisionMap<Entity>(0, w, h);
            PlayersCollision = new CollisionMap<Entity>(1, w, h);

            foreach (var i in Map.InstantiateEntities(GameServer))
                _ = EnterWorld(i);
        }

        public Entity FindPlayerTarget(Entity host)
        {
            Entity closestObj = null;
            var closestSqDist = double.MaxValue;
            foreach (var obj in host.World.Players.Values)
            {
                if (!(obj as IPlayer).IsVisibleToEnemy())
                    continue;

                var sqDist = obj.SqDistTo(host);
                if (sqDist < closestSqDist)
                {
                    closestSqDist = sqDist;
                    closestObj = obj;
                }
            }
            return closestObj;
        }

        public void ProcessPlayerIO(ref TickTime time)
        {
            foreach (var player in Players.Values)
                player.HandleIO(ref time);
            WorldBranch.HandleIO(ref time);
        }

        public bool Update(ref TickTime time)
        {
            try
            {
                Lifetime += time.ElapsedMsDelta;

                WorldBranch.Update(ref time);

                if (IsPastLifetime(ref time))
                    return true;

                UpdateLogic(ref time);
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine($"World Tick: {e}");
                return false;
            }
        }

        public WorldTimer StartNewTimer(int timeMs, Action<World, TickTime> callback)
        {
            var ret = new WorldTimer(timeMs, callback);
            Timers.Add(ret);
            return ret;
        }

        public WorldTimer StartNewTimer(int timeMs, Func<World, TickTime, bool> callback)
        {
            var ret = new WorldTimer(timeMs, callback);
            Timers.Add(ret);
            return ret;
        }

        protected virtual void UpdateLogic(ref TickTime time)
        {
            foreach (var player in Players.Values)
                player.Tick(ref time);

            foreach (var stat in StaticObjects.Values)
                stat.Tick(ref time);

            foreach (var container in Portals.Values)
                container.Tick(ref time);

            foreach (var container in Containers.Values)
                container.Tick(ref time);

            foreach (var pet in Pets.Values)
                pet.Tick(ref time);

            if (EnemiesCollision != null)
            {
                foreach (var i in EnemiesCollision.GetActiveChunks(PlayersCollision))
                    i.Tick(ref time);

                //foreach (var i in StaticObjects.Where(x => x.Value != null && x.Value is Decoy))
                //    i.Value.Tick(time);
            }
            else
            {
                foreach (var i in Enemies)
                    i.Value.Tick(ref time);

                //foreach (var i in StaticObjects)
                //    i.Value.Tick(time);
            }

            for (var i = Timers.Count - 1; i >= 0; i--)
                try
                {
                    if (Timers[i].Tick(this, ref time))
                        Timers.RemoveAt(i);
                }
                catch (Exception e)
                {
                    StaticLogger.Instance.Error($"{e.Message}\n{e.StackTrace}");
                    Timers.RemoveAt(i);
                }

            var projectilesToRemove = new List<Projectile>();
            foreach (var k in Projectiles.Values)
                foreach (var projectile in k.Values)
                    if (!projectile.Tick(ref time))
                        projectilesToRemove.Add(projectile);

            foreach (var projectile in projectilesToRemove)
                RemoveProjectile(projectile);
        }

        public void FlagForClose()
        {
            ForceLifetimeExpire = true;
        }

        private bool ForceLifetimeExpire = false;

        private bool IsPastLifetime(ref TickTime time)
        {
            if (WorldBranch.HasBranches())
                return false;

            if (Players.Count > 0)
                return false;

            if (ForceLifetimeExpire)
                return true;

            if (Persist)
                return false;

            if (Deleted)
                return false;

            if (Lifetime >= 60000)
                return true;
            return false;
        }

        public void OnRemovedFromWorldManager()
        {
            Map.Clear();
        }
    }
}
