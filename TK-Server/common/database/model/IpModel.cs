using common.database.extension;
using common.database.info;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace common.database.model
{
    public class IpModel : IRedisModel
    {
        private IDatabase _db;
        private string _key;

        public bool EntryExist { get; private set; }
        public IpInfo Info { get; private set; }
        public string Ip { get; private set; }
        public bool IsNull { get; private set; }

        public async Task AddAsync(int accountId)
        {
            if (IsNull)
                Info = new IpInfo()
                {
                    Accounts = new List<int>() { accountId },
                    Banned = false,
                    Notes = string.Empty
                };
            else
                Info.Accounts.Add(accountId);

            await _db.WriteAsync(_key, Ip, JsonConvert.SerializeObject(Info));
        }

        public async Task BanAsync(int accountId, string notes = "")
        {
            var info = new IpInfo()
            {
                Accounts = Info.Accounts,
                Banned = true,
                Notes = string.IsNullOrWhiteSpace(notes) ? Info.Notes : notes
            };

            Info = info;

            await _db.WriteAsync(_key, Ip, JsonConvert.SerializeObject(Info));
        }

        /// <summary>
        /// <paramref name="args"/>: [0] -> ip
        /// </summary>
        /// <param name="db"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task InitAsync(IDatabase db, params string[] args)
        {
            Ip = args[0];

            _db = db;
            _key = KeyPattern();

            EntryExist = await db.KeyExistsAsync(_key);

            if (!EntryExist)
            {
                IsNull = true;
                return;
            }

            var data = await db.ReadAsync<string>(_key, Ip);

            if (data != null)
                Info = JsonConvert.DeserializeObject<IpInfo>(data);
            else
                IsNull = true;
        }

        public string KeyPattern(params string[] args) => $"ips";

        public async Task UnbanAsync()
        {
            var info = new IpInfo()
            {
                Accounts = Info.Accounts,
                Banned = false,
                Notes = Info.Notes
            };

            Info = info;

            await _db.WriteAsync(_key, Ip, JsonConvert.SerializeObject(Info));
        }
    }
}
