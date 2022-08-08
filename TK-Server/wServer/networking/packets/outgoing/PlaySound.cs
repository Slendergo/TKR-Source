using common;

namespace wServer.networking.packets.outgoing
{
    public class PlaySound : OutgoingMessage
    {
        public int OwnerId { get; set; }
        public int SoundId { get; set; }

        public override PacketId MessageId => PacketId.PLAYSOUND;

        protected override void Write(NWriter wtr)
        {
            wtr.Write(OwnerId);
            wtr.Write((byte)SoundId);
        }
    }
}
