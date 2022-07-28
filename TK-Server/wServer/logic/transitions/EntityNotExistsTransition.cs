using System.Linq;
using wServer.core;
using wServer.core.objects;

namespace wServer.logic.transitions
{
    internal class EntityNotExistsTransition : Transition
    {
        //State storage: none

        private readonly double _dist;
        private readonly ushort? _target;
        private readonly bool _attackTarget;

        public EntityNotExistsTransition(string target, double dist, string targetState, bool checkAttackTarget = false)
            : base(targetState)
        {
            _dist = dist;

            if (target != null)
                _target = Behavior.GetObjType(target);

            _attackTarget = checkAttackTarget;
        }

        protected override bool TickCore(Entity host, TickData time, ref object state)
        {
            if (_attackTarget)
            {
                if (host.AttackTarget == null || !host.Owner.GetPlayers().Contains(host.AttackTarget))
                {
                    host.AttackTarget = null;
                    return true;
                }
                return false;
            }
            return host.GetNearestEntity(_dist, _target) == null;
        }
    }
}
