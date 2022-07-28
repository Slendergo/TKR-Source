using common;

namespace wServer.networking.packets.incoming
{
    internal class SmallSkillTree : IncomingMessage
    {
        public int skillNumber { get; set; }
        public bool removePoint { get; set; }

        public override PacketId ID => PacketId.SMALLSKILLTREE;

        public override Packet CreateInstance()
        {
            return new SmallSkillTree();
        }

        protected override void Read(NReader rdr)
        {
            skillNumber = rdr.ReadInt32();
            removePoint = rdr.ReadBoolean();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write(skillNumber);
            wtr.Write(removePoint);
        }
    }
}
