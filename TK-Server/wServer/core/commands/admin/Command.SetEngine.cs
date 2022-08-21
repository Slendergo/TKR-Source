using common;
using wServer.core.objects;
using wServer.core.worlds.logic;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class SetEngine : Command
        {
            public override RankingType RankRequirement => RankingType.Admin;
            public override string CommandName => "se";

            protected override bool Process(Player player, TickTime time, string args)
            {
                var manager = player.GameServer;
                var db = manager.Database;
                var amount = int.Parse(args);
                db.EngineAddFuel(amount);
                foreach (var entry in player.World.StaticObjects)
                {
                    var entity = entry.Value;
                    if (entity.ObjectDesc.ObjectId.StartsWith("Engine"))
                    {
                        //Engine Engine = entity as Engine;
                        //Engine.CurrentAmount = amount;
                    }
                }

                player.SendInfo("Set engine fuel to: "+amount);
                return true;
            }
        }
    }
}
