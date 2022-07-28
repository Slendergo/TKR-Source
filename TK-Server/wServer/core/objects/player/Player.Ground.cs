using common.resources;
using wServer.networking;
using wServer.networking.packets.outgoing;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace wServer.core.objects
{
    public partial class Player
    {
        private bool anticheat;
        private long l;

        private const float MoveSpeedThreshold = 1.1f;

        public float MoveMultiplier = 1f;
        public int MoveTime;
        public int AwaitingMoves;
        public Queue<int> AwaitingGoto;
        public int TickId;
        public float PushX;
        public float PushY;

        /*public bool ValidMove(int time, Position pos)
        {
            int diff = time - MoveTime;
            float speed = Stats.GetSpeed() * diff * MoveSpeedThreshold;
            Position pushedServer = new Position(X - (diff * PushX), Y - (diff * PushY));
            if (pos.Distance(pos, pushedServer) > speed && pos.Dist(new Position() { X = RealX, Y = RealY }) > speed)
            {
                return false;
            }
            return true;
        }*/

        public void ForceGroundHit(TickData time, Position pos, int timeHit)
        {
            if (Owner == null || Owner.Map == null || HasConditionEffect(ConditionEffects.Paused) || HasConditionEffect(ConditionEffects.Invincible))
                return;

            var tile = Owner.Map[(int)pos.X, (int)pos.Y];
            var objDesc = tile.ObjType == 0 ? null : CoreServerManager.Resources.GameData.ObjectDescs[tile.ObjType];
            var tileDesc = CoreServerManager.Resources.GameData.Tiles[tile.TileId];

            if (tileDesc.Damaging && (objDesc == null || !objDesc.ProtectFromGroundDamage))
            {
                int dmg = (int)Client.Random.NextIntRange((uint)tileDesc.MinDamage, (uint)tileDesc.MaxDamage);

                HP -= dmg;

                if (HP <= 0)
                {
                    Death(tileDesc.ObjectId, tile: tile);
                    return;
                }

                Owner.BroadcastIfVisibleExclude(new Damage()
                {
                    TargetId = Id,
                    DamageAmount = (ushort)dmg,
                    Kill = HP <= 0,
                }, this, this, PacketPriority.Low);
                anticheat = true;
            }
        }

        public void GroundEffect(TickData time)
        {
            if (Owner == null || Owner.Map == null || HasConditionEffect(ConditionEffects.Paused) || HasConditionEffect(ConditionEffects.Invincible) || HasConditionEffect(ConditionEffects.Stasis))
                return;

            var tile = Owner.Map[(int)X, (int)Y];
            var objDesc = tile.ObjType == 0 ? null : CoreServerManager.Resources.GameData.ObjectDescs[tile.ObjType];
            var tileDesc = CoreServerManager.Resources.GameData.Tiles[tile.TileId];

            if (tileDesc.Effects != null)
                ApplyConditionEffect(tileDesc.Effects);

            if (time.TotalElapsedMs - l > 500 && !anticheat)
            {
                if (HasConditionEffect(ConditionEffects.Paused) ||
                    HasConditionEffect(ConditionEffects.Invincible))
                    return;

                if (tileDesc.Damaging && (objDesc == null || !objDesc.ProtectFromGroundDamage))
                {
                    int dmg = (int)Client.Random.NextIntRange((uint)tileDesc.MinDamage, (uint)tileDesc.MaxDamage);

                    HP -= dmg;

                    if (HP <= 0)
                    {
                        Death(tileDesc.ObjectId, tile: tile);
                        return;
                    }

                    Owner.BroadcastIfVisibleExclude(new Damage()
                    {
                        TargetId = Id,
                        DamageAmount = (ushort)dmg,
                        Kill = HP <= 0,
                    }, this, this, PacketPriority.Low);

                    l = time.TotalElapsedMs;
                }
            }
        }
    }
}
