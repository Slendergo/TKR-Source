using common;

namespace wServer.networking.packets.incoming
{
    internal class PartyInvite : IncomingMessage
    {
        public string Name { get; set; }

        public override PacketId ID => PacketId.PARTY_INVITE;

        public override Packet CreateInstance()
        {
            return new PartyInvite();
        }

        protected override void Read(NReader rdr)
        {
            Name = rdr.ReadUTF();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.WriteUTF(Name);
        }
    }
}
