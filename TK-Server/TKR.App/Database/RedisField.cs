namespace TKR.App.Database
{
    public sealed class RedisField<T>
    {
        private RedisObject redisObject;
        private string key;

        public T Value
        {
            get => redisObject.GetValue<T>(key);
            set => redisObject.SetValue(key, value);
        }

        public void Reload() => redisObject.ReloadKey(key);

        private RedisField() { }

        public static RedisField<T> Create(RedisObject redisObject, string key)
        {
            var redisField = new RedisField<T>()
            {
                redisObject = redisObject,
                key = key
            };
            return redisField;
        }
    }
}
