using common.resources;
using wServer.core.objects;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class SummonAll : Command
        {
            public SummonAll() : base("summonall", permLevel: 110)
            {
            }

            protected override bool Process(Player player, TickData time, string args)
            {
                foreach (var i in player.Owner.Players)
                {
                    // probably someone hidden doesn't want to be summoned, so we leave it as before here
                    if (i.Value.HasConditionEffect(ConditionEffects.Hidden))
                        break;

                    i.Value.Teleport(time, player.Id, true);
                    i.Value.SendInfo($"You've been summoned by {player.Name}.");
                }

                player.SendInfo("All players summoned!");
                return true;
            }
        }
    }
}
