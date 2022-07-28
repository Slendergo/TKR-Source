using common;

namespace wServer.networking.packets.incoming
{
    internal class UpgradeBaseStat : IncomingMessage
    {
        public int Num { get; set; }

        public override PacketId ID => PacketId.UPGRADESTAT;

        public override Packet CreateInstance()
        {
            return new UpgradeBaseStat();
        }

        protected override void Read(NReader rdr)
        {
        }

        protected override void Write(NWriter wtr)
        {
        }
    }
}
