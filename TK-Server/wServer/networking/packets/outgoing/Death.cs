using common;

namespace wServer.networking.packets.outgoing
{
    public class Death : OutgoingMessage
    {
        public int AccountId { get; set; }
        public int CharId { get; set; }
        public string KilledBy { get; set; }

        public override PacketId ID => PacketId.DEATH;

        public override Packet CreateInstance()
        {
            return new Death();
        }

        protected override void Read(NReader rdr)
        {
            AccountId = rdr.ReadInt32();
            CharId = rdr.ReadInt32();
            KilledBy = rdr.ReadUTF();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write(AccountId);
            wtr.Write(CharId);
            wtr.WriteUTF(KilledBy);
        }
    }
}
