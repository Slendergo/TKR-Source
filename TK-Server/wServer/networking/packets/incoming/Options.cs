using common;

namespace wServer.networking.packets.incoming
{
    public class Options : IncomingMessage
    {
        public bool AllyShots { get; set; }

        public override PacketId ID => PacketId.OPTIONS;

        public override Packet CreateInstance()
        {
            return new Options();
        }

        protected override void Read(NReader rdr)
        {
            AllyShots = rdr.ReadBoolean();
        }

        protected override void Write(NWriter wtr)
        {
        }
    }
}
