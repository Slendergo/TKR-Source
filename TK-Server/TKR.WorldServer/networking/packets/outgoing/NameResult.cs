using TKR.Shared;

namespace TKR.WorldServer.networking.packets.outgoing
{
    public class NameResult : OutgoingMessage
    {
        public bool Success { get; set; }
        public string ErrorText { get; set; }

        public override MessageId MessageId => MessageId.NAMERESULT;

        public override void Write(NWriter wtr)
        {
            wtr.Write(Success);
            wtr.WriteUTF(ErrorText);
        }
    }
}
