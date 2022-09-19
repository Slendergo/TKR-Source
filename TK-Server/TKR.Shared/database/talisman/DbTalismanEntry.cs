using Newtonsoft.Json;

namespace TKR.Shared.database.talisman
{
    public struct DbTalismanEntry
    {
        public byte Type;
        public byte Level;
        public int Exp;
        public int Goal;
        public byte Tier;
        [JsonIgnore] public bool IsNull;
    }
}
