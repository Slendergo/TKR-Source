using TKR.Shared;

namespace TKR.WorldServer.networking.packets.outgoing
{
    public class AccountList : OutgoingMessage
    {
        public int AccountListId { get; set; }
        public string[] AccountIds { get; set; }

        public override MessageId MessageId => MessageId.ACCOUNTLIST;

        public override void Write(NWriter wtr)
        {
            wtr.Write(AccountListId);
            wtr.Write((short)AccountIds.Length);
            foreach (var i in AccountIds)
                wtr.WriteUTF(i);
        }
    }
}
