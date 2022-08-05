using common.resources;
using wServer.core.objects;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class DestroyWorld : Command
        {
            public DestroyWorld() : base("destroyWorld", permLevel: 100, alias: "dw")
            { }

            protected override bool Process(Player player, TickTime time, string args)
            {
                var world = player.World;

                if (world.InstanceType != WorldResourceInstanceType.Dungeon)
                {
                    player.SendError("You cannot destroy worlds that aren't dungeon types.");
                    return false;
                }

                if (player.GameServer.WorldManager.RemoveWorld(world))
                {
                    player.SendInfo("Successfully removed this world instance! Disconnecting all players within 3 seconds.");
                    player.World.Timers.Add(new WorldTimer(3000, (w, t) => w.ForeachPlayer(_ => _.Client.Disconnect("World destroyed by Admin"))));
                    return true;
                }

                player.SendError("Couldn't remove this world instance.");
                return false;
            }
        }
    }
}
