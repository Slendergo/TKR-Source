using common.database.extension;
using StackExchange.Redis;
using System.Linq;
using System.Threading.Tasks;

namespace common.database.model
{
    public class VaultModel : IRedisModel
    {
        private int _accountId;
        private IDatabase _db;
        private string _key;

        public bool EntryExist { get; private set; }
        public ushort[] Items { get; private set; }

        public ushort[] this[int index]
        {
            get => _db.ReadAsync<ushort[]>(_key, $"vault.{index}").Result ?? Enumerable.Repeat((ushort)0xffff, 8).ToArray();
#pragma warning disable
            set => _db.WriteAsync(_key, $"vault.{index}", value);
#pragma warning restore
        }

        public async Task<int> AddAsync()
        {
            var nextId = (int)await _db.HashIncrementAsync($"account.{_accountId}", "vaultCount", flags: CommandFlags.FireAndForget);
            var field = KeyPattern(nextId.ToString());

            await _db.WriteAsync(_key, field, Enumerable.Repeat((ushort)0xffff, 8).ToArray());

            return nextId;
        }

        /// <summary>
        /// <paramref name="args"/>: [0] -> account id, [1] -> vault index (optional)
        /// </summary>
        /// <param name="db"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task InitAsync(IDatabase db, params string[] args)
        {
            _accountId = int.Parse(args[0]);
            _db = db;
            _key = KeyPattern(_accountId.ToString());

            EntryExist = await db.KeyExistsAsync(_key);

            if (!EntryExist)
                return;

            if (args.Length == 1)
                return;

            Items = await db.ReadAsync(_key, "items", Enumerable.Repeat((ushort)0xffff, 8).ToArray());
        }

        public string KeyPattern(params string[] args) => $"vault.{args[0]}";
    }
}
