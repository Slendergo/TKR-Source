using common;

namespace wServer.networking.packets.outgoing
{
    public class AccountList : OutgoingMessage
    {
        public int AccountListId { get; set; }
        public string[] AccountIds { get; set; }

        public override PacketId MessageId => PacketId.ACCOUNTLIST;

        protected override void Write(NWriter wtr)
        {
            wtr.Write(AccountListId);
            wtr.Write((short)AccountIds.Length);
            foreach (var i in AccountIds)
                wtr.WriteUTF(i);
        }
    }
}
