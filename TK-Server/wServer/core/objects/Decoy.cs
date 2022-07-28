using Mono.Game;
using System;
using System.Collections.Generic;
using wServer.networking;
using wServer.networking.packets.outgoing;

namespace wServer.core.objects
{
    internal class Decoy : StaticObject, IPlayer
    {
        private static Random rand = new Random();

        private readonly int duration;
        private readonly float speed;

        private Vector2 direction;
        private bool exploded = false;
        private Player player;

        public Decoy(Player player, int duration, float tps) : base(player.CoreServerManager, 0x0715, duration, true, true, true)
        {
            this.player = player;
            this.duration = duration;

            speed = tps;

            var history = player.TryGetHistory(1);

            if (history == null)
                direction = GetRandDirection();
            else
            {
                direction = new Vector2(player.X - history.Value.X, player.Y - history.Value.Y);

                if (direction.LengthSquared() == 0)
                    direction = GetRandDirection();
                else
                    direction.Normalize();
            }
        }

        public void Damage(int dmg, Entity src)
        { }

        public bool IsVisibleToEnemy() => true;

        public override void Tick(TickData time)
        {
            if (HP > duration - 2000)
                ValidateAndMove(X + direction.X * speed * time.ElaspedMsDelta / 1000, Y + direction.Y * speed * time.ElaspedMsDelta / 1000);

            if (HP < 250 && !exploded)
            {
                exploded = true;

                Owner.BroadcastIfVisible(new ShowEffect()
                {
                    EffectType = EffectType.AreaBlast,
                    Color = new ARGB(0xffff0000),
                    TargetObjectId = Id,
                    Pos1 = new Position() { X = 1 }
                }, this, PacketPriority.Low);
            }

            base.Tick(time);
        }

        protected override void ExportStats(IDictionary<StatDataType, object> stats)
        {
            stats[StatDataType.Texture1] = player.Texture1;
            stats[StatDataType.Texture2] = player.Texture2;

            base.ExportStats(stats);
        }

        private Vector2 GetRandDirection()
        {
            var angle = rand.NextDouble() * 2 * Math.PI;

            return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }
    }
}
