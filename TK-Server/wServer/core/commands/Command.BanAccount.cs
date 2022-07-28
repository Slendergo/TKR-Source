using CA.Extensions.Concurrent;
using common;
using common.database;
using System.Linq;
using System.Text.RegularExpressions;
using wServer.core.objects;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class BanAccount : Command
        {
            public BanAccount() : base("ban", permLevel: 80)
            { }

            protected override bool Process(Player player, TickData time, string args)
            {
                BanInfo bInfo;

                if (args.StartsWith("{"))
                    bInfo = Utils.FromJson<BanInfo>(args);
                else
                {
                    bInfo = new BanInfo();

                    // validate command
                    var rgx = new Regex(@"^(\w+) (.+)$");
                    var match = rgx.Match(args);

                    if (!match.Success)
                    {
                        player.SendError("Usage: /ban <account id or name> <reason>");

                        // dispose vars
                        rgx = null;
                        match = null;
                        return false;
                    }

                    // get info from args
                    bInfo.Name = match.Groups[1].Value;

                    if (!int.TryParse(bInfo.Name, out bInfo.accountId))
                        bInfo.accountId = player.CoreServerManager.Database.ResolveId(bInfo.Name);

                    bInfo.banReasons = match.Groups[2].Value;
                    bInfo.banLiftTime = -1;

                    // dispose vars
                    rgx = null;
                    match = null;
                }

                // run checks
                if (Database.GuestNames.Any(n => n.ToLower().Equals(bInfo.Name?.ToLower())))
                {
                    player.SendError("If you specify a player name to ban, the name needs to be unique.");

                    // dispose vars
                    bInfo = null;
                    return false;
                }

                if (bInfo.accountId == 0)
                {
                    player.SendError("Account not found...");

                    // dispose vars
                    bInfo = null;
                    return false;
                }

                if (string.IsNullOrWhiteSpace(bInfo.banReasons))
                {
                    player.SendError("A reason must be provided.");

                    // dispose vars
                    bInfo = null;
                    return false;
                }

                var acc = player.CoreServerManager.Database.GetAccount(bInfo.accountId);

                if (player.AccountId != acc.AccountId && player.Rank <= acc.Rank)
                {
                    player.SendError("Cannot ban players of equal or higher rank than yourself.");

                    // dispose vars
                    bInfo = null;
                    acc = null;
                    return false;
                }

                // ban player + disconnect if currently connected
                player.CoreServerManager.Database.Ban(bInfo.accountId, bInfo.banReasons, bInfo.banLiftTime);

                var target = player.CoreServerManager.ConnectionManager.Clients
                    .KeyWhereAsParallel(_ => _.Account != null && _.Account.AccountId == bInfo.accountId)
                    .FirstOrDefault();
                target?.Disconnect("BanAccountCommand");
                player.SendInfo(
                    !string.IsNullOrEmpty(bInfo.Name)
                        ? $"{bInfo.Name} successfully banned."
                        : "Ban successful."
                );
                return true;
            }

            private class BanInfo
            {
                public int accountId;
                public string Name;
                public string banReasons;
                public int banLiftTime;
            }
        }
    }
}
