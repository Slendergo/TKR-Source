using common.isc.data;
using System.Linq;
using System.Text;
using wServer.core.objects;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class Online : Command
        {
            public Online() : base("online", permLevel: 100)
            { }

            protected override bool Process(Player player, TickTime time, string args)
            {
                var playerSvr = player.CoreServerManager.ServerConfig.serverInfo.name;
                var servers = Program.CoreServerManager.InterServerManager.GetServerList();
                var s = servers.Where(_ => _.type != common.isc.ServerType.Account);
                var sb = new StringBuilder($"There are: {s.Sum(_ => _.players)} Online Across: {string.Join(", ", s.Select(_ => _.name))}: ");
                player.SendInfo(sb.ToString());
                return true;
            }
        }
    }
}