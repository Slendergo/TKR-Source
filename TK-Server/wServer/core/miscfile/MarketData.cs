using common;

namespace wServer
{
    public struct MarketData
    {
        public int Currency;
        public int Id;
        public ushort ItemType;
        public int Price;
        public int SellerId;
        public string SellerName;
        public int StartTime;
        public int TimeLeft;
        public string ItemData;

        public static MarketData Read(NReader rdr) => new MarketData
        {
            Id = rdr.ReadInt32(),
            ItemType = rdr.ReadUInt16(),
            SellerName = rdr.ReadUTF(),
            SellerId = rdr.ReadInt32(),
            Currency = rdr.ReadInt32(),
            Price = rdr.ReadInt32(),
            StartTime = rdr.ReadInt32(),
            TimeLeft = rdr.ReadInt32(),
            ItemData = rdr.ReadUTF()
        };

        public void Write(NWriter wtr)
        {
            wtr.Write(Id);
            wtr.Write(ItemType);
            wtr.WriteUTF(SellerName);
            wtr.Write(SellerId);
            wtr.Write(Currency);
            wtr.Write(Price);
            wtr.Write(StartTime);
            wtr.Write(TimeLeft);
            wtr.WriteUTF(ItemData);
        }
    }
}
