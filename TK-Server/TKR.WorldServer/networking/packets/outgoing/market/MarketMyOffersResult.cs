using TKR.Shared;
using TKR.WorldServer.core.miscfile.datas;

namespace TKR.WorldServer.networking.packets.outgoing.market
{
    public class MarketMyOffersResult : OutgoingMessage
    {
        public override MessageId MessageId => MessageId.MARKET_MY_OFFERS_RESULT;

        public MarketData[] Results;

        public override void Write(NWriter wtr)
        {
            wtr.Write((short)Results.Length);
            for (int i = 0; i < Results.Length; i++)
            {
                Results[i].Write(wtr);
            }
        }
    }
}
