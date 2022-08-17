using common;

namespace wServer.networking.packets.outgoing
{
    public class TradeChanged : OutgoingMessage
    {
        public bool[] Offer { get; set; }

        public override MessageId MessageId => MessageId.TRADECHANGED;

        protected override void Write(NWriter wtr)
        {
            wtr.Write((short)Offer.Length);
            foreach (var i in Offer)
                wtr.Write(i);
        }
    }
}
