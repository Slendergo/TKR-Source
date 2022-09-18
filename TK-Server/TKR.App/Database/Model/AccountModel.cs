using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Text;

namespace TKR.App.Database.Models
{
    public sealed class AccountModel : RedisObject
    {
        public readonly int AccountId;
        public readonly RedisField<string> UUID;
        public readonly RedisField<string> Name;
        public readonly RedisField<int> Credits;
        public readonly RedisField<int> Fame;
        public readonly RedisField<bool> FirstDeath;
        public readonly RedisField<int> GuildId;
        public readonly RedisField<int> GuildRank;
        public readonly RedisField<int> MaxCharSlot;
        public readonly RedisField<int> NextCharId;
        public readonly RedisField<int> TotalCredits;
        public readonly RedisField<int> TotalFame;

        public AccountModel(IDatabase db, int accountId)
        {
            AccountId = accountId;

            Init(db, $"account.{accountId}");

            UUID = RedisField<string>.Create(this, "uuid");
            Name = RedisField<string>.Create(this, "name");
            Credits = RedisField<int>.Create(this, "credits");
            Fame = RedisField<int>.Create(this, "fame");
            FirstDeath = RedisField<bool>.Create(this, "firstDeath");
            GuildId = RedisField<int>.Create(this, "guildId");
            GuildRank = RedisField<int>.Create(this, "guildRank");
            MaxCharSlot = RedisField<int>.Create(this, "maxCharSlot");
            NextCharId = RedisField<int>.Create(this, "nextCharId");
            TotalCredits = RedisField<int>.Create(this, "totalCredits");
            TotalFame = RedisField<int>.Create(this, "totalFame");
        }
    }
}
