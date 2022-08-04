using common;

namespace wServer.networking.packets.outgoing
{
    public class BountyMemberListSend : OutgoingMessage
    {
        public int[] AccountIds { get; set; }

        public override PacketId MessageId => PacketId.BOUNTYMEMBERLISTSEND;

        public override Packet CreateInstance()
        {
            return new BountyMemberListSend();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write(AccountIds.Length);
            foreach (var i in AccountIds)
                wtr.Write(i);
        }
    }
}
