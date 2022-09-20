using TKR.Shared.resources;
using System;
using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.utils;
using TKR.WorldServer.core.miscfile.structures;

namespace TKR.WorldServer.logic.behaviors
{
    internal class OrderedBehavior : Behavior
    {
        private Behavior[] Children;

        public OrderedBehavior(params Behavior[] children) => Children = children;

        protected override void TickCore(Entity host, TickTime time, ref object state)
        {
            foreach (var child in Children)
                if (child.TickOrdered(host, time))
                    break;
        }
    }

    public sealed class NewFollow : Behavior
    {
        private readonly float acquireRange;
        private readonly float range;
        private readonly float speed;
        private readonly float duration;
        private readonly float cooldown;
        private readonly bool predictive;

        public NewFollow(float acquireRange = 10.0f, float range = 3.0f, float speed = 1.0f, float duration = 30.0f, float cooldown = 5.0f, bool predictive = false)
        {
            this.acquireRange = acquireRange;
            this.range = range;
            this.speed = speed;
            this.duration = duration;
            this.cooldown = cooldown;
            this.predictive = predictive;
        }

        protected override bool TickCoreOrdered(Entity host, TickTime time, ref object state)
        {
            var s = state == null ? new FollowState() : (FollowState)state;

            s.TimeLeft -= time.DeltaTime;
            if (s.TimeLeft < 0.0f)
                s.TimeLeft = 0.0f;

            if (!s.Following && s.TimeLeft > 0.0f)
                return false;

            if (duration != 0.0f && s.Following && s.TimeLeft == 0.0f)
            {
                s.Following = false;
                s.TimeLeft = cooldown;
                return false;
            }

            var target = host.FindPlayerTarget();//.FindPlayerTarget(Host);
            if (target == null)
            {
                if (s.Following)
                {
                    s.Following = false;
                    s.TimeLeft = cooldown;
                }
                return false;
            }

            var dist = host.DistTo(target);
            if (dist > acquireRange)
            {
                if (s.Following)
                {
                    s.Following = false;
                    s.TimeLeft = cooldown;
                }
                return false;
            }

            if (!s.Following)
            {
                s.Following = true;
                s.TimeLeft = duration;
            }

            if (dist < range)
                return false;

            var targetX = target.X;
            var targetY = target.Y;
            if (predictive)
            {
                const int PREDICT_NUM_TICKS = 4;
                targetX += PREDICT_NUM_TICKS * (target.X - target.PrevX);
                targetY += PREDICT_NUM_TICKS * (target.Y - target.PrevY);
            }

            var facing = (float)Math.Atan2(targetY - host.Y, targetX - host.X);

            var spd = GetSpeedMultiplier(host, speed * time.BehaviourTickTime);
            spd = ClampSpeed(spd, 0.0f, (float)host.DistTo(targetX, targetY));

            var newPos = PointAt(host, facing, spd);

            if (host.X != newPos.X || host.Y != newPos.Y)
            {
                host.ValidateAndMove(newPos.X, newPos.Y);
                return true;
            }
            return false;
        }

        class FollowState
        {
            public bool Following;
            public float TimeLeft;
        }
    }

    public sealed class NewWander : Behavior
    {
        private readonly float speed;

        public NewWander(float speed = 0.4f)
        {
            this.speed = speed;
        }

        protected override bool TickCoreOrdered(Entity host, TickTime time, ref object state)
        {
            // todo refine in the future

            var targetX = host.X + (Random.NextDouble() > 0.5 ? -speed : speed);
            var targetY = host.Y + (Random.NextDouble() > 0.5 ? -speed : speed);

            var facing = (float)Math.Atan2(targetY - host.Y, targetX - host.X);

            var spd = GetSpeedMultiplier(host, speed * time.BehaviourTickTime);
            spd = ClampSpeed(spd, 0.0f, (float)host.DistTo(targetX, targetY));

            var newPos = PointAt(host, facing, spd);

            if (host.X != newPos.X || host.Y != newPos.Y)
            {
                host.ValidateAndMove(newPos.X, newPos.Y);
                return true;
            }
            return true;
        }
    }

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
