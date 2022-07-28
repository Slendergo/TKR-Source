using wServer.core.objects;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class TpQuest : Command
        {
            public TpQuest() : base("tq", permLevel: 100)
            {
            }

            protected override bool Process(Player player, TickData time, string args)
            {
                if (player.Quest == null)
                {
                    player.SendError("Player does not have a quest!");
                    return false;
                }

                player.SetNewbiePeriod();
                player.TeleportPosition(time, player.Quest.RealX, player.Quest.RealY, true);
                player.SendInfo("Teleported to Quest Location: (" + player.Quest.X + ", " + player.Quest.Y + ")");
                return true;
            }
        }
    }
}
