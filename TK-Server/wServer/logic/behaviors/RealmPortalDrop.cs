using wServer.core;
using wServer.core.objects;

namespace wServer.logic.behaviors
{
    public class RealmPortalDrop : Behavior
    {
        public override void OnDeath(Entity host, ref TickTime time)
        {
            var owner = host.World;

            if (owner.IdName.Contains("DeathArena") || host.Spawned)
                return;

            var en = host.GetNearestEntity(100, 0x0704);
            var portal = Entity.Resolve(host.CoreServerManager, "Realm Portal");

            if (en != null)
                portal.Move(en.X, en.Y);
            else
                portal.Move(host.X, host.Y);

            host.World.EnterWorld(portal);
        }

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
