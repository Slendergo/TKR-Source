using wServer.core.objects;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class ClearGraves : Command
        {
            public ClearGraves() : base("cleargraves", permLevel: 90, alias: "cgraves")
            { }

            protected override bool Process(Player player, TickData time, string args)
            {
                var total = 0;
                foreach (var entry in player.Owner.StaticObjects)
                {
                    var entity = entry.Value;
                    if (entity is Container || entity.ObjectDesc == null)
                        continue;

                    if (entity.ObjectDesc.ObjectId.StartsWith("Gravestone") && entity.Dist(player) < 15d)
                    {
                        player.Owner.LeaveWorld(entity);
                        total++;
                    }
                }

                player.SendInfo($"{total} gravestone{(total > 1 ? "s" : "")} removed!");
                return true;
            }
        }
    }
}
