using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.core.objects;

namespace TKR.WorldServer.logic.behaviors
{
    internal class SetNoXP : Behavior
    {
        protected override void OnStateEntry(Entity host, TickTime time, ref object state) => host.GivesNoXp = true;

        protected override void TickCore(Entity host, TickTime time, ref object state)
        { }
    }
}
