using TKR.Shared.resources;
using TKR.WorldServer.core.miscfile.structures;
using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.core.worlds.logic;
using TKR.WorldServer.logic.behaviors.@new;
using TKR.WorldServer.utils;

namespace TKR.WorldServer.logic.behaviors.@new.movements
{
    internal class NewStayAbove : OrderedBehavior
    {
        private readonly int Altitude;
        private readonly float Speed;

        public NewStayAbove(double speed, int altitude)
        {
            Speed = (float)speed;
            Altitude = altitude;
        }

        protected override bool TickCoreOrdered(Entity host, TickTime time, ref object state)
        {
            if (!(host.World is RealmWorld))
                return true; // its a success 

            var map = host.World.Map;
            var tile = map[(int)host.X, (int)host.Y];

            if (tile.Elevation != 0 && tile.Elevation < Altitude)
            {
                var targetX = map.Width / 2;
                var targetY = map.Height / 2;
                return host.MoveToward(targetX, targetY, Speed * time.BehaviourTickTime);
            }
            return false;
        }
    }
}
