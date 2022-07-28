using System;
using System.Linq;

namespace common
{
    public class ServerSettings
    {
        private DateTime _enemyDamageEndAt = DateTime.MinValue;
        private DateTime _enemyDamageStartAt = DateTime.MinValue;
        private DateTime _enemyHealthBoostEndAt = DateTime.MinValue;
        private DateTime _enemyHealthBoostStartAt = DateTime.MinValue;
        private DateTime _expEventEndAt = DateTime.MinValue;
        private DateTime _expEventStartAt = DateTime.MinValue;
        private DateTime _lootEventEndAt = DateTime.MinValue;
        private DateTime _lootEventStartAt = DateTime.MinValue;

        public string clientKey { get; set; } = "";
        public bool donorOnly { get; set; } = false;
        public string enemyDamageEndAt { get; set; } = DateTime.UtcNow.ToString();
        public double enemyDamageRate { get; set; } = 1.0;
        public string enemyDamageStartAt { get; set; } = DateTime.UtcNow.ToString();
        public string enemyHealthBoostEndAt { get; set; } = DateTime.UtcNow.ToString();
        public double enemyHealthBoostRate { get; set; } = 1.0;
        public string enemyHealthBoostStartAt { get; set; } = DateTime.UtcNow.ToString();
        public string expEventEndAt { get; set; } = DateTime.UtcNow.ToString();
        public double expEventRate { get; set; } = 1.0;
        public string expEventStartAt { get; set; } = DateTime.UtcNow.ToString();

        public DateTime GetEnemyDamageEndAt
        {
            get
            {
                if (_enemyDamageEndAt == DateTime.MinValue)
                    _enemyDamageEndAt = FromStringToDateTime(enemyDamageEndAt);

                return _enemyDamageEndAt;
            }
        }

        public DateTime GetEnemyDamageStartAt
        {
            get
            {
                if (_enemyDamageStartAt == DateTime.MinValue)
                    _enemyDamageStartAt = FromStringToDateTime(enemyDamageStartAt);

                return _enemyDamageStartAt;
            }
        }

        public DateTime GetEnemyHealthBoostEndAt
        {
            get
            {
                if (_enemyHealthBoostEndAt == DateTime.MinValue)
                    _enemyHealthBoostEndAt = FromStringToDateTime(enemyHealthBoostEndAt);

                return _enemyHealthBoostEndAt;
            }
        }

        public DateTime GetEnemyHealthBoostStartAt
        {
            get
            {
                if (_enemyHealthBoostStartAt == DateTime.MinValue)
                    _enemyHealthBoostStartAt = FromStringToDateTime(enemyHealthBoostStartAt);

                return _enemyHealthBoostStartAt;
            }
        }

        public DateTime GetExpEventEndAt
        {
            get
            {
                if (_expEventEndAt == DateTime.MinValue)
                    _expEventEndAt = FromStringToDateTime(expEventEndAt);

                return _expEventEndAt;
            }
        }

        public DateTime GetExpEventStartAt
        {
            get
            {
                if (_expEventStartAt == DateTime.MinValue)
                    _expEventStartAt = FromStringToDateTime(expEventStartAt);

                return _expEventStartAt;
            }
        }

        public DateTime GetLootEventEndAt
        {
            get
            {
                if (_lootEventEndAt == DateTime.MinValue)
                    _lootEventEndAt = FromStringToDateTime(lootEventEndAt);

                return _lootEventEndAt;
            }
        }

        public DateTime GetLootEventStartAt
        {
            get
            {
                if (_lootEventStartAt == DateTime.MinValue)
                    _lootEventStartAt = FromStringToDateTime(lootEventStartAt);

                return _lootEventStartAt;
            }
        }

        public string[] hashes { get; set; } = new string[] { };
        public string logFolder { get; set; } = "undefined";
        public string lootEventEndAt { get; set; } = DateTime.UtcNow.ToString();
        public double lootEventRate { get; set; } = 1.0;
        public string lootEventStartAt { get; set; } = DateTime.UtcNow.ToString();
        public bool marketEnabled { get; set; } = true;
        public int maxConnections { get; set; } = 0;
        public int maxPlayers { get; set; } = 0;
        public int realms { get; set; } = 3;
        public string resourceFolder { get; set; } = "undefined";
        public int restartTime { get; set; } = 0;
        public string serverKey { get; set; } = "";
        public string version { get; set; } = "undefined";
        public int[] whitelist { get; set; } = new int[] { };

        // 31.12.1970 23:59:59
        private DateTime FromStringToDateTime(string pattern)
        {
            var args = pattern.Split(' ');
            var dateArgs = args[0].Split('.').Select(int.Parse).ToArray();
            var timeArgs = args[1].Split(':').Select(int.Parse).ToArray();

            return new DateTime(dateArgs[2], dateArgs[1], dateArgs[0], timeArgs[0], timeArgs[1], timeArgs[2]);
        }
    }
}
