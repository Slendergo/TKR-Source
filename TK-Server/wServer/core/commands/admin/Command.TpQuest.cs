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

                var x = player.Quest.RealX;
                var y = player.Quest.RealY;

                if (player.Quest.ObjectDesc.ObjectId.Contains("Hermit"))
                    y += 6;

                player.TeleportPosition(time, x, y);
                player.SendInfo($"Teleported to Quest Location: ({x}, {y})");
                return true;
            }
        }
    }
}
