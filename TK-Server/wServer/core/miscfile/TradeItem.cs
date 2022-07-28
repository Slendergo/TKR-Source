using common;

namespace wServer
{
    public struct TradeItem
    {
        public bool Included;
        public int Item;
        public int SlotType;
        public bool Tradeable;
        public string ItemData;

        public static TradeItem Read(NReader rdr) => new TradeItem
        {
            Item = rdr.ReadInt32(),
            SlotType = rdr.ReadInt32(),
            Tradeable = rdr.ReadBoolean(),
            Included = rdr.ReadBoolean(),
            ItemData = rdr.ReadUTF()
        };

        public void Write(NWriter wtr)
        {
            wtr.Write(Item);
            wtr.Write(SlotType);
            wtr.Write(Tradeable);
            wtr.Write(Included);
            wtr.WriteUTF(ItemData);
        }
    }
}
