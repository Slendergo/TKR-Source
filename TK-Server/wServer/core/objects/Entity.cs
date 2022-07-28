using common.resources;
using System;
using System.Collections.Generic;
using System.Linq;
using wServer.core.objects.vendors;
using wServer.core.worlds;
using wServer.logic;
using wServer.logic.transitions;
using wServer.utils;

namespace wServer.core.objects
{
    public class Entity : IProjectileOwner, ICollidable<Entity>
    {
        public Player Controller;
        public bool GivesNoXp;
        public float? savedAngle;
        public bool Spawned;
        public bool SpawnedByBehavior;

        protected byte projectileId;

        private const float colSkipBoundary = .4f;
        private const int EffectCount = 54;

        private SV<int> _altTextureIndex;
        private ConditionEffects _conditionEffects;
        private SV<int> _conditionEffects1;
        private SV<int> _conditionEffects2;
        private ObjectDesc _desc;
        private int[] _effects;
        private SV<string> _name;
        private int _originalSize;
        private Position[] _posHistory;
        private byte _posIdx;
        private Projectile[] _projectiles;
        private SV<int> _size;
        private bool _stateEntry;
        private State _stateEntryCommonRoot;
        private Dictionary<object, object> _states;
        private bool _tickingEffects;
        private SV<float> _x;
        private SV<float> _y;
        private Player playerOwner;

        protected Entity(CoreServerManager coreServerManager, ushort objType)
        {
            CoreServerManager = coreServerManager;

            _name = new SV<string>(this, StatDataType.Name, "");
            _size = new SV<int>(this, StatDataType.Size, 100);
            _originalSize = 100;
            _altTextureIndex = new SV<int>(this, StatDataType.AltTextureIndex, -1);
            _x = new SV<float>(this, StatDataType.None, 0);
            _y = new SV<float>(this, StatDataType.None, 0);
            _conditionEffects1 = new SV<int>(this, StatDataType.Effects, 0);
            _conditionEffects2 = new SV<int>(this, StatDataType.Effects2, 0);

            ObjectType = objType;

            coreServerManager.BehaviorDb.ResolveBehavior(this);
            coreServerManager.Resources.GameData.ObjectDescs.TryGetValue(ObjectType, out _desc);

            if (_desc == null)
                return;

            QuestLevel = _desc.Level;

            _posHistory = new Position[256];
            _projectiles = new Projectile[256];
            _effects = new int[EffectCount];
        }

        public event EventHandler<StatChangedEventArgs> StatChanged;

        public int AltTextureIndex { get => _altTextureIndex.GetValue(); set => _altTextureIndex?.SetValue(value); }
        public Player AttackTarget { get; set; }
        public CollisionNode<Entity> CollisionNode { get; set; }

        public ConditionEffects ConditionEffects
        {
            get => _conditionEffects;
            set
            {
                _conditionEffects = value;
                _conditionEffects1?.SetValue((int)value);
                _conditionEffects2?.SetValue((int)((ulong)value >> 31));
            }
        }

        public CoreServerManager CoreServerManager { get; private set; }
        public State CurrentState { get; private set; }
        public int Id { get; internal set; }
        public bool IsRemovedFromWorld { get; private set; }
        public string Name { get => _name.GetValue(); set => _name?.SetValue(value); }
        public ObjectDesc ObjectDesc => _desc;
        public ushort ObjectType { get; protected set; }
        public World Owner { get; private set; }
        public CollisionMap<Entity> Parent { get; set; }
        Projectile[] IProjectileOwner.Projectiles => _projectiles;
        public int QuestLevel { get; set; } = 1;
        public float RealX => _x.GetValue();
        public float RealY => _y.GetValue();
        Entity IProjectileOwner.Self => this;
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

        public bool TickStateManually { get; set; }

        public float X { get => _x.GetValue(); set => _x.SetValue(value); }
        public float Y { get => _y.GetValue(); set => _y.SetValue(value); }

        public static Entity Resolve(CoreServerManager manager, string name)
        {
            if (!manager.Resources.GameData.IdToObjectType.TryGetValue(name, out ushort id))
                return null;

            return Resolve(manager, id);
        }

        public static Entity Resolve(CoreServerManager manager, ushort id)
        {
            var node = manager.Resources.GameData.ObjectTypeToElement[id];
            var type = node.Element("Class").Value;

            switch (type)
            {
                case "Projectile":
                    throw new Exception("Projectile should not instantiated using Entity.Resolve");
                case "Sign":
                    return new Sign(manager, id);

                case "Wall":
                case "DoubleWall":
                    return new Wall(manager, id, node);

                case "ConnectedWall":
                case "CaveWall":
                    return new ConnectedObject(manager, id);

                case "GameObject":
                case "CharacterChanger":
                case "MoneyChanger":
                case "NameChanger":
                    return new StaticObject(manager, id, StaticObject.GetHP(node), true, false, true);

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

                case "ClosedVaultChestGold":
                case "ClosedGiftChest":
                case "SpecialClosedVaultChest":
                case "VaultChest":
                case "MarketNPC":
                case "SkillTree":
                case "Forge":
                case "StatNPC":
                case "Merchant":
                    return new WorldMerchant(manager, id);

                case "GuildMerchant":
                    return new GuildMerchant(manager, id);

                case "BountyBoard":
                    return new StaticObject(manager, id, null, false, false, false);

                case "PotionStorage":
                    return new StaticObject(manager, id, null, false, false, false);

                default:
                    SLogger.Instance.Warn("Not supported type: {0}", type);
                    return new Entity(manager, id);
            }
        }

        public void ApplyConditionEffect(params ConditionEffect[] effs)
        {
            foreach (var i in effs)
            {
                if (!ApplyCondition(i.Effect))
                    continue;

                var eff = (int)i.Effect;

                _effects[eff] = i.DurationMS;

                if (i.DurationMS != 0)
                    ConditionEffects |= (ConditionEffects)((ulong)1 << eff);
            }

            _tickingEffects = true;
        }

        public void ApplyConditionEffect(ConditionEffectIndex effect, int durationMs = -1)
        {
            if (!ApplyCondition(effect))
                return;

            var eff = (int)effect;

            _effects[eff] = durationMs;

            if (durationMs != 0)
                ConditionEffects |= (ConditionEffects)((ulong)1 << eff);

            _tickingEffects = true;
        }

        public virtual bool CanBeSeenBy(Player player) => true;

        public Projectile CreateProjectile(ProjectileDesc desc, ushort container, int dmg, long time, Position pos, float angle)
        {
            var ret = new Projectile(CoreServerManager, desc) //Assume only one
            {
                ProjectileOwner = this,
                ProjectileId = projectileId++,
                Container = container,
                Damage = dmg,

                CreationTime = time,
                StartPos = pos,
                Angle = angle,

                X = pos.X,
                Y = pos.Y
            };

            if (_projectiles[ret.ProjectileId] != null)
                _projectiles[ret.ProjectileId].Destroy();

            _projectiles[ret.ProjectileId] = ret;

            return ret;
        }

        public void Destroy()
        {
            IsRemovedFromWorld = true;

            WhenDestroying(this);
            Owner.DisposeEntity(this);
        }

        public ObjectStats ExportStats()
        {
            var stats = new Dictionary<StatDataType, object>();

            ExportStats(stats);

            return new ObjectStats()
            {
                Id = Id,
                Position = new Position() { X = RealX, Y = RealY },
                Stats = stats.ToArray()
            };
        }

        public bool HasConditionEffect(ConditionEffects eff) => (ConditionEffects & eff) != 0;

        public virtual bool HitByProjectile(Projectile projectile, TickData time)
        {
            if (ObjectDesc == null)
                return true;

            return ObjectDesc.Enemy || ObjectDesc.Player;
        }

        public virtual void Init(World owner) => Owner = owner;

        public void InvokeStatChange(StatDataType t, object val, bool updateSelfOnly = false) => StatChanged?.Invoke(this, new StatChangedEventArgs(t, val, updateSelfOnly));

        public virtual void Move(float x, float y)
        {
            if (Controller != null)
                return;

            MoveEntity(x, y);
        }

        public void MoveEntity(float x, float y)
        {
            if (Owner != null && !(this is Projectile) && !(this is Pet) && (!(this is StaticObject) || (this as StaticObject).Hittestable))
                ((this is Enemy || this is StaticObject && !(this is Decoy)) ? Owner.EnemiesCollision : Owner.PlayersCollision).Move(this, x, y);

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

        public virtual void Tick(TickData time)
        {
            if (this == null || this is Projectile || Owner == null)
                return;

            if (CurrentState != null && Owner != null)
            {
                if (!HasConditionEffect(ConditionEffects.Stasis) && !TickStateManually && (this.AnyPlayerNearby() || ConditionEffects != 0))
                    TickState(time);
            }

            if (_posHistory != null)
                _posHistory[++_posIdx] = new Position() { X = X, Y = Y };

            if (_effects != null)
                ProcessConditionEffects(time);
        }

        public void TickState(TickData time)
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
                        if (this == null || Owner == null)
                            break;

                        i.Tick(this, time);
                    }
                }
                catch (Exception e)
                {
                    SLogger.Instance.Error(e);
                    continue;
                }

                if (this == null || Owner == null)
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

            if (!Owner.Map.Contains(xx, yy))
                return true;

            var tile = Owner.Map[xx, yy];

            if (tile.ObjType != 0)
            {
                var objDesc = CoreServerManager.Resources.GameData.ObjectDescs[tile.ObjType];

                if (objDesc?.FullOccupy == true)
                    return true;
            }

            return false;
        }

        public bool TileOccupied(float x, float y)
        {
            if (this == null || Owner == null)
                return false;

            var x_ = (int)x;
            var y_ = (int)y;

            var map = Owner.Map;

            if (map == null)
                return false;

            if (!map.Contains(x_, y_))
                return true;

            var tile = map[x_, y_];

            if (tile == null)
                return false;

            var tiles = CoreServerManager.Resources.GameData.Tiles;

            if (!tiles.ContainsKey(tile.TileId))
            {
                SLogger.Instance.Error($"There is no tile for tile ID '{tile.TileId}'.");
                return false;
            }

            var tileDesc = tiles[tile.TileId];

            if (tileDesc != null && tileDesc.NoWalk)
                return true;

            if (tile.ObjType != 0)
            {
                var objDescs = CoreServerManager.Resources.GameData.ObjectDescs;

                if (!objDescs.ContainsKey(tile.ObjType))
                {
                    SLogger.Instance.Error($"There is no object description for tile object type '{tile.ObjType}'.");

                    return false;
                }

                var objDesc = objDescs[tile.ObjType];

                return objDesc != null && objDesc.EnemyOccupySquare;
            }

            return false;
        }

        public ObjectDef ToDefinition() => new ObjectDef()
        {
            ObjectType = ObjectType,
            Stats = ExportStats()
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
            if (this == null || Owner == null)
                return;

            var pos = new FPoint();

            ResolveNewLocation(x, y, pos);
            Move(pos.X, pos.Y);
        }

        protected virtual void ExportStats(IDictionary<StatDataType, object> stats)
        {
            stats[StatDataType.Name] = Name;
            stats[StatDataType.Size] = Size;
            stats[StatDataType.AltTextureIndex] = AltTextureIndex;
            stats[StatDataType.Effects] = _conditionEffects1.GetValue();
            stats[StatDataType.Effects2] = _conditionEffects2.GetValue();
        }

        private bool ApplyCondition(ConditionEffectIndex effect)
        {
            if (effect == ConditionEffectIndex.Stunned && HasConditionEffect(ConditionEffects.StunImmune))
                return false;

            if (effect == ConditionEffectIndex.Stasis && HasConditionEffect(ConditionEffects.StasisImmune))
                return false;

            if (effect == ConditionEffectIndex.Paralyzed && HasConditionEffect(ConditionEffects.ParalyzeImmune))
                return false;

            if (effect == ConditionEffectIndex.ArmorBroken && HasConditionEffect(ConditionEffects.ArmorBreakImmune))
                return false;

            if (effect == ConditionEffectIndex.Curse && HasConditionEffect(ConditionEffects.CurseImmune))
                return false;

            if (effect == ConditionEffectIndex.Petrify && HasConditionEffect(ConditionEffects.PetrifyImmune))
                return false;

            if (effect == ConditionEffectIndex.Dazed && HasConditionEffect(ConditionEffects.DazedImmune))
                return false;

            if (effect == ConditionEffectIndex.Slowed && HasConditionEffect(ConditionEffects.SlowedImmune))
                return false;

            return true;
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

        private void ProcessConditionEffects(TickData time)
        {
            if (_effects == null || !_tickingEffects) return;

            ConditionEffects newEffects = 0;

            _tickingEffects = false;

            for (var i = 0; i < _effects.Length; i++)
            {
                if (_effects[i] > 0)
                {
                    _effects[i] -= time.ElaspedMsDelta;

                    if (_effects[i] > 0)
                    {
                        newEffects |= (ConditionEffects)((ulong)1 << i);
                        _tickingEffects = true;
                    }
                    else
                        _effects[i] = 0;
                }
                else if (_effects[i] == -1)
                    newEffects |= (ConditionEffects)((ulong)1 << i);
            }

            ConditionEffects = newEffects;
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
            if (HasConditionEffect(ConditionEffects.Paralyzed))
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
                tds = tds + ds;
            }
        }

        private void WhenDestroying(Entity entity)
        {
            if (entity is Projectile)
                return;

            var players = Owner.GetPlayers().ToArray();
            if (players.Length == 0)
                return;

            for (var i = 0; i < players.Length; i++)
                players[i].PlayerUpdate.DeleteEntry(entity);
        }

        private class FPoint
        {
            public float X;
            public float Y;
        }
    }
}
