using common;

namespace wServer.networking.packets.incoming
{
    public class UpdateAck : IncomingMessage
    {
        public override PacketId MessageID => PacketId.UPDATEACK;

        public override Packet CreateInstance()
        {
            return new UpdateAck();
        }

        protected override void Read(NReader rdr)
        {
        }

        protected override void Write(NWriter wtr)
        {
        }
    }
}
