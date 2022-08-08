using common;

namespace wServer.networking.packets.outgoing
{
    public class InvResult : OutgoingMessage
    {
        public int Result { get; set; }

        public override PacketId MessageId => PacketId.INVRESULT;

        protected override void Write(NWriter wtr)
        {
            wtr.Write(Result);
        }
    }
}
