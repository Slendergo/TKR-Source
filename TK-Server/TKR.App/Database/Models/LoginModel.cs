using Newtonsoft.Json;
using StackExchange.Redis;
using System.Security.Cryptography;
using System.Text;

namespace TKR.App.Database.Models
{
    [Flags]
    public enum LoginModelResult
    {
        OK,
        AccountNotExists,
        InvalidCredentials
    }

    public sealed class LoginModel
    {
        private readonly IDatabase _db;

        public int AccountId { get; set; }
        public string HashedPassword { get; set; }

        [JsonIgnore] public string UUID { get; private set; }
        [JsonIgnore] public bool Exists { get; private set; }

        public LoginModel(IDatabase db, string uuid)
        {
            _db = db;
            UUID = uuid;

            var json = (string)db.HashGet("logins", uuid.ToUpperInvariant());

            if (json == null)
                Exists = true;
            else
                JsonConvert.PopulateObject(json, this);
        }

        public void Flush() => _db.HashSet("logins", UUID.ToUpperInvariant(), JsonConvert.SerializeObject(this));

        public static string GetHashedPassword(string password)
        {
            var sb = new StringBuilder();
            using (var md5 = MD5.Create())
            {
                var computedBuffer = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (var i = 0; i < computedBuffer.Length; i++)
                    _ = sb.Append(computedBuffer[i].ToString("X2"));

                var salt = password.Substring(0, password.Length / 2);
                var buffer = md5.ComputeHash(Encoding.UTF8.GetBytes(salt));
                for (var i = 0; i < computedBuffer.Length; i++)
                    _ = sb.Append(buffer[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
