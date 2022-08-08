using common;

namespace wServer.networking.packets.outgoing
{
    public class QuestObjId : OutgoingMessage
    {
        public int ObjectId { get; set; }

        public override PacketId MessageId => PacketId.QUESTOBJID;


        protected override void Write(NWriter wtr)
        {
            wtr.Write(ObjectId);
        }
    }
}
