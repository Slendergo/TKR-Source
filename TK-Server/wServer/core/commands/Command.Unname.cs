using common.database;
using wServer.core.objects;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class Unname : Command
        {
            public Unname() : base("unname", permLevel: 90)
            {
            }

            protected override bool Process(Player player, TickData time, string args)
            {
                if (string.IsNullOrWhiteSpace(args))
                {
                    player.SendInfo("Usage: /unname <player name>");
                    return false;
                }

                var playerName = args;

                var id = player.CoreServerManager.Database.ResolveId(playerName);
                if (id == 0)
                {
                    player.SendError("Player account not found!");
                    return false;
                }

                string lockToken = null;
                var key = Database.NAME_LOCK;
                var db = player.CoreServerManager.Database;

                try
                {
                    while ((lockToken = db.AcquireLock(key)) == null) ;

                    var acc = db.GetAccount(id);
                    if (acc == null)
                    {
                        player.SendError("Account doesn't exist.");
                        return false;
                    }

                    using (var l = db.Lock(acc))
                        if (db.LockOk(l))
                        {
                            while (!db.UnnameIGN(acc, lockToken)) ;
                            player.SendInfo("Account succesfully unnamed.");
                        }
                        else
                            player.SendError("Account in use.");
                }
                finally
                {
                    if (lockToken != null)
                        db.ReleaseLock(key, lockToken);
                }

                return true;
            }
        }
    }
}
