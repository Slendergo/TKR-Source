using TKR.Shared.resources;
using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.core.worlds.logic;
using TKR.WorldServer.logic;

namespace TKR.WorldServer.logic.behaviors
{
    internal class Transform : Behavior
    {
        private readonly ushort target;

        public Transform(string target) => this.target = GetObjType(target);

        protected override void TickCore(Entity host, TickTime time, ref object state)
        {
            var entity = Entity.Resolve(host.GameServer, target);

            if (entity is Portal && host.World.IdName.Contains("Arena"))
                return;

            entity.Move(host.X, host.Y);

            if (host is Enemy && entity is Enemy && (host as Enemy).Spawned)
            {
                (entity as Enemy).Spawned = true;
                (entity as Enemy).ApplyPermanentConditionEffect(ConditionEffectIndex.Invisible);
            }

            host.World.EnterWorld(entity);
            host.World.LeaveWorld(host);
        }
    }

    internal class EngineTransform : Behavior
    {
        private readonly ushort target;

        public EngineTransform(string target) => this.target = GetObjType(target);

        protected override void TickCore(Entity host, TickTime time, ref object state)
        {
            var entity = (Engine)Entity.Resolve(host.GameServer, target);
            if (entity == null)
                return;
            entity.Move(host.X, host.Y);

            host.World.EnterWorld(entity);
            host.World.LeaveWorld(host);
            (host.World as NexusWorld).SetEngine(entity);
        }
    }
}
