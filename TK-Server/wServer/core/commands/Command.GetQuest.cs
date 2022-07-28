using wServer.core.objects;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class GetQuest : Command
        {
            public GetQuest() : base("getQuest", permLevel: 90)
            {
            }

            protected override bool Process(Player player, TickData time, string args)
            {
                if (player.Quest == null)
                {
                    player.SendError("Player does not have a quest!");
                    return false;
                }
                player.SendInfo("Quest location: (" + player.Quest.X + ", " + player.Quest.Y + ")");
                return true;
            }
        }
    }
}
