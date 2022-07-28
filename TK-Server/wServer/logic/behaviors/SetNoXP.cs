using wServer.core;
using wServer.core.objects;

namespace wServer.logic.behaviors
{
    internal class SetNoXP : Behavior
    {
        protected override void OnStateEntry(Entity host, TickTime time, ref object state) => host.GivesNoXp = true;

        protected override void TickCore(Entity host, TickTime time, ref object state)
        { }
    }
}
