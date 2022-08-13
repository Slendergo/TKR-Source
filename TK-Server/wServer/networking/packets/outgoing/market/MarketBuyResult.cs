using common;

namespace wServer.networking.packets.outgoing.market
{
    public class MarketBuyResult : OutgoingMessage
    {
        public override MessageId MessageId => MessageId.MARKET_BUY_RESULT;

        public const int BOUGHT = -1;
        public const int ERROR = 1;

        public int Code;
        public string Description;
        public int OfferId;

        protected override void Write(NWriter wtr)
        {
            wtr.Write(Code);
            wtr.WriteUTF(Description);
            wtr.Write(OfferId);
        }
    }
}
