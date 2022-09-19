using TKR.Shared;

namespace TKR.WorldServer.networking.packets.outgoing
{
    public class FileMessage : OutgoingMessage
    {
        public string Name { get; set; }
        public byte[] Bytes { get; set; }

        public override MessageId MessageId => MessageId.FILE;

        protected override void Write(NWriter wtr)
        {
            wtr.WriteUTF(Name);
            wtr.Write(Bytes.Length);
            wtr.Write(Bytes);
        }
    }
}
