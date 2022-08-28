using common.resources;
using System;
using System.Collections.Generic;
using System.Linq;
using wServer.core.objects.connection;
using wServer.core.objects.vendors;
using wServer.core.worlds;
using wServer.logic;
using wServer.logic.transitions;
using wServer.memory;
using wServer.utils;

namespace wServer.core.objects
{
    public class Entity : ICollidable<Entity>
    {
        public Player Controller;
        public bool GivesNoXp;
        public float? savedAngle;
        public bool Spawned;
        public bool SpawnedByBehavior;

        private const float colSkipBoundary = .4f;

        private SV<int> _altTextureIndex;
        private ConditionEffectIndex _conditionEffects;
        private ObjectDesc _desc;
        private int[] _effects;
        private SV<string> _name;
        private int _originalSize;
        private Position[] _posHistory;
        private byte _posIdx;
        private SV<int> _size;
        private bool _stateEntry;
        private State _stateEntryCommonRoot;
        private Dictionary<object, object> _states;
        private SV<float> _x;
        private SV<float> _y;
        private Player playerOwner;
        private ConditionEffectManager ConditionEffectManager;

        protected Entity(GameServer coreServerManager, ushort objType)
        {
            GameServer = coreServerManager;

            _name = new SV<string>(this, StatDataType.Name, "");
            _size = new SV<int>(this, StatDataType.Size, 100);
            _originalSize = 100;
            _altTextureIndex = new SV<int>(this, StatDataType.AltTextureIndex, -1);
            _x = new SV<float>(this, StatDataType.None, 0);
            _y = new SV<float>(this, StatDataType.None, 0);

            ObjectType = objType;

            coreServerManager.BehaviorDb.ResolveBehavior(this);
            coreServerManager.Resources.GameData.ObjectDescs.TryGetValue(ObjectType, out _desc);

            ConditionEffectManager = new ConditionEffectManager(this);

            if (_desc == null)
                return;

            QuestLevel = _desc.Level;

            _posHistory = new Position[256];
        }

        public event EventHandler<StatChangedEventArgs> StatChanged;

        public int AltTextureIndex { get => _altTextureIndex.GetValue(); set => _altTextureIndex?.SetValue(value); }
        public Player AttackTarget { get; set; }
        public CollisionNode<Entity> CollisionNode { get; set; }

        public GameServer GameServer { get; private set; }
        public State CurrentState { get; private set; }
        public int Id { get; internal set; }
        public bool IsRemovedFromWorld { get; private set; }
        public string Name { get => _name.GetValue(); set => _name?.SetValue(value); }
        public ObjectDesc ObjectDesc => _desc;
        public ushort ObjectType { get; protected set; }
        public World World { get; private set; }
        public CollisionMap<Entity> Parent { get; set; }
        public int QuestLevel { get; set; } = 1;
        public float RealX => _x.GetValue();
        public float RealY => _y.GetValue();
        public int Size { get => _size.GetValue(); set => _size?.SetValue(value); }

        public IDictionary<object, object> StateStorage
        {
            get
            {
                if (_states == null)
                    _states = new Dictionary<object, object>();

                return _states;
            }
        }

        public float X { get => _x.GetValue(); set => _x.SetValue(value); }
        public float Y { get => _y.GetValue(); set => _y.SetValue(value); }

        public static Entity Resolve(GameServer manager, string name)
        {
            if (!manager.Resources.GameData.IdToObjectType.TryGetValue(name, out ushort id))
                return null;

            return Resolve(manager, id);
        }

        public static Entity Resolve(GameServer manager, ushort id)
        {
            var node = manager.Resources.GameData.ObjectDescs[id];
            int? hp;
            if (node.MaxHP == 0)
                hp = null;
            else
                hp = node.MaxHP;
            var type = node.Class;
            switch (type)
            {
                case "Projectile":
                    throw new Exception("Projectile should not instantiated using Entity.Resolve");
                case "Sign":
                    return new Sign(manager, id);

                case "Wall":
                case "DoubleWall":
                    return new Wall(manager, id, hp);

                case "ConnectedWall":
                case "CaveWall":
                    return new ConnectedObject(manager, id);

                case "GameObject":
                case "CharacterChanger":
                case "MoneyChanger":
                case "NameChanger":
                    return new StaticObject(manager, id, hp, true, false, true);

                case "GuildRegister":
                case "GuildChronicle":
                case "GuildBoard":
                    return new StaticObject(manager, id, null, false, false, false);

                case "Container":
                    return new Container(manager, id);

                case "Player":
                    throw new Exception("Player should not instantiated using Entity.Resolve");
                case "Character":   //Other characters means enemy
                    return new Enemy(manager, id);

                case "ArenaPortal":
                case "Portal":
                    return new Portal(manager, id, null);

                case "GuildHallPortal":
                    return new GuildHallPortal(manager, id, null);

                case "ClosedVaultChest":
                    return new ClosedVaultChest(manager, id);

                case "Merchant":
                    return new NexusMerchant(manager, id);

                case "ClosedVaultChestGold":
                case "VaultChest":
                case "MarketNPC":
                case "SkillTree":
                case "Forge":
                case "StatNPC":
                    return new SellableMerchant(manager, id);

                case "ClosedGiftChest":
                case "SpecialClosedVaultChest":
                    return new StaticObject(manager, id, null, false, false, false);

                case "Engine":
                    return new Engine(manager, id);

                case "GuildMerchant":
                    return new GuildMerchant(manager, id);

                case "BountyBoard":
                    return new StaticObject(manager, id, null, false, false, false);

                case "PotionStorage":
                    return new StaticObject(manager, id, null, false, false, false);

                case "Essence":
                    return new StaticObject(manager, id, null, false, false, false);

                default:
                    StaticLogger.Instance.Warn("Not supported type: {0}", type);
                    return new Entity(manager, id);
            }
        }

        public void ApplyPermanentConditionEffect(ConditionEffectIndex effect)
        {
            if (!CanApplyCondition(effect))
                return;
            ConditionEffectManager.AddPermanentCondition((byte)effect);
        }

        public void ApplyConditionEffect(ConditionEffectIndex effect, int durationMs)
        {
            if (!CanApplyCondition(effect))
                return;
            ConditionEffectManager.AddCondition((byte)effect, durationMs);
        }

        public void ApplyConditionEffect(params ConditionEffect[] effs)
        {
            foreach (var i in effs)
            {
                if (!CanApplyCondition(i.Effect))
                    continue;
                ConditionEffectManager.AddCondition((byte)i.Effect, i.DurationMS);
            }
        }

        public bool HasConditionEffect(ConditionEffectIndex effect) => ConditionEffectManager.HasCondition((byte)effect);
        public void RemoveCondition(ConditionEffectIndex effect) => ConditionEffectManager.RemoveCondition((byte)effect);

        private bool CanApplyCondition(ConditionEffectIndex effect)
        {
            if (effect == ConditionEffectIndex.Stunned && HasConditionEffect(ConditionEffectIndex.StunImmune))
                return false;
            if (effect == ConditionEffectIndex.Stasis && HasConditionEffect(ConditionEffectIndex.StasisImmune))
                return false;
            if (effect == ConditionEffectIndex.Paralyzed && HasConditionEffect(ConditionEffectIndex.ParalyzeImmune))
                return false;
            if (effect == ConditionEffectIndex.ArmorBroken && HasConditionEffect(ConditionEffectIndex.ArmorBreakImmune))
                return false;
            if (effect == ConditionEffectIndex.Unstable && HasConditionEffect(ConditionEffectIndex.UnstableImmune))
                return false;
            if (effect == ConditionEffectIndex.Curse && HasConditionEffect(ConditionEffectIndex.CurseImmune))
                return false;
            if (effect == ConditionEffectIndex.Petrify && HasConditionEffect(ConditionEffectIndex.PetrifyImmune))
                return false;
            if (effect == ConditionEffectIndex.Dazed && HasConditionEffect(ConditionEffectIndex.DazedImmune))
                return false;
            if (effect == ConditionEffectIndex.Slowed && HasConditionEffect(ConditionEffectIndex.SlowedImmune))
                return false;
            return true;
        }

        public virtual bool CanBeSeenBy(Player player) => true;

        public ObjectStats ExportStats(bool isOtherPlayer)
        {
            var stats = new Dictionary<StatDataType, object>();
            ExportStats(stats, isOtherPlayer);
            return new ObjectStats()
            {
                Id = Id,
                Position = new Position() { X = RealX, Y = RealY },
                Stats = stats.ToArray()
            };
        }

        public virtual bool HitByProjectile(Projectile projectile, TickTime time)
        {
            if (ObjectDesc == null)
                return true;

            return ObjectDesc.Enemy || ObjectDesc.Player;
        }

        public virtual void Init(World owner) => World = owner;

        public void InvokeStatChange(StatDataType t, object val, bool updateSelfOnly = false) => StatChanged?.Invoke(this, new StatChangedEventArgs(t, val, updateSelfOnly));

        public virtual void Move(float x, float y)
        {
            if (Controller != null)
                return;

            MoveEntity(x, y);
        }

        public void MoveEntity(float x, float y)
        {
            if (World != null && !(this is Projectile) && !(this is Pet) && (!(this is StaticObject) || (this as StaticObject).Hittestable))
                ((this is Enemy || this is StaticObject && !(this is Decoy)) ? World.EnemiesCollision : World.PlayersCollision).Move(this, x, y);

            X = x; Y = y;
        }

        public void OnChatTextReceived(Player player, string text)
        {
            var state = CurrentState;

            while (state != null)
            {
                foreach (var t in state.Transitions.OfType<PlayerTextTransition>())
                    t.OnChatReceived(player, text);

                state = state.Parent;
            }
        }

        public void RestoreDefaultSize() => Size = _originalSize;

        public void SetDefaultSize(int size)
        {
            _originalSize = size;

            Size = size;
        }

        public void SetPlayerOwner(Player target) => playerOwner = target;

        public void SwitchTo(State state)
        {
            var origState = CurrentState;

            CurrentState = state;
            GoDeeeeeeeep();

            _stateEntryCommonRoot = State.CommonParent(origState, CurrentState);
            _stateEntry = true;
        }

        public virtual void Tick(ref TickTime time)
        {
            if (this == null || World == null)
                return;

            if (CurrentState != null && World != null)
            {
                if (!HasConditionEffect(ConditionEffectIndex.Stasis) && this.AnyPlayerNearby())
                    TickState(time);
            }

            ConditionEffectManager.Update(ref time);

            if (_posHistory != null)
                _posHistory[++_posIdx] = new Position() { X = X, Y = Y };
        }

        public void TickState(TickTime time)
        {
            if (_stateEntry)
            {
                //State entry
                var s = CurrentState;

                while (s != null && s != _stateEntryCommonRoot)
                {
                    foreach (var i in s.Behaviors)
                        i.OnStateEntry(this, time);

                    s = s.Parent;
                }

                _stateEntryCommonRoot = null;
                _stateEntry = false;
            }

            var origState = CurrentState;
            var state = CurrentState;
            var transited = false;

            while (state != null)
            {
                if (!transited)
                    foreach (var i in state.Transitions)
                        if (i.Tick(this, time))
                        {
                            transited = true;
                            break;
                        }

                try
                {
                    foreach (var i in state.Behaviors)
                    {
                        if (this == null || World == null)
                            break;

                        i.Tick(this, time);
                    }
                }
                catch (Exception e)
                {
                    StaticLogger.Instance.Error(e);
                    continue;
                }

                if (this == null || World == null)
                    break;

                state = state.Parent;
            }

            if (transited)
            {
                //State exit
                var s = origState;

                while (s != null && s != _stateEntryCommonRoot)
                {
                    foreach (var i in s.Behaviors)
                        i.OnStateExit(this, time);

                    s = s.Parent;
                }
            }
        }

        public bool TileFullOccupied(float x, float y)
        {
            var xx = (int)x;
            var yy = (int)y;

            if (!World.Map.Contains(xx, yy))
                return true;

            var tile = World.Map[xx, yy];

            if (tile.ObjType != 0)
            {
                var objDesc = GameServer.Resources.GameData.ObjectDescs[tile.ObjType];

                if (objDesc?.FullOccupy == true)
                    return true;
            }

            return false;
        }

        public bool TileOccupied(float x, float y)
        {
            if (this == null || World == null)
                return false;

            var x_ = (int)x;
            var y_ = (int)y;

            var map = World.Map;

            if (map == null)
                return false;

            if (!map.Contains(x_, y_))
                return true;

            var tile = map[x_, y_];

            if (tile == null)
                return false;

            var tiles = GameServer.Resources.GameData.Tiles;

            if (!tiles.ContainsKey(tile.TileId))
            {
                StaticLogger.Instance.Error($"There is no tile for tile ID '{tile.TileId}'.");
                return false;
            }

            var tileDesc = tiles[tile.TileId];

            if (tileDesc != null && tileDesc.NoWalk)
                return true;

            if (tile.ObjType != 0)
            {
                var objDescs = GameServer.Resources.GameData.ObjectDescs;

                if (!objDescs.ContainsKey(tile.ObjType))
                {
                    StaticLogger.Instance.Error($"There is no object description for tile object type '{tile.ObjType}'.");
                    return false;
                }

                var objDesc = objDescs[tile.ObjType];
                return objDesc != null && objDesc.EnemyOccupySquare;
            }

            return false;
        }

        public ObjectDef ToDefinition(bool isOtherPlayer = false) => new ObjectDef()
        {
            ObjectType = ObjectType,
            Stats = ExportStats(isOtherPlayer)
        };

        public Position? TryGetHistory(long ticks)
        {
            if (_posHistory == null)
                return null;
            if (ticks > 255)
                return null;
            return _posHistory[(byte)(_posIdx - (byte)ticks)];
        }

        public void ValidateAndMove(float x, float y)
        {
            if (this == null || World == null)
                return;

            var pos = new FPoint();

            ResolveNewLocation(x, y, pos);
            Move(pos.X, pos.Y);
        }

        protected virtual void ExportStats(IDictionary<StatDataType, object> stats, bool isOtherPlayer)
        {
            stats[StatDataType.Name] = Name;
            stats[StatDataType.Size] = Size;
            stats[StatDataType.AltTextureIndex] = AltTextureIndex;
            ConditionEffectManager.ExportStats(stats);
        }

        private void CalcNewLocation(float x, float y, FPoint pos)
        {
            var fx = 0f;
            var fy = 0f;
            var isFarX = (X % .5f == 0 && x != X) || (int)(X / .5f) != (int)(x / .5f);
            var isFarY = (Y % .5f == 0 && y != Y) || (int)(Y / .5f) != (int)(y / .5f);

            if ((!isFarX && !isFarY) || RegionUnblocked(x, y))
            {
                pos.X = x;
                pos.Y = y;
                return;
            }

            if (isFarX)
            {
                fx = (x > X) ? (int)(x * 2) / 2f : (int)(X * 2) / 2f;

                if ((int)fx > (int)X)
                    fx = fx - 0.01f;
            }

            if (isFarY)
            {
                fy = (y > Y) ? (int)(y * 2) / 2f : (int)(Y * 2) / 2f;

                if ((int)fy > (int)Y)
                    fy = fy - 0.01f;
            }

            if (!isFarX)
            {
                pos.X = x;
                pos.Y = fy;
                return;
            }

            if (!isFarY)
            {
                pos.X = fx;
                pos.Y = y;
                return;
            }

            var ax = (x > X) ? x - fx : fx - x;
            var ay = (y > Y) ? y - fy : fy - y;

            if (ax > ay)
            {
                if (RegionUnblocked(x, fy))
                {
                    pos.X = x;
                    pos.Y = fy;
                    return;
                }

                if (RegionUnblocked(fx, y))
                {
                    pos.X = fx;
                    pos.Y = y;
                    return;
                }
            }
            else
            {
                if (RegionUnblocked(fx, y))
                {
                    pos.X = fx;
                    pos.Y = y;
                    return;
                }

                if (RegionUnblocked(x, fy))
                {
                    pos.X = x;
                    pos.Y = fy;
                    return;
                }
            }

            pos.X = fx;
            pos.Y = fy;
        }

        private void GoDeeeeeeeep()
        {
            if (CurrentState == null)
                return;

            while (CurrentState?.States.Count > 0)
                if (this == null || CurrentState == null)
                    break;
                else
                    CurrentState = CurrentState.States[0];
        }

        private bool RegionUnblocked(float x, float y)
        {
            if (TileOccupied(x, y))
                return false;

            var xFrac = x - (int)x;
            var yFrac = y - (int)y;

            if (xFrac < 0.5)
            {
                if (TileFullOccupied(x - 1, y))
                    return false;

                if (yFrac < 0.5)
                {
                    if (TileFullOccupied(x, y - 1) || TileFullOccupied(x - 1, y - 1))
                        return false;
                }
                else
                {
                    if (yFrac > 0.5)
                        if (TileFullOccupied(x, y + 1) || TileFullOccupied(x - 1, y + 1))
                            return false;
                }

                return true;
            }

            if (xFrac > 0.5)
            {
                if (TileFullOccupied(x + 1, y))
                    return false;

                if (yFrac < 0.5)
                {
                    if (TileFullOccupied(x, y - 1) || TileFullOccupied(x + 1, y - 1))
                        return false;
                }
                else
                {
                    if (yFrac > 0.5)
                        if (TileFullOccupied(x, y + 1) || TileFullOccupied(x + 1, y + 1))
                            return false;
                }

                return true;
            }

            if (yFrac < 0.5)
            {
                if (TileFullOccupied(x, y - 1))
                    return false;

                return true;
            }

            if (yFrac > 0.5)
                if (TileFullOccupied(x, y + 1))
                    return false;

            return true;
        }

        private void ResolveNewLocation(float x, float y, FPoint pos)
        {
            if (HasConditionEffect(ConditionEffectIndex.Paralyzed))
            {
                pos.X = X;
                pos.Y = Y;
                return;
            }

            var dx = x - X;
            var dy = y - Y;

            if (dx < colSkipBoundary && dx > -colSkipBoundary && dy < colSkipBoundary && dy > -colSkipBoundary)
            {
                CalcNewLocation(x, y, pos);
                return;
            }

            var ds = colSkipBoundary / Math.Max(Math.Abs(dx), Math.Abs(dy));
            var tds = 0f;

            pos.X = X;
            pos.Y = Y;

            var done = false;

            while (!done)
            {
                if (tds + ds >= 1)
                {
                    ds = 1 - tds;
                    done = true;
                }

                CalcNewLocation(pos.X + dx * ds, pos.Y + dy * ds, pos);
                tds += ds;
            }
        }

        public virtual void Destroy()
        {
            IsRemovedFromWorld = true;
        }

        private class FPoint
        {
            public float X;
            public float Y;
        }

        protected byte projectileId;

        public Projectile CreateProjectile(ProjectileDesc desc, ushort container, int dmg, long time, Position pos, float angle)
        {
            var ret = World.ObjectPools.Projectiles.Rent();
            ret.Host = this;
            ret.ProjDesc = desc;
            ret.ProjectileId = projectileId++;
            ret.Container = container;
            ret.Damage = dmg;
            ret.CreationTime = time;
            ret.Angle = angle;
            ret.StartX = pos.X;
            ret.StartY = pos.Y;
            return ret;
        }
    }
}
