using common;
using common.database;
using wServer.core;
using wServer.core.objects;

namespace wServer.logic
{
    public class FameCounter
    {
        public Player Host;
        private int elapsed = 0;

        public FameCounter(Player player)
        {
            Host = player;
            Stats = FameStats.Read(player.Client.Character.FameStats);
            ClassStats = new DbClassStats(player.Client.Account);
        }

        public DbClassStats ClassStats { get; private set; }
        public FameStats Stats { get; private set; }

        public void CompleteDungeon(string name)
        {
            switch (name)
            {
                case "PirateCave":
                    Stats.PirateCavesCompleted++;
                    break;

                case "Undead Lair":
                    Stats.UndeadLairsCompleted++;
                    break;

                case "Abyss":
                    Stats.AbyssOfDemonsCompleted++;
                    break;

                case "Snake Pit":
                    Stats.SnakePitsCompleted++;
                    break;

                case "Spider Den":
                    Stats.SpiderDensCompleted++;
                    break;

                case "Sprite World":
                    Stats.SpriteWorldsCompleted++;
                    break;

                case "Tomb":
                    Stats.TombsCompleted++;
                    break;

                case "OceanTrench":
                    Stats.TrenchesCompleted++;
                    break;

                case "Forbidden Jungle":
                    Stats.JunglesCompleted++;
                    break;

                case "Manor of the Immortals":
                    Stats.ManorsCompleted++;
                    break;
            }
        }

        public void DrinkPot() => Stats.PotionsDrunk++;

        public void Hit(Projectile proj, Enemy enemy) => Stats.ShotsThatDamage++;

        public void Killed(Enemy enemy, bool killer)
        {
            if (enemy.ObjectDesc.God)
                Stats.GodAssists++;
            else
                Stats.MonsterAssists++;
            if (Host.Quest == enemy)
                Stats.QuestsCompleted++;
            if (killer)
            {
                if (enemy.ObjectDesc.God)
                    Stats.GodKills++;
                else
                    Stats.MonsterKills++;

                if (enemy.ObjectDesc.Cube)
                    Stats.CubeKills++;
                if (enemy.ObjectDesc.Oryx)
                    Stats.OryxKills++;
            }
        }

        public void LevelUpAssist(int count) => Stats.LevelUpAssists += count;

        public void Shoot(Projectile proj) => Stats.Shots++;

        public void Teleport() => Stats.Teleports++;

        public void Tick(TickData time)
        {
            elapsed += time.ElaspedMsDelta;

            if (elapsed > 60000)
            {
                elapsed -= 60000;
                Stats.MinutesActive++;
            }
        }

        public void TileSent(int num) => Stats.TilesUncovered += num;

        public void UseAbility() => Stats.SpecialAbilityUses++;
    }
}
