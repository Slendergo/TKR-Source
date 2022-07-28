using common;

namespace wServer.networking.packets.outgoing.market
{
    public class MarketRemoveResult : OutgoingMessage
    {
        public override Packet CreateInstance() => new MarketRemoveResult();

        public override PacketId ID => PacketId.MARKET_REMOVE_RESULT;

        public const int NOT_YOUR_ITEM = 0;
        public const int ITEM_DOESNT_EXIST = 1;

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
