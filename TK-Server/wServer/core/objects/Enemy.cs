using common.resources;
using System;
using System.Collections.Generic;
using wServer.core.worlds;
using wServer.logic;
using wServer.networking;
using wServer.networking.packets.outgoing;
using common;

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
        
        protected SV<int> _defense;
        public int Defense
        {
            get => _defense.GetValue(); 
            set => _defense.SetValue(value);
        }

        private SV<int> _glowcolor;
        public int GlowEnemy
        {
            get => _glowcolor.GetValue(); 
            set => _glowcolor.SetValue(value);
        }


        public Enemy(GameServer manager, ushort objType) : base(manager, objType)
        {
            _defense = new SV<int>(this, StatDataType.Defense, ObjectDesc.Defense);
            stat = ObjectDesc.MaxHP == 0;
            DamageCounter = new DamageCounter(this);
            _glowcolor = new SV<int>(this, StatDataType.GlowEnemy, 0);
        }

        protected override void ExportStats(IDictionary<StatDataType, object> stats, bool isOtherPlayer)
        {
            base.ExportStats(stats, isOtherPlayer);
            stats[StatDataType.GlowEnemy] = GlowEnemy;
        }

        public bool Epic { get; set; }
        public bool Legendary { get; set; }
        public bool Rare { get; set; }
        public Position SpawnPoint { get { return pos ?? new Position() { X = X, Y = Y }; } }
        public TerrainType Terrain { get; set; }

        public void ClasifyEnemy()
        {
            var chance = World.Random.NextDouble();

            if(chance < 0.2)
            {
                var type = World.Random.Next(0, 3);
                switch (type)
                {
                    case 2:
                        {
                            Legendary = true;
                            GlowEnemy = 0xFFFFFF;
                        }
                        break;
                    case 1:
                        {
                            Epic = true;
                            GlowEnemy = 0x4B0082;
                        }
                        break;
                    case 0:
                        {
                            Rare = true;
                            GlowEnemy = 0xEAC117;
                        }
                        break;
                }

                Size += (type + 1) * 25;
                MaximumHP *= (type + 1);
                HP = MaximumHP;
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
                    Size = World.Random.Next(Size + 200, Size + 300);

                MaximumHP = MaximumHP * 3;
                HP = MaximumHP;
                Defense += 10;
                GlowEnemy = 0xFFFFFF;
            }
            else if (clasified == "epic")
            {
                Epic = true;

                if (Size <= 0)
                    Size = Size;
                else
                    Size = World.Random.Next(Size + 100, Size + 200);

                MaximumHP = MaximumHP * 2;
                HP = MaximumHP;
                Defense += 5;
                GlowEnemy = 0x4B0082;
            }
            else if (clasified == "rare")
            {
                Rare = true;

                if (Size <= 0)
                    Size = Size;
                else
                    Size = World.Random.Next(Size, Size + 100);
                Defense += 2;
                GlowEnemy = 0xEAC117;
            }
        }

        public int Damage(Player from, TickTime time, int dmg, bool noDef, bool itsPoison = false, params ConditionEffect[] effs)
        {
            if (stat)
                return 0;

            if (!itsPoison && HasConditionEffect(ConditionEffectIndex.Invincible))
                return 0;

            if (!HasConditionEffect(ConditionEffectIndex.Paused) && !HasConditionEffect(ConditionEffectIndex.Stasis))
            {
                var def = Defense;
                var dmgd = (int)StatsManager.DamageWithDefense(this, dmg, noDef, def);

                var effDmg = dmgd;

                if (effDmg > HP)
                    effDmg = HP;

                if (!HasConditionEffect(ConditionEffectIndex.Invulnerable))
                    HP -= effDmg;

                ApplyConditionEffect(effs);
                World.BroadcastIfVisible(new Damage()
                {
                    TargetId = Id,
                    Effects = 0,
                    DamageAmount = (ushort)effDmg,
                    Kill = HP < 0,
                    BulletId = 0,
                    ObjectId = from.Id
                }, this);

                DamageCounter?.HitBy(from, time, null, effDmg);

                if (HP < 0 && World != null)
                    Death(ref time);

                return effDmg;
            }

            return 0;
        }

        private bool Dead;
        public void Death(ref TickTime time)
        {
            Dead = true;
            DamageCounter.Death(time);
            CurrentState?.OnDeath(this, ref time);
            if (GameServer.BehaviorDb.Definitions.TryGetValue(ObjectType, out var loot))
                loot.Item2?.Handle(this, time);
            World.LeaveWorld(this);
        }

        public override bool HitByProjectile(Projectile projectile, TickTime time)
        {
            if (stat)
                return false;

            if (projectile == null)
                return false;

            if (HasConditionEffect(ConditionEffectIndex.Invincible))
                return false;

            if (projectile.Host is Player && !HasConditionEffect(ConditionEffectIndex.Paused) && !HasConditionEffect(ConditionEffectIndex.Stasis))
            {
                var player = projectile.Host as Player;
                var Inventory = player.Inventory;
                var def = Defense;

                var dmg = StatsManager.DamageWithDefense(this, projectile.Damage, projectile.ProjDesc.ArmorPiercing, def);
                if (!HasConditionEffect(ConditionEffectIndex.Invulnerable))
                    HP -= dmg;
                
                for (var i = 0; i < 4; i++)
                {
                    var item = Inventory[i];

                    if (item == null || !item.Legendary && !item.Revenge && !item.Mythical && !item.Eternal)
                        continue;

                    if (item.Demonized)
                        Demonized(player, i);

                    if (item.Vampiric)
                        VampireBlast(player, i, time, this, projectile.ProjDesc.MultiHit);

                    if (item.Electrify)
                        Electrify(player, i, time, this);
                }

                ApplyConditionEffect(projectile.ProjDesc.Effects);

                World.BroadcastIfVisibleExclude(new Damage()
                {
                    TargetId = Id,
                    Effects = 0,
                    DamageAmount = (ushort)dmg,
                    Kill = HP < 0,
                    BulletId = projectile.ProjectileId,
                    ObjectId = projectile.Host.Id
                }, this, projectile.Host as Player);

                DamageCounter.HitBy(projectile.Host as Player, time, projectile, dmg);

                if (HP < 0 && World != null)
                    Death(ref time);

                return true;
            }

            return false;
        }

        public override void Init(World owner)
        {
            base.Init(owner);

            if (ObjectDesc.Quest || ObjectDesc.Hero || ObjectDesc.Encounter)
                ClasifyEnemy();
        }

        public override void Tick(ref TickTime time)
        {
            if (pos == null)
                pos = new Position() { X = X, Y = Y };

            if (HP == 0 && !Dead)
                Death(ref time);

            if (!stat && HasConditionEffect(ConditionEffectIndex.Bleeding))
            {
                if (bleeding > 1)
                {
                    HP -= (int)bleeding;
                    bleeding -= (int)bleeding;
                }

                bleeding += 28 * time.DeltaTime;
            }

            base.Tick(ref time);
        }

        private void Demonized(Player player, int slot)
        {
            if (player == null || player.World == null || player.Client == null)
                return;
            if (World.Random.NextDouble() < 0.3 && player.ApplyEffectCooldown(slot))
            {
                player.setCooldownTime(4, slot);

                ApplyConditionEffect(ConditionEffectIndex.Curse, 5000);

                player.World.BroadcastIfVisible(new ShowEffect()
                {
                    EffectType = EffectType.AreaBlast,
                    TargetObjectId = Id,
                    Color = new ARGB(0xFFFFFF00),
                    Pos1 = new Position() { X = 1.5f }
                }, player);
                player.World.BroadcastIfVisible(new Notification()
                {
                    ObjectId = Id,
                    PlayerId = player.Id,
                    Message = "Demonized!",
                    Color = new ARGB(0xFFFF0000)
                }, player);
            }
        }

        private void VampireBlast(Player player, int slot, TickTime time, Entity firstHit, bool multi)
        {
            if (player == null || player.World == null || player.Client == null)
                return;
            var chance = 0.03;
            chance -= (player.Inventory[0].NumProjectiles / 3) / 100;
            chance = multi ? chance / 1.5 : chance;
            if (World.Random.NextDouble() < chance)
            {

                Position procPos = new Position() { X = firstHit.X, Y = firstHit.Y };
                Position playerPos = new Position() { X = player.X, Y = player.Y };
                var pkts = new List<OutgoingMessage>()
                {
                    new ShowEffect()
                    {
                        EffectType = EffectType.Trail,
                        TargetObjectId = firstHit.Id,
                        Pos1 = playerPos,
                        Color = new ARGB(0xFFFF0000)
                    },
                    new ShowEffect
                    {
                        EffectType = EffectType.Diffuse,
                        Color = new ARGB(0xFFFF0000),
                        TargetObjectId = Id,
                        Pos1 = procPos,
                        Pos2 = new Position { X = firstHit.X + 3, Y = firstHit.Y }
                    },
                        new Notification
                        {
                            Color = new ARGB(0xFFD336B3),
                            ObjectId = firstHit.Id,
                            PlayerId = player.Id,
                            Message = "Vampiric!"
                        }
                };

                World.BroadcastIfVisible(pkts[0], ref procPos);
                World.BroadcastIfVisible(pkts[1], ref procPos);

                var totalDmg = 300;
                var enemies = new List<Enemy>();

                World.AOE(procPos, 3, false, enemy =>
                {
                    enemies.Add(enemy as Enemy);
                    totalDmg += (enemy as Enemy).Damage(player, time, (int)totalDmg, false);
                });

                if (!player.HasConditionEffect(ConditionEffectIndex.Sick))
                    ActivateHealHp(player, 50);

                if (player.HP < player.MaximumHP && enemies.Count > 0)
                {
                    for (var i = 0; i < 5; i++)
                    {
                        var a = player.World.Random.NextLength(enemies);

                        World.BroadcastIfVisible(new ShowEffect()
                        {
                            EffectType = EffectType.Flow,
                            TargetObjectId = player.Id,
                            Pos1 = new Position() { X = a.X, Y = a.Y },
                            Color = new ARGB(0xffffffff)
                        }, ref playerPos);
                    }
                }
            }
        }
        private static void ActivateHealHp(Player player, int amount)
        {
            if (amount <= 0)
                return;

            var maxHp = player.Stats[0];
            var newHp = Math.Min(maxHp, player.HP + amount);
            if (newHp == player.HP)
                return;

            player.World.BroadcastIfVisible(new ShowEffect()
            {
                EffectType = EffectType.Potion,
                TargetObjectId = player.Id,
                Color = new ARGB(0xffffffff)
            }, player);
            player.World.BroadcastIfVisible(new Notification()
            {
                Color = new ARGB(0xff00ff00),
                ObjectId = player.Id,
                Message = "+" + (newHp - player.HP)
            }, player);

            player.HP = newHp;
        }

        private void Electrify(Player player, int slot, TickTime time, Entity firstHit)
        {
            if (player == null || player.World == null || player.Client == null)
                return;
            if (World.Random.NextDouble() < 0.03)
            {
                var current = firstHit;
                var targets = new Entity[5];

                for (var i = 0; i < targets.Length; i++)
                {
                    targets[i] = current;

                    var next = current.GetNearestEntity(10, false, e =>
                    {
                        if (!(e is Enemy) || e.HasConditionEffect(ConditionEffectIndex.Invincible) || e.HasConditionEffect(ConditionEffectIndex.Stasis) || Array.IndexOf(targets, e) != -1)
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
                    targets[i].ApplyConditionEffect(ConditionEffectIndex.Slowed, 3000);

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

                    World.BroadcastIfVisible(pkts[0], player);
                    World.BroadcastIfVisible(pkts[1], player);
                }
            }
        }
    }
}
