using Nancy.Validation;
using NLog.LayoutRenderers;
using Org.BouncyCastle.Asn1.X509.Qualified;
using Pipelines.Sockets.Unofficial.Arenas;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Threading;
using TKR.Shared.database;
using TKR.Shared.resources;
using TKR.WorldServer.core.miscfile.census;
using TKR.WorldServer.core.miscfile.structures;
using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.core.miscfile.world;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.core.objects.containers;
using TKR.WorldServer.core.objects.@new;
using TKR.WorldServer.core.objects.player;
using TKR.WorldServer.core.terrain;
using TKR.WorldServer.core.worlds.impl;
using TKR.WorldServer.core.worlds.logic;
using TKR.WorldServer.networking;
using TKR.WorldServer.networking.packets.outgoing;
using TKR.WorldServer.utils;

namespace TKR.WorldServer.core.worlds
{
    public sealed class SpatialStorage<T> where T : EntityBase
    {
        private const int SCALE_FACTOR = 16;

        private ConcurrentDictionary<int, ConcurrentDictionary<int, T>> Bucket = new ConcurrentDictionary<int, ConcurrentDictionary<int, T>>();

        public void Insert(T go)
        {
            var hash = HashScaleFactor(go.X, go.Y);
            var bucket = Bucket.GetOrAdd(hash, _ => new ConcurrentDictionary<int, T>());
            bucket[go.Id] = go;
        }

        public void Move(T go)
        {
            var objectId = go.Id;

            var hash = HashScaleFactor(go.PrevX, go.PrevY);
            var bucket = Bucket.GetOrAdd(hash, _ => new ConcurrentDictionary<int, T>());
            _ = bucket.TryRemove(objectId, out var dummy);

            var newHash = HashScaleFactor(go.X, go.Y);
            bucket = Bucket.GetOrAdd(newHash, _ => new ConcurrentDictionary<int, T>());
            bucket[objectId] = go;
        }

        public void Remove(T go)
        {
            var hash = HashScaleFactor(go.X, go.Y);
            var bucket = Bucket[hash];
            _ = bucket.TryRemove(go.Id, out _);
        }

        public List<T> Query(float x, float y)
        {
            var ret = new List<T>();
            var hash = HashScaleFactor(x, y);
            if (Bucket.TryGetValue(hash, out var bucket))
                ret.AddRange(bucket.Values);
            return ret;
        }

        public List<T> Query(float _x, float _y, float radius)
        {
            var ret = new List<T>();
            var xl = (int)(_x - radius) / SCALE_FACTOR;
            var xh = (int)(_x + radius) / SCALE_FACTOR;
            var yl = (int)(_y - radius) / SCALE_FACTOR;
            var yh = (int)(_y + radius) / SCALE_FACTOR;
            for (var x = xl; x <= xh; x++)
                for (var y = yl; y <= yh; y++)
                {
                    var hash = Hash(x, y);
                    if (Bucket.TryGetValue(hash, out var bucket))
                        foreach (var obj in bucket.Values)
                            if (obj.SqDistTo(_x, _y) < radius * radius)
                                ret.Add(obj);
                }
            return ret;
        }

        public void Query(List<EntityBase> ret, float _x, float _y, float radius)
        {
            var xl = (int)(_x - radius) / SCALE_FACTOR;
            var xh = (int)(_x + radius) / SCALE_FACTOR;
            var yl = (int)(_y - radius) / SCALE_FACTOR;
            var yh = (int)(_y + radius) / SCALE_FACTOR;
            for (var x = xl; x <= xh; x++)
                for (var y = yl; y <= yh; y++)
                {
                    var hash = Hash(x, y);
                    if (Bucket.TryGetValue(hash, out var bucket))
                        foreach (var obj in bucket.Values)
                            if (obj.SqDistTo(_x, _y) < radius * radius)
                                ret.Add(obj);
                }
        }

        private static int HashScaleFactor(double x, double y)
        {
            var dx = (int)x / SCALE_FACTOR;
            var dy = (int)y / SCALE_FACTOR;
            return Hash(dx, dy);
        }

        private static int Hash(int x, int y) => (x << 16) | y;

        public void Dispose()
        {
            foreach (var key in Bucket.Keys)
            {
                Bucket[key].Clear();
                Bucket[key] = null;
            }
            Bucket.Clear();
        }
    }

    public sealed class Visibility
    {
        private bool[,] Blocking { get; set; }
        private bool[,] Static { get; set; }
        private bool[,] Walkable { get; set; }

        public Visibility(int width, int height)
        {
            Blocking = new bool[width, height];
            Static = new bool[width, height];
            Walkable = new bool[width, height];
        }

        public void SetStatic(int x, int y, bool isStatic) => Static[x, y] = isStatic;
        public bool IsStatic(int x, int y)
        {
            x = Math.Clamp(x, 0, Blocking.GetLength(0) - 1);
            y = Math.Clamp(y, 0, Blocking.GetLength(1) - 1);
            return Static[x, y];
        }

        public void SetWalkable(int x, int y, bool walkable) => Walkable[x, y] = walkable;
        public bool IsWalkable(int x, int y)
        {
            x = Math.Clamp(x, 0, Blocking.GetLength(0) - 1);
            y = Math.Clamp(y, 0, Blocking.GetLength(1) - 1);
            return Walkable[x, y];
        }

        public void SetBlocking(int x, int y, bool blocking) => Blocking[x, y] = blocking;
        public bool IsBlocking(int x, int y)
        {
            x = Math.Clamp(x, 0, Blocking.GetLength(0) - 1);
            y = Math.Clamp(y, 0, Blocking.GetLength(1) - 1);
            return Blocking[x, y];
        }

        public void Invalidate(int x, int y)
        {
            Blocking[x, y] = false;
            Static[x, y] = false;
            Walkable[x, y] = false;
        }

        public bool CanSee(double startX, double startY, double endX, double endY)
        {
            var x = (int)startX;
            var y = (int)startY;

            var w = (int)endX - (int)startX;
            var h = (int)endY - (int)startY;

            var dx1 = 0;
            var dy1 = 0;
            var dx2 = 0;
            var dy2 = 0;
            if (w < 0)
                dx1 = -1;
            else if (w > 0)
                dx1 = 1;
            if (h < 0)
                dy1 = -1;
            else if (h > 0)
                dy1 = 1;
            if (w < 0)
                dx2 = -1;
            else if (w > 0)
                dx2 = 1;

            var longest = Math.Abs(w);
            var shortest = Math.Abs(h);
            if (!(longest > shortest))
            {
                longest = Math.Abs(h);
                shortest = Math.Abs(w);
                if (h < 0)
                    dy2 = -1;
                else if (h > 0)
                    dy2 = 1;
                dx2 = 0;
            }

            var numerator = longest >> 1;
            for (var i = 0; i <= longest; i++)
            {
                if (Blocking[x, y])
                    return false;

                numerator += shortest;
                if (!(numerator < longest))
                {
                    numerator -= longest;
                    x += dx1;
                    y += dy1;
                }
                else
                {
                    x += dx2;
                    y += dy2;
                }
            }

            return true;
        }

        public static void DrawLine(int x, int y, int x2, int y2, Func<int, int, bool> func)
        {
            var w = x2 - x;
            var h = y2 - y;
            var dx1 = 0;
            var dy1 = 0;
            var dx2 = 0;
            var dy2 = 0;
            if (w < 0)
                dx1 = -1;
            else if (w > 0)
                dx1 = 1;
            if (h < 0)
                dy1 = -1;
            else if (h > 0)
                dy1 = 1;
            if (w < 0)
                dx2 = -1;
            else if (w > 0)
                dx2 = 1;

            var longest = Math.Abs(w);
            var shortest = Math.Abs(h);
            if (!(longest > shortest))
            {
                longest = Math.Abs(h);
                shortest = Math.Abs(w);
                if (h < 0)
                    dy2 = -1;
                else if (h > 0)
                    dy2 = 1;
                dx2 = 0;
            }

            var numerator = longest >> 1;
            for (var i = 0; i <= longest; i++)
            {
                if (func(x, y))
                    break;

                numerator += shortest;
                if (!(numerator < longest))
                {
                    numerator -= longest;
                    x += dx1;
                    y += dy1;
                }
                else
                {
                    x += dx2;
                    y += dy2;
                }
            }
        }
    }

    public sealed class Census
    {
        private readonly SpatialStorage<NewPlayer> PlayerStorage = new SpatialStorage<NewPlayer>();
        private readonly SpatialStorage<NewEnemy> EnemyStorage = new SpatialStorage<NewEnemy>();
        private readonly SpatialStorage<NewPortal> PortalStorage = new SpatialStorage<NewPortal>();
        private readonly SpatialStorage<NewContainer> ContainerStorage = new SpatialStorage<NewContainer>();
        private readonly SpatialStorage<NewStatic> StaticStorage = new SpatialStorage<NewStatic>();

        private readonly Dictionary<int, NewPlayer> Players = new Dictionary<int, NewPlayer>();
        private readonly Dictionary<int, NewEnemy> Enemies = new Dictionary<int, NewEnemy>();
        private readonly Dictionary<int, NewPortal> Portals = new Dictionary<int, NewPortal>();
        private readonly Dictionary<int, NewContainer> Containers = new Dictionary<int, NewContainer>();
        private readonly Dictionary<int, NewStatic> Statics = new Dictionary<int, NewStatic>();

        private readonly Dictionary<int, EntityBase> EntitiesToAdd = new Dictionary<int, EntityBase>();
        private readonly Dictionary<int, EntityBase> EntitiesToRemove = new Dictionary<int, EntityBase>();
        private readonly Dictionary<int, EntityBase> DeadEntities = new Dictionary<int, EntityBase>();

        private readonly World World;

        public Census(World world) => World = world;

        public NewPlayer FindPlayer(int objectId) => Players.TryGetValue(objectId, out var ret) ? ret : null;
        public NewEnemy FindEnemy(int objectId) => Enemies.TryGetValue(objectId, out var ret) ? ret : null;
        public NewPortal FindPortal(int objectId) => Portals.TryGetValue(objectId, out var ret) ? ret : null;
        public NewContainer FindContainer(int objectId) => Containers.TryGetValue(objectId, out var ret) ? ret : null;
        public NewStatic FindStatic(int objectId) => Statics.TryGetValue(objectId, out var ret) ? ret : null;

        public bool IsAlive(int objectId)
        {
            if (DeadEntities.TryGetValue(objectId, out var _))
                return false;

            if (EntitiesToAdd.TryGetValue(objectId, out var _))
                return true;

            if (Players.TryGetValue(objectId, out var _))
                return true;

            if (Enemies.TryGetValue(objectId, out var _))
                return true;

            if (Portals.TryGetValue(objectId, out var _))
                return true;

            if (Containers.TryGetValue(objectId, out var _))
                return true;

            if (Statics.TryGetValue(objectId, out var _))
                return true;

            return false;
        }

        public IEnumerable<NewPlayer> GetPlayers() => Players.Values;

        public List<NewPlayer> PlayersWithinRadius(float x, float y, float radius) => PlayerStorage.Query(x, y, radius);
        public List<NewEnemy> EnemiesWithinRadius(float x, float y, float radius) => EnemyStorage.Query(x, y, radius);
        public List<NewPortal> PortalsWithinRadius(float x, float y, float radius) => PortalStorage.Query(x, y, radius);
        public List<NewContainer> ContainersWithinRadius(float x, float y, float radius) => ContainerStorage.Query(x, y, radius);
        public List<NewStatic> StaticsWithinRadius(float x, float y, float radius) => StaticStorage.Query(x, y, radius);

        public List<EntityBase> EntitiesWithinRadius(float x, float y, float radius)
        {
            var ret = new List<EntityBase>();
            PlayerStorage.Query(ret, x, y, radius);
            EnemyStorage.Query(ret, x, y, radius);
            PortalStorage.Query(ret, x, y, radius);
            ContainerStorage.Query(ret, x, y, radius);
            StaticStorage.Query(ret, x, y, radius);
            return ret;
        }

        public List<EntityBase> UpdateEntitiesWithinRadius(float x, float y, float radius)
        {
            var ret = new List<EntityBase>();
            EnemyStorage.Query(ret, x, y, radius);
            PortalStorage.Query(ret, x, y, radius);
            ContainerStorage.Query(ret, x, y, radius);
            StaticStorage.Query(ret, x, y, radius);
            return ret;
        }

        public void AddEntity(EntityBase entity) => EntitiesToAdd.Add(entity.Id, entity);

        private void Insert(EntityBase entity)
        {
            if (entity is NewPlayer player)
            {
                Players.Add(entity.Id, player);
                PlayerStorage.Insert(player);
                return;
            }

            if (entity is NewEnemy enemy)
            {
                Enemies.Add(entity.Id, enemy);
                EnemyStorage.Insert(enemy);
                return;
            }

            if (entity is NewPortal portal)
            {
                Portals.Add(entity.Id, portal);
                PortalStorage.Insert(portal);
                return;
            }

            if (entity is NewContainer container)
            {
                Containers.Add(entity.Id, container);
                ContainerStorage.Insert(container);
                return;
            }

            if (entity is NewStatic stat)
            {
                Statics.Add(entity.Id, stat);
                StaticStorage.Insert(stat);
                return;
            }

            throw new Exception($"Unknown Insert: {entity.ObjectDesc.ObjectId}");
        }

        public void Move(EntityBase entity)
        {
            if (entity is NewPlayer player)
            {
                PlayerStorage.Move(player);
                return;
            }

            if (entity is NewEnemy enemy)
            {
                EnemyStorage.Move(enemy);
                return;
            }

            if (entity is NewPortal portal)
            {
                PortalStorage.Move(portal);
                return;
            }

            if (entity is NewContainer container)
            {
                ContainerStorage.Move(container);
                return;
            }

            if (entity is NewStatic stat)
            {
                StaticStorage.Move(stat);
                return;
            }

            throw new Exception($"Unknown Move: {entity.ObjectDesc.ObjectId}");
        }

        public void Update(ref TickTime time)
        {
            // update entities here

            foreach(var player in Players.Values)
            {
                player.Update(ref time);
                if (player.Dead)
                    EntitiesToRemove.Add(player.Id, player);
            }

            foreach (var entity in EntitiesToAdd.Values)
                Insert(entity);
            EntitiesToAdd.Clear();

            foreach (var entity in EntitiesToRemove.Values)
                Remove(entity);
            EntitiesToRemove.Clear();
        }

        public void Remove(EntityBase entity)
        {
            if (entity is NewPlayer player)
            {
                PlayerStorage.Remove(player);
                return;
            }

            if (entity is NewEnemy enemy)
            {
                EnemyStorage.Remove(enemy);
                return;
            }

            if (entity is NewPortal portal)
            {
                PortalStorage.Remove(portal);
                return;
            }

            if (entity is NewContainer container)
            {
                ContainerStorage.Remove(container);
                return;
            }

            if (entity is NewStatic stat)
            {
                StaticStorage.Remove(stat);
                return;
            }

            throw new Exception($"Unknown Remove: {entity.ObjectDesc.ObjectId}");
        }
    }

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
        public byte Background { get; protected set; }
        public byte Blocking { get; protected set; }
        public string Music { get; set; }
        public byte Difficulty { get; protected set; }
        public bool Deleted { get; protected set; }
        public bool DisableShooting { get; set; }
        public bool DisableAbilities { get; set; }
        private long Lifetime { get; set; }
        public bool ShowDisplays { get; protected set; }

        public readonly Wmap Map;
        public readonly GameServer GameServer;

        public CollisionMap<Entity> EnemiesCollision { get; private set; }
        public CollisionMap<Entity> PlayersCollision { get; private set; }
        public Dictionary<int, Player> Players = new Dictionary<int, Player>();
        public Dictionary<int, Enemy> Enemies = new Dictionary<int, Enemy>();
        public Dictionary<int, Enemy> Quests = new Dictionary<int, Enemy>();
        public Dictionary<int, StaticObject> StaticObjects = new Dictionary<int, StaticObject>();
        public Dictionary<int, Container> Containers = new Dictionary<int, Container>();
        public Dictionary<int, Portal> Portals = new Dictionary<int, Portal>();
        public Dictionary<int, Pet> Pets = new Dictionary<int, Pet>();

        private readonly List<Entity> EntitiesToAdd = new List<Entity>();
        private readonly List<Entity> EntitiesToRemove = new List<Entity>();

        private readonly List<WorldTimer> Timers = new List<WorldTimer>();

        public Census Census { get; private set; }
        public Visibility Visibility { get; private set; }

        public WorldBranch WorldBranch { get; private set; }
        public World ParentWorld { get; set; }

        public World(GameServer gameServer, int id, WorldResource resource, World parent = null)
        {
            GameServer = gameServer;
            Map = new Wmap(this);

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

            Census = new Census(this);
            Visibility = new Visibility(resource.Width, resource.Height);

            WorldBranch = new WorldBranch(this);
            ParentWorld = parent;
        }

        private int NextGameObjectId = -1;

        public NewPlayer CreateNewPlayer(Client client, string idName, float x, float y)
        {
            if (!GameServer.Resources.GameData.IdToObjectType.TryGetValue(idName, out var objectType))
                throw new Exception($"[CreateNewPlayer] Unknown IdName: {idName}");
            return CreateNewPlayer(client, objectType, x, y);
        }
        public NewPlayer CreateNewPlayer(Client client, int objectType, float x, float y)
        {
            if (!GameServer.Resources.GameData.ObjectDescs.TryGetValue((ushort)objectType, out var desc))
                throw new Exception($"[CreateNewPlayer] Unknown ObjectType: {objectType}");

            var nextObjectId = Interlocked.Increment(ref NextGameObjectId);

            var ret = new NewPlayer(client, this, desc);
            ret.SetObjectId(nextObjectId);
            ret.SetPosition(x, y);
            Census.AddEntity(ret);
            return ret;
        }

        public NewEnemy CreateNewEnemy(string idName, float x, float y)
        {
            if (!GameServer.Resources.GameData.IdToObjectType.TryGetValue(idName, out var objectType))
                throw new Exception($"[CreateNewEnemy] Unknown IdName: {idName}");
            return CreateNewEnemy(objectType, x, y);
        }
        public NewEnemy CreateNewEnemy(int objectType, float x, float y)
        {
            if (!GameServer.Resources.GameData.ObjectDescs.TryGetValue((ushort)objectType, out var desc))
                throw new Exception($"[CreateNewEnemy] Unknown ObjectType: {objectType}");

            var nextObjectId = Interlocked.Increment(ref NextGameObjectId);

            var ret = new NewEnemy(this, desc);
            ret.SetObjectId(nextObjectId);
            ret.SetPosition(x, y);
            Census.AddEntity(ret);
            return ret;
        }

        public NewPortal CreateNewPortal(string idName, float x, float y)
        {
            if (!GameServer.Resources.GameData.IdToObjectType.TryGetValue(idName, out var objectType))
                throw new Exception($"[CreateNewPortal] Unknown IdName: {idName}");
            return CreateNewPortal(objectType, x, y);
        }
        public NewPortal CreateNewPortal(int objectType, float x, float y)
        {
            if (!GameServer.Resources.GameData.ObjectDescs.TryGetValue((ushort)objectType, out var desc))
                throw new Exception($"[CreateNewPortal] Unknown ObjectType: {objectType}");

            var nextObjectId = Interlocked.Increment(ref NextGameObjectId);

            var ret = new NewPortal(this, desc);
            ret.SetObjectId(nextObjectId);
            ret.SetPosition(x, y);
            Census.AddEntity(ret);
            return ret;
        }

        public NewContainer CreateNewContainer(string idName, float x, float y)
        {
            if (!GameServer.Resources.GameData.IdToObjectType.TryGetValue(idName, out var objectType))
                throw new Exception($"[CreateNewContainer] Unknown IdName: {idName}");
            return CreateNewContainer(objectType, x, y);
        }
        public NewContainer CreateNewContainer(int objectType, float x, float y)
        {
            if (!GameServer.Resources.GameData.ObjectDescs.TryGetValue((ushort)objectType, out var desc))
                throw new Exception($"[CreateNewContainer] Unknown ObjectType: {objectType}");

            var nextObjectId = Interlocked.Increment(ref NextGameObjectId);

            var ret = new NewContainer(this, desc);
            ret.SetObjectId(nextObjectId);
            ret.SetPosition(x, y);
            Census.AddEntity(ret);
            return ret;
        }

        public NewStatic CreateNewStatic(string idName, float x, float y)
        {
            if (!GameServer.Resources.GameData.IdToObjectType.TryGetValue(idName, out var objectType))
                throw new Exception($"[CreateNewStatic] Unknown IdName: {idName}");
            return CreateNewStatic(objectType, x, y);
        }
        public NewStatic CreateNewStatic(int objectType, float x, float y)
        {
            if (!GameServer.Resources.GameData.ObjectDescs.TryGetValue((ushort)objectType, out var desc))
                throw new Exception($"[CreateNewStatic] Unknown ObjectType: {objectType}");

            var nextObjectId = Interlocked.Increment(ref NextGameObjectId);

            var ret = new NewStatic(this, desc);
            ret.SetObjectId(nextObjectId);
            ret.SetPosition(x, y);
            Census.AddEntity(ret);
            return ret;
        }

        public virtual bool AllowedAccess(Client client) => true;

        public void Broadcast(OutgoingMessage outgoingMessage)
        {
            foreach (var player in Players.Values)
                player.Client.SendMessage(outgoingMessage);
        }

        public void BroadcastIfVisible(List<OutgoingMessage> outgoingMessages, ref Position worldPosData)
        {
            foreach (var outgoingMessage in outgoingMessages)
                BroadcastIfVisible(outgoingMessage, ref worldPosData);
        }

        public void BroadcastIfVisible(OutgoingMessage outgoingMessage, ref Position worldPosData)
        {
            foreach (var player in Players.Values)
                if (player.SqDistTo(ref worldPosData) < PlayerUpdate.VISIBILITY_RADIUS_SQR)
                {
                    if (outgoingMessage is ServerPlayerShoot)
                        player.ServerPlayerShoot(outgoingMessage as ServerPlayerShoot);
                    player.Client.SendMessage(outgoingMessage);
                }
        }

        public void BroadcastIfVisible(OutgoingMessage outgoingMessage, Entity host)
        {
            foreach (var player in Players.Values)
                if (player.SqDistTo(host) < PlayerUpdate.VISIBILITY_RADIUS_SQR)
                {
                    if (outgoingMessage is EnemyShoot)
                        player.EnemyShoot(outgoingMessage as EnemyShoot);
                    player.Client.SendMessage(outgoingMessage);
                }
        }

        public void BroadcastIfVisibleExclude(List<OutgoingMessage> outgoingMessage, Entity broadcaster, Entity exclude)
        {
            foreach (var player in Players.Values)
                if (player.Id != exclude.Id && player.SqDistTo(broadcaster) <= PlayerUpdate.VISIBILITY_RADIUS_SQR)
                    player.Client.SendPackets(outgoingMessage);
        }

        public void BroadcastIfVisibleExclude(OutgoingMessage outgoingMessage, Entity broadcaster, Entity exclude)
        {
            foreach (var player in Players.Values)
                if (player.Id != exclude.Id && player.SqDistTo(broadcaster) <= PlayerUpdate.VISIBILITY_RADIUS_SQR)
                    player.Client.SendMessage(outgoingMessage);
        }

        public void BroadcastToPlayer(OutgoingMessage outgoingMessage, int playerId)
        {
            foreach (var player in Players.Values)
                if (player.Id == playerId)
                {
                    player.Client.SendMessage(outgoingMessage);
                    break;
                }
        }

        public void BroadcastToPlayers(OutgoingMessage outgoingMessage, List<int> playerIds)
        {
            foreach (var player in Players.Values)
                if (playerIds.Contains(player.Id))
                    player.Client.SendMessage(outgoingMessage);
        }

        public void ChatReceived(Player player, string text)
        {
            foreach (var en in Enemies)
                en.Value.OnChatTextReceived(player, text);
            foreach (var en in StaticObjects)
                en.Value.OnChatTextReceived(player, text);
        }

        public virtual int EnterWorld(Entity entity)
        {
            entity.Id = GetNextEntityId();
            entity.Init(this);
            EntitiesToAdd.Add(entity);
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

        public virtual void LeaveWorld(Entity entity) => entity.Expunge();

        private void AddToWorld(Entity entity)
        {
            if (entity is Player)
            {
                Players.TryAdd(entity.Id, entity as Player);
                PlayersCollision.Insert(entity, entity.X, entity.Y);
            }
            else if (entity is Enemy)
            {
                Enemies.TryAdd(entity.Id, entity as Enemy);
                EnemiesCollision.Insert(entity, entity.X, entity.Y);
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
                    PlayersCollision.Insert(entity, entity.X, entity.Y);
                else
                    EnemiesCollision.Insert(entity, entity.X, entity.Y);
            }
            else if (entity is Pet)
            {
                Pets.TryAdd(entity.Id, entity as Pet);
                PlayersCollision.Insert(entity, entity.X, entity.Y);
            }
        }

        private void RemoveFromWorld(Entity entity)
        {
            if (entity is Player player)
            {
                Players.Remove(entity.Id);
                PlayersCollision.Remove(entity);

                // if in trade, cancel it...
                if (player != null && player.tradeTarget != null)
                    player.CancelTrade();

                if (player != null && player.Pet != null)
                    LeaveWorld(player.Pet);
            }
            else if (entity is Enemy)
            {
                Enemies.Remove(entity.Id);
                EnemiesCollision.Remove(entity);
                if (entity.ObjectDesc.Quest)
                    Quests.Remove(entity.Id);
            }
            else if (entity is Container)
                Containers.Remove(entity.Id);
            else if (entity is Portal)
                Portals.Remove(entity.Id);
            else if (entity is StaticObject)
            {
                StaticObjects.Remove(entity.Id);
                if (entity is Decoy)
                    PlayersCollision.Remove(entity);
                else
                    EnemiesCollision.Remove(entity);
            }
            else if (entity is Pet)
            {
                Pets.Remove(entity.Id);
                PlayersCollision.Remove(entity);
            }
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
            Census.Update(ref time);

            foreach (var player in Players.Values)
            {
                player.Tick(ref time);
                if (player.Dead)
                    EntitiesToRemove.Add(player);
            }

            foreach (var stat in StaticObjects.Values)
            {
                stat.Tick(ref time);
                if (stat.Dead)
                    EntitiesToRemove.Add(stat);
            }

            foreach (var portal in Portals.Values)
            {
                portal.Tick(ref time);
                if (portal.Dead)
                    EntitiesToRemove.Add(portal);
            }

            foreach (var container in Containers.Values)
            {
                container.Tick(ref time);
                if (container.Dead)
                    EntitiesToRemove.Add(container);
            }

            foreach (var pet in Pets.Values)
            {
                pet.Tick(ref time);
                if (pet.Dead)
                    EntitiesToRemove.Add(pet);
            }

            if (EnemiesCollision != null)
            {
                foreach (var entity in EnemiesCollision.GetActiveChunks(PlayersCollision))
                {
                    entity.Tick(ref time);
                    if (entity.Dead)
                        EntitiesToRemove.Add(entity);
                }
            }
            else
            {
                foreach (var entity in Enemies.Values)
                {
                    entity.Tick(ref time);
                    if (entity.Dead)
                        EntitiesToRemove.Add(entity);
                }
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

            foreach (var entity in EntitiesToAdd)
                AddToWorld(entity);
            EntitiesToAdd.Clear();

            foreach (var removed in EntitiesToRemove)
                RemoveFromWorld(removed);
            EntitiesToRemove.Clear();

            foreach (var player in Players.Values)
                player.PlayerUpdate.UpdateState(time.ElapsedMsDelta);
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
