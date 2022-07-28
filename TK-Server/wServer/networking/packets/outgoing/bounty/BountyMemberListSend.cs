using common;

namespace wServer.networking.packets.outgoing
{
    public class BountyMemberListSend : OutgoingMessage
    {
        public int[] AccountIds { get; set; }

        public override PacketId ID => PacketId.BOUNTYMEMBERLISTSEND;

        public override Packet CreateInstance()
        {
            return new BountyMemberListSend();
        }

        protected override void Read(NReader rdr)
        {
            AccountIds = new int[rdr.ReadInt32()];
            for (int i = 0; i < AccountIds.Length; i++)
                AccountIds[i] = rdr.ReadInt32();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write(AccountIds.Length);
            foreach (var i in AccountIds)
                wtr.Write(i);
        }
    }
}
