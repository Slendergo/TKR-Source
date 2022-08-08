using common;

namespace wServer.networking.packets.outgoing
{
    public class File : OutgoingMessage
    {
        public string Name { get; set; }
        public byte[] Bytes { get; set; }

        public override PacketId MessageId => PacketId.FILE;

        protected override void Write(NWriter wtr)
        {
            wtr.WriteUTF(Name);
            wtr.Write(Bytes.Length);
            wtr.Write(Bytes);
        }
    }
}
