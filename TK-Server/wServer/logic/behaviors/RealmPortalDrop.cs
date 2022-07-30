using wServer.core;
using wServer.core.objects;

namespace wServer.logic.behaviors
{
    public class RealmPortalDrop : Behavior
    {
        protected internal override void Resolve(State parent) => parent.Death += (e, s) =>
        {
            var owner = s.Host.World;

            if (owner.IdName.Contains("DeathArena") || s.Host.Spawned)
                return;

            var en = s.Host.GetNearestEntity(100, 0x0704);
            var portal = Entity.Resolve(s.Host.CoreServerManager, "Realm Portal");

            if (en != null)
                portal.Move(en.X, en.Y);
            else
                portal.Move(s.Host.X, s.Host.Y);

            s.Host.World.EnterWorld(portal);
        };

        protected override void OnStateEntry(Entity host, TickTime time, ref object state)
        {
            if (host.GetNearestEntity(100, 0x5e4b) != null)
                return;

            var opener = Entity.Resolve(host.CoreServerManager, "Realm Portal Opener");

            host.World.EnterWorld(opener);
            opener.Move(host.X, host.Y);
        }

        protected override void TickCore(Entity host, TickTime time, ref object state)
        { }
    }
}
