using CA.Extensions.Concurrent;
using common.database;
using System;
using System.Linq;
using wServer.core.objects;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class MakeDonor : Command
        {
            public MakeDonor() : base("donor", permLevel: 100)
            {
            }

            protected override bool Process(Player player, TickData time, string args)
            {
                var index = args.IndexOf(' ');
                if (string.IsNullOrEmpty(args) || index == -1)
                {
                    player.SendInfo("Usage: /donor <player name> <rank>\\10: Donor-1, 20: Donor-2, 30: Donor-3, 40: Donor-4, 50: Donor-5, 60 VIP");
                    return false;                                   //GAME MASTER, DEVELOPER, OWNER, STAFF, DONOR
                }

                var name = args.Substring(0, index);
                var rank = int.Parse(args.Substring(index + 1));

                if (rank > 60)
                {
                    player.SendError("That's not a Donor number!");
                    return false;
                }

                if (rank > 60)
                {
                    player.SendError("Its more than a donator!");
                    return false;
                }

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

                if (rank == acc.LegacyRank)
                {
                    player.SendError("Already has that rank.");
                    return false;
                }

                if (rank <= acc.LegacyRank)
                {
                    player.SendError("Rank is lower than current rank");
                    return false;
                }

                // kick player from server to set rank
                var client = player.CoreServerManager.ConnectionManager.Clients
                    .KeyWhereAsParallel(_ => _.Account.Name.EqualsIgnoreCase(name))
                    .SingleOrDefault();
                client?.Disconnect("MakeDonor");

                if (rank == 10 && acc.LegacyRank != 10)
                {
                    acc.LegacyRank = rank;
                    acc.Credits += 500;
                    acc.TotalCredits += 500;
                }
                if (rank == 20 && acc.LegacyRank != 20)
                {
                    acc.LegacyRank = rank;
                    acc.Credits += 700;
                    acc.TotalCredits += 700;
                }
                if (rank == 30 && acc.LegacyRank != 30)
                {
                    acc.LegacyRank = rank;
                    acc.Credits += 1000;
                    acc.TotalCredits += 1000;
                }
                if (rank == 40 && acc.LegacyRank != 40)
                {
                    acc.LegacyRank = rank;
                    acc.Credits += 1500;
                    acc.TotalCredits += 1500;
                }
                if (rank == 50 && acc.LegacyRank != 50)
                {
                    acc.LegacyRank = rank;
                    acc.Credits += 2500;
                    acc.TotalCredits += 2500;
                }

                acc.LegacyRank = rank;
                acc.FlushAsync();

                player.SendInfo($"{acc.Name} given Donor Rank. ({rank})");
                return true;
            }
        }
    }
}
