using TKR.Shared;

namespace TKR.WorldServer.networking.packets.outgoing
{
    public class ClientStat : OutgoingMessage
    {
        public string Name { get; set; }
        public int Value { get; set; }

        public override MessageId MessageId => MessageId.CLIENTSTAT;

        public override void Write(NWriter wtr)
        {
            wtr.WriteUTF(Name);
            wtr.Write(Value);
        }
    }
}
