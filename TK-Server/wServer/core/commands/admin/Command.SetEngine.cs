using common;
using wServer.core.objects;
using wServer.core.worlds.logic;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class SetEngine : Command
        {
            public override RankingType RankRequirement => RankingType.Admin;
            public override string CommandName => "se";

            protected override bool Process(Player player, TickTime time, string args)
            {
                var manager = player.GameServer;
                var success = int.TryParse(args, out var amount);
                if (!success)
                {
                    player.SendInfo("Enter valid amount");
                    return true;
                }

                if(!manager.WorldManager.Nexus.TryAddFuelToEngine(player, amount))
                {
                    player.SendError($"Engine is max capacity");
                    return true;
                }

                player.SendInfo($"Added {amount} fuel to engine");
                return true;
            }
        }
    }
}
