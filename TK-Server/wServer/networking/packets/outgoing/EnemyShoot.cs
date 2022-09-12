using common;
using wServer.core.objects;

namespace wServer.networking.packets.outgoing
{
    public class EnemyShoot : OutgoingMessage
    {
        public int BulletId { get; }
        public int ObjectId { get; }
        public byte BulletType { get; }
        public int ObjectType { get; }
        public Position StartingPos { get; }
        public float Angle { get; }
        public int Damage { get; }
        public byte NumShots { get; }
        public float AngleInc { get; }

        public override MessageId MessageId => MessageId.ENEMYSHOOT;

        public EnemyShoot(int bulletId, int objectId, byte bulletType, int objectType, ref Position startingPosition, float angle, int damage, byte numShots, float angleInc)
        {
            BulletId = bulletId;
            ObjectId = objectId;
            BulletType = bulletType;
            ObjectType = objectType;
            StartingPos = startingPosition;
            Angle = angle;
            Damage = damage;
            NumShots = numShots;
            AngleInc = angleInc;
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write(BulletId);
            wtr.Write(ObjectId);
            wtr.Write(BulletType);
            StartingPos.Write(wtr);
            wtr.Write(Angle);
            wtr.Write(Damage);
            wtr.Write(NumShots);
            wtr.Write(AngleInc);
        }
    }
}
