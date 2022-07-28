using wServer.core;
using wServer.core.objects;

namespace wServer.logic.behaviors
{
    internal class SwirlingMistDeathParticles : Behavior
    {
        protected override void OnStateEntry(Entity host, TickTime time, ref object state)
        {
            var entity = Entity.Resolve(host.CoreServerManager, "SwirlingMist Particles");
            entity.Move(host.X, host.Y);

            host.Owner.Timers.Add(new WorldTimer(1000, (w, t) => w.LeaveWorld(entity)));
            host.Owner.EnterWorld(entity);
        }

        protected override void TickCore(Entity host, TickTime time, ref object state)
        { }
    }
}
