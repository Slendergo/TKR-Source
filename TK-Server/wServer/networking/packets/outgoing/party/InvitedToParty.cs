using common;

namespace wServer.networking.packets.outgoing
{
    internal class InvitedToParty : OutgoingMessage
    {
        public string Name { get; set; }

        public int PartyId { get; set; }

        public override PacketId ID => PacketId.INVITED_TO_PARTY;

        public override Packet CreateInstance()
        {
            return new InvitedToParty();
        }

        protected override void Read(NReader rdr)
        {
            Name = rdr.ReadUTF();
            PartyId = rdr.ReadInt32();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.WriteUTF(Name);
            wtr.Write(PartyId);
        }
    }
}
