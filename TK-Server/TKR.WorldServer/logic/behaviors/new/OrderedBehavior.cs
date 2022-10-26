using TKR.WorldServer.core.objects;
using TKR.WorldServer.core.worlds;

namespace TKR.WorldServer.logic.behaviors.@new
{
    internal class OrderedBehavior : Behavior
    {
        private Behavior[] Children;

        public OrderedBehavior(params Behavior[] children) => Children = children;

        protected override void TickCore(Entity host, TickTime time, ref object state)
        {
            foreach (var child in Children)
                if (child.TickOrdered(host, time))
                    break;
        }
    }
}
