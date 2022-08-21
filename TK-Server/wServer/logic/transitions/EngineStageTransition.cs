using wServer.core;
using wServer.core.objects;
using wServer.core.worlds.logic;

namespace wServer.logic.transitions
{
    internal class EngineStageTransition : Transition
    {
        //State storage: none

        private readonly int State;

        public EngineStageTransition(int state, string targetState)
            : base(targetState)
        {
            State = state;
        }

        protected override bool TickCore(Entity host, TickTime time, ref object state)
        {
            return host.GameServer.WorldManager.Nexus.EngineStage == State;
        }
    }
}
