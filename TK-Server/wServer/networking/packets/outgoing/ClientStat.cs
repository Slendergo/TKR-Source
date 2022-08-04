using common;

namespace wServer.networking.packets.outgoing
{
    public class ClientStat : OutgoingMessage
    {
        public string Name { get; set; }
        public int Value { get; set; }

        public override PacketId MessageId => PacketId.CLIENTSTAT;

        public override Packet CreateInstance()
        {
            return new ClientStat();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.WriteUTF(Name);
            wtr.Write(Value);
        }
    }
}
