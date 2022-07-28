using common;

namespace wServer.networking.packets.incoming.market
{
    public class MarketRemove : IncomingMessage
    {
        public override Packet CreateInstance() => new MarketRemove();

        public override PacketId ID => PacketId.MARKET_REMOVE;

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
