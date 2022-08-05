using System;
using wServer.core.objects;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class Unmute : Command
        {
            public Unmute() : base("unmute", permLevel: 80)
            {
            }

            protected override bool Process(Player player, TickTime time, string name)
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    player.SendError("Usage: /unmute <player name>");
                    return false;
                }

                // gather needed info
                var id = player.GameServer.Database.ResolveId(name);
                var acc = player.GameServer.Database.GetAccount(id);

                // run checks
                if (id == 0 || acc == null)
                {
                    player.SendError("Account not found!");
                    return false;
                }
                if (acc.IP == null)
                {
                    player.SendError("Account has no associated IP address. Player must login at least once before being unmuted.");
                    return false;
                }

                // unmute ip address
                player.GameServer.Database.IsMuted(acc.IP).ContinueWith(t =>
                {
                    if (!t.IsCompleted)
                    {
                        player.SendInfo("Db access error while trying to unmute.");
                        return;
                    }

                    if (t.Result)
                    {
                        player.GameServer.Database.Mute(acc.IP, TimeSpan.FromSeconds(1));
                        player.SendInfo(name + " successfully unmuted.");
                    }
                    else
                    {
                        player.SendInfo(name + " wasn't muted...");
                    }
                });

                // expire event will handle unmuting of connected players
                return true;
            }
        }
    }
}
