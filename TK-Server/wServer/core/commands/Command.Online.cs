using common.isc.data;
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

            protected override bool Process(Player player, TickData time, string args)
            {
                var playerSvr = player.CoreServerManager.ServerConfig.serverInfo.name;
                var servers = Program.CoreServerManager.InterServerManager.GetServerList();
                var sb = new StringBuilder($"Players Online in {playerSvr} : ");
                foreach (var server in servers)
                    if (server.name == playerSvr)
                        foreach (PlayerInfo plr in server.playerList)
                            sb.Append($"\n {plr.Name}; World Name: {plr.WorldName}; World Instance: {plr.WorldInstance}");
                player.SendInfo(sb.ToString());
                return true;
            }
        }
    }
}