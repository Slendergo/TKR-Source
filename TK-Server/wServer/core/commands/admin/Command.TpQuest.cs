using common;
using wServer.core.objects;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class TpQuest : Command
        {
            public override RankingType RankRequirement => RankingType.Admin;
            public override string CommandName => "tq";

            protected override bool Process(Player player, TickTime time, string args)
            {
                if (player.Quest == null)
                {
                    player.SendError("Player does not have a quest!");
                    return false;
                }

                if (!player.TPCooledDown())
                {
                    player.SendError($"Teleport is on cooldown");
                    return true;
                }
                player.SetNewbiePeriod();
                player.TeleportPosition(time, player.Quest.RealX, player.Quest.RealY);
                player.SendInfo($"Teleported to Quest Location: ({player.Quest.X}, {player.Quest.Y})");
                return true;
            }
        }
    }
}
