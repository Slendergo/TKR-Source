using TKR.Shared;

namespace TKR.WorldServer.core.net.datas
{
    public struct FuelEngine
    {
        public bool Included;
        public ushort ObjectType;
        public int slotID;
        public int ItemData;

        public static FuelEngine Read(NetworkReader rdr) => new FuelEngine
        {
            ObjectType = rdr.ReadUInt16(),
            slotID = rdr.ReadInt32(),
            Included = rdr.ReadBoolean(),
            ItemData = rdr.ReadInt32()
        };

        public void Write(NetworkWriter wtr)
        {
            wtr.Write(ObjectType);
            wtr.Write(slotID);
            wtr.Write(Included);
            wtr.Write(ItemData);
        }
    }
}
