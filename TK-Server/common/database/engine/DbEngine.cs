using Newtonsoft.Json;
using StackExchange.Redis;
using System.Collections.Generic;

namespace common.database
{
    public class DbEngine//DbIpInfo
    {
        public int engineFuel { get; set; }
        public int engineStage { get; set; }
        public int engineStageTime { get; set; }
        public List<int> Contributors { get; set; }

        [JsonIgnore] public string STATE { get; private set; }
        [JsonIgnore] public bool IsNull { get; private set; }

        private readonly IDatabase _db;

        public DbEngine(IDatabase db, string state)
        {
            _db = db;
            STATE = state;

            var json = (string)db.HashGet("engine", "STATE");
            if (json == null)
                IsNull = true;
            else
                JsonConvert.PopulateObject(json, this);
        }

        public void Flush() => _db.HashSetAsync("engine", STATE, JsonConvert.SerializeObject(this));
    }
}
