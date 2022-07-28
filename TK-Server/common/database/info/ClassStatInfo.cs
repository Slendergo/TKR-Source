using Newtonsoft.Json;

namespace common.database.info
{
    public struct ClassStatInfo
    {
        public int BestFame;
        public int BestLevel;

        [JsonIgnore] public bool IsNull;
    }
}
