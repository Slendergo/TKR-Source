using System.Linq;
using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.logic;
using TKR.WorldServer.utils;

namespace TKR.WorldServer.logic.behaviors
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

        protected override void OnStateEntry(Entity host, TickTime time, ref object state)
        {
            foreach (var entity in host.GetNearestEntitiesByName(dist, children).OfType<Enemy>())
                entity.Expunge();
        }

        protected override void TickCore(Entity host, TickTime time, ref object state)
        {
        }
    }
}
