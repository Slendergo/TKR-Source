using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.logic;
using TKR.WorldServer.core.worlds.logic;

namespace TKR.WorldServer.logic.transitions
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
