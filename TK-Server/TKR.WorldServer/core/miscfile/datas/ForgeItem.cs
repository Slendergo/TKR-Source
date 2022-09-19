using TKR.Shared;

namespace TKR.WorldServer.core.miscfile.datas
{
    public struct ForgeItem
    {
        public bool Included;
        public ushort ObjectType;
        public int slotID;

        public static ForgeItem Read(NReader rdr) => new ForgeItem
        {
            ObjectType = rdr.ReadUInt16(),
            slotID = rdr.ReadInt32(),
            Included = rdr.ReadBoolean()
        };

        public void Write(NWriter wtr)
        {
            wtr.Write(ObjectType);
            wtr.Write(slotID);
            wtr.Write(Included);
        }
    }
}
