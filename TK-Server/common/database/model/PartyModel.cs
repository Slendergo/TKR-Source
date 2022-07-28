using common.database.extension;
using StackExchange.Redis;
using System.Linq;
using System.Threading.Tasks;

namespace common.database.model
{
    public class PartyModel : IRedisModel
    {
        private IDatabase _db;
        private string _key;

        public bool EntryExist { get; private set; }
        public int Id { get; private set; }
        public int[] Members { get; private set; }

        /// <summary>
        /// <paramref name="args"/>: [0] -> party id
        /// </summary>
        /// <param name="db"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task InitAsync(IDatabase db, params string[] args)
        {
            Id = int.Parse(args[0]);

            _db = db;
            _key = KeyPattern();

            EntryExist = await db.KeyExistsAsync(_key);

            if (!EntryExist)
                return;

            Members = await db.ReadAsync<int[]>(_key, "members");
        }

        public string KeyPattern(params string[] args) => $"party";

        public async Task SaveAsync() => await _db.WriteAsync(_key, "members", Members);

        public async Task UpdateMemberAsync(int accoundId, bool isAdding)
        {
            if (isAdding && Members.Contains(accoundId))
                return;

            if (!isAdding && !Members.Contains(accoundId))
                return;

            var members = Members.ToList();

            if (isAdding)
                members.Add(accoundId);
            else
                members.Remove(accoundId);

            Members = members.ToArray();

            await _db.WriteAsync(_key, "members", Members);
        }
    }
}
