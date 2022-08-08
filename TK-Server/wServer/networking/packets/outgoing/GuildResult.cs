using common;

namespace wServer.networking.packets.outgoing
{
    internal class GuildResult : OutgoingMessage
    {
        public bool Success { get; set; }
        public string ErrorText { get; set; }

        public override PacketId MessageId => PacketId.GUILDRESULT;

        protected override void Write(NWriter wtr)
        {
            wtr.Write(Success);
            wtr.WriteUTF(ErrorText);
        }
    }
}
