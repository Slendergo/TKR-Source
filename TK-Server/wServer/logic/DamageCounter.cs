using common;
using System;
using System.Collections.Generic;
using System.Linq;
using wServer.core;
using wServer.core.objects;
using wServer.core.worlds.logic;

namespace wServer.logic
{
    public class DamageCounter
    {
        private WeakDictionary<Player, int> Hitters;

        public Enemy Host;

        public DamageCounter(Enemy enemy) => Host = enemy;

        public DamageCounter Corpse { get; set; }
        public Player LastHitter { get; private set; }
        public Projectile LastProjectile { get; private set; }
        public DamageCounter Parent { get; set; }
        public int TotalDamage { get; private set; }

        public WeakDictionary<Player, int> GetHitters()
        {
            if (Hitters == null)
                Hitters = new WeakDictionary<Player, int>();
            return Hitters;
        }

        private bool Dead;
        public void Death(TickTime time)
        {
            if (Dead)
                return;
            Dead = true;

            if (Corpse != null)
            {
                Corpse.Parent = this;
                return;
            }

            var enemy = (Parent ?? this).Host;

            if (enemy.World is RealmWorld)
                (enemy.World as RealmWorld).EnemyKilled(enemy, (Parent ?? this).LastHitter);

            var lastHitPlayer_ = (Parent ?? this).LastHitter;

            if (lastHitPlayer_ != null)
            {
                var account = lastHitPlayer_.GameServer.Database.GetAccount(lastHitPlayer_.AccountId);
                account.EnemiesKilled++;
                account.FlushAsync();
                if (account.EnemiesKilled % 1000 == 0)
                {
                    var guild = lastHitPlayer_.GameServer.Database.GetGuild(account.GuildId);
                    if (guild != null)
                    {
                        lastHitPlayer_.SendInfo("Congratulations! You just killed another 1000 enemies! Reward: 1 Guild point.");
                        guild.GuildPoints += 1;
                        guild.FlushAsync();
                    }
                }
            }

            var lvlUps = 0;
            foreach (var player in enemy.World.Players.Values)
            {
                if (player.Dist(enemy) > 25)
                    continue;

                var level = player.Level;
                if (player.HasConditionEffect(common.resources.ConditionEffectIndex.Paused))
                    continue;

                var xp = (enemy.ObjectDesc.MaxHP / 10.0f) * enemy.ObjectDesc.ExpMultiplier;
                var upperLimit = player.ExperienceGoal * 0.1f;
                if (player.Quest == enemy)
                    upperLimit = player.ExperienceGoal * 0.5f;

                float playerXp;
                if (upperLimit < xp)
                    playerXp = upperLimit;
                else
                    playerXp = xp;

                if (player.Level < 20 && player.XPBoostTime != 0)
                    playerXp *= 1.5f;

                if (enemy.GivesNoXp)
                    playerXp = 0;

                var killer = (Parent ?? this).LastHitter == player;
                if (player.EnemyKilled(enemy, (int)playerXp, killer) && !killer)
                    lvlUps++;

                if (enemy.ObjectDesc.Quest)
                {
                    if (enemy.HP > 3000)
                    {
                        var essenceToGive = player.World.Random.Next(25, 100);
                        if (essenceToGive > 0)
                            player.GiveEssence(essenceToGive);
                    }
                }
            }

            if ((Parent ?? this).LastHitter != null)
                (Parent ?? this).LastHitter.FameCounter.LevelUpAssist(lvlUps);
        }

        public Tuple<Player, int>[] GetPlayerData()
        {
            if (Parent != null)
                return Parent.GetPlayerData();
            List<Tuple<Player, int>> dat = new List<Tuple<Player, int>>();

            var hitters = GetHitters();
            foreach (var i in hitters)
            {
                if (i.Key == null || i.Key.World == null || i.Key.World.GetEntity(i.Key.Id) == null) continue;

                dat.Add(new Tuple<Player, int>(i.Key, i.Value));
            }
            return dat.ToArray();
        }

        public void HitBy(Player player, TickTime time, Projectile projectile, int dmg)
        {
            var hitters = GetHitters();
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
            
            var hitters = GetHitters();
            foreach (var plr in hitters.Keys)
            {
                if (!hitters.TryGetValue(plr, out var totalDmg))
                    totalDmg = 0;

                var dch = dc.GetHitters();
                if (!dch.TryGetValue(plr, out int totalExistingDmg))
                    totalExistingDmg = 0;
                dch[plr] = totalDmg + totalExistingDmg;
            }
        }
    }
}
