using wServer.core.objects;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class WorldID : Command
        {
            public WorldID() : base("worldId", permLevel: 110, alias: "wi")
            {
            }

            protected override bool Process(Player player, TickData time, string color)
            {
                player.SendInfo(player.Owner.Id.ToString());
                return true;
            }
        }
    }
}
