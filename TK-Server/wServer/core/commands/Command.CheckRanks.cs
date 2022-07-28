using System.Text;
using wServer.core.objects;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class CheckRanks : Command
        {
            public CheckRanks() : base("checkrank", 110, alias: "cr")
            {
            }

            protected override bool Process(Player player, TickData time, string args)
            {
                var owner = player.Owner;
                var servers = player.CoreServerManager.InterServerManager.GetServerList();
                var sb = new StringBuilder("Accounts Over or Equal Rank 90 that are currently Online: ");
                foreach (var server in servers)
                    foreach (Player plr in server.playerList)
                    {
                        if (plr.Rank >= 90)
                        {
                            sb.Append(plr.Name + ", ");
                        }
                    }

                player.SendInfo(sb.ToString());
                return true;
            }
        }
    }
}
