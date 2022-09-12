using common;
using wServer.core.objects;

namespace wServer.networking.packets.outgoing
{
    public class ServerPlayerShoot : OutgoingMessage
    {
        public int BulletId { get; }
        public int ObjectId { get; }
        public int ObjectType { get; }
        public Position StartingPos { get; }
        public float Angle { get; }
        public int Damage { get; }

        public override MessageId MessageId => MessageId.SERVERPLAYERSHOOT;

        public ServerPlayerShoot(int bulletId, int objectId, int objectType, Position startingPosition, float angle, int damage)
        {
            BulletId = bulletId;
            ObjectId = objectId;
            ObjectType = objectType;
            StartingPos = startingPosition;
            Angle = angle;
            Damage = damage;
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write(BulletId);
            wtr.Write(ObjectId);
            wtr.Write(ObjectType);
            StartingPos.Write(wtr);
            wtr.Write(Angle);
            wtr.Write(Damage);
        }
    }
}
