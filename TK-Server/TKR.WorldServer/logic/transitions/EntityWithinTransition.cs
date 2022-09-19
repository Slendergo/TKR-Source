using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.logic;
using TKR.WorldServer.utils;

namespace TKR.WorldServer.logic.transitions
{
    internal class EntityWithinTransition : Transition
    {
        //State storage: none

        private readonly double _dist;
        private readonly string _entity;

        public EntityWithinTransition(double dist, string entity, string targetState)
            : base(targetState)
        {
            _dist = dist;
            _entity = entity;
        }

        protected override bool TickCore(Entity host, TickTime time, ref object state)
        {
            return host.GetNearestEntityByName(_dist, _entity) != null;
        }
    }
}
