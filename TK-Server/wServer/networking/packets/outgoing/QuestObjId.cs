using common;

namespace wServer.networking.packets.outgoing
{
    public class QuestObjId : OutgoingMessage
    {
        public int ObjectId { get; set; }

        public override PacketId MessageId => PacketId.QUESTOBJID;

        public override Packet CreateInstance()
        {
            return new QuestObjId();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write(ObjectId);
        }
    }
}
