using common.resources;
using Mono.Game;
using wServer.core;
using wServer.core.objects;

namespace wServer.logic.behaviors
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

        protected override void TickCore(Entity host, TickData time, ref object state)
        {
            Status = CycleStatus.NotStarted;

            if (host == null || host.Owner == null)
                return;

            if (host.HasConditionEffect(ConditionEffects.Paralyzed))
                return;

            var map = host.Owner.Map;
            var tile = map[(int)host.X, (int)host.Y];

            if (tile == null)
                return;

            if (tile.Elevation != 0 && tile.Elevation < altitude)
            {
                var vect = new Vector2(map.Width / 2 - host.X, map.Height / 2 - host.Y);
                vect.Normalize();

                var dist = host.GetSpeed(speed) * time.DeltaTime;
                host.ValidateAndMove(host.X + vect.X * dist, host.Y + vect.Y * dist);

                Status = CycleStatus.InProgress;
            }
            else
                Status = CycleStatus.Completed;
        }
    }
}
