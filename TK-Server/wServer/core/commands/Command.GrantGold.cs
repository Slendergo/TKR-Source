using CA.Extensions.Concurrent;
using common.database;
using System;
using System.Linq;
using wServer.core.objects;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class GrantGold : Command
        {
            public GrantGold() : base("grantGold", permLevel: 100)
            {
            }

            protected override bool Process(Player player, TickData time, string args)
            {
                var index = args.IndexOf(' ');
                if (string.IsNullOrEmpty(args) || index == -1)
                {
                    player.SendInfo("Usage: /grantGold <player name> <amount>");
                    return false;
                }

                var name = args.Substring(0, index);
                var amount = int.Parse(args.Substring(index + 1));

                if (Database.GuestNames.Contains(name, StringComparer.InvariantCultureIgnoreCase))
                {
                    player.SendError("Cannot grant gold to unnamed accounts.");
                    return false;
                }

                var id = player.CoreServerManager.Database.ResolveId(name);
                if (id == player.AccountId)
                {
                    player.SendError("Cannot grant gold to self.");
                    return false;
                }

                var acc = player.CoreServerManager.Database.GetAccount(id);
                if (id == 0 || acc == null)
                {
                    player.SendError("Account not found!");
                    return false;
                }

                // kick player from server to set new gold
                var client = player.CoreServerManager.ConnectionManager.Clients
                    .KeyWhereAsParallel(_ => _.Account.Name.EqualsIgnoreCase(name))
                    .SingleOrDefault();
                client?.Disconnect("GrantGold");

                acc.Credits += amount;
                acc.TotalCredits += amount;
                acc.FlushAsync();

                player.SendInfo($"{acc.Name} was given {amount} gold");
                return true;
            }
        }
    }
}
