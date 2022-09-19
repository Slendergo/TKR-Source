using TKR.Shared;

namespace TKR.WorldServer.networking.packets.outgoing
{
    public class QuestObjId : OutgoingMessage
    {
        public int ObjectId { get; set; }

        public override MessageId MessageId => MessageId.QUESTOBJID;

        protected override void Write(NWriter wtr)
        {
            wtr.Write(ObjectId);
        }
    }
}
