using System.Linq;
using wServer.core;
using wServer.core.objects;

namespace wServer.logic.behaviors
{
    internal class RemoveEntity : Behavior
    {
        private readonly string children;
        private readonly float dist;

        public RemoveEntity(double dist, string children)
        {
            this.dist = (float)dist;
            this.children = children;
        }

        protected override void OnStateEntry(Entity host, TickData time, ref object state)
        {
            var lastKilled = -1;
            var killed = 0;

            while (killed != lastKilled)
            {
                lastKilled = killed;

                foreach (var entity in host.GetNearestEntitiesByName(dist, children).OfType<Enemy>())
                {
                    entity.Spawned = true;
                    entity.Death(time);
                    killed++;
                }
            }
        }

        protected override void TickCore(Entity host, TickData time, ref object state)
        { }
    }
}
