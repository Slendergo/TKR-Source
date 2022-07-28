using common;

namespace wServer.networking.packets.incoming
{
    public class BountyMemberListRequest : IncomingMessage
    {
        public override PacketId ID => PacketId.BOUNTYMEMBERLISTREQUEST;

        public override Packet CreateInstance()
        {
            return new BountyMemberListRequest();
        }

        protected override void Read(NReader rdr)
        {
        }

        protected override void Write(NWriter wtr)
        {
        }
    }
}
