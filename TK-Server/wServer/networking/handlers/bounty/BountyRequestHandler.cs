using System.Collections.Concurrent;
using wServer.core.objects;
using wServer.core.worlds;
using wServer.networking.packets;
using wServer.networking.packets.incoming;

namespace wServer.networking.handlers
{
    internal class BountyRequestHandler : PacketHandlerBase<BountyRequest>
    {
        private readonly string EasyWrld = "The Sacred Chamber of Wingus";
        private readonly string HardWrld = "Gates of Hades";
        // private readonly string EasyWrld = "Undead Lair";

        public override PacketId ID => PacketId.BOUNTYREQUEST;

        protected override void HandlePacket(Client client, BountyRequest packet)
        {
            if (client == null || IsTest(client) || client.Player == null || client.Player.Owner == null)
                return;

            Handle(client, packet);
        }

        private void Handle(Client client, BountyRequest packet)
        {
            var RequiredLeaders = 0; /* Leaders */
            Player[] playersAllowed = getPlayers(client, packet); /* Players */

            foreach (var player in playersAllowed) /* Check players */
            {
                if (player == null) continue; /* Just in case */
                if (player.GuildRank >= 30) /* Add Leader if true */
                    RequiredLeaders++;
            }

            if (client.Account.GuildRank >= 30) /* Start */
            {
                if (RequiredLeaders < 1 && client.Player.Rank < 100) /* Check Leaders Again */
                {
                    client.Player.SendError("There's need to be at least 1 or more Leaders/Founder allowed to begin this Bounty!");
                    return;
                }

                switch (packet.BountyId) /* Check Bounty ID */
                {
                    case 1:
                        LaunchRaid(client, playersAllowed, 10000, 1);
                        return;

                    case 2:
                        LaunchRaid(client, playersAllowed, 50000, 2);
                        return;

                    case 3:
                        LaunchRaid(client, playersAllowed, 75000, 3);
                        return;

                    case 4:
                        LaunchRaid(client, playersAllowed, 150000, 4);
                        return;

                    default:
                        client.Player.SendError("Bounty ID Undefinished.");
                        return;
                }
            }
            else client.Player.SendError("You must be ranked Leader or higher in order to launch Guild Bounties.");
        }

        #region Raids

        public void LaunchRaid(Client originalClient, Player[] players, int fameCost, int raidId)
        {
            var cManager = originalClient.CoreServerManager;
            var db = cManager.Database;
            var account = db.GetAccount(originalClient.Player.AccountId);
            var guild = db.GetGuild(account.GuildId);

            if (guild.Fame >= fameCost)
            {
                switch (raidId)
                {
                    default:
                        originalClient.Player.SendError("Raid ID Unexpected.");
                        return;

                    #region Easy

                    case 1:
                        var easy = originalClient.Player.Owner.Manager.Resources.Worlds[EasyWrld];

                        DynamicWorld.TryGetWorld(easy, originalClient, out var world);

                        var wManager = cManager.WorldManager;
                        world = wManager.AddWorld(new World(easy));
                        db.UpdateGuildFame(guild, -fameCost);

                        foreach (var player in players)
                        {
                            if (player == null) continue;
                            if (world != null)
                            {
                                player.SendInfo("The bounty of The Sacred Chamber of Wingus has been started!");
                                player.Reconnect(world);
                            }
                        }
                        return;

                    #endregion Easy

                    #region Hard

                    case 2:
                        var hard = originalClient.Player.Owner.Manager.Resources.Worlds[HardWrld];

                        DynamicWorld.TryGetWorld(hard, originalClient, out var world2);

                        var wManager2 = cManager.WorldManager;
                        world2 = wManager2.AddWorld(new World(hard));
                        db.UpdateGuildFame(guild, -fameCost);

                        foreach (var player in players)
                        {
                            if (player == null) continue;
                            if (world2 != null)
                            {
                                player.SendInfo("The bounty to Gates of Hades has been started!");
                                player.Reconnect(world2);
                            }
                        }
                        return;

                    #endregion Hard

                    #region Hell

                    case 3:
                        return;

                    #endregion Hell

                    #region Godly

                    case 4:
                        return;

                        #endregion Godly
                }
            }
            else originalClient.Player.SendError("You need at least " + fameCost + " guild fame to begin this bounty.");
        }

        #endregion Raids

        #region Utils

        private Player[] getPlayers(Client client, BountyRequest packet)
        {
            var playersAllowed = new ConcurrentBag<Player>();
            var world = client.Player.Owner;
            foreach (var allowedPlayersId in packet.PlayersAllowed) /* Get AlL ID's */
            {
                if (allowedPlayersId == 0)
                    continue; /* ID = 0 -> Disabled player */

                world.PlayersBroadcastAsParallel(_ =>
                {
                    if (allowedPlayersId == 0 || _.Id != allowedPlayersId)
                        return;

                    playersAllowed.Add(_);
                });
            }

            return playersAllowed.ToArray(); /* Return the Variable */
        }

        #endregion Utils
    }
}
