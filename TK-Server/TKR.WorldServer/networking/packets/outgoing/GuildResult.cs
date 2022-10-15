using TKR.Shared;

namespace TKR.WorldServer.networking.packets.outgoing
{
    internal class GuildResult : OutgoingMessage
    {
        public bool Success { get; set; }
        public string ErrorText { get; set; }

        public override MessageId MessageId => MessageId.GUILDRESULT;

        public override void Write(NetworkWriter wtr)
        {
            wtr.Write(Success);
            wtr.WriteUTF16(ErrorText);
        }
    }
}
