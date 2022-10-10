using System;
using System.Collections.Generic;
using TKR.WorldServer.core.miscfile;
using TKR.WorldServer.core.miscfile.datas;
using TKR.WorldServer.core.miscfile.stats;
using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.networking;
using TKR.WorldServer.networking.packets.outgoing;
using TKR.WorldServer.core.miscfile.structures;

namespace TKR.WorldServer.core.objects
{
    internal class Decoy : StaticObject, IPlayer
    {

        private readonly int duration;

        private Vector2 direction;
        private bool exploded = false;
        private Player player;

        public Decoy(Player player, int duration) : base(player.GameServer, 0x0715, duration, true, true, true)
        {
            this.player = player;
            this.duration = duration;

            direction = new Vector2(player.X - player.PrevX, player.Y - player.PrevY);
            if (direction.LengthSquared() == 0)
                direction = GetRandDirection();
            else
                direction.Normalize();
        }

        public void Damage(int dmg, Entity src)
        { }

        public bool IsVisibleToEnemy() => true;

        public override void Tick(ref TickTime time)
        {
            if (HP > duration - 2000)
                ValidateAndMove(X + direction.X * 1.0f * time.BehaviourTickTime, Y + direction.Y * 1.0f * time.BehaviourTickTime);

            if (HP < 250 && !exploded)
            {
                exploded = true;

                World.BroadcastIfVisible(new ShowEffect()
                {
                    EffectType = EffectType.AreaBlast,
                    Color = new ARGB(0xffff0000),
                    TargetObjectId = Id,
                    Pos1 = new Position() { X = 1 }
                }, this);
            }

            base.Tick(ref time);
        }

        protected override void ExportStats(List<ValueTuple<StatDataType, object>> stats, bool isOtherPlayer)
        {
            stats.Add(ValueTuple.Create(StatDataType.Texture1, player.Texture1));
            stats.Add(ValueTuple.Create(StatDataType.Texture2, player.Texture2));

            base.ExportStats(stats, isOtherPlayer);
        }

        private Vector2 GetRandDirection()
        {
            var angle = World == null ? 0 : World.Random.NextDouble() * 2 * Math.PI;
            return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }
    }
}
