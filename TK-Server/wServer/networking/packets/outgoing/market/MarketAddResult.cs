using common;

namespace wServer.networking.packets.outgoing.market
{
    public class MarketAddResult : OutgoingMessage
    {
        public override Packet CreateInstance() => new MarketAddResult();

        public override PacketId ID => PacketId.MARKET_ADD_RESULT;

        public const int INVALID_UPTIME = 0;
        public const int SLOT_IS_NULL = 1;
        public const int ITEM_IS_SOULBOUND = 2;
        public const int INVALID_PRICE = 3;
        public const int INVALID_CURRENCY = 4;

        public int Code;
        public string Description;

        protected override void Read(NReader rdr)
        {
            Code = rdr.ReadInt32();
            Description = rdr.ReadUTF();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write(Code);
            wtr.WriteUTF(Description);
        }
    }
}
