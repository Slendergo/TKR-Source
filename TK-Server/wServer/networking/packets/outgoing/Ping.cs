using common;

namespace wServer.networking.packets.outgoing
{
    public class Ping : OutgoingMessage
    {
        public int Serial { get; set; }

        public override PacketId MessageId => PacketId.PING;

        public override Packet CreateInstance()
        {
            return new Ping();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write(Serial);
        }
    }
}
