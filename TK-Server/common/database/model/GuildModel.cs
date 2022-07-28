using common.database.extension;
using common.database.status;
using common.resources;
using StackExchange.Redis;
using System.Linq;
using System.Threading.Tasks;

namespace common.database.model
{
    public class GuildModel : IRedisModel
    {
        private const int _maxLevel = 3;
        private const int _maxSize = 128;

        private IDatabase _db;
        private string _key;

        public string Board { get; private set; }
        public int Currency { get; private set; }
        public bool EntryExist { get; private set; }
        public int Fame { get; private set; }
        public int FameHistory { get; private set; }
        public int Id { get; private set; }
        public int Level { get; private set; }
        public float LootBoost { get; private set; }
        public int[] Members { get; private set; }
        public string Name { get; private set; }
        public int Points { get; private set; }

        public async Task<GuildMemberStatus> AddMemberAsync(AccountModel model, bool isFounder = false)
        {
            if (!model.IsNameChosen)
                return GuildMemberStatus.NameNotChosen;

            if (model.GuildId == Id)
                return GuildMemberStatus.AlreadyIn;

            if (model.GuildId > 0)
                return GuildMemberStatus.InAnother;

            var members = Members.ToList();

            if (members.Count >= _maxSize)
                return GuildMemberStatus.Full;

            if (members.Contains(model.Id))
                return GuildMemberStatus.AlreadyMember;

            members.Add(model.Id);

            Members = members.ToArray();

            await _db.WriteAsync(_key, "members", Members);
            await model.SetGuildAsync(Id, isFounder ? GuildRank.Founder : GuildRank.Initiate, Fame);

            return GuildMemberStatus.Ok;
        }

        public async Task BuildAsync(string name)
        {
            Name = name;
            Level = 0;
            Fame = 0;
            FameHistory = 0;
            LootBoost = 0;
            Points = 0;
            Currency = 0;

            await _db.WriteAsync(_key, "name", Name);
            await _db.WriteAsync(_key, "level", Level);
            await _db.WriteAsync(_key, "fame", Fame);
            await _db.WriteAsync(_key, "totalFame", FameHistory);
            await _db.WriteAsync(_key, "guildLootBoost", LootBoost);
            await _db.WriteAsync(_key, "guildPoints", Points);
            await _db.WriteAsync(_key, "guildCurrency", Currency);
        }

        /// <summary>
        /// <paramref name="args"/>: [0] -> guild id
        /// </summary>
        /// <param name="db"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task InitAsync(IDatabase db, params string[] args)
        {
            Id = int.Parse(args[0]);

            _db = db;
            _key = KeyPattern(args);

            EntryExist = await db.KeyExistsAsync(_key);

            if (!EntryExist)
                return;

            Board = await db.ReadAsync<string>(_key, "board");
            Fame = await db.ReadAsync<int>(_key, "fame");
            Currency = await db.ReadAsync<int>(_key, "guildCurrency");
            LootBoost = await db.ReadAsync<float>(_key, "guildLootBoost");
            Points = await db.ReadAsync<int>(_key, "guildPoints");
            Level = await db.ReadAsync<int>(_key, "level");
            Members = await db.ReadAsync<int[]>(_key, "members");
            Name = await db.ReadAsync<string>(_key, "name");
            FameHistory = await db.ReadAsync<int>(_key, "totalFame");
        }

        public string KeyPattern(params string[] args) => $"guild.{args[0]}";

        public async Task RemoveMemberAsync(AccountModel model)
        {
            if (!Members.Contains(model.Id))
                return;

            var members = Members.ToList();
            members.Remove(model.Id);

            Members = members.ToArray();

            await model.LeaveGuildAsync();
            await _db.WriteAsync(_key, "members", Members);

            if (Members.Length == 0)
            {
                await _db.KeyDeleteAsync(_key, CommandFlags.FireAndForget);
                await _db.HashDeleteAsync("guilds", Name.ToUpperInvariant(), CommandFlags.FireAndForget);
            }
        }

        public async Task UpdateBoardAsync(string text)
        {
            Board = text;

            await _db.WriteAsync(_key, "board", Board);
        }

        public async Task UpdateCurrencyAsync(int amount)
        {
            if (amount > 0)
                await _db.HashIncrementAsync(_key, CurrencyType.GuildFame.ToField(true), amount)
                    .ContinueWith(task =>
                    {
                        if (task.IsCanceled)
                            return;

                        FameHistory = (int)task.Result;
                    });

            await _db.HashIncrementAsync(_key, CurrencyType.GuildFame.ToField(), amount)
                .ContinueWith(task =>
                {
                    if (task.IsCanceled)
                        return;

                    Fame = (int)task.Result;
                });
        }

        public async Task UpdateMemberRankAsync(AccountModel model, GuildRank rank)
        {
            if (model.GuildId <= 0)
                return;

            await model.SetGuildRankAsync(rank);
        }

        public async Task UpgradeAsync()
        {
            var level = Level + 1;

            if (level > _maxLevel)
                return;

            Level = level;

            await _db.WriteAsync(_key, "level", Level);
        }
    }
}
