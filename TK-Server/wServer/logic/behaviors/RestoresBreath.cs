using System;
using wServer.core;
using wServer.core.objects;

namespace wServer.logic.behaviors
{
    public sealed class RestoresBreath : Behavior
    {
        private readonly float radius;
        private readonly float radiusSqr;
        private readonly float speed;

        public RestoresBreath(double radius = 1.0, double speed = 50)
        {
            this.radius = (float)radius;

            radiusSqr = (float)(radius * radius);

            this.speed = (float)speed;
        }

        protected override void TickCore(Entity host, TickData time, ref object state)
        {
            var players = host.Owner.PlayersCollision.HitTest(host.X, host.Y, radius);

            foreach (var o in players)
            {
                if (!(o is Player))
                    continue;

                var player = o as Player;
                var distSq = player.DistSqr(host);

                if (distSq < radiusSqr)
                    player.Breath = Math.Min(100.0, player.Breath + time.DeltaTime * speed);
            }
        }
    }
}
