using wServer.core.objects;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class DeltaTimeCommand : Command
        {
            public override string CommandName => "deltatime";

            protected override bool Process(Player player, TickTime time, string args)
            {
                player.SendInfo($"[DeltaTime]: {player.World.DisplayName} -> {time.ElaspedMsDelta} | {time.LogicTime}");
                return true;
            }
        }
    }
}
