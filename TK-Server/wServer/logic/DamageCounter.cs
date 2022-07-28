using CA.Extensions.Concurrent;
using common;
using System;
using System.Collections.Generic;
using wServer.core;
using wServer.core.objects;
using wServer.core.worlds.logic;

namespace wServer.logic
{
    public class DamageCounter
    {
        public WeakDictionary<Player, int> hitters = new WeakDictionary<Player, int>();

        public Enemy Host;

        public DamageCounter(Enemy enemy) => Host = enemy;

        public DamageCounter Corpse { get; set; }
        public Player LastHitter { get; private set; }
        public Projectile LastProjectile { get; private set; }
        public DamageCounter Parent { get; set; }
        public int TotalDamage { get; private set; }

        public void Death(TickData time)
        {
            if (Corpse != null)
            {
                Corpse.Parent = this;
                return;
            }

            var enemy = (Parent ?? this).Host;

            if (enemy.Owner is Realm)
                (enemy.Owner as Realm).EnemyKilled(enemy, (Parent ?? this).LastHitter);

            var lastHitPlayer_ = (Parent ?? this).LastHitter;

            if (lastHitPlayer_ != null)
            {
                var account = lastHitPlayer_.CoreServerManager.Database.GetAccount(lastHitPlayer_.AccountId);
                account.EnemiesKilled++;
                account.FlushAsync();
                if (account.EnemiesKilled % 1000 == 0)
                {
                    var guild = lastHitPlayer_.CoreServerManager.Database.GetGuild(account.GuildId);
                    if (guild != null)
                    {
                        lastHitPlayer_.SendInfo("Congratulations! You just killed another 1000 enemies! Reward: 1 Guild point.");
                        guild.GuildPoints += 1;
                        guild.FlushAsync();
                    }
                }
            }

            var lvlUps = 0;
            var players = enemy.Owner.Players.ValueWhereAsParallel(_ => enemy.Dist(_) < 25d);
            foreach (var player in players)
            {
                var level = player.Level;
                var rank = player.Rank;
                if (player.HasConditionEffect(common.resources.ConditionEffects.Paused))
                    continue;

                var xp = enemy.ObjectDesc.MaxHP / 15f * enemy.ObjectDesc.ExpMultiplier;
                var upperLimit = player.ExperienceGoal * 0.1f;
                if (player.Quest == enemy)
                    upperLimit = player.ExperienceGoal * 0.5f;

                float playerXp;
                if (upperLimit < xp)
                    playerXp = upperLimit;
                else
                    playerXp = xp;

                if (player.Level < 20)
                {
                    playerXp *= 4f;
                    switch (player.Rank)
                    {
                        case Player.DONOR_1: playerXp *= 1.5f; break;
                        case Player.DONOR_2: playerXp *= 2f; break;
                        case Player.DONOR_3: playerXp *= 2.5f; break;
                        case Player.DONOR_4: playerXp *= 3f; break;
                        case Player.DONOR_5: playerXp *= 3.5f; break;
                        case Player.VIP: playerXp *= 4f; break;
                    }
                }

                if (player.Level < 20 && player.XPBoostTime != 0)
                {
                    playerXp *= 1.5f;
                }

                playerXp *= (float)player.CoreServerManager.GetExperienceRate();

                var enemyClasified = enemy.Legendary ? 1.75f : enemy.Epic ? 1.50f : 1.0f;
                playerXp *= enemyClasified;

                if (enemy.GivesNoXp)
                    playerXp = 0;

                var killer = (Parent ?? this).LastHitter == player;
                if (player.EnemyKilled(enemy, (int)playerXp, killer) && !killer)
                    lvlUps++;
            }

            if ((Parent ?? this).LastHitter != null)
                (Parent ?? this).LastHitter.FameCounter.LevelUpAssist(lvlUps);
        }

        public Tuple<Player, int>[] GetPlayerData()
        {
            if (Parent != null)
                return Parent.GetPlayerData();
            List<Tuple<Player, int>> dat = new List<Tuple<Player, int>>();
            foreach (var i in hitters)
            {
                if (i.Key == null || i.Key.Owner == null || i.Key.Owner.GetEntity(i.Key.Id) == null) continue;

                dat.Add(new Tuple<Player, int>(i.Key, i.Value));
            }
            return dat.ToArray();
        }

        public void HitBy(Player player, TickData time, Projectile projectile, int dmg)
        {
            if (!hitters.TryGetValue(player, out int totalDmg))
                totalDmg = 0;

            int trueDmg = Math.Min(dmg, Host.MaximumHP);
            TotalDamage += trueDmg;
            hitters[player] = totalDmg + trueDmg;

            LastProjectile = projectile;
            LastHitter = player;

            player.FameCounter.Hit(projectile, Host);
        }

        public void TransferData(DamageCounter dc)
        {
            dc.LastProjectile = LastProjectile;
            dc.LastHitter = LastHitter;
            dc.TotalDamage = TotalDamage;

            foreach (var plr in hitters.Keys)
            {
                if (!hitters.TryGetValue(plr, out int totalDmg))
                    totalDmg = 0;
                if (!dc.hitters.TryGetValue(plr, out int totalExistingDmg))
                    totalExistingDmg = 0;

                dc.hitters[plr] = totalDmg + totalExistingDmg;
            }
        }
    }
}
