using Newtonsoft.Json;

namespace common.database.info
{
    public struct DbTalismanEntry
    {
        public byte Type;
        public byte Level;
        public int Exp;
        public int Goal;
        public byte Tier;
        public bool Active;
        [JsonIgnore] public bool IsNull;
    }
}
