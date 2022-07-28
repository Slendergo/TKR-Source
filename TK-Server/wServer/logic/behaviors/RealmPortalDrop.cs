using wServer.core;
using wServer.core.objects;

namespace wServer.logic.behaviors
{
    public class RealmPortalDrop : Behavior
    {
        protected internal override void Resolve(State parent) => parent.Death += (e, s) =>
        {
            var owner = s.Host.Owner;

            if (owner.Name.Contains("DeathArena") || s.Host.Spawned)
                return;

            var en = s.Host.GetNearestEntity(100, 0x0704);
            var portal = Entity.Resolve(s.Host.CoreServerManager, "Realm Portal");

            if (en != null)
                portal.Move(en.X, en.Y);
            else
                portal.Move(s.Host.X, s.Host.Y);

            s.Host.Owner.EnterWorld(portal);
        };

        protected override void OnStateEntry(Entity host, TickData time, ref object state)
        {
            if (host.GetNearestEntity(100, 0x5e4b) != null)
                return;

            var opener = Entity.Resolve(host.CoreServerManager, "Realm Portal Opener");

            host.Owner.EnterWorld(opener);
            opener.Move(host.X, host.Y);
        }

        protected override void TickCore(Entity host, TickData time, ref object state)
        { }
    }
}
