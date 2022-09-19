using TKR.Shared;

namespace TKR.WorldServer.core.miscfile.datas
{
    public struct ObjectDef
    {
        public int ObjectType;
        public ObjectStats Stats;

        public static ObjectDef Read(NReader rdr) => new ObjectDef
        {
            ObjectType = rdr.ReadInt32(),
            Stats = ObjectStats.Read(rdr)
        };

        public void Write(NWriter wtr)
        {
            wtr.Write(ObjectType);
            Stats.Write(wtr);
        }
    }
}
