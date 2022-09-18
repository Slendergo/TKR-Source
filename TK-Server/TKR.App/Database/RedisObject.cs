using Newtonsoft.Json;
using StackExchange.Redis;
using System.Text;

namespace TKR.App.Database
{
    public struct Entry
    {
        public byte[] Data;
        public bool NeedsUpdate;

        public Entry(byte[] data, bool needsUpdate)
        {
            Data = data;
            NeedsUpdate = needsUpdate;
        }
    }

    public abstract class RedisObject
    {
        public string Key { get; private set; }

        private Dictionary<RedisValue, Entry> Entries = new Dictionary<RedisValue, Entry>();

        public IDatabase Database { get; private set; }

        public bool HasKeys => Entries.Count != 0;

        protected void Init(IDatabase db, string key)
        {
            Key = key;
            Database = db;

            Entries = db.HashGetAll(key).ToDictionary(x => x.Name, x => new Entry(x.Value, false));
        }

        public T GetValue<T>(RedisValue key, T def = default)
        {
            if (key.IsNullOrEmpty || !Entries.TryGetValue(key, out Entry val) || val.Data == null)
                return def;

            var data = val.Data;

            var type = typeof(T);

            if (type == typeof(int))
                try { return (T)(object)int.Parse(Encoding.UTF8.GetString(data)); }
                catch (OverflowException) { return (T)(object)int.MaxValue; }

            if (type == typeof(uint))
                try { return (T)(object)uint.Parse(Encoding.UTF8.GetString(data)); }
                catch (OverflowException) { return (T)(object)uint.MaxValue; }

            if (type == typeof(ushort))
                try { return (T)(object)ushort.Parse(Encoding.UTF8.GetString(data)); }
                catch (OverflowException) { return (T)(object)ushort.MaxValue; }

            if (type == typeof(float))
                try { return (T)(object)float.Parse(Encoding.UTF8.GetString(data)); }
                catch (OverflowException) { return (T)(object)float.MaxValue; }

            if (type == typeof(bool))
                return (T)(object)(data[0] != 0);

            if (type == typeof(DateTime))
                return (T)(object)DateTime.FromBinary(BitConverter.ToInt64(data, 0));

            if (type == typeof(byte[]))
                return (T)(object)data;

            if (type == typeof(ushort[]))
            {
                var ret = new ushort[data.Length / 2];
                Buffer.BlockCopy(data, 0, ret, 0, data.Length);
                return (T)(object)ret;
            }

            if (type == typeof(int[]) || type == typeof(uint[]))
            {
                var ret = new int[data.Length / 4];
                Buffer.BlockCopy(data, 0, ret, 0, data.Length);
                return (T)(object)ret;
            }

            if (type == typeof(string))
                return (T)(object)Encoding.UTF8.GetString(data);

            if (type == typeof(string[]))
                return (T)(object)JsonConvert.SerializeObject(data);

            throw new NotSupportedException();
        }

        public void SetValue<T>(RedisValue key, T val)
        {
            if (val == null)
                return;

            byte[] buff;

            var type = typeof(T);

            if (type == typeof(byte)
               || type == typeof(int)
               || type == typeof(uint)
               || type == typeof(ushort)
               || type == typeof(string)
               || type == typeof(float)
               || type == typeof(long)
               || type == typeof(ulong)
               || type == typeof(double))
                buff = Encoding.UTF8.GetBytes(val.ToString());
            else if (type == typeof(bool))
                buff = new byte[] { (byte)((bool)(object)val ? 1 : 0) };
            else if (type == typeof(DateTime))
                buff = BitConverter.GetBytes(((DateTime)(object)val).ToBinary());
            else if (type == typeof(byte[]))
                buff = (byte[])(object)val;
            else if (type == typeof(ushort[]))
            {
                var v = (ushort[])(object)val;

                buff = new byte[v.Length * 2];

                Buffer.BlockCopy(v, 0, buff, 0, buff.Length);
            }
            else if (type == typeof(int[]) || type == typeof(uint[]))
            {
                var v = (int[])(object)val;

                buff = new byte[v.Length * 4];

                Buffer.BlockCopy(v, 0, buff, 0, buff.Length);
            }
            else if (type == typeof(string[]))
                buff = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(val));
            else
                throw new NotSupportedException();

            if (!Entries.ContainsKey(Key) || Entries[Key].Data == null || !buff.SequenceEqual(Entries[Key].Data))
                Entries[key] = new Entry(buff, true);
        }

        public void Reload() => Entries = Database.HashGetAll(Key).ToDictionary(x => x.Name, x => new Entry(x.Value, false));

        public void ReloadKey(string field) => Entries[field] = new Entry(Database.HashGet(Key, field), false);

        public Task FlushAsync()
        {
            var updated = Update();
            return Database.HashSetAsync(Key, updated);
        }

        public Task FlushTransactionAsync(ITransaction transaction)
        {
            var updated = Update();
            return transaction.HashSetAsync(Key, updated);
        }

        private HashEntry[] Update()
        {
            var updated = new List<HashEntry>();
            foreach (var name in Entries.Keys.ToList())
            {
                var needUpdate = Entries[name].NeedsUpdate;
                if (needUpdate)
                    updated.Add(new HashEntry(name, needUpdate));
            }
            foreach (var update in updated)
                Entries[update.Name] = new Entry(Entries[update.Name].Data, false);
            return updated.ToArray();
        }
    }
}
