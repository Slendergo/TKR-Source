using System.Linq;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.core.worlds;
using TKR.WorldServer.utils;

namespace TKR.WorldServer.logic.behaviors
{
    internal class OrderOnDeath : Behavior
    {
        private readonly float _probability;
        private readonly double _range;
        private readonly string _stateName;
        private readonly ushort _target;

        private State _targetState;

        public OrderOnDeath(double range, string target, string state, double probability = 1)
        {
            _range = range;
            _target = GetObjType(target);
            _stateName = state;
            _probability = (float)probability;
        }

        public override void OnDeath(Entity host, ref TickTime time)
        {
            if (_targetState == null)
                _targetState = FindState(host.GameServer.BehaviorDb.Definitions[_target].Item1, _stateName);

            if (Random.NextDouble() < _probability)
                foreach (var i in host.GetNearestEntities(_range, _target))
                    if (!i.CurrentState.Is(_targetState))
                        i.SwitchTo(_targetState);
        }

        protected override void TickCore(Entity host, TickTime time, ref object state)
        {
        }

        private static State FindState(State state, string name)
        {
            if (state.Name == name)
                return state;

            return state.States.Select(i => FindState(i, name)).FirstOrDefault(s => s != null);
        }
    }
}
