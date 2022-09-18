using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System;
using System.Security.Cryptography;
using System.Text;
using TKR.App.Database.Models;

namespace TKR.App.Database
{

    public sealed class RedisDatabase
    {
        private readonly ILogger<RedisDatabase> _logger;
        private readonly IDatabase _database;
        private readonly ConnectionMultiplexer _multiplexer;

        public RedisDatabase(IConfiguration configuration, ILogger<RedisDatabase> logger)
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
            if (string.IsNullOrWhiteSpace(guid) || string.IsNullOrWhiteSpace(password))
                return LoginModelResult.InvalidCredentials;

            var model = new LoginModel(_database, guid);
            if (model.Exists)
                return LoginModelResult.AccountNotExists;

            var hashedPassword = LoginModel.GetHashedPassword(password);
            if (hashedPassword != model.HashedPassword)
                return LoginModelResult.InvalidCredentials;

            return LoginModelResult.OK;
        }
    }
}
