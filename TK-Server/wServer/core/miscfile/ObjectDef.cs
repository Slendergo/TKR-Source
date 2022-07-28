using common;

namespace wServer
{
    public struct ObjectDef
    {
        public ushort ObjectType;
        public ObjectStats Stats;

        public static ObjectDef Read(NReader rdr) => new ObjectDef
        {
            ObjectType = rdr.ReadUInt16(),
            Stats = ObjectStats.Read(rdr)
        };

        public void Write(NWriter wtr)
        {
            wtr.Write(ObjectType);
            Stats.Write(wtr);
        }
    }
}
