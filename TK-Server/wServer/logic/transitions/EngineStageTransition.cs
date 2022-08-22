using wServer.core;
using wServer.core.objects;
using wServer.core.worlds.logic;

namespace wServer.logic.transitions
{
    internal class EngineStageTransition : Transition
    {

        private readonly int Stage;

        public EngineStageTransition(int stage, string targetState)
            : base(targetState)
        {
            Stage = stage;
        }

        protected override bool TickCore(Entity host, TickTime time, ref object state)
        {
            return host.GameServer.WorldManager.Nexus.EngineStage == Stage;
        }
    }
}
