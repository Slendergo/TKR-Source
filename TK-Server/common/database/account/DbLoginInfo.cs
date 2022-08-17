using Newtonsoft.Json;
using StackExchange.Redis;

namespace common.database
{
    public class DbLoginInfo
    {
        private readonly IDatabase _db;

        public DbLoginInfo(IDatabase db, string uuid)
        {
            _db = db;

            UUID = uuid;

            var json = (string)db.HashGet("logins", uuid.ToUpperInvariant());

            if (json == null)
                IsNull = true;
            else
                JsonConvert.PopulateObject(json, this);
        }

        public int AccountId { get; set; }
        public string HashedPassword { get; set; }

        [JsonIgnore] public bool IsNull { get; private set; }

        public string Salt { get; set; }

        [JsonIgnore] public string UUID { get; private set; }

        public void Flush() => _db.HashSet("logins", UUID.ToUpperInvariant(), JsonConvert.SerializeObject(this));
    }
}
