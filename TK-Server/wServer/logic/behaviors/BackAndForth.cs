using common.resources;
using wServer.core;
using wServer.core.objects;

namespace wServer.logic.behaviors
{
    internal class BackAndForth : CycleBehavior
    {
        private readonly int distance;
        private readonly float speed;

        public BackAndForth(double speed, int distance = 5)
        {
            this.speed = (float)speed;
            this.distance = distance;
        }

        protected override void TickCore(Entity host, TickData time, ref object state)
        {
            var dist = state == null ? distance : (float)state;

            Status = CycleStatus.NotStarted;

            if (host.HasConditionEffect(ConditionEffects.Paralyzed))
                return;

            var moveDist = host.GetSpeed(speed) * time.DeltaTime;

            if (dist > 0)
            {
                Status = CycleStatus.InProgress;

                host.ValidateAndMove(host.X + moveDist, host.Y);
                dist -= moveDist;

                if (dist <= 0)
                {
                    dist = -distance;
                    Status = CycleStatus.Completed;
                }
            }
            else
            {
                Status = CycleStatus.InProgress;

                host.ValidateAndMove(host.X - moveDist, host.Y);
                dist += moveDist;

                if (dist >= 0)
                {
                    dist = distance;
                    Status = CycleStatus.Completed;
                }
            }

            state = dist;
        }
    }
}
