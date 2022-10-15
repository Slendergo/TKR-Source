using TKR.Shared;

namespace TKR.WorldServer.networking.packets.outgoing
{
    public class ClientStat : OutgoingMessage
    {
        public string Name { get; set; }
        public int Value { get; set; }

        public override MessageId MessageId => MessageId.CLIENTSTAT;

        public override void Write(NetworkWriter wtr)
        {
            wtr.WriteUTF16(Name);
            wtr.Write(Value);
        }
    }
}
