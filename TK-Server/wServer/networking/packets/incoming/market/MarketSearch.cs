using common;

namespace wServer.networking.packets.incoming.market
{
    public class MarketSearch : IncomingMessage
    {
        public override Packet CreateInstance() => new MarketSearch();

        public override PacketId ID => PacketId.MARKET_SEARCH;

        public int ItemType;

        protected override void Read(NReader rdr)
        {
            ItemType = rdr.ReadInt32();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write(ItemType);
        }
    }
}
