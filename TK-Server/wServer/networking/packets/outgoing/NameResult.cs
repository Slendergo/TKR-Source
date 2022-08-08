using common;

namespace wServer.networking.packets.outgoing
{
    public class NameResult : OutgoingMessage
    {
        public bool Success { get; set; }
        public string ErrorText { get; set; }

        public override PacketId MessageId => PacketId.NAMERESULT;

        protected override void Write(NWriter wtr)
        {
            wtr.Write(Success);
            wtr.WriteUTF(ErrorText);
        }
    }
}
