using common;

namespace wServer.networking.packets.outgoing
{
    public class ServerPlayerShoot : OutgoingMessage
    {
        public byte BulletId { get; set; }
        public int ObjectId { get; set; }
        public int ObjectType { get; set; }
        public Position StartingPos { get; set; }
        public float Angle { get; set; }
        public int Damage { get; set; }

        public override MessageId MessageId => MessageId.SERVERPLAYERSHOOT;

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
