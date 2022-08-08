using common;

namespace wServer.networking.packets.outgoing
{
    public class GlobalNotification : OutgoingMessage
    {
        public int Type { get; set; }
        public string Text { get; set; }

        public override PacketId MessageId => PacketId.GLOBAL_NOTIFICATION;

        protected override void Write(NWriter wtr)
        {
            wtr.Write(Type);
            wtr.WriteUTF(Text);
        }
    }
}
