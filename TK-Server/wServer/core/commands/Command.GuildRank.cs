using CA.Extensions.Concurrent;
using common;
using common.database;
using System;
using System.Linq;
using wServer.core.objects;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class GuildRank : Command
        {
            public GuildRank() : base("grank", permLevel: 90, listCommand: false)
            {
            }

            protected override bool Process(Player player, TickData time, string args)
            {
                if (player == null)
                    return false;

                // verify argument
                var index = args.IndexOf(' ');
                if (string.IsNullOrWhiteSpace(args) || index == -1)
                {
                    player.SendInfo("Usage: /grank <player name> <guild rank>");
                    return false;
                }

                // get command args
                var playerName = args.Substring(0, index);
                var rank = args.Substring(index + 1).IsInt() ? args.Substring(index + 1).ToInt32() : RankNumberFromName(args.Substring(index + 1));
                if (rank == -1)
                {
                    player.SendError("Unknown rank!");
                    return false;
                }
                else if (rank % 10 != 0)
                {
                    player.SendError("Valid ranks are multiples of 10!");
                    return false;
                }

                // get player account
                if (Database.GuestNames.Contains(playerName, StringComparer.InvariantCultureIgnoreCase))
                {
                    player.SendError("Cannot rank the unnamed...");
                    return false;
                }
                var id = player.CoreServerManager.Database.ResolveId(playerName);
                var acc = player.CoreServerManager.Database.GetAccount(id);
                if (id == 0 || acc == null)
                {
                    player.SendError("Account not found!");
                    return false;
                }

                // change rank
                acc.GuildRank = rank;
                acc.FlushAsync();

                // send out success notifications
                player.SendInfo($"You changed the guildrank of player {acc.Name} to {rank}.");
                var target = player.CoreServerManager.ConnectionManager.Clients.KeyWhereAsParallel(_ => _.Account.AccountId == acc.AccountId).FirstOrDefault();
                if (target?.Player == null)
                    return true;

                target.Player.GuildRank = rank;
                target.Player.SendInfo("Your guild rank was changed");
                return true;
            }

            private int RankNumberFromName(string val)
            {
                switch (val.ToLower())
                {
                    case "initiate":
                        return 0;

                    case "member":
                        return 10;

                    case "officer":
                        return 20;

                    case "leader":
                        return 30;

                    case "founder":
                        return 40;
                }
                return -1;
            }
        }
    }
}
