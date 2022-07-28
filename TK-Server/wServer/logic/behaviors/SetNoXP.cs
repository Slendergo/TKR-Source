using wServer.core;
using wServer.core.objects;

namespace wServer.logic.behaviors
{
    internal class SetNoXP : Behavior
    {
        protected override void OnStateEntry(Entity host, TickData time, ref object state) => host.GivesNoXp = true;

        protected override void TickCore(Entity host, TickData time, ref object state)
        { }
    }
}
