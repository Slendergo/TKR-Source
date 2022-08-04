using common;
using wServer.core;
using wServer.core.worlds.logic;
using wServer.networking;
using wServer.networking.packets.outgoing;

namespace wServer.core.net.handlers
{
    internal class SmallSkillTreeHandler : IMessageHandler
    {
        public override PacketId MessageId => PacketId.SMALLSKILLTREE;

        public override void Handle(Client client, NReader rdr, ref TickTime tickTime)
        {
            var skillNumber = rdr.ReadInt32();
            var removePoint = rdr.ReadBoolean();

            var player = client.Player;
            var chr = client.Character;
            if (player == null || client?.Player?.World is TestWorld)
                return;

            #region Check of things
            if (player.Node1TickMin > 5)
                player.Node1TickMin = 5;
            if (player.Node1TickMaj > 10)
            {
                player.Node1TickMaj = 10;
            }
            if (player.Node1Med > 3)
            {
                player.Node1Med = 3;
            }
            if (player.Node1Big > 2)
            {
                player.Node1Big = 2;
            }
            if (player.Node2TickMin > 5)
            {
                player.Node2TickMin = 5;
            }
            if (player.Node2TickMaj > 10)
            {
                player.Node2TickMaj = 10;
            }
            if (player.Node2Med > 3)
            {
                player.Node2Med = 3;
            }
            if (player.Node2Big > 2)
            {
                player.Node2Big = 2;
            }
            if (player.Node3TickMin > 5)
            {
                player.Node3TickMin = 5;
            }
            if (player.Node3TickMaj > 10)
            {
                player.Node3TickMaj = 10;
            }
            if (player.Node3Med > 3)
            {
                player.Node3Med = 3;
            }
            if (player.Node3Big > 2)
            {
                player.Node3Big = 2;
            }
            if (player.Node4TickMin > 5)
            {
                player.Node4TickMin = 5;
            }
            if (player.Node4TickMaj > 10)
            {
                player.Node4TickMaj = 10;
            }
            if (player.Node4Med > 3)
            {
                player.Node4Med = 3;
            }
            if (player.Node4Big > 2)
            {
                player.Node4Big = 2;
            }
            if (player.Node5TickMin > 5)
            {
                player.Node5TickMin = 5;
            }
            if (player.Node5TickMaj > 10)
            {
                player.Node5TickMaj = 10;
            }
            if (player.Node5Med > 3)
            {
                player.Node5Med = 3;
            }
            if (player.Node5Big > 2)
            {
                player.Node5Big = 2;
            }

            if (player.Node1TickMin < 0)
            {
                player.Node1TickMin = 0;
            }
            if (player.Node1TickMaj < 0)
            {
                player.Node1TickMaj = 0;
            }
            if (player.Node1Med < 0)
            {
                player.Node1Med = 0;
            }
            if (player.Node1Big < 0)
            {
                player.Node1Big = 0;
            }
            if (player.Node2TickMin < 0)
            {
                player.Node2TickMin = 0;
            }
            if (player.Node2TickMaj < 0)
            {
                player.Node2TickMaj = 0;
            }
            if (player.Node2Med < 0)
            {
                player.Node2Med = 0;
            }
            if (player.Node2Big < 0)
            {
                player.Node2Big = 0;
            }
            if (player.Node3TickMin < 0)
            {
                player.Node3TickMin = 0;
            }
            if (player.Node3TickMaj < 0)
            {
                player.Node3TickMaj = 0;
            }
            if (player.Node3Med < 0)
            {
                player.Node3Med = 0;
            }
            if (player.Node3Big < 0)
            {
                player.Node3Big = 0;
            }
            if (player.Node4TickMin < 0)
            {
                player.Node4TickMin = 0;
            }
            if (player.Node4TickMaj < 0)
            {
                player.Node4TickMaj = 0;
            }
            if (player.Node4Med < 0)
            {
                player.Node4Med = 0;
            }
            if (player.Node4Big < 0)
            {
                player.Node4Big = 0;
            }
            if (player.Node5TickMin < 0)
            {
                player.Node5TickMin = 0;
            }
            if (player.Node5TickMaj < 0)
            {
                player.Node5TickMaj = 0;
            }
            if (player.Node5Med < 0)
            {
                player.Node5Med = 0;
            }
            if (player.Node5Big < 0)
            {
                player.Node5Big = 0;
            }

            var newMaxedInt = 0;

            //Give 30 Skill Points on 8/8
            if (player.MaxedLife)
            {
                newMaxedInt += 6;
            }
            if (player.MaxedMana)
            {
                newMaxedInt += 6;
            }
            if (player.MaxedAtt)
            {
                newMaxedInt += 3;
            }
            if (player.MaxedDef)
            {
                newMaxedInt += 3;
            }
            if (player.MaxedSpd)
            {
                newMaxedInt += 3;
            }
            if (player.MaxedDex)
            {
                newMaxedInt += 3;
            }
            if (player.MaxedVit)
            {
                newMaxedInt += 3;
            }
            if (player.MaxedWis)
            {
                newMaxedInt += 3;
            }

            //Give 20 points for 16/16
            var statInfo = player.CoreServerManager.Resources.GameData.Classes[player.ObjectType];
            var upgradeCount = 0;
            if (player.Stats.Base[0] >= statInfo.Stats[0].MaxValue + 50)
            {
                newMaxedInt += 4;
                upgradeCount++;
            }
            if (player.Stats.Base[1] >= statInfo.Stats[1].MaxValue + 50)
            {
                newMaxedInt += 4;
                upgradeCount++;
            }
            if (player.Stats.Base[2] >= statInfo.Stats[2].MaxValue + 10)
            {
                newMaxedInt += 2;
                upgradeCount++;
            }
            if (player.Stats.Base[3] >= statInfo.Stats[3].MaxValue + 10)
            {
                newMaxedInt += 2;
                upgradeCount++;
            }
            if (player.Stats.Base[4] >= statInfo.Stats[4].MaxValue + 10)
            {
                newMaxedInt += 2;
                upgradeCount++;
            }
            if (player.Stats.Base[5] >= statInfo.Stats[5].MaxValue + 10)
            {
                newMaxedInt += 2;
                upgradeCount++;
            }
            if (player.Stats.Base[6] >= statInfo.Stats[6].MaxValue + 10)
            {
                newMaxedInt += 2;
                upgradeCount++;
            }
            if (player.Stats.Base[7] >= statInfo.Stats[7].MaxValue + 10)
            {
                newMaxedInt += 2;
                upgradeCount++;
            }

            //Give 29 skillpoints for 1450+ fame || Fame / 50 
            if (upgradeCount > 7)
            {
                newMaxedInt += player.Fame > 1449 ? 29 : player.Fame / 50;
            }

            //Check max points
            if (newMaxedInt > (player.Node1TickMin + player.Node1TickMaj + (player.Node1Med * 5) + (player.Node1Big * 10) + player.Node2TickMin + player.Node2TickMaj + (player.Node2Med * 5) + (player.Node2Big * 10) + player.Node3TickMin + player.Node3TickMaj + (player.Node3Med * 5) + (player.Node3Big * 10) + player.Node4TickMin + player.Node4TickMaj + (player.Node4Med * 5) + (player.Node4Big * 10) + player.Node5TickMin + player.Node5TickMaj + (player.Node5Med * 5) + (player.Node5Big * 10)))
            {
                //Console.WriteLine("Math: " + newMaxedInt + " - " + (player.Node1TickMin + player.Node1TickMaj + (player.Node1Med * 5) + (player.Node1Big * 10) + player.Node2TickMin + player.Node2TickMaj + (player.Node2Med * 5) + (player.Node2Big * 10) + player.Node3TickMin + player.Node3TickMaj + (player.Node3Med * 5) + (player.Node3Big * 10) + player.Node4TickMin + player.Node4TickMaj + (player.Node4Med * 5) + (player.Node4Big * 10) + player.Node5TickMin + player.Node5TickMaj + (player.Node5Med * 5) + (player.Node5Big * 10)));
                player.Points = newMaxedInt - (player.Node1TickMin + player.Node1TickMaj + (player.Node1Med * 5) + (player.Node1Big * 10) + player.Node2TickMin + player.Node2TickMaj + (player.Node2Med * 5) + (player.Node2Big * 10) + player.Node3TickMin + player.Node3TickMaj + (player.Node3Med * 5) + (player.Node3Big * 10) + player.Node4TickMin + player.Node4TickMaj + (player.Node4Med * 5) + (player.Node4Big * 10) + player.Node5TickMin + player.Node5TickMaj + (player.Node5Med * 5) + (player.Node5Big * 10));
            }

            #endregion Check of things
            if (skillNumber == 13)
            {
                if (player.Points <= 0)
                {
                    player.Points = 0;
                    player.Client.SendPacket(new InvResult() { Result = 1 });
                    player.SendError("You don't have Points!");
                    return;
                }
                if (player.Points < 1)
                {
                    player.Client.SendPacket(new InvResult() { Result = 1 });
                    player.SendError("You don't have Points!");
                    return;
                }
                player.Node1TickMin += !removePoint ? 1 : -1;
                player.Points -= !removePoint ? 1 : -1;
                player.Stats.ReCalculateValues();
                player.Stats.Base.ReCalculateValues();
                player.Stats.Boost.ReCalculateValues();
            }

            if (skillNumber == 14)
            {
                if (player.Points <= 0)
                {
                    player.Points = 0;
                    player.Client.SendPacket(new InvResult() { Result = 1 });
                    player.SendError("You don't have Points!");
                    return;
                }
                if (player.Points < 1)
                {
                    player.Client.SendPacket(new InvResult() { Result = 1 });
                    player.SendError("You don't have Points!");
                    return;
                }
                player.Node1TickMaj += !removePoint ? 1 : -1;
                player.Points -= !removePoint ? 1 : -1;
                player.Stats.ReCalculateValues();
                player.Stats.Base.ReCalculateValues();
                player.Stats.Boost.ReCalculateValues();
            }

            if (skillNumber == 15)
            {
                if (player.Points <= 0)
                {
                    player.Points = 0;
                    player.Client.SendPacket(new InvResult() { Result = 1 });
                    player.SendError("You don't have Points!");
                    return;
                }
                if (player.Points < 5)
                {
                    player.Client.SendPacket(new InvResult() { Result = 1 });
                    player.SendError("You don't have Points!");
                    return;
                }
                player.Node1Med += !removePoint ? 1 : -1;
                player.Points -= !removePoint ? 5 : -5;
                player.Stats.ReCalculateValues();
                player.Stats.Base.ReCalculateValues();
                player.Stats.Boost.ReCalculateValues();
            }

            if (skillNumber == 16)
            {
                if (player.Points <= 0)
                {
                    player.Points = 0;
                    player.Client.SendPacket(new InvResult() { Result = 1 });
                    player.SendError("You don't have Points!");
                    return;
                }
                if (player.Points < 10)
                {
                    player.Client.SendPacket(new InvResult() { Result = 1 });
                    player.SendError("You don't have Points!");
                    return;
                }
                player.Node1Big += !removePoint ? 1 : -1;
                player.Points -= !removePoint ? 10 : -10;
                player.Stats.ReCalculateValues();
                player.Stats.Base.ReCalculateValues();
                player.Stats.Boost.ReCalculateValues();
            }

            if (skillNumber == 17)
            {
                if (player.Points <= 0)
                {
                    player.Points = 0;
                    player.Client.SendPacket(new InvResult() { Result = 1 });
                    player.SendError("You don't have Points!");
                    return;
                }
                if (player.Points < 1)
                {
                    player.Client.SendPacket(new InvResult() { Result = 1 });
                    player.SendError("You don't have Points!");
                    return;
                }
                player.Node2TickMin += !removePoint ? 1 : -1;
                player.Points -= !removePoint ? 1 : -1;
                player.Stats.ReCalculateValues();
                player.Stats.Base.ReCalculateValues();
                player.Stats.Boost.ReCalculateValues();
            }

            if (skillNumber == 18)
            {
                if (player.Points <= 0)
                {
                    player.Points = 0;
                    player.Client.SendPacket(new InvResult() { Result = 1 });
                    player.SendError("You don't have Points!");
                    return;
                }
                if (player.Points < 1)
                {
                    player.Client.SendPacket(new InvResult() { Result = 1 });
                    player.SendError("You don't have Points!");
                    return;
                }
                player.Node2TickMaj += !removePoint ? 1 : -1;
                player.Points -= !removePoint ? 1 : -1;
            }

            if (skillNumber == 19)
            {
                if (player.Points <= 0)
                {
                    player.Points = 0;
                    player.Client.SendPacket(new InvResult() { Result = 1 });
                    player.SendError("You don't have Points!");
                    return;
                }
                if (player.Points < 5)
                {
                    player.Client.SendPacket(new InvResult() { Result = 1 });
                    player.SendError("You don't have Points!");
                    return;
                }
                player.Node2Med += !removePoint ? 1 : -1;
                player.Points -= !removePoint ? 5 : -5;
            }

            if (skillNumber == 20)
            {
                if (player.Points <= 0)
                {
                    player.Points = 0;
                    player.Client.SendPacket(new InvResult() { Result = 1 });
                    player.SendError("You don't have Points!");
                    return;
                }
                if (player.Points < 10)
                {
                    player.Client.SendPacket(new InvResult() { Result = 1 });
                    player.SendError("You don't have Points!");
                    return;
                }
                player.Node2Big += !removePoint ? 1 : -1;
                player.Points -= !removePoint ? 10 : -10;
            }

            if (skillNumber == 21)
            {
                if (player.Points <= 0)
                {
                    player.Points = 0;
                    player.Client.SendPacket(new InvResult() { Result = 1 });
                    player.SendError("You don't have Points!");
                    return;
                }
                if (player.Points < 1)
                {
                    player.Client.SendPacket(new InvResult() { Result = 1 });
                    player.SendError("You don't have Points!");
                    return;
                }
                player.Node3TickMin += !removePoint ? 1 : -1;
                player.Points -= !removePoint ? 1 : -1;
            }

            if (skillNumber == 22)
            {
                if (player.Points <= 0)
                {
                    player.Points = 0;
                    player.Client.SendPacket(new InvResult() { Result = 1 });
                    player.SendError("You don't have Points!");
                    return;
                }
                if (player.Points < 1)
                {
                    player.Client.SendPacket(new InvResult() { Result = 1 });
                    player.SendError("You don't have Points!");
                    return;
                }
                player.Node3TickMaj += !removePoint ? 1 : -1;
                player.Points -= !removePoint ? 1 : -1;
            }

            if (skillNumber == 23)
            {
                if (player.Points <= 0)
                {
                    player.Points = 0;
                    player.Client.SendPacket(new InvResult() { Result = 1 });
                    player.SendError("You don't have Points!");
                    return;
                }
                if (player.Points < 5)
                {
                    player.Client.SendPacket(new InvResult() { Result = 1 });
                    player.SendError("You don't have Points!");
                    return;
                }
                player.Node3Med += !removePoint ? 1 : -1;
                player.Points -= !removePoint ? 5 : -5;
            }

            if (skillNumber == 24)
            {
                if (player.Points <= 0)
                {
                    player.Points = 0;
                    player.Client.SendPacket(new InvResult() { Result = 1 });
                    player.SendError("You don't have Points!");
                    return;
                }
                if (player.Points < 10)
                {
                    player.Client.SendPacket(new InvResult() { Result = 1 });
                    player.SendError("You don't have Points!");
                    return;
                }
                player.Node3Big += !removePoint ? 1 : -1;
                player.Points -= !removePoint ? 10 : -10;
            }

            if (skillNumber == 25)
            {
                if (player.Points <= 0)
                {
                    player.Points = 0;
                    player.Client.SendPacket(new InvResult() { Result = 1 });
                    player.SendError("You don't have Points!");
                    return;
                }
                if (player.Points < 1)
                {
                    player.Client.SendPacket(new InvResult() { Result = 1 });
                    player.SendError("You don't have Points!");
                    return;
                }
                player.Node4TickMin += !removePoint ? 1 : -1;
                player.Points -= !removePoint ? 1 : -1;
            }

            if (skillNumber == 26)
            {
                if (player.Points <= 0)
                {
                    player.Points = 0;
                    player.Client.SendPacket(new InvResult() { Result = 1 });
                    player.SendError("You don't have Points!");
                    return;
                }
                if (player.Points < 1)
                {
                    player.Client.SendPacket(new InvResult() { Result = 1 });
                    player.SendError("You don't have Points!");
                    return;
                }
                player.Node4TickMaj += !removePoint ? 1 : -1;
                player.Points -= !removePoint ? 1 : -1;
            }

            if (skillNumber == 27)
            {
                if (player.Points <= 0)
                {
                    player.Points = 0;
                    player.Client.SendPacket(new InvResult() { Result = 1 });
                    player.SendError("You don't have Points!");
                    return;
                }
                if (player.Points < 5)
                {
                    player.Client.SendPacket(new InvResult() { Result = 1 });
                    player.SendError("You don't have Points!");
                    return;
                }
                player.Node4Med += !removePoint ? 1 : -1;
                player.Points -= !removePoint ? 5 : -5;
            }

            if (skillNumber == 28)
            {
                if (player.Points <= 0)
                {
                    player.Points = 0;
                    player.Client.SendPacket(new InvResult() { Result = 1 });
                    player.SendError("You don't have Points!");
                    return;
                }
                if (player.Points < 10)
                {
                    player.Client.SendPacket(new InvResult() { Result = 1 });
                    player.SendError("You don't have Points!");
                    return;
                }
                player.Node4Big += !removePoint ? 1 : -1;
                player.Points -= !removePoint ? 10 : -10;
            }

            if (skillNumber == 29)
            {
                if (player.Points <= 0)
                {
                    player.Points = 0;
                    player.Client.SendPacket(new InvResult() { Result = 1 });
                    player.SendError("You don't have Points!");
                    return;
                }
                if (player.Points < 1)
                {
                    player.Client.SendPacket(new InvResult() { Result = 1 });
                    player.SendError("You don't have Points!");
                    return;
                }
                player.Node5TickMin += !removePoint ? 1 : -1;
                player.Points -= !removePoint ? 1 : -1;
            }

            if (skillNumber == 30)
            {
                if (player.Points <= 0)
                {
                    player.Points = 0;
                    player.Client.SendPacket(new InvResult() { Result = 1 });
                    player.SendError("You don't have Points!");
                    return;
                }
                if (player.Points < 1)
                {
                    player.Client.SendPacket(new InvResult() { Result = 1 });
                    player.SendError("You don't have Points!");
                    return;
                }
                player.Node5TickMaj += !removePoint ? 1 : -1;
                player.Points -= !removePoint ? 1 : -1;
            }

            if (skillNumber == 31)
            {
                if (player.Points <= 0)
                {
                    player.Points = 0;
                    player.Client.SendPacket(new InvResult() { Result = 1 });
                    player.SendError("You don't have Points!");
                    return;
                }
                if (player.Points < 5)
                {
                    player.Client.SendPacket(new InvResult() { Result = 1 });
                    player.SendError("You don't have Points!");
                    return;
                }
                player.Node5Med += !removePoint ? 1 : -1;
                player.Points -= !removePoint ? 5 : -5;
            }

            if (skillNumber == 32)
            {
                if (player.Points <= 0)
                {
                    player.Points = 0;
                    player.Client.SendPacket(new InvResult() { Result = 1 });
                    player.SendError("You don't have Points!");
                    return;
                }
                if (player.Points < 10)
                {
                    player.Client.SendPacket(new InvResult() { Result = 1 });
                    player.SendError("You don't have Points!");
                    return;
                }
                player.Node5Big += !removePoint ? 1 : -1;
                player.Points -= !removePoint ? 10 : -10;
            }



            if (skillNumber == 40)
            {
                //Console.WriteLine("Save: " + (player.Node1TickMin + player.Node1TickMaj + (player.Node1Med * 5) + (player.Node1Big * 10) + player.Node2TickMin + player.Node2TickMaj + (player.Node2Med * 5) + (player.Node2Big * 10) + player.Node3TickMin + player.Node3TickMaj + (player.Node3Med * 5) + (player.Node3Big * 10) + player.Node4TickMin + player.Node4TickMaj + (player.Node4Med * 5) + (player.Node4Big * 10) + player.Node5TickMin + player.Node5TickMaj + (player.Node5Med * 5) + (player.Node5Big * 10)) + " > " + player.Points);
                if (player.Node1TickMin + player.Node1TickMaj + (player.Node1Med * 5) + (player.Node1Big * 10) + player.Node2TickMin + player.Node2TickMaj + (player.Node2Med * 5) + (player.Node2Big * 10) + player.Node3TickMin + player.Node3TickMaj + (player.Node3Med * 5) + (player.Node3Big * 10) + player.Node4TickMin + player.Node4TickMaj + (player.Node4Med * 5) + (player.Node4Big * 10) + player.Node5TickMin + player.Node5TickMaj + (player.Node5Med * 5) + (player.Node5Big * 10) > newMaxedInt)
                {
                    player.Client.SendPacket(new InvResult() { Result = 1 });
                    player.SendError("Error saving attributes! Please try again later.");
                    client.Disconnect("Error saving attributes! Please try again later.");
                    return;
                }

                for (int i = 1; i < 6; i++)
                {
                    var local1 = CalcTreePoints(client, i);
                    //Console.WriteLine("Check Odd Number: " + local1);
                    if (local1 == 99)
                    {
                        player.Client.SendPacket(new InvResult() { Result = 1 });
                        player.SendError("Error saving attributes! Please try again later.");
                        client.Disconnect("Error saving attributes! Please try again later.");
                        return;
                    }
                }

                chr.Node1TickMin = player.Node1TickMin;
                chr.Node1TickMaj = player.Node1TickMaj;
                chr.Node1Med = player.Node1Med;
                chr.Node1Big = player.Node1Big;
                chr.Node2TickMin = player.Node2TickMin;
                chr.Node2TickMaj = player.Node2TickMaj;
                chr.Node2Med = player.Node2Med;
                chr.Node2Big = player.Node2Big;
                chr.Node3TickMin = player.Node3TickMin;
                chr.Node3TickMaj = player.Node3TickMaj;
                chr.Node3Med = player.Node3Med;
                chr.Node3Big = player.Node3Big;
                chr.Node4TickMin = player.Node4TickMin;
                chr.Node4TickMaj = player.Node4TickMaj;
                chr.Node4Med = player.Node4Med;
                chr.Node4Big = player.Node4Big;
                chr.Node5TickMin = player.Node5TickMin;
                chr.Node5TickMaj = player.Node5TickMaj;
                chr.Node5Med = player.Node5Med;
                chr.Node5Big = player.Node5Big;

                chr.Points = player.Points;
                player.Stats.ReCalculateValues();
                player.Stats.Base.ReCalculateValues();
                player.Stats.Boost.ReCalculateValues();
            }
        }

        public int CalcNextCost(Client client, int treeNumber, bool removePoint = false)
        {
            var currentTreePoints = CalcTreePoints(client, treeNumber);
            switch (currentTreePoints)
            {
                case 0: return removePoint ? 0 : 1;
                case 1: return removePoint ? -1 : 1;
                case 2: return removePoint ? -1 : 1;
                case 3: return removePoint ? -1 : 5;
                case 8: return removePoint ? -5 : 1;
                case 9: return removePoint ? -1 : 1;
                case 10: return removePoint ? -1 : 1;
                case 11: return removePoint ? -1 : 5;
                case 16: return removePoint ? -5 : 1;
                case 17: return removePoint ? -1 : 1;
                case 18: return removePoint ? -1 : 1;
                case 19: return removePoint ? -1 : 10;
                case 29: return removePoint ? -10 : 1;
                case 30: return removePoint ? -1 : 1;
                case 31: return removePoint ? -1 : 1;
                case 32: return removePoint ? -1 : 5;
                case 37: return removePoint ? -5 : 1;
                case 38: return removePoint ? -1 : 1;
                case 39: return removePoint ? -1 : 1;
                case 40: return removePoint ? -1 : 10;
                case 50: return removePoint ? -10 : 98;
                default: return 99;
            }
        }

        public int CalcTreePoints(Client client, int treeNumber)
        {
            switch (treeNumber)
            {
                case 1:
                    return (client.Player.Node1TickMaj) + (client.Player.Node1Med * 5) + (client.Player.Node1TickMin) + (client.Player.Node1Big * 10);
                case 2:
                    return (client.Player.Node2TickMaj) + (client.Player.Node2Med * 5) + (client.Player.Node2TickMin) + (client.Player.Node2Big * 10);
                case 3:
                    return (client.Player.Node3TickMaj) + (client.Player.Node3Med * 5) + (client.Player.Node3TickMin) + (client.Player.Node3Big * 10);
                case 4:
                    return (client.Player.Node4TickMaj) + (client.Player.Node4Med * 5) + (client.Player.Node4TickMin) + (client.Player.Node4Big * 10);
                case 5:
                    return (client.Player.Node5TickMaj) + (client.Player.Node5Med * 5) + (client.Player.Node5TickMin) + (client.Player.Node5Big * 10);
                default:
                    return 0;
            }
        }
    }
}
