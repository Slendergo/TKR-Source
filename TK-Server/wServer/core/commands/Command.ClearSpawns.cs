using CA.Extensions.Concurrent;
using wServer.core.objects;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class ClearSpawns : Command
        {
            public ClearSpawns() : base("clearspawn", permLevel: 90, alias: "cs")
            { }

            protected override bool Process(Player player, TickData time, string args)
            {
                var entities = player.Owner.Enemies
                    .ValueWhereAsParallel(_ => _.Spawned);
                for (var i = 0; i < entities.Length; i++)
                    entities[i].Death(time);

                var total = entities.Length;
                var staticObjs = player.Owner.StaticObjects
                    .ValueWhereAsParallel(_ => _.Spawned);
                for (var i = 0; i < staticObjs.Length; i++)
                    player.Owner.LeaveWorld(staticObjs[i]);

                total += staticObjs.Length;
                player.SendInfo($"{total} spawned entit{(total > 1 ? "ies" : "y")} removed!");
                return true;
            }
        }
    }
}
