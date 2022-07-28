using common.resources;
using Mono.Game;
using wServer.core;
using wServer.core.objects;

namespace wServer.logic.behaviors
{
    internal class StayBack : CycleBehavior
    {
        private readonly float distance;
        private readonly string entity;
        private readonly float speed;

        public StayBack(double speed, double distance = 8, string entity = null)
        {
            this.speed = (float)speed;
            this.distance = (float)distance;
            this.entity = entity;
        }

        protected override void TickCore(Entity host, TickData time, ref object state)
        {
            int cooldown;
            if (state == null) cooldown = 1000;
            else cooldown = (int)state;

            Status = CycleStatus.NotStarted;

            if (host.HasConditionEffect(ConditionEffects.Paralyzed))
                return;

            var e = entity != null ? host.GetNearestEntityByName(distance, entity) : host.GetNearestEntity(distance, null);

            if (e != null)
            {
                var vect = new Vector2(e.X - host.X, e.Y - host.Y);
                vect.Normalize();

                var dist = host.GetSpeed(speed) * time.DeltaTime;

                host.ValidateAndMove(host.X + (-vect.X) * dist, host.Y + (-vect.Y) * dist);

                if (cooldown <= 0)
                {
                    Status = CycleStatus.Completed;

                    cooldown = 1000;
                }
                else
                {
                    Status = CycleStatus.InProgress;

                    cooldown -= time.ElaspedMsDelta;
                }
            }

            state = cooldown;
        }
    }
}
