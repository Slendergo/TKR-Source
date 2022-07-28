using common;

namespace wServer.networking.packets.incoming.market
{
    public class MarketBuy : IncomingMessage
    {
        public override Packet CreateInstance() => new MarketBuy();

        public override PacketId ID => PacketId.MARKET_BUY;

        public int Id;

        protected override void Read(NReader rdr)
        {
            Id = rdr.ReadInt32();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write(Id);
        }
    }
}
