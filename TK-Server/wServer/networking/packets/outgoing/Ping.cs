using common;

namespace wServer.networking.packets.outgoing
{
    public class Ping : OutgoingMessage
    {
        public int Serial { get; set; }
        public int RTT { get; set; }

        public override MessageId MessageId => MessageId.PING;

        protected override void Write(NWriter wtr)
        {
            wtr.Write(Serial);
            wtr.Write(RTT);
        }
    }
}
