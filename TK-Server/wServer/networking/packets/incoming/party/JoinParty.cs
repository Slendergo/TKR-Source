using common;

namespace wServer.networking.packets.incoming
{
    internal class JoinParty : IncomingMessage
    {
        public string PartyLeader { get; set; }
        public int PartyId { get; set; }

        public override PacketId ID => PacketId.JOIN_PARTY;

        public override Packet CreateInstance()
        {
            return new JoinParty();
        }

        protected override void Read(NReader rdr)
        {
            PartyLeader = rdr.ReadUTF();
            PartyId = rdr.ReadInt32();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.WriteUTF(PartyLeader);
            wtr.Write(PartyId);
        }
    }
}
