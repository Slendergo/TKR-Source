using Newtonsoft.Json;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;

namespace common.database
{
    public class EngineFuelContributer
    {
        public string Name { get; set; }
        public int Amount { get; set; }

        public EngineFuelContributer(string name, int amount)
        {
            Name = name;
            Amount = amount;
        }
    }

    public sealed class DbEngine
    {
        public int EngineFuel { get; set; }
        public int EngineStage { get; set; }
        public int EngineStageTime { get; set; }
        public List<EngineFuelContributer> Contributors { get; set; }

        [JsonIgnore] public bool IsNull { get; private set; }

        [JsonIgnore] public string ServerName { get; private set; }

        private readonly IDatabase _db;

        public DbEngine(IDatabase db, string serverName)
        {
            _db = db;
            ServerName = serverName;

            var json = (string)db.HashGet("engine", ServerName);
            if (json == null)
                IsNull = true;
            else
                JsonConvert.PopulateObject(json, this);
        }

        public void AddFuel(string contributer, int amount)
        {
            if (Contributors == null)
                Contributors = new List<EngineFuelContributer>();
            var engineFuelContributer = Contributors.FirstOrDefault(_ => _.Name == contributer);
            if (engineFuelContributer == null)
                Contributors.Add(new EngineFuelContributer(contributer, amount));
            else
                engineFuelContributer.Amount += amount;
            EngineFuel += amount;
        }

        public void SetEngineStage(int state, int time)
        {
            EngineStage = state;
            EngineStageTime = time;
        }

        public void Save() => _db.HashSetAsync("engine", ServerName, JsonConvert.SerializeObject(this));
    }
}
