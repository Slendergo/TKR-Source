using TKR.Shared;

namespace TKR.WorldServer.networking.packets.outgoing
{
    public class GlobalNotification : OutgoingMessage
    {
        public int Type { get; set; }
        public string Text { get; set; }

        public override MessageId MessageId => MessageId.GLOBAL_NOTIFICATION;

        public override void Write(NetworkWriter wtr)
        {
            wtr.Write(Type);
            wtr.WriteUTF16(Text);
        }
    }
}
