using TKR.WorldServer.core.objects;
using TKR.WorldServer.core.worlds;
using TKR.WorldServer.logic;
using TKR.WorldServer.utils;

namespace TKR.WorldServer.logic.behaviors
{
    internal class DestroyOnDeath : Behavior
    {
        private readonly string _target;

        public DestroyOnDeath(string target) => _target = target;

        public override void OnDeath(Entity host, ref TickTime time)
        {
            var owner = host.World;
            var entities = host.GetNearestEntitiesByName(250, _target);

            if (entities != null)
                foreach (var ent in entities)
                    owner.LeaveWorld(ent);
        }

        protected override void TickCore(Entity host, TickTime time, ref object state)
        {
        }
    }
}
