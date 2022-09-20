using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.logic;
using TKR.WorldServer.core;
using TKR.WorldServer.core.miscfile.world;

namespace TKR.WorldServer.logic.behaviors
{
    internal class SwirlingMistDeathParticles : Behavior
    {
        protected override void OnStateEntry(Entity host, TickTime time, ref object state)
        {
            var entity = Entity.Resolve(host.GameServer, "SwirlingMist Particles");
            entity.Move(host.X, host.Y);

            host.World.StartNewTimer(1000, (w, t) => w.LeaveWorld(entity));
            host.World.EnterWorld(entity);
        }

        protected override void TickCore(Entity host, TickTime time, ref object state)
        { }
    }
}
