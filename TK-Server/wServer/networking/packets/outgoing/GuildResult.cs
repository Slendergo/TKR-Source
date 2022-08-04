using common;

namespace wServer.networking.packets.outgoing
{
    internal class GuildResult : OutgoingMessage
    {
        public bool Success { get; set; }
        public string ErrorText { get; set; }

        public override PacketId MessageId => PacketId.GUILDRESULT;

        public override Packet CreateInstance()
        {
            return new GuildResult();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write(Success);
            wtr.WriteUTF(ErrorText);
        }
    }
}
