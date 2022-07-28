using common.resources;
using Mono.Game;
using System;
using wServer.core;
using wServer.core.objects;

namespace wServer.logic.behaviors
{
    internal class Buzz : CycleBehavior
    {
        private readonly float dist;
        private readonly float speed;
        private Cooldown coolDown;

        public Buzz(double speed = 2, double dist = 0.5, Cooldown coolDown = new Cooldown())
        {
            this.speed = (float)speed;
            this.dist = (float)dist;
            this.coolDown = coolDown.Normalize(1);
        }

        protected override void OnStateEntry(Entity host, TickData time, ref object state) => state = new BuzzStorage();

        protected override void TickCore(Entity host, TickData time, ref object state)
        {
            var storage = (BuzzStorage)state;

            Status = CycleStatus.NotStarted;

            if (host.HasConditionEffect(ConditionEffects.Paralyzed))
                return;

            if (storage.RemainingTime > 0)
            {
                storage.RemainingTime -= time.ElaspedMsDelta;

                Status = CycleStatus.NotStarted;
            }
            else
            {
                Status = CycleStatus.InProgress;

                if (storage.RemainingDistance <= 0)
                {
                    do storage.Direction = new Vector2(Random.Next(-1, 2), Random.Next(-1, 2));
                    while (storage.Direction.X == 0 && storage.Direction.Y == 0);

                    storage.Direction.Normalize();
                    storage.RemainingDistance = this.dist;

                    Status = CycleStatus.Completed;
                }

                var dist = host.GetSpeed(speed) * time.DeltaTime;

                host.ValidateAndMove(host.X + storage.Direction.X * dist, host.Y + storage.Direction.Y * dist);

                storage.RemainingDistance -= dist;
            }

            state = storage;
        }

        private class BuzzStorage
        {
            public Vector2 Direction;
            public float RemainingDistance;
            public int RemainingTime;
        }
    }
}
