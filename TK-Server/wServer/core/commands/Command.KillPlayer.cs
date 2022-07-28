using CA.Extensions.Concurrent;
using common.database;
using System.Linq;
using wServer.core.objects;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class KillPlayer : Command
        {
            public KillPlayer() : base("killPlayer", permLevel: 110)
            {
            }

            protected override bool Process(Player player, TickData time, string args)
            {
                var client = player.CoreServerManager.ConnectionManager.Clients
                    .KeyWhereAsParallel(_ => _.Account.Name.EqualsIgnoreCase(args))
                    .SingleOrDefault();
                if (client != default)
                {
                    client.Player.HP = 0;
                    client.Player.Death(player.Name);
                    player.SendInfo("Player killed!");
                    return true;
                }

                player.SendError($"Player '{args}' could not be found!");
                return false;
            }
        }
    }
}
