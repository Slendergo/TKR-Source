using common.database.extension;
using common.database.info;
using common.resources;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace common.database.model
{
    public class AccountModel : IRedisModel
    {
        private IDatabase _db;
        private string _key;
        private string _token;

        public int BanLifetime { get; private set; }
        public int Bestiary { get; private set; }
        public string Email { get; private set; }
        public bool EntryExist { get; private set; }
        public int Fame { get; private set; }
        public int FameHistory { get; private set; }
        public ushort[] Gifts { get; private set; }
        public int GlowColor { get; private set; }
        public int Gold { get; private set; }
        public int GoldHistory { get; private set; }
        public int GuildFame { get; private set; }
        public int GuildId { get; private set; }
        public GuildRank GuildRank { get; private set; }
        public bool HasDonorLoot { get; private set; }
        public int Id { get; private set; }
        public int[] IgnoreList { get; private set; }
        public string Ip { get; private set; }
        public bool IsAdmin { get; private set; }
        public bool IsBanned { get; private set; }
        public bool IsFirstDeath { get; private set; }
        public bool IsGuest { get; private set; }
        public bool IsHidden { get; private set; }
        public bool IsNameChosen { get; private set; }
        public DateTime LastRecoveryTime { get; private set; }
        public int LastSeen { get; private set; }
        public int[] LockList { get; private set; }
        public int MaxCharSlot { get; private set; }
        public string Name { get; private set; }
        public int NameColor { get; private set; }
        public int NextCharId { get; private set; }
        public string Notes { get; private set; }
        public int Rank { get; private set; }
        public DateTime Registration { get; private set; }
        public string ResetToken { get; private set; }
        public int SetBaseStat { get; private set; }
        public int Size { get; private set; }
        public ushort[] Skins { get; private set; }
        public int TextColor { get; private set; }
        public int VaultAmount { get; private set; }

        public async Task AddGifsAsync(IList<ushort> items)
        {
            var gifts = Gifts.ToList();
            gifts.AddRange(items);

            Gifts = gifts.ToArray();

            await _db.WriteAsync(_key, "gifts", Gifts);
        }

        public async Task<bool> AddLockAsync(RedisDb redis)
        {
            var key = $"lock:{Id}";
            var trans = _db.CreateTransaction();
            trans.AddCondition(Condition.KeyNotExists(key));

            await trans.StringSetAsync(key, redis.LockToken, redis.LockExpiry);

            var result = await trans.ExecuteAsync();

            _token = result ? redis.LockToken : string.Empty;

            return result;
        }

        public async Task AddSkinAsync(ushort skin)
        {
            if (Skins.Contains(skin))
                return;

            var skins = Skins.ToList();
            skins.Add(skin);

            Skins = skins.ToArray();

            await _db.WriteAsync(_key, "skins", Skins);
        }

        public async Task<int[]> AliveCharacterIdsAsync()
        {
            var buffers = await _db.SetMembersAsync($"alive.{Id}");
            var ids = new int[buffers.Length];

            for (var i = 0; i < ids.Length; i++)
                ids[i] = BitConverter.ToInt32(buffers[i], 0);

            return ids;
        }

        public async Task BanAsync(string notes = "", int banLifetime = -1)
        {
            IsBanned = true;

            await _db.WriteAsync(_key, "banned", IsBanned);

            if (!string.IsNullOrWhiteSpace(notes))
            {
                Notes = notes;

                await _db.WriteAsync(_key, "notes", Notes);
            }
        }

        public void BuildGuest(string email, AccountInfo info)
        {
            Id = 0;
            Email = email;
            Name = info.Name;
            IsAdmin = false;
            IsNameChosen = false;
            IsFirstDeath = true;
            GuildId = 0;
            GuildRank = GuildRank.Initiate;
            VaultAmount = info.VaultAmount;
            MaxCharSlot = info.MaxCharSlot;
            Registration = DateTime.UtcNow;
            IsGuest = true;
            Fame = info.Fame;
            FameHistory = info.Fame;
            Gold = info.Gold;
            GoldHistory = info.Gold;
            Rank = 0;
            ResetToken = string.Empty;
            Skins = info.Skins;
        }

        public void BuildNew(string email, AccountInfo info)
        {
            Email = email;
            IsAdmin = false;
            IsNameChosen = false;
            IsFirstDeath = true;
            GuildId = 0;
            GuildRank = 0;
            VaultAmount = info.VaultAmount;
            MaxCharSlot = info.MaxCharSlot;
            Registration = DateTime.UtcNow;
            IsGuest = info.IsGuest;
            Fame = info.Fame;
            FameHistory = info.Fame;
            Gold = info.Gold;
            GoldHistory = info.Gold;
            ResetToken = string.Empty;
            Skins = info.Skins;
            Rank = 0;
            LastSeen = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            Bestiary = 0;
        }

        public async Task<int[]> DeadCharacterIdsAsync()
        {
            var buffers = await _db.SetMembersAsync($"dead.{Id}");

            var ids = new int[buffers.Length];

            for (var i = 0; i < ids.Length; i++)
                ids[i] = BitConverter.ToInt32(buffers[i], 0);

            return ids;
        }

        public async Task<bool> HasAccountLockAsync() => await _db.KeyExistsAsync($"lock:{Id}");

        public async Task IgnoreAccountIdAsync(int id, bool isAdding)
        {
            if (isAdding && IgnoreList.Contains(id))
                return;

            if (!isAdding && !IgnoreList.Contains(id))
                return;

            var ignoreList = IgnoreList.ToList();

            if (isAdding)
                ignoreList.Add(id);
            else
                ignoreList.Remove(id);

            IgnoreList = ignoreList.ToArray();

            await _db.WriteAsync(_key, "ignoreList", IgnoreList);
        }

        public async Task<int> IncrementGiftsAsync() => (int)await _db.HashIncrementAsync(_key, "giftCount");

        public async Task<int> IncrementNextCharacterIdAsync() => (int)await _db.HashIncrementAsync(_key, "nextCharId");

        /// <summary>
        /// <paramref name="args"/>: [0] -> account id
        /// </summary>
        /// <param name="db"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task InitAsync(IDatabase db, params string[] args)
        {
            Id = int.Parse(args[0]);

            _db = db;
            _key = KeyPattern(Id.ToString());

            EntryExist = await db.KeyExistsAsync(_key);

            if (!EntryExist)
                return;

            IsAdmin = await db.ReadAsync<bool>(_key, "admin");
            BanLifetime = await db.ReadAsync<int>(_key, "banLiftTime");
            IsBanned = await db.ReadAsync<bool>(_key, "banned");
            TextColor = await db.ReadAsync<int>(_key, "colorchat");
            NameColor = await db.ReadAsync<int>(_key, "colornamechat");
            Gold = await db.ReadAsync<int>(_key, "credits");
            Bestiary = await db.ReadAsync<int>(_key, "enemyKilled");
            Fame = await db.ReadAsync<int>(_key, "fame");
            IsFirstDeath = await db.ReadAsync<bool>(_key, "firstDeath");
            Gifts = await db.ReadAsync<ushort[]>(_key, "gifts");
            GlowColor = await db.ReadAsync<int>(_key, "glow");
            IsGuest = await db.ReadAsync<bool>(_key, "guest");
            GuildFame = await db.ReadAsync<int>(_key, "guildFame");
            GuildId = await db.ReadAsync<int>(_key, "guildId");
            GuildRank = (GuildRank)await db.ReadAsync<int>(_key, "guildRank");
            IsHidden = await db.ReadAsync<bool>(_key, "hidden");
            IgnoreList = await db.ReadAsync<int[]>(_key, "ignoreList");
            Ip = await db.ReadAsync<string>(_key, "ip");
            LastSeen = await db.ReadAsync<int>(_key, "lastSeen");
            Rank = await db.ReadAsync<int>(_key, "rank");
            LockList = await db.ReadAsync<int[]>(_key, "lockList");
            MaxCharSlot = await db.ReadAsync<int>(_key, "maxCharSlot");
            Name = await db.ReadAsync<string>(_key, "name");
            IsNameChosen = await db.ReadAsync<bool>(_key, "nameChosen");
            NextCharId = await db.ReadAsync<int>(_key, "nextCharId");
            Notes = await db.ReadAsync<string>(_key, "notes");
            ResetToken = await db.ReadAsync<string>(_key, "passResetToken");
            Registration = await db.ReadAsync<DateTime>(_key, "regTime");
            LastRecoveryTime = await db.ReadAsync<DateTime>(_key, "lastRecoveryTime");
            SetBaseStat = await db.ReadAsync<int>(_key, "setBaseStat");
            HasDonorLoot = await db.ReadAsync<bool>(_key, "setDonorLoot");
            Size = await db.ReadAsync<int>(_key, "size");
            Skins = await db.ReadAsync<ushort[]>(_key, "skins");
            GoldHistory = await db.ReadAsync<int>(_key, "totalCredits");
            FameHistory = await db.ReadAsync<int>(_key, "totalFame");
            Email = await db.ReadAsync<string>(_key, "uuid");
            VaultAmount = await db.ReadAsync<int>(_key, "vaultCount");
        }

        public async Task<bool> IsMutedAsync(int accountId) => await _db.KeyExistsAsync($"mutes:{Ip}");

        public string KeyPattern(params string[] args) => $"account.{args[0]}";

        public async Task LeaveGuildAsync()
        {
            GuildId = 0;
            GuildRank = 0;
            GuildFame = 0;

            await _db.WriteAsync(_key, "guildId", GuildId);
            await _db.WriteAsync(_key, "guildRank", GuildRank);
            await _db.WriteAsync(_key, "guildFame", GuildFame);
        }

        public async Task LockAccountIdAsync(int id, bool isAdding)
        {
            if (isAdding && LockList.Contains(id))
                return;

            if (!isAdding && !LockList.Contains(id))
                return;

            var lockList = LockList.ToList();

            if (isAdding)
                lockList.Add(id);
            else
                lockList.Remove(id);

            LockList = lockList.ToArray();

            await _db.WriteAsync(_key, "lockList", LockList);
        }

        public async Task<TimeSpan?> LockTimeAsync() => await _db.KeyTimeToLiveAsync($"lock:{Id}");

        public async Task MuteAsync(TimeSpan timeSpan) => await _db.StringSetAsync($"mutes:{Ip}", "", timeSpan, flags: CommandFlags.FireAndForget);

        public async Task ReleaseLockAsync(RedisDb redis)
        {
            if (string.IsNullOrWhiteSpace(_token))
                return;

            var key = $"lock:{Id}";
            var trans = _db.CreateTransaction();
            trans.AddCondition(Condition.StringEqual(key, _token));

            await trans.KeyDeleteAsync(key);
            await trans.ExecuteAsync(CommandFlags.FireAndForget);
        }

        public async Task RemoveGiftAsync(ushort item)
        {
            if (!Gifts.Contains(item))
                return;

            var gifts = Gifts.ToList();
            gifts.Remove(item);

            Gifts = gifts.ToArray();

            await _db.WriteAsync(_key, "gifts", Gifts);
        }

        public async Task<bool> RenameAsync(RedisDb redis, string name)
        {
            var trans = _db.CreateTransaction();
            trans.AddCondition(Condition.StringEqual(RedisDb.NameLockKey, redis.LockToken));

            await trans.HashDeleteAsync("names", Name.ToUpperInvariant());
            await trans.HashSetAsync("names", name.ToUpperInvariant(), Id);

            if (await trans.ExecuteAsync(CommandFlags.FireAndForget))
            {
                Name = name;
                IsNameChosen = true;

                await _db.WriteAsync(_key, "name", Name);
                await _db.WriteAsync(_key, "nameChosen", IsNameChosen);

                return true;
            }

            return false;
        }

        public async Task<bool> RenewLockAsync(RedisDb redis)
        {
            _token = redis.LockToken;

            var key = $"lock:{Id}";
            var tran = _db.CreateTransaction();
            tran.AddCondition(Condition.StringEqual(key, _token));

            await tran.KeyExpireAsync(key, redis.LockExpiry);

            return await tran.ExecuteAsync(CommandFlags.FireAndForget);
        }

        public async Task SaveAsync()
        {
            await _db.WriteAsync(_key, "admin", IsAdmin);
            await _db.WriteAsync(_key, "banLiftTime", BanLifetime);
            await _db.WriteAsync(_key, "banned", IsBanned);
            await _db.WriteAsync(_key, "colorchat", TextColor);
            await _db.WriteAsync(_key, "colornamechat", NameColor);
            await _db.WriteAsync(_key, "credits", Gold);
            await _db.WriteAsync(_key, "enemyKilled", Bestiary);
            await _db.WriteAsync(_key, "fame", Fame);
            await _db.WriteAsync(_key, "firstDeath", IsFirstDeath);
            await _db.WriteAsync(_key, "gifts", Gifts);
            await _db.WriteAsync(_key, "glow", GlowColor);
            await _db.WriteAsync(_key, "guest", IsGuest);
            await _db.WriteAsync(_key, "guildFame", GuildFame);
            await _db.WriteAsync(_key, "guildId", GuildId);
            await _db.WriteAsync(_key, "guildRank", GuildRank);
            await _db.WriteAsync(_key, "hidden", IsHidden);
            await _db.WriteAsync(_key, "ignoreList", IgnoreList);
            await _db.WriteAsync(_key, "ip", Ip);
            await _db.WriteAsync(_key, "lastSeen", LastSeen);
            await _db.WriteAsync(_key, "rank", Rank);
            await _db.WriteAsync(_key, "lockList", LockList);
            await _db.WriteAsync(_key, "maxCharSlot", MaxCharSlot);
            await _db.WriteAsync(_key, "name", Name);
            await _db.WriteAsync(_key, "nameChosen", IsNameChosen);
            await _db.WriteAsync(_key, "nextCharId", NextCharId);
            await _db.WriteAsync(_key, "notes", Notes);
            await _db.WriteAsync(_key, "passResetToken", ResetToken);
            await _db.WriteAsync(_key, "regTime", Registration);
            await _db.WriteAsync(_key, "lastRecoveryTime", LastRecoveryTime);
            await _db.WriteAsync(_key, "setBaseStat", SetBaseStat);
            await _db.WriteAsync(_key, "setDonorLoot", HasDonorLoot);
            await _db.WriteAsync(_key, "size", Size);
            await _db.WriteAsync(_key, "skins", Skins);
            await _db.WriteAsync(_key, "totalCredits", GoldHistory);
            await _db.WriteAsync(_key, "totalFame", FameHistory);
            await _db.WriteAsync(_key, "uuid", Email);
            await _db.WriteAsync(_key, "vaultCount", VaultAmount);
        }

        public async Task SetGuildAsync(int id, GuildRank rank, int fame)
        {
            GuildId = id;
            GuildRank = rank;
            GuildFame = fame;

            await _db.WriteAsync(_key, "guildId", GuildId);
            await _db.WriteAsync(_key, "guildRank", GuildRank);
            await _db.WriteAsync(_key, "guildFame", GuildFame);
        }

        public async Task SetGuildRankAsync(GuildRank rank)
        {
            GuildRank = rank;

            await _db.WriteAsync(_key, "guildRank", GuildRank);
        }

        public async Task UnbanAsync()
        {
            IsBanned = false;

            await _db.WriteAsync(_key, "banned", IsBanned);
        }

        public async Task UpdateCurrencyAsync(CurrencyType currency, int amount)
        {
            if (amount > 0)
                if (currency == CurrencyType.Fame || currency == CurrencyType.Gold)
                    await _db.HashIncrementAsync(_key, currency.ToField(true), amount)
                        .ContinueWith(task =>
                        {
                            if (task.IsCanceled)
                                return;

                            if (currency == CurrencyType.Fame)
                                FameHistory = (int)task.Result;
                            else
                                GoldHistory = (int)task.Result;
                        });

            await _db.HashIncrementAsync(_key, currency == CurrencyType.GuildFame ? "guildFame" : currency.ToField(), amount)
                .ContinueWith(task =>
                {
                    if (task.IsCanceled)
                        return;

                    switch (currency)
                    {
                        case CurrencyType.Fame: Fame = (int)task.Result; break;
                        case CurrencyType.Gold: Gold = (int)task.Result; break;
                        case CurrencyType.GuildFame: GuildFame = (int)task.Result; break;
                    }
                });
        }
    }
}
