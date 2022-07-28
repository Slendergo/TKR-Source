using common.resources;
using Mono.Game;
using wServer.core;
using wServer.core.objects;

namespace wServer.logic.behaviors
{
    internal class StayCloseToSpawn : CycleBehavior
    {
        private readonly int range;
        private readonly float speed;

        public StayCloseToSpawn(double speed, int range = 5)
        {
            this.speed = (float)speed;
            this.range = range;
        }

        protected override void OnStateEntry(Entity host, TickData time, ref object state) => state = new Vector2(host.X, host.Y);

        protected override void TickCore(Entity host, TickData time, ref object state)
        {
            Status = CycleStatus.NotStarted;

            if (host.HasConditionEffect(ConditionEffects.Paralyzed))
                return;

            if (!(state is Vector2))
            {
                state = new Vector2(host.X, host.Y);
                Status = CycleStatus.Completed;

                return;
            }

            var vect = (Vector2)state;

            if ((vect - new Vector2(host.X, host.Y)).Length() > range)
            {
                vect -= new Vector2(host.X, host.Y);
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
