using common;

namespace wServer.networking.packets.incoming.market
{
    public class MarketMyOffers : IncomingMessage
    {
        public override Packet CreateInstance() => new MarketMyOffers();

        public override PacketId ID => PacketId.MARKET_MY_OFFERS;

        protected override void Read(NReader rdr)
        {
        }

        protected override void Write(NWriter wtr)
        {
        }
    }
}
