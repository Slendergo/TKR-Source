using CA.Extensions.Concurrent;
using common.database;
using System.Linq;
using System.Text.RegularExpressions;
using wServer.core.objects;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class BanIP : Command
        {
            public BanIP() : base("banip", permLevel: 80, alias: "ipban")
            {
            }

            protected override bool Process(Player player, TickData time, string args)
            {
                var manager = player.CoreServerManager;
                var db = manager.Database;

                // validate command
                var rgx = new Regex(@"^(\w+) (.+)$");
                var match = rgx.Match(args);
                if (!match.Success)
                {
                    player.SendError("Usage: /banip <account id or name> <reason>");
                    return false;
                }

                // get info from args
                var idstr = match.Groups[1].Value;
                if (!int.TryParse(idstr, out int id))
                {
                    id = db.ResolveId(idstr);
                }
                var reason = match.Groups[2].Value;

                // run checks
                if (Database.GuestNames.Any(n => n.ToLower().Equals(idstr.ToLower())))
                {
                    player.SendError("If you specify a player name to ban, the name needs to be unique.");
                    return false;
                }
                if (id == 0)
                {
                    player.SendError("Account not found...");
                    return false;
                }
                if (string.IsNullOrWhiteSpace(reason))
                {
                    player.SendError("A reason must be provided.");
                    return false;
                }
                var acc = db.GetAccount(id);
                if (string.IsNullOrEmpty(acc.IP))
                {
                    player.SendError("Failed to ip ban player. IP not logged...");
                    return false;
                }
                if (player.AccountId != acc.AccountId && acc.IP.Equals(player.Client.Account.IP))
                {
                    player.SendError("IP ban failed. That action would cause yourself to be banned (IPs are the same).");
                    return false;
                }
                if (player.AccountId != acc.AccountId && player.Rank <= acc.Rank)
                {
                    player.SendError("Cannot ban players of equal or higher rank than yourself.");
                    return false;
                }

                // ban
                db.Ban(acc.AccountId, reason);
                db.BanIp(acc.IP, reason);

                // disconnect currently connected
                var targets = manager.ConnectionManager.Clients.KeyWhereAsParallel(_ => _.IpAddress.Equals(acc.IP));
                for (var i = 0; i < targets.Length; i++)
                    targets[i].Disconnect("BanIPCommand");

                // send notification
                player.SendInfo($"Banned {acc.Name} (both account and ip).");
                return true;
            }
        }
    }
}
