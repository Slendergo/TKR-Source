using common;

namespace wServer.networking.packets.outgoing.market
{
    public class MarketBuyResult : OutgoingMessage
    {
        public override Packet CreateInstance() => new MarketBuyResult();

        public override PacketId ID => PacketId.MARKET_BUY_RESULT;

        public const int BOUGHT = -1;
        public const int ERROR = 1;

        public int Code;
        public string Description;
        public int OfferId;

        protected override void Read(NReader rdr)
        {
            Code = rdr.ReadInt32();
            Description = rdr.ReadUTF();
            OfferId = rdr.ReadInt32();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write(Code);
            wtr.WriteUTF(Description);
            wtr.Write(OfferId);
        }
    }
}
