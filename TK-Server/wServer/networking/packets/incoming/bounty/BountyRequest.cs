using common;

namespace wServer.networking.packets.incoming
{
    public class BountyRequest : IncomingMessage
    {
        public int BountyId { get; set; }
        public int[] PlayersAllowed { get; set; }

        public override PacketId ID => PacketId.BOUNTYREQUEST;

        public override Packet CreateInstance()
        {
            return new BountyRequest();
        }

        protected override void Read(NReader rdr)
        {
            BountyId = rdr.ReadInt32();
            PlayersAllowed = new int[rdr.ReadInt32()];
            for (var i = 0; i < PlayersAllowed.Length; i++)
            {
                PlayersAllowed[i] = rdr.ReadInt32();
            }
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write(BountyId);
            wtr.Write(PlayersAllowed.Length);
            foreach (var i in PlayersAllowed)
            {
                wtr.Write(i);
            }
        }
    }
}
