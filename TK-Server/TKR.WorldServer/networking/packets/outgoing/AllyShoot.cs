using TKR.Shared;

namespace TKR.WorldServer.networking.packets.outgoing
{
    public class AllyShoot : OutgoingMessage
    {
        public int BulletId { get; set; }
        public int OwnerId { get; set; }
        public int ContainerType { get; set; }
        public float Angle { get; set; }

        public override MessageId MessageId => MessageId.ALLYSHOOT;

        public AllyShoot(int bulletId, int ownerId, int containerType, float angle)
        {
            BulletId = bulletId;
            OwnerId = ownerId;
            ContainerType = containerType;
            Angle = angle;
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write(BulletId);
            wtr.Write(OwnerId);
            wtr.Write(ContainerType);
            wtr.Write(Angle);
        }
    }
}
