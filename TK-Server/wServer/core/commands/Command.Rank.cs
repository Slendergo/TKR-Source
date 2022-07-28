using CA.Extensions.Concurrent;
using common.database;
using System;
using System.Linq;
using wServer.core.objects;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class Rank : Command
        {
            public Rank() : base("rank", permLevel: 120)
            {
            }

            protected override bool Process(Player player, TickData time, string args)
            {
                var index = args.IndexOf(' ');
                if (string.IsNullOrEmpty(args) || index == -1)
                {
                    player.SendInfo("Usage: /rank <player name> <rank>\\n0: Normal Player, 70: Former Staff, 80: Tester, 90: Dev, 100: Owner");
                    return false;                                   //GAME MASTER, DEVELOPER, OWNER, STAFF, DONOR
                }

                var name = args.Substring(0, index);
                var rank = int.Parse(args.Substring(index + 1));

                if (Database.GuestNames.Contains(name, StringComparer.InvariantCultureIgnoreCase))
                {
                    player.SendError("Cannot rank unnamed accounts.");
                    return false;
                }

                var id = player.CoreServerManager.Database.ResolveId(name);
                if (id == player.AccountId)
                {
                    player.SendError("Cannot rank self.");
                    return false;
                }

                var acc = player.CoreServerManager.Database.GetAccount(id);
                if (id == 0 || acc == null)
                {
                    player.SendError("Account not found!");
                    return false;
                }

                // kick player from server to set rank
                var client = player.CoreServerManager.ConnectionManager.Clients
                    .KeyWhereAsParallel(_ => _.Account.Name.EqualsIgnoreCase(name))
                    .SingleOrDefault();
                client?.Disconnect("RankCommand");

                acc.Admin = rank >= 80;
                acc.LegacyRank = rank;
                acc.Hidden = false;
                acc.FlushAsync();

                player.SendInfo($"{acc.Name} given legacy rank {acc.LegacyRank}{(acc.Admin ? " and now has admin status" : "")}.");
                return true;
            }
        }
    }
}
