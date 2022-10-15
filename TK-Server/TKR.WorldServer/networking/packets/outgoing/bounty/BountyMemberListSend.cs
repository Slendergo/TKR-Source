using TKR.Shared;
using TKR.WorldServer.networking.packets.outgoing;

namespace TKR.WorldServer.networking.packets.outgoing.bounty
{
    public class BountyMemberListSend : OutgoingMessage
    {
        public int[] AccountIds { get; set; }

        public override MessageId MessageId => MessageId.BOUNTYMEMBERLISTSEND;

        public override void Write(NetworkWriter wtr)
        {
            wtr.Write(AccountIds.Length);
            foreach (var i in AccountIds)
                wtr.Write(i);
        }
    }
}
