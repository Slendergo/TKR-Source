using TKR.Shared;

namespace TKR.WorldServer.networking.packets.outgoing
{
    public class FileMessage : OutgoingMessage
    {
        public string Name { get; set; }
        public byte[] Bytes { get; set; }

        public override MessageId MessageId => MessageId.FILE;

        public override void Write(NetworkWriter wtr)
        {
            wtr.WriteUTF16(Name);
            wtr.Write(Bytes.Length);
            wtr.Write(Bytes);
        }
    }
}
