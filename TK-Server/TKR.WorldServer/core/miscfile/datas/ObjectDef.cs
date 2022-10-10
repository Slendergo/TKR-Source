using TKR.Shared;

namespace TKR.WorldServer.core.miscfile.datas
{
    public struct ObjectDef
    {
        public int ObjectType;
        public ObjectStats Stats;

        public void Write(NWriter wtr)
        {
            wtr.Write(ObjectType);
            Stats.Write(wtr);
        }
    }
}
