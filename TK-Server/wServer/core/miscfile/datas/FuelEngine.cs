using common;

namespace wServer
{
    public struct FuelEngine
    {
        public bool Included;
        public ushort ObjectType;
        public int slotID;
        public int ItemData;

        public static FuelEngine Read(NReader rdr) => new FuelEngine
        {
            ObjectType = rdr.ReadUInt16(),
            slotID = rdr.ReadInt32(),
            Included = rdr.ReadBoolean(),
            ItemData = rdr.ReadInt32()
        };

        public void Write(NWriter wtr)
        {
            wtr.Write(ObjectType);
            wtr.Write(slotID);
            wtr.Write(Included);
            wtr.Write(ItemData);
        }
    }
}
