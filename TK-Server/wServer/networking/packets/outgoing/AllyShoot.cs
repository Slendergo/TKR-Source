using common;

namespace wServer.networking.packets.outgoing
{
    public class AllyShoot : OutgoingMessage
    {
        public byte BulletId { get; set; }
        public int OwnerId { get; set; }
        public ushort ContainerType { get; set; }
        public float Angle { get; set; }

        public override PacketId MessageId => PacketId.ALLYSHOOT;

        protected override void Write(NWriter wtr)
        {
            wtr.Write(BulletId);
            wtr.Write(OwnerId);
            wtr.Write((short)ContainerType);
            wtr.Write(Angle);
        }
    }
}
