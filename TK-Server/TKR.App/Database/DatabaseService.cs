using StackExchange.Redis;
using System.Text;
using TKR.App.Database.Models;

namespace TKR.App.Database
{
    public sealed class DatabaseService
    {
        private readonly ILogger<DatabaseService> _logger;
        private readonly IDatabase _database;
        private readonly ConnectionMultiplexer _multiplexer;

        public DatabaseService(IConfiguration configuration, ILogger<DatabaseService> logger)
        {
            _logger = logger;

            var host = configuration.GetConnectionString("redis_host");
            var port = configuration.GetConnectionString("redis_port");
            var auth = configuration.GetConnectionString("redis_auth");
            var index = int.Parse(configuration.GetConnectionString("redis_index"));

            var sb = new StringBuilder($"{host}:{port},syncTimeout=120000");
            if (!string.IsNullOrWhiteSpace(auth))
                _ = sb.Append($",password={auth}");

            _multiplexer = ConnectionMultiplexer.Connect(sb.ToString());
            _database = _multiplexer.GetDatabase(index);
        }

        public LoginModelResult ValidateLogin(string guid, string password)
        {
            if (string.IsNullOrWhiteSpace(guid))
                return LoginModelResult.InvalidCredentials;

            var model = new LoginModel(_database, guid);
            if (model.Exists)
                return LoginModelResult.AccountNotExists;

            var hashedPassword = LoginModel.GetHashedPassword(password);
            if (hashedPassword != model.HashedPassword)
                return LoginModelResult.InvalidCredentials;

            return LoginModelResult.OK;
        }

        public AccountModel GetAccount(string uuid)
        {
            var loginModel = new LoginModel(_database, uuid);
            if (loginModel.Exists)
                return null;
            var accountModel = new AccountModel(_database, loginModel.AccountId);
            return accountModel.HasKeys ? accountModel : null;
        }

        public AccountModel GetAccount(int accountId)
        {
            var accountModel = new AccountModel(_database, accountId);
            return accountModel.HasKeys ? accountModel : null;
        }
    }
}
