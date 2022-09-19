using TKR.Shared;
using TKR.WorldServer.core.miscfile.datas;

namespace TKR.WorldServer.networking.packets.outgoing
{
    public class Notification : OutgoingMessage
    {
        public int ObjectId { get; set; }
        public string Message { get; set; }
        public ARGB Color { get; set; }
        public int PlayerId { get; set; }

        public override MessageId MessageId => MessageId.NOTIFICATION;

        protected override void Write(NWriter wtr)
        {
            wtr.Write(ObjectId);
            wtr.WriteUTF(Message);
            Color.Write(wtr);
            wtr.Write(PlayerId);
        }
    }
}
