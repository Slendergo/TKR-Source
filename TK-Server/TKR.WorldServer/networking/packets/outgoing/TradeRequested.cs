using TKR.Shared;

namespace TKR.WorldServer.networking.packets.outgoing
{
    public class TradeRequested : OutgoingMessage
    {
        public string Name { get; set; }

        public override MessageId MessageId => MessageId.TRADEREQUESTED;

        public override void Write(NWriter wtr)
        {
            wtr.WriteUTF(Name);
        }
    }
}
