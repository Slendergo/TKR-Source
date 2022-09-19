using System.Linq;
using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.logic;
using TKR.WorldServer.utils;

namespace TKR.WorldServer.logic.transitions
{
    internal class EntitiesNotExistsTransition : Transition
    {
        //State storage: none

        private readonly double _dist;
        private readonly ushort[] _targets;

        public EntitiesNotExistsTransition(double dist, string targetState, params string[] targets)
            : base(targetState)
        {
            _dist = dist;

            if (targets.Length <= 0)
                return;

            _targets = targets.Select(Behavior.GetObjType).ToArray();
        }

        protected override bool TickCore(Entity host, TickTime time, ref object state)
        {
            if (_targets == null)
                return false;

            return _targets.All(t => host.GetNearestEntity(_dist, t) == null);
        }
    }
}
