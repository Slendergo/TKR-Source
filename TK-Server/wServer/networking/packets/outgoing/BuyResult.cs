using common;

namespace wServer.networking.packets.outgoing
{
    public enum BuyResultType
    {
        Success = 0,
        NotEnoughGold = 1,
        NotEnoughFame = 2
    }

    public class BuyResult : OutgoingMessage
    {
        public const int Success = 0;
        public const int Dialog = 1;

        public int Result { get; set; }
        public string ResultString { get; set; }

        public override PacketId ID => PacketId.BUYRESULT;

        public override Packet CreateInstance()
        {
            return new BuyResult();
        }

        protected override void Read(NReader rdr)
        {
            Result = rdr.ReadInt32();
            ResultString = rdr.ReadUTF();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write(Result);
            wtr.WriteUTF(ResultString);
        }
    }
}
