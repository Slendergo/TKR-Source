using TKR.Shared;
using TKR.WorldServer.networking.packets.outgoing;

namespace TKR.WorldServer.networking.packets.outgoing.party
{
    internal class InvitedToParty : OutgoingMessage
    {
        public string Name { get; set; }

        public int PartyId { get; set; }

        public override MessageId MessageId => MessageId.INVITED_TO_PARTY;

        public override void Write(NWriter wtr)
        {
            wtr.WriteUTF(Name);
            wtr.Write(PartyId);
        }
    }
}
