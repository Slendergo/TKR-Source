using common.database.extension;
using common.database.info;
using common.database.model;
using common.database.service;
using common.database.status;
using common.resources;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace common.database
{
    public sealed class RedisDb
    {
        public const string NameLockKey = "nameLock";

        public static readonly string[] GUEST_NAMES = new[]
        {
            "Darq", "Deyst", "Drac", "Drol", "Eango", "Eashy", "Eati", "Eendi", "Ehoni",
            "Gharr", "Iatho", "Iawa", "Idrae", "Iri", "Issz", "Itani", "Laen", "Lauk",
            "Lorz", "Oalei", "Odaru", "Oeti", "Orothi", "Oshyu", "Queq", "Radph", "Rayr",
            "Ril", "Rilr", "Risrr", "Saylt", "Scheev", "Sek", "Serl", "Seus", "Tal", "Tiar",
            "Uoro", "Urake", "Utanu", "Vorck", "Vorv", "Yangu", "Yimi", "Zhiar"
        };

        public readonly TimeSpan LockExpiry;
        public readonly string LockToken;

        public IDatabase Db;

        private const int _lockTtl = 60;
        private const string _registerLockKey = "registerLock";

        private readonly string _connection;
        private readonly int _index;
        private readonly Resources _resources;
        private readonly Random _rnd;
        private Dictionary<string, IRedisService> _services;

        public RedisDb(DbInfo info, Resources resource)
        {
            _resources = resource;
            _index = info.index;
            _rnd = new Random();

            LockToken = Guid.NewGuid().ToString();
            LockExpiry = TimeSpan.FromSeconds(_lockTtl);

            var connection = new StringBuilder($"{info.host}:{info.port},syncTimeout=120000");

            if (!string.IsNullOrWhiteSpace(info.auth))
                connection.Append($",password={info.auth}");

            _connection = connection.ToString();
        }

        public LegendService GetLegendService => _services["legend"] as LegendService;
        public MarketService GetMarketService => _services["market"] as MarketService;

        public ISubscriber Subscriber { get; private set; }

        public async Task<(AddCharacterStatus status, CharacterModel model)> AddCharacterToAccountAsync(AccountModel account, ushort type, ushort skin)
        {
            if (await Db.SetLengthAsync($"alive.{account.Id}") >= account.MaxCharSlot)
                return (AddCharacterStatus.ReachedLimit, null);

            if (skin != 0)
            {
                var skinDesc = _resources.GameData.Skins[skin];

                if (!account.Skins.Contains(skin) || skinDesc.PlayerClassType != type)
                    return (AddCharacterStatus.SkinUnavailable, null);
            }

            var objDesc = _resources.GameData.ObjectDescs[type];
            var playerDesc = _resources.GameData.Classes[type];
            var classStats = new ClassStatsModel();

            await classStats.InitAsync(Db, account.Id.ToString());

            var unlockInfo = classStats[playerDesc.Unlock?.Type ?? ushort.MaxValue];
            var classInfo = classStats[type];

            if (unlockInfo.IsNull || classInfo.IsNull)
                return (AddCharacterStatus.Error, null);

            if (classInfo.BestLevel < unlockInfo.BestLevel)
                return (AddCharacterStatus.Locked, null);

            var newCharacters = _resources.Settings.NewCharacters;
            var id = await account.IncrementNextCharacterIdAsync();
            var character = new CharacterModel();

            await character.InitAsync(Db, account.Id.ToString(), id.ToString());

            var stats = new int[playerDesc.Stats.Length];

            for (var i = 0; i < stats.Length; i++)
                stats[i] = playerDesc.Stats[i].StartingValue;

            await character.AddAsync(new CharacterInfo()
            {
                ObjectType = type,
                Level = newCharacters.Level,
                Items = Enumerable.Repeat((ushort)0xffff, 20).ToArray(),
                Stats = stats,
                Skin = skin
            });
            await Db.SetAddAsync($"alive.{account.Id}", BitConverter.GetBytes(character.Id), CommandFlags.FireAndForget);

            return (AddCharacterStatus.Ok, character);
        }

        public async Task<bool> HasEmailAsync(string email) => await Db.HashExistsAsync("logins", email.ToUpperInvariant());

        public async Task<bool> IsIpBannedAsync(string ip)
        {
            var model = new IpModel();

            await model.InitAsync(Db, ip);

            return model.EntryExist && !model.IsNull ? model.Info.Banned : false;
        }

        public async Task<(LoginStatus status, AccountModel model)> LoginAsync(string email, string password)
        {
            var login = new LoginModel();

            await login.InitAsync(Db, email);

            if (!login.EntryExist)
                return (LoginStatus.AccountNotExists, null);

            var hash = Convert.ToBase64String($"{password}{login.Info.Salt}".Sha1Digest());

            if (!login.Info.HashedPassword.Equals(hash))
                return (LoginStatus.InvalidCredentials, null);

            var newAccounts = _resources.Settings.NewAccounts;
            var account = new AccountModel();

            await account.InitAsync(Db, login.Info.AccountId.ToString());

            var classStats = new ClassStatsModel();

            await classStats.InitAsync(Db, account.Id.ToString());

            if (newAccounts.ClassesUnlocked)
                foreach (var type in _resources.GameData.Classes.Keys)
                    await classStats.UnlockAsync(type);

            return (LoginStatus.Ok, account);
        }

        public async Task NameAcquireLockAsync()
        {
            var trans = Db.CreateTransaction();
            trans.AddCondition(Condition.KeyNotExists(NameLockKey));

            await trans.StringSetAsync(NameLockKey, LockToken, LockExpiry);
            await trans.ExecuteAsync(CommandFlags.FireAndForget);
        }

        public async Task<int> NextAccountIdAsync() => int.Parse(await Db.StringGetAsync("nextAccId"));

        public async Task<int> NextGuildIdAsync() => int.Parse(await Db.StringGetAsync("nextGuildId"));

        public async Task PurchaseSkinAsync(AccountModel account, ushort skin, int amount)
        {
            await account.UpdateCurrencyAsync(CurrencyType.Gold, amount);
            await account.AddSkinAsync(skin);
        }

        public async Task<(AccountRegisterStatus, AccountModel model)> RegisterAccountAsync(string email, string password, bool isGuest)
        {
            if (await HasEmailAsync(email))
                return (AccountRegisterStatus.EmailInUse, null);

            var newAccounts = _resources.Settings.NewAccounts;
            var account = new AccountModel();

            var id = await NextAccountIdAsync();

            await account.InitAsync(Db, id.ToString());

            account.BuildNew(email, new AccountInfo()
            {
                Name = GUEST_NAMES[_rnd.Next(0, GUEST_NAMES.Length)],
                VaultAmount = newAccounts.VaultCount,
                MaxCharSlot = newAccounts.MaxCharSlot,
                Fame = newAccounts.Fame,
                Gold = newAccounts.Credits,
                Skins = newAccounts.SkinsUnlocked ? _resources.GameData.Skins.Values.Select(_ => _.Type).ToArray() : new ushort[0],
                IsGuest = isGuest
            });

            await account.SaveAsync();

            var login = new LoginModel();

            await login.InitAsync(Db, email);
            await login.SetLoginInfoAsync(password);

            var classStats = new ClassStatsModel();

            await classStats.InitAsync(Db, account.Id.ToString());

            if (newAccounts.ClassesUnlocked)
                foreach (var type in _resources.GameData.Classes.Keys)
                    await classStats.UnlockAsync(type);

            return (AccountRegisterStatus.Ok, account);
        }

        public async Task RegisterAcquireLockAsync()
        {
            var trans = Db.CreateTransaction();
            trans.AddCondition(Condition.KeyNotExists(_registerLockKey));

            await trans.StringSetAsync(_registerLockKey, LockToken, LockExpiry);
            await trans.ExecuteAsync(CommandFlags.FireAndForget);
        }

        public async Task RegisterCharacterDeathAsync(AccountModel account, CharacterModel character, FameStats stats, string killer)
        {
            var classStats = new ClassStatsModel();

            await classStats.InitAsync(Db, account.Id.ToString());

            var fame = stats.CalculateBonuses(_resources.GameData, character, classStats, out bool isFirstBorn);

            await character.SetDeathAsync(fame);
            await SaveCharacterAsync(account, character, classStats, true);

            var death = new DeathModel();

            await death.InitAsync(Db, account.Id.ToString(), character.Id.ToString());
            await death.AddAsync(new DeathInfo()
            {
                ObjectType = character.ObjectType,
                Level = character.Level,
                TotalFame = fame,
                Killer = killer,
                IsFirstBorn = isFirstBorn
            });
            await account.UpdateCurrencyAsync(CurrencyType.Fame, fame);

            if (account.Rank >= 60 || account.IsAdmin)
                return;

            if (account.GuildId > 0)
            {
                var guild = new GuildModel();

                await guild.InitAsync(Db, account.GuildId.ToString());

                if (guild.EntryExist)
                {
                    await guild.UpdateCurrencyAsync(fame);
                    await account.UpdateCurrencyAsync(CurrencyType.GuildFame, fame);
                }
            }

            await GetLegendService.InsertAsync(account.Id, character.Id, fame);
        }

        public async Task<AccountModel> RegisterGuestAccountAsync(string email)
        {
            var newAccounts = _resources.Settings.NewAccounts;
            var model = new AccountModel();
            model.BuildGuest(email, new AccountInfo()
            {
                Name = GUEST_NAMES[_rnd.Next(0, GUEST_NAMES.Length)],
                VaultAmount = newAccounts.VaultCount,
                MaxCharSlot = newAccounts.MaxCharSlot,
                Fame = newAccounts.Fame,
                Gold = newAccounts.Credits,
                Skins = newAccounts.SkinsUnlocked ? _resources.GameData.Skins.Values.Select(_ => _.Type).ToArray() : new ushort[0]
            });

            var classStats = new ClassStatsModel();

            await classStats.InitAsync(Db, model.Id.ToString());

            if (newAccounts.ClassesUnlocked)
                foreach (var type in _resources.GameData.Classes.Keys)
                    await classStats.UnlockAsync(type);
            else
                await Db.KeyDeleteAsync($"classStats.{model.Id}");

            return model;
        }

        public async Task<(GuildCreateStatus status, GuildModel model)> RegisterGuildAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return (GuildCreateStatus.InvalidName, null);

            var regex = new Regex(@"\s+");

            name = regex.Replace(name, " ");
            name = name.Trim();

            regex = new Regex(@"^[A-Za-z\s]{1,20}$");

            if (!regex.IsMatch(name))
                return (GuildCreateStatus.InvalidName, null);

            var id = await NextGuildIdAsync();

            if (!await Db.HashSetAsync("guilds", name.ToUpperInvariant(), id, When.NotExists))
                return (GuildCreateStatus.UsedName, null);

            var model = new GuildModel();

            await model.InitAsync(Db, id.ToString());
            await model.BuildAsync(name);

            return (GuildCreateStatus.Ok, model);
        }

        public async Task SaveCharacterAsync(AccountModel account, CharacterModel character, ClassStatsModel stats, bool lockAccount)
        {
            if (lockAccount && !await account.HasAccountLockAsync())
                await account.AddLockAsync(this);

            await character.SaveAsync();
            await stats.UpdateAsync(character);
        }

        public async Task StartAsync()
        {
            var connectionMultiplex = await ConnectionMultiplexer.ConnectAsync(_connection);

            Db = connectionMultiplex.GetDatabase(_index);
            _services = new Dictionary<string, IRedisService>()
            {
                ["legend"] = new LegendService(Db),
                ["market"] = new MarketService(Db)
            };

            foreach (var service in _services.Values)
                service.Start();

            Subscriber = connectionMultiplex.GetSubscriber();
        }

        public void Stop()
        {
            foreach (var service in _services.Values)
                service.Stop();

            Subscriber.Multiplexer.Dispose();
        }
    }
}
