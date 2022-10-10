using TKR.Shared;

namespace TKR.WorldServer.networking.packets.outgoing
{
    public class TradeChanged : OutgoingMessage
    {
        public bool[] Offer { get; set; }

        public override MessageId MessageId => MessageId.TRADECHANGED;

        public override void Write(NWriter wtr)
        {
            wtr.Write((short)Offer.Length);
            foreach (var i in Offer)
                wtr.Write(i);
        }
    }
}
