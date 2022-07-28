using wServer.core.objects;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class HealthTest : Command
        {
            public HealthTest() : base("healthtest", permLevel: 110, alias: "htest")
            {
            }

            protected override bool Process(Player player, TickData time, string color)
            {
                var e = Entity.Resolve(player.CoreServerManager, "Coral Gift");
                e.SetDefaultSize(300);
                e.Move(player.X, player.Y);
                (e as Enemy).HP = 100000000;
                (e as Enemy).MaximumHP = 100000000;
                player.Owner.EnterWorld(e);
                e.Spawned = true;
                return true;
            }
        }
    }
}
