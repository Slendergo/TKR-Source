using System.Text.RegularExpressions;
using wServer.core.objects;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class UnbanAccount : Command
        {
            public UnbanAccount() : base("unban", permLevel: 90)
            {
            }

            protected override bool Process(Player player, TickData time, string args)
            {
                var db = player.CoreServerManager.Database;

                // validate command
                var rgx = new Regex(@"^(\w+)$");
                if (!rgx.IsMatch(args))
                {
                    player.SendError("Usage: /unban <account id or name>");
                    return false;
                }

                // get info from args
                if (!int.TryParse(args, out int id))
                    id = db.ResolveId(args);

                // run checks
                if (id == 0)
                {
                    player.SendError("Account doesn't exist...");
                    return false;
                }

                var acc = db.GetAccount(id);

                // unban
                var banned = db.UnBan(id);
                var ipBanned = acc.IP != null && db.UnBanIp(acc.IP);

                // send notification
                if (!banned && !ipBanned)
                {
                    player.SendInfo($"{acc.Name} wasn't banned...");
                    return true;
                }
                if (banned && ipBanned)
                {
                    player.SendInfo($"Success! {acc.Name}'s account and IP no longer banned.");
                    return true;
                }
                if (banned)
                {
                    player.SendInfo($"Success! {acc.Name}'s account no longer banned.");
                    return true;
                }

                player.SendInfo($"Success! {acc.Name}'s IP no longer banned.");
                return true;
            }
        }
    }
}
