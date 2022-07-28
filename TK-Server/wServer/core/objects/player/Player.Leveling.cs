using CA.Extensions.Concurrent;
using System;
using System.Linq;
using wServer.core.worlds.logic;
using wServer.networking;
using wServer.networking.packets.outgoing;

namespace wServer.core.objects
{
    public partial class Player
    {
        public Entity AvatarQuest { get; private set; }
        public Entity CrystalQuest { get; private set; }
        public Entity Quest { get; private set; }
        public Entity SpookyQuest { get; private set; }
        public Entity JuliusQuest { get; private set; }

        public static int GetExpGoal(int level) => 50 + (level - 1) * 100;

        public static int GetFameGoal(int fame)
        {
            if (fame >= 2000) return 0;
            else if (fame >= 800) return 2000;
            else if (fame >= 400) return 800;
            else if (fame >= 150) return 400;
            else if (fame >= 20) return 150;
            else return 20;
        }

        public static int GetLevelExp(int level)
        {
            if (level == 1)
                return 0;

            return 50 * (level - 1) + (level - 2) * (level - 1) * 50;
        }

        public void CalculateFame()
        {
            var newFame = (Experience < 200 * 1000) ? Experience / 1000 : 200 + (Experience - 200 * 1000) / 1000;

            if (newFame == Fame)
                return;

            var Stats = FameCounter.ClassStats[ObjectType];
            var newGoal = GetFameGoal(Stats.BestFame > newFame ? Stats.BestFame : newFame);

            if (newGoal > FameGoal)
            {
                Owner.BroadcastIfVisible(new Notification()
                {
                    ObjectId = Id,
                    Color = new ARGB(0xFF00FF00),
                    Message = "Quest Complete!"
                }, this, PacketPriority.Low);
                Stars = GetStars();
            }
            else if (newFame != Fame)
            {
                Owner.BroadcastIfVisible(new Notification()
                {
                    ObjectId = Id,
                    Color = new ARGB(0xFFE25F00),
                    Message = "+" + (newFame - Fame) + "Fame"
                }, this, PacketPriority.Low);
            }

            Fame = newFame;
            FameGoal = newGoal;
        }

        public void CheckForEncounter(Position? destination = null)
        {
            var newQuest = FindQuest(destination);
            if (newQuest != null && newQuest != Quest)
            {
                Client.SendPacket(new QuestObjId()
                {
                    ObjectId = newQuest.Id
                });
                Quest = newQuest;
            }
        }

        public bool CheckLevel() => CheckLevelUp();

        public bool EnemyKilled(Enemy enemy, int exp, bool killer)
        {
            if (enemy == Quest)
            {
                Owner.BroadcastIfVisible(new Notification()
                {
                    ObjectId = Id,
                    Color = new ARGB(0xFF00FF00),
                    Message = "Quest Complete!"
                }, this, PacketPriority.Low);
                Quest = null;
            }

            if (exp != 0)
                Experience += exp;

            FameCounter.Killed(enemy, killer);
            return CheckLevelUp();
        }

        public int GetStars()
        {
            int ret = 0;
            foreach (var i in FameCounter.ClassStats.AllKeys)
            {
                var entry = FameCounter.ClassStats[ushort.Parse(i)];
                if (entry.BestFame >= 2000) ret += 5;
                else if (entry.BestFame >= 800) ret += 4;
                else if (entry.BestFame >= 400) ret += 3;
                else if (entry.BestFame >= 150) ret += 2;
                else if (entry.BestFame >= 20) ret += 1;
            }
            return ret;
        }

        public void HandleQuest(TickData time, bool force = false, Position? destination = null)
        {
            if (Owner is Realm)
                CheckForEncounter();

            if (force || Quest == null || Quest.Owner == null || time.TickCount % 500 == 0)
                CheckForEncounter();
        }

        public void HandleSpecialEnemies(TickData time, bool force = false)
        {
            if (this == null || Owner == null || Owner.SpecialEnemies == null || time.TickCount % 500 != 0)
                return;

            if (force || SpookyQuest == null || AvatarQuest == null || CrystalQuest == null || JuliusQuest == null)
            {
                var newSpooky = FindSpecialEnemy("Spectral Sentry");
                if (newSpooky != null && newSpooky != SpookyQuest)
                    SpookyQuest = newSpooky;

                var newAvatar = FindSpecialEnemy("shtrs Defense System");
                if (newAvatar != null && newAvatar != AvatarQuest)
                    AvatarQuest = newAvatar;

                var newCrystal = FindSpecialEnemy("Crystal Prisoner");
                if (newCrystal != null && newCrystal != CrystalQuest)
                    CrystalQuest = newCrystal;

                var newJulius = FindSpecialEnemy("Julius Caesar");
                if (newJulius != null && newJulius != JuliusQuest)
                    JuliusQuest = newJulius;
            }
        }

        private bool CheckLevelUp()
        {
            var statInfo = CoreServerManager.Resources.GameData.Classes[ObjectType].Stats;
            if (Experience - GetLevelExp(Level) >= ExperienceGoal && Level < 20)
            {
                Level++;
                ExperienceGoal = GetExpGoal(Level);
                var rand = new Random();
                for (var i = 0; i < statInfo.Length; i++)
                {
                    if (Level < 20)
                    {
                        switch (UpgradeEnabled)
                        {
                            case true:
                                if (i == 0 || i == 1 ? Stats.Base[i] >= statInfo[i].MaxValue + 50 : Stats.Base[i] >= statInfo[i].MaxValue + 10)
                                    Stats.Base[i] = (i == 0 || i == 1) ? statInfo[i].MaxValue + 50 : statInfo[i].MaxValue + 10;
                                break;

                            case false:
                                if (Stats.Base[i] >= statInfo[i].MaxValue)
                                    Stats.Base[i] = statInfo[i].MaxValue;
                                break;
                        }
                    }
                    var min = statInfo[i].MinIncrease;
                    var max = statInfo[i].MaxIncrease + 1;
                    Stats.Base[i] += rand.Next(min, max);
                    if (Stats.Base[i] > statInfo[i].MaxValue && !UpgradeEnabled)
                        Stats.Base[i] = statInfo[i].MaxValue;
                }
                HP = Stats[0];
                MP = Stats[1];

                if (Level == 20)
                    Owner.PlayersBroadcastAsParallel(_ => _.SendInfo($"{Name} achieved level 20"));
                else
                    // to get exp scaled to new exp goal
                    InvokeStatChange(StatDataType.Experience, Experience - GetLevelExp(Level), true);

                Quest = null;
                return true;
            }

            for (var i = 0; i < statInfo.Length; i++)
            {
                if (Level >= 20)
                {
                    switch (UpgradeEnabled)
                    {
                        case true:
                            if (i == 0 || i == 1 ? Stats.Base[i] >= statInfo[i].MaxValue + 50 : Stats.Base[i] >= statInfo[i].MaxValue + 10)
                                Stats.Base[i] = (i == 0 || i == 1) ? statInfo[i].MaxValue + 50 : statInfo[i].MaxValue + 10;
                            break;

                        case false:
                            if (Stats.Base[i] >= statInfo[i].MaxValue)
                                Stats.Base[i] = statInfo[i].MaxValue;
                            break;
                    }
                }
            }

            CalculateFame();
            return false;
        }

        private Entity FindQuest(Position? destination = null)
        {
            Entity ret = null;
            var pX = !destination.HasValue ? X : destination.Value.X;
            var pY = !destination.HasValue ? Y : destination.Value.Y;

            if (Owner == null || Owner.Quests == null)
                return null;

            double? bestScore = null;
            foreach (var entry in Owner.Quests)
            {
                var quest = entry.Value;
                if (quest.ObjectDesc == null || !quest.ObjectDesc.Quest)
                    continue;

                var maxVisibleLevel = Math.Min(quest.QuestLevel + 5, 20);
                var minVisibleLevel = Math.Max(quest.QuestLevel - 5, 1);
                var force = false;
                if (!(quest.Owner is Realm) && quest.ObjectDesc.Quest && !quest.ObjectDesc.Hero)
                    force = true;

                if (Level >= minVisibleLevel && Level <= maxVisibleLevel || force)
                {
                    var priority = quest.ObjectDesc.Encounter ? 3 : quest.ObjectDesc.Hero ? 2 : 1;
                    var score =
                        //priority * level diff
                        (20 - Math.Abs(quest.ObjectDesc.Level - Level)) * priority -
                        //minus 1 for every 100 tile distance
                        this.Dist(quest) / 100;

                    if (bestScore == null || score > bestScore)
                    {
                        bestScore = score;
                        ret = quest;
                    }
                }
            }
            return ret;
        }

        private Enemy FindSpecialEnemy(string objectId)
            => Owner.SpecialEnemies
                .ValueWhereAsParallel(_ => _ != null
                    && _.ObjectDesc != null
                    && _.ObjectDesc.ObjectId.Equals(objectId))
                .FirstOrDefault();
    }
}
