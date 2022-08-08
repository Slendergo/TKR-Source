using common;

namespace wServer.networking.packets.outgoing.market
{
    public class MarketSearchResult : OutgoingMessage
    {
        public override PacketId MessageId => PacketId.MARKET_SEARCH_RESULT;

        public MarketData[] Results;
        public string Description;

        protected override void Write(NWriter wtr)
        {
            wtr.Write((short)Results.Length);
            for (int i = 0; i < Results.Length; i++)
            {
                Results[i].Write(wtr);
            }
            wtr.WriteUTF(Description);
        }
    }
}
