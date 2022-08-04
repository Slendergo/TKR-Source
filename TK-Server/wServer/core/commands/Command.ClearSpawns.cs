using CA.Extensions.Concurrent;
using System.Linq;
using wServer.core.objects;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class ClearSpawns : Command
        {
            public ClearSpawns() : base("clearspawn", permLevel: 90, alias: "cs")
            { }

            protected override bool Process(Player player, TickTime time, string args)
            {
                var total = 0;
                foreach (var entity in player.World.Enemies.Values)
                    if (entity.Spawned)
                    {
                        entity.Death(time);
                        total++;
                    }

                foreach (var entity in player.World.StaticObjects.Values)
                    if (entity.Spawned)
                    {
                        entity.World.LeaveWorld(entity);
                        total++;
                    }

                player.SendInfo($"{total} spawned entit{(total > 1 ? "ies" : "y")} removed!");
                return true;
            }
        }
    }
}
