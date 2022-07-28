using common.resources;
using Mono.Game;
using System;
using wServer.core;
using wServer.core.objects;

namespace wServer.logic.behaviors
{
    internal class MoveLine : CycleBehavior
    {
        private readonly float _direction;
        private readonly float _distance;
        private readonly float _speed;

        public MoveLine(double speed, double direction = 0, double distance = 0)
        {
            _speed = (float)speed;
            _direction = (float)direction * (float)Math.PI / 180;
            _distance = (float)distance;
        }

        protected override void OnStateExit(Entity host, TickData time, ref object state) => state = null;

        protected override void TickCore(Entity host, TickData time, ref object state)
        {
            var dist = state == null ? _distance : (float)state;

            Status = CycleStatus.NotStarted;

            if (host.HasConditionEffect(ConditionEffects.Paralyzed))
                return;

            if (_distance == 0)
            {
                Status = CycleStatus.InProgress;

                var vect = new Vector2((float)Math.Cos(_direction), (float)Math.Sin(_direction));
                var moveDist = host.GetSpeed(_speed) * time.ElaspedMsDelta / 1000f;

                host.ValidateAndMove(host.X + vect.X * moveDist, host.Y + vect.Y * moveDist);
                Status = CycleStatus.Completed;
            }
            if (dist > 0)
            {
                Status = CycleStatus.InProgress;

                var moveDist = host.GetSpeed(_speed) * time.ElaspedMsDelta / 1000f;
                var vect = new Vector2((float)Math.Cos(_direction), (float)Math.Sin(_direction));

                host.ValidateAndMove(host.X + vect.X * moveDist, host.Y + vect.Y * moveDist);
                dist -= moveDist;
            }
            else
                Status = CycleStatus.Completed;

            state = dist;
        }
    }
}
