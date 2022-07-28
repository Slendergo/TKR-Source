using common.database.extension;
using common.database.info;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace common.database.model
{
    public class LoginModel : IRedisModel
    {
        private byte[] _buffer;
        private IDatabase _db;
        private string _key;

        private RandomNumberGenerator _rng;
        public string Email { get; private set; }
        public bool EntryExist { get; private set; }
        public LoginInfo Info { get; private set; }
        public bool IsNull { get; private set; }

        /// <summary>
        /// <paramref name="args"/>: [0] -> email
        /// </summary>
        /// <param name="db"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task InitAsync(IDatabase db, params string[] args)
        {
            Email = args[0].ToUpperInvariant();

            _db = db;
            _key = KeyPattern(Email);
            _rng = RandomNumberGenerator.Create();
            _buffer = new byte[0x10];

            EntryExist = await db.KeyExistsAsync(_key);

            if (!EntryExist)
                return;

            var data = await db.ReadAsync<string>(_key, Email);

            if (data != null)
                Info = JsonConvert.DeserializeObject<LoginInfo>(data);
            else
                IsNull = true;
        }

        public string KeyPattern(params string[] args) => $"logins.{args[0]}";

        public async Task SetLoginInfoAsync(string password)
        {
            _rng.GetNonZeroBytes(_buffer);

            var salt = Convert.ToBase64String(_buffer);
            var hash = Convert.ToBase64String($"{password}{salt}".Sha1Digest());
            var info = new LoginInfo()
            {
                AccountId = Info.AccountId,
                HashedPassword = hash,
                Salt = salt
            };

            await _db.WriteAsync(_key, Email, JsonConvert.SerializeObject(info));
        }
    }
}
