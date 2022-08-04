using common;

namespace wServer.networking.packets.outgoing
{
    public class TradeChanged : OutgoingMessage
    {
        public bool[] Offer { get; set; }

        public override PacketId MessageId => PacketId.TRADECHANGED;

        public override Packet CreateInstance()
        {
            return new TradeChanged();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write((short)Offer.Length);
            foreach (var i in Offer)
                wtr.Write(i);
        }
    }
}
