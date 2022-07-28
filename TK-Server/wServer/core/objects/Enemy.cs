using common.resources;
using System;
using System.Collections.Generic;
using wServer.core.worlds;
using wServer.logic;
using wServer.networking;
using wServer.networking.packets.outgoing;

namespace wServer.core.objects
{
    public class Enemy : Character
    {
        public DamageCounter DamageCounter;
        public bool isPet; // TODO quick hack for backwards compatibility
        public Enemy ParentEntity;

        private readonly bool stat;

        private float bleeding = 0;
        private Position? pos;

        public Enemy(CoreServerManager manager, ushort objType) : base(manager, objType)
        {
            stat = ObjectDesc.MaxHP == 0;
            DamageCounter = new DamageCounter(this);
        }

        public event EventHandler<BehaviorEventArgs> OnDeath;

        public bool Epic { get; set; }
        public bool Legendary { get; set; }
        public bool Rare { get; set; }
        public Position SpawnPoint { get { return pos ?? new Position() { X = X, Y = Y }; } }
        public TerrainType Terrain { get; set; }

        public void ClasifyEnemy()
        {
            var Chance = _random.NextDouble();

            if (Chance < 0.02)
            {
                Legendary = true;
                Size += 200;
                MaximumHP = MaximumHP * 3;
                HP = MaximumHP;
                GlowEnemy = 0xEAC117;
            }
            else if (Chance < 0.04)
            {
                Epic = true;
                Size += 125;
                MaximumHP = MaximumHP * 2;
                HP = MaximumHP;
                GlowEnemy = 0x4B0082;
            }
            else if (Chance < 0.06)
            {
                Rare = true;
                Size += 50;
                GlowEnemy = 0xFFFFFF;
            }
        }

        public void ClasifyEnemyJson(string clasify)
        {
            var clasified = clasify.ToLowerInvariant();

            if (clasified == "legendary")
            {
                Legendary = true;

                if (Size <= 0)
                    Size = Size;
                else
                    Size = _random.Next(Size + 200, Size + 300);

                MaximumHP = MaximumHP * 3;
                HP = MaximumHP;
                GlowEnemy = 0xEAC117;
            }
            else if (clasified == "epic")
            {
                Epic = true;

                if (Size <= 0)
                    Size = Size;
                else
                    Size = _random.Next(Size + 100, Size + 200);

                MaximumHP = MaximumHP * 2;
                HP = MaximumHP;
                GlowEnemy = 0x4B0082;
            }
            else if (clasified == "rare")
            {
                Rare = true;

                if (Size <= 0)
                    Size = Size;
                else
                    Size = _random.Next(Size, Size + 100);

                GlowEnemy = 0xFFFFFF;
            }
        }

        public int Damage(Player from, TickData time, int dmg, bool noDef, bool itsPoison = false, params ConditionEffect[] effs)
        {
            if (stat)
                return 0;

            if (!itsPoison && HasConditionEffect(ConditionEffects.Invincible))
                return 0;

            if (!HasConditionEffect(ConditionEffects.Paused) && !HasConditionEffect(ConditionEffects.Stasis))
            {
                var def = ObjectDesc.Defense;

                if (noDef)
                    def = 0;

                var dmgd = (int)StatsManager.GetDefenseDamage(this, dmg, def);

                var effDmg = dmgd;

                if (effDmg > HP)
                    effDmg = HP;

                if (!HasConditionEffect(ConditionEffects.Invulnerable))
                    HP -= effDmg;

                ApplyConditionEffect(effs);
                Owner.BroadcastIfVisible(new Damage()
                {
                    TargetId = Id,
                    Effects = 0,
                    DamageAmount = (ushort)effDmg,
                    Kill = HP < 0,
                    BulletId = 0,
                    ObjectId = from.Id
                }, this, PacketPriority.Low);

                DamageCounter?.HitBy(from, time, null, effDmg);

                if (HP < 0 && Owner != null)
                    Death(time);

                return effDmg;
            }

            return 0;
        }

        public void Death(TickData time)
        {
            if (this == null || Owner == null)
                return;

            DamageCounter.Death(time);

            if (CurrentState != null)
                CurrentState.OnDeath(new BehaviorEventArgs(this, time));

            OnDeath?.Invoke(this, new BehaviorEventArgs(this, time));
            Owner.LeaveWorld(this);
        }

        public override bool HitByProjectile(Projectile projectile, TickData time)
        {
            if (stat)
                return false;

            if (projectile == null)
                return false;

            if (HasConditionEffect(ConditionEffects.Invincible))
                return false;

            if (projectile.ProjectileOwner is Player && !HasConditionEffect(ConditionEffects.Paused) && !HasConditionEffect(ConditionEffects.Stasis))
            {
                var player = projectile.ProjectileOwner as Player;
                var Inventory = player.Inventory;
                var def = ObjectDesc.Defense;

                if (projectile.ProjDesc.ArmorPiercing)
                    def = 0;

                var dmg = (int)StatsManager.GetDefenseDamage(this, projectile.Damage, def);

                if (!HasConditionEffect(ConditionEffects.Invulnerable))
                    HP -= dmg;

                for (var i = 0; i < 4; i++)
                {
                    
                    var item = Inventory[i];
                    if (item == null || !item.Legendary && !item.Revenge && !item.Mythical && !item.Eternal)
                        continue;

                    if (item.Demonized)
                        Demonized(player, i);

                    if (item.Electrify)
                        Electrify(player, i, time, this);

                }

                ApplyConditionEffect(projectile.ProjDesc.Effects);

                Owner.BroadcastIfVisibleExclude(new Damage()
                {
                    TargetId = Id,
                    Effects = projectile.ConditionEffects,
                    DamageAmount = (ushort)dmg,
                    Kill = HP < 0,
                    BulletId = projectile.ProjectileId,
                    ObjectId = projectile.ProjectileOwner.Self.Id
                }, this, projectile.ProjectileOwner as Player, PacketPriority.Low);

                DamageCounter.HitBy(projectile.ProjectileOwner as Player, time, projectile, dmg);

                if (HP < 0 && Owner != null)
                    Death(time);

                return true;
            }

            return false;
        }

        public override void Init(World owner)
        {
            base.Init(owner);

            if (ObjectDesc.StunImmune)
                ApplyConditionEffect(new ConditionEffect()
                {
                    Effect = ConditionEffectIndex.StunImmune,
                    DurationMS = -1
                });

            if (ObjectDesc.StasisImmune)
                ApplyConditionEffect(new ConditionEffect()
                {
                    Effect = ConditionEffectIndex.StasisImmune,
                    DurationMS = -1
                });

            ClasifyEnemy();
        }

        public override void Tick(TickData time)
        {
            if (pos == null)
                pos = new Position() { X = X, Y = Y };

            if (HP == 0)
                Death(time);

            if (!stat && HasConditionEffect(ConditionEffects.Bleeding))
            {
                if (bleeding > 1)
                {
                    HP -= (int)bleeding;
                    bleeding -= (int)bleeding;
                }

                bleeding += 28 * time.DeltaTime;
            }

            base.Tick(time);
        }

        private void Demonized(Player player, int slot)
        {
            if (player == null || player.Owner == null || player.Client == null)
                return;
            if (_random.NextDouble() < 0.3 && player.ApplyEffectCooldown(slot))
            {
                player.setCooldownTime(4, slot);

                ApplyConditionEffect(ConditionEffectIndex.Curse, 5000);

                player.Owner.BroadcastIfVisible(new ShowEffect()
                {
                    EffectType = EffectType.AreaBlast,
                    TargetObjectId = Id,
                    Color = new ARGB(0xFFFFFF00),
                    Pos1 = new Position() { X = 1.5f }
                }, player, PacketPriority.Low);
                player.Owner.BroadcastIfVisible(new Notification()
                {
                    ObjectId = Id,
                    PlayerId = player.Id,
                    Message = "Demonized!",
                    Color = new ARGB(0xFFFF0000)
                }, player, PacketPriority.Low);
            }
        }

        private void Electrify(Player player, int slot, TickData time, Entity firstHit)
        {
            if (player == null || player.Owner == null || player.Client == null)
                return;
            if (_random.NextDouble() < 0.03)
            {
                var current = firstHit;
                var targets = new Entity[5];

                for (var i = 0; i < targets.Length; i++)
                {
                    targets[i] = current;

                    var next = current.GetNearestEntity(10, false, e =>
                    {
                        if (!(e is Enemy) || e.HasConditionEffect(ConditionEffects.Invincible) || e.HasConditionEffect(ConditionEffects.Stasis) || Array.IndexOf(targets, e) != -1)
                            return false;

                        return true;
                    });

                    if (next == null)
                        break;

                    current = next;
                }

                for (var i = 0; i < targets.Length; i++)
                {
                    if (targets[i] == null)
                        break;

                    var damage = 1000;

                    (targets[i] as Enemy).Damage(player, time, damage, false);
                    targets[i].ApplyConditionEffect(new ConditionEffect()
                    {
                        Effect = ConditionEffectIndex.Slowed,
                        DurationMS = 3000
                    });

                    var prev = i == 0 ? player : targets[i - 1];
                    var notprev = targets[i];
                    var pkts = new List<OutgoingMessage>
                    {
                        new ShowEffect()
                        {
                            EffectType = EffectType.Lightning,
                            TargetObjectId = prev.Id,
                            Color = new ARGB(0xFFFFFF00),
                            Pos1 = new Position()
                            {
                                X = targets[i].X,
                                Y = targets[i].Y
                            },
                            Pos2 = new Position() { X = 350 }
                        },
                        new Notification
                        {
                            Color = new ARGB(0xFFFFFF00),
                            ObjectId = notprev.Id,
                            PlayerId = player.Id,
                            Message = "Electrified!"
                        }
                    };

                    Owner.BroadcastIfVisible(pkts[0], player, PacketPriority.Low);
                    Owner.BroadcastIfVisible(pkts[1], player, PacketPriority.Low);
                }
            }
        }
    }
}
