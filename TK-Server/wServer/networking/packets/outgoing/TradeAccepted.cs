using common;

namespace wServer.networking.packets.outgoing
{
    public class TradeAccepted : OutgoingMessage
    {
        public bool[] MyOffer { get; set; }
        public bool[] YourOffer { get; set; }

        public override PacketId MessageId => PacketId.TRADEACCEPTED;

        public override Packet CreateInstance()
        {
            return new TradeAccepted();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write((short)MyOffer.Length);
            foreach (var i in MyOffer)
                wtr.Write(i);
            wtr.Write((short)YourOffer.Length);
            foreach (var i in YourOffer)
                wtr.Write(i);
        }
    }
}
