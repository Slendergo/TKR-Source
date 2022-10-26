using TKR.Shared.resources;
using System;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.utils;
using TKR.WorldServer.core.worlds;

namespace TKR.WorldServer.logic.behaviors
{
    internal class Wander : CycleBehavior
    {
        private readonly float speed;

        public Wander(double speed) => this.speed = (float)speed;

        protected override void TickCore(Entity host, TickTime time, ref object state)
        {
            var storage = state == null ? new WanderStorage() : (WanderStorage)state;

            Status = CycleStatus.NotStarted;

            if (host.HasConditionEffect(ConditionEffectIndex.Paralyzed))
                return;

            Status = CycleStatus.InProgress;

            if (storage.RemainingDistance <= 0)
            {
                storage.Direction = new Vector2(Random.Next() % 2 == 0 ? -1 : 1, Random.Next() % 2 == 0 ? -1 : 1);
                storage.Direction.Normalize();
                storage.RemainingDistance = speed;
                Status = CycleStatus.Completed;
            }

            var dist = host.GetSpeed(speed) * time.BehaviourTickTime;
            host.ValidateAndMove(host.X + storage.Direction.X * dist, host.Y + storage.Direction.Y * dist);

            storage.RemainingDistance -= dist;

            state = storage;
        }

        private class WanderStorage
        {
            public Vector2 Direction;
            public float RemainingDistance;
        }
    }
}
