using TKR.Shared;

namespace TKR.WorldServer.networking.packets.outgoing
{
    public class CreateSuccess : OutgoingMessage
    {
        public int ObjectId { get; set; }
        public int CharId { get; set; }

        public override MessageId MessageId => MessageId.CREATE_SUCCESS;

        public override void Write(NWriter wtr)
        {
            wtr.Write(ObjectId);
            wtr.Write(CharId);
        }
    }
}
