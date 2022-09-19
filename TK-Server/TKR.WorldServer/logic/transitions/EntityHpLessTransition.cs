using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.logic;
using TKR.WorldServer.utils;

namespace TKR.WorldServer.logic.transitions
{
    internal class EntityHpLessTransition : Transition
    {
        //State storage: none

        private readonly double _dist;
        private readonly string _entity;
        private readonly double _threshold;

        public EntityHpLessTransition(double dist, string entity, double threshold, string targetState)
            : base(targetState)
        {
            _dist = dist;
            _entity = entity;
            _threshold = threshold;
        }

        protected override bool TickCore(Entity host, TickTime time, ref object state)
        {
            var entity = host.GetNearestEntityByName(_dist, _entity);

            if (entity == null)
                return false;

            return (double)(entity as Enemy).HP / (entity as Enemy).MaximumHP < _threshold;
        }
    }
}
