using wServer.core.objects;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class SetGuildLootBoost : Command
        {
            public SetGuildLootBoost() : base("sg", permLevel: 100)
            {
            }

            protected override bool Process(Player player, TickData time, string args)
            {
                var db = Program.CoreServerManager.Database;
                db.CreateAndSetHashToAllGuilds();
                return true;
            }
        }
    }
}
