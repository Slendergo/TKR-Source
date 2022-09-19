using TKR.Shared;

namespace TKR.WorldServer.networking.packets.outgoing
{
    public class TradeRequested : OutgoingMessage
    {
        public string Name { get; set; }

        public override MessageId MessageId => MessageId.TRADEREQUESTED;

        protected override void Write(NWriter wtr)
        {
            wtr.WriteUTF(Name);
        }
    }
}
