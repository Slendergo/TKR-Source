using common;

namespace wServer.networking.packets.outgoing
{
    public class TradeDone : OutgoingMessage
    {
        public int Code { get; set; }
        public string Description { get; set; }

        public override MessageId MessageId => MessageId.TRADEDONE;

        protected override void Write(NWriter wtr)
        {
            wtr.Write(Code);
            wtr.WriteUTF(Description);
        }
    }
}
