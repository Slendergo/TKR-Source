using common.database;
using common.resources;
using wServer.core.objects;
using wServer.core.worlds;
using wServer.core.worlds.logic;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class Summon : Command
        {
            public Summon() : base("summon", permLevel: 110)
            {
            }

            protected override bool Process(Player player, TickData time, string args)
            {
                foreach (var i in player.Owner.Players)
                {
                    if (i.Value.Name.EqualsIgnoreCase(args))
                    {
                        if ((player.Owner.Id == World.Vault || player.Owner.Name.Contains("Vault")) && player.Rank < 110)
                        {
                            player.SendError("Only rank 110 accounts can summon other player to vault.");
                            return false;
                        }

                        // probably someone hidden doesn't want to be summoned, so we leave it as before here
                        if (i.Value.HasConditionEffect(ConditionEffects.Hidden) || i.Value.Owner is Vault)
                            break;

                        i.Value.Teleport(time, player.Id, true);
                        i.Value.SendInfo($"You've been summoned by {player.Name}.");
                        player.SendInfo("Player summoned!");
                        return true;
                    }
                }
                player.SendError($"Player '{args}' could not be found!");
                return false;
            }
        }
    }
}
