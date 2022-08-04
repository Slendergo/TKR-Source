using common;

namespace wServer.networking.packets.outgoing
{
    public class TradeRequested : OutgoingMessage
    {
        public string Name { get; set; }

        public override PacketId MessageId => PacketId.TRADEREQUESTED;

        public override Packet CreateInstance()
        {
            return new TradeRequested();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.WriteUTF(Name);
        }
    }
}
