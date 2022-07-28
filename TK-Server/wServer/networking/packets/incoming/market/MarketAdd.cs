using common;

namespace wServer.networking.packets.incoming.market
{
    public class MarketAdd : IncomingMessage
    {
        public override Packet CreateInstance() => new MarketAdd();

        public override PacketId ID => PacketId.MARKET_ADD;

        public byte[] Slots;
        public int Price;
        public int Currency;
        public int Hours;

        protected override void Read(NReader rdr)
        {
            Slots = new byte[rdr.ReadByte()];
            for (int i = 0; i < Slots.Length; i++)
            {
                Slots[i] = rdr.ReadByte();
            }
            Price = rdr.ReadInt32();
            Currency = rdr.ReadInt32();
            Hours = rdr.ReadInt32();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write((byte)Slots.Length);
            for (int i = 0; i < Slots.Length; i++)
            {
                wtr.Write(Slots[i]);
            }
            wtr.Write(Price);
            wtr.Write(Currency);
            wtr.Write(Hours);
        }
    }
}
