using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Text;
using System.Threading.Tasks;

namespace common.database.extension
{
    public static class RedisModelExtensions
    {
        public static void FromValue<T>(this byte[] buffer, T value)
        {
            if (value == null)
                return;

            if (typeof(T) == typeof(int) || typeof(T) == typeof(uint) || typeof(T) == typeof(ushort) || typeof(T) == typeof(string) || typeof(T) == typeof(float))
                buffer = Encoding.UTF8.GetBytes(value.ToString());
            else if (typeof(T) == typeof(bool))
                buffer = new byte[] { (byte)((bool)(object)value ? 1 : 0) };
            else if (typeof(T) == typeof(DateTime))
                buffer = BitConverter.GetBytes(((DateTime)(object)value).ToBinary());
            else if (typeof(T) == typeof(byte[]))
                buffer = (byte[])(object)value;
            else if (typeof(T) == typeof(ushort[]))
            {
                var v = (ushort[])(object)value;

                buffer = new byte[v.Length * 2];

                Buffer.BlockCopy(v, 0, buffer, 0, buffer.Length);
            }
            else if (typeof(T) == typeof(int[]) || typeof(T) == typeof(uint[]))
            {
                var v = (int[])(object)value;

                buffer = new byte[v.Length * 4];

                Buffer.BlockCopy(v, 0, buffer, 0, buffer.Length);
            }
            else if (typeof(T) == typeof(string[]))
                buffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value));
            else
                throw new NotSupportedException($"[WriteAsync] {typeof(RedisModelExtensions).Name} doesn't implements support for following type: {typeof(T).Name}");
        }

        public static async Task<T> ReadAsync<T>(this ITransaction transaction, string key, string field, T def = default)
        {
            var buffer = (byte[])await transaction.HashGetAsync(key, field);

            return buffer.ToValue(def);
        }

        public static async Task<T> ReadAsync<T>(this IDatabase db, string key, string field, T def = default)
        {
            var buffer = (byte[])await db.HashGetAsync(key, field);

            return buffer.ToValue(def);
        }

        public static async Task WriteAsync<T>(this ITransaction transaction, string key, string field, T value)
        {
            var buffer = new byte[0];
            buffer.FromValue(value);

            await transaction.HashSetAsync(key, field, buffer, flags: CommandFlags.FireAndForget);
        }

        public static async Task WriteAsync<T>(this IDatabase db, string key, string field, T value)
        {
            var buffer = new byte[0];
            buffer.FromValue(value);

            await db.HashSetAsync(key, field, buffer, flags: CommandFlags.FireAndForget);
        }

        private static T ToValue<T>(this byte[] buffer, T def = default)
        {
            if (buffer == null || buffer.Length == 0)
                return def;

            if (typeof(T) == typeof(int))
                try { return (T)(object)int.Parse(Encoding.UTF8.GetString(buffer)); }
                catch (OverflowException) { return (T)(object)int.MaxValue; }

            if (typeof(T) == typeof(uint))
                try { return (T)(object)uint.Parse(Encoding.UTF8.GetString(buffer)); }
                catch (OverflowException) { return (T)(object)uint.MaxValue; }

            if (typeof(T) == typeof(ushort))
                try { return (T)(object)ushort.Parse(Encoding.UTF8.GetString(buffer)); }
                catch (OverflowException) { return (T)(object)ushort.MaxValue; }

            if (typeof(T) == typeof(float))
                try { return (T)(object)float.Parse(Encoding.UTF8.GetString(buffer)); }
                catch (OverflowException) { return (T)(object)float.MaxValue; }

            if (typeof(T) == typeof(bool))
                return (T)(object)(buffer[0] != 0);

            if (typeof(T) == typeof(DateTime))
                return (T)(object)DateTime.FromBinary(BitConverter.ToInt64(buffer, 0));

            if (typeof(T) == typeof(byte[]))
                return (T)(object)buffer;

            if (typeof(T) == typeof(ushort[]))
            {
                var ret = new ushort[buffer.Length / 2];

                Buffer.BlockCopy(buffer, 0, ret, 0, buffer.Length);

                return (T)(object)ret;
            }

            if (typeof(T) == typeof(int[]) || typeof(T) == typeof(uint[]))
            {
                var ret = new int[buffer.Length / 4];

                Buffer.BlockCopy(buffer, 0, ret, 0, buffer.Length);

                return (T)(object)ret;
            }

            if (typeof(T) == typeof(string))
                return (T)(object)Encoding.UTF8.GetString(buffer);

            if (typeof(T) == typeof(string[]))
                return (T)(object)JsonConvert.SerializeObject(buffer);

            throw new NotSupportedException($"[ReadAsync] {typeof(RedisModelExtensions).Name} doesn't implements support for following type: {typeof(T).Name}");
        }
    }
}
