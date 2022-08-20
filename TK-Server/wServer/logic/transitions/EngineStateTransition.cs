using wServer.core;
using wServer.core.objects;
using wServer.core.worlds.logic;

namespace wServer.logic.transitions
{
    internal class EngineStateTransition : Transition
    {
        //State storage: none

        private readonly int State;

        public EngineStateTransition(int state, string targetState)
            : base(targetState)
        {
            State = state;
        }

        protected override bool TickCore(Entity host, TickTime time, ref object state)
        {
            return host.GameServer.WorldManager.Nexus.EngineState == State;
        }
    }
}
