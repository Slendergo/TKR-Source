using System;
using TKR.Shared.resources;
using TKR.WorldServer.core.miscfile.datas;
using TKR.WorldServer.core.miscfile.structures;
using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.utils;

namespace TKR.WorldServer.logic.behaviors
{
    internal class StayAbove : CycleBehavior
    {
        private readonly int altitude;
        private readonly float speed;

        public StayAbove(double speed, int altitude)
        {
            this.speed = (float)speed;
            this.altitude = altitude;
        }

        protected override void TickCore(Entity host, TickTime time, ref object state)
        {
            Status = CycleStatus.NotStarted;

            if (host == null || host.World == null)
                return;

            if (host.HasConditionEffect(ConditionEffectIndex.Paralyzed))
                return;

            var map = host.World.Map;
            var tile = map[(int)host.X, (int)host.Y];

            if (tile == null)
                return;

            if (tile.Elevation != 0 && tile.Elevation < altitude)
            {
                var vect = new Vector2(map.Width / 2 - host.X, map.Height / 2 - host.Y);
                vect.Normalize();

                var dist = host.GetSpeed(speed) * time.BehaviourTickTime;
                host.ValidateAndMove(host.X + vect.X * dist, host.Y + vect.Y * dist);

                Status = CycleStatus.InProgress;
            }
            else
                Status = CycleStatus.Completed;
        }
    }
}
