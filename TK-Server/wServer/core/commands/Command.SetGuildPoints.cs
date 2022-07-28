using wServer.core.objects;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class SetGuildPoints : Command
        {
            public SetGuildPoints() : base("uac", permLevel: 120)
            {
            }

            protected override bool Process(Player player, TickData time, string args)
            {
                var db = Program.CoreServerManager.Database;
                db.UpdateAllCurrency();
                player.SendInfo("Success!");
                return true;
            }
        }
    }
}
