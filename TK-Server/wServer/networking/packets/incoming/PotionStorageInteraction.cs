using common;

namespace wServer.networking.packets.incoming
{
    internal class PotionStorageInteraction : IncomingMessage
    {
        public byte Type { get; private set; }
        public byte Action { get; private set; }

        public override PacketId ID => PacketId.POTION_STORAGE_INTERACTION;

        public override Packet CreateInstance() => new PotionStorageInteraction();

        protected override void Read(NReader rdr)
        {
            Type = rdr.ReadByte();
            Action = rdr.ReadByte();
        }

        protected override void Write(NWriter wtr) { }
    }
}
