using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace tk.bot
{
    public static class Extensions
    {
        public static string BotName(this IConfigurationRoot config) => config["bot-name"];

        public static string ComputeTokenHash(this string token)
        {
            var sha1 = new SHA1Managed();
            var buffer = Encoding.UTF8.GetBytes(token);

            sha1.ComputeHash(buffer);

            var sb = new StringBuilder(sha1.HashSize * 2);

            foreach (var b in sha1.Hash)
                sb.Append($"{b:x2}");

            return sb.ToString();
        }

        public static void ConcatArray(this IConfigurationRoot config, string key)
        {
            var sb = new StringBuilder();
            var sections = config.GetSection(key).GetChildren().ToArray();

            for (var i = 0; i < sections.Length; i++)
                sb.Append($"{sections[i].Value}{(i + 1 == sections.Length ? "" : "§")}");

            config[key] = sb.ToString();
        }

        public static int CountRolesFromString(this SocketGuildUser user, string roles) => user.Roles.Count(role => roles.Contains(role.Name));

        public static string GenerateIndexes(this string array) => string.Join("§", Enumerable.Range(1, array.Split("§").Length));

        public static T GetAttribute<T>(this XElement e, string n, T def = default)
        {
            if (e.Attribute(n) == null) return def;

            var val = e.Attribute(n).Value;
            var t = typeof(T);

            if (t == typeof(string)) return (T)Convert.ChangeType(val, t);
            else if (t == typeof(ushort)) return (T)Convert.ChangeType(Convert.ToUInt16(val, 16), t);
            else if (t == typeof(int)) return (T)Convert.ChangeType(GetInt(val), t);
            else if (t == typeof(uint)) return (T)Convert.ChangeType(Convert.ToUInt32(val, 16), t);
            else if (t == typeof(double)) return (T)Convert.ChangeType(double.Parse(val, CultureInfo.InvariantCulture), t);
            else if (t == typeof(float)) return (T)Convert.ChangeType(float.Parse(val, CultureInfo.InvariantCulture), t);
            else if (t == typeof(bool)) return (T)Convert.ChangeType(string.IsNullOrWhiteSpace(val) || bool.Parse(val), t);

            Console.WriteLine($"Type of {t} is not supported by this method, returning default value: {def}...");

            return def;
        }

        public static int GetInt(string x) => x.Contains("x") ? Convert.ToInt32(x, 16) : int.Parse(x);

        public static T GetValue<T>(this XElement e, string n, T def = default)
        {
            if (e.Element(n) == null)
                return def;

            var val = e.Element(n).Value;
            var t = typeof(T);

            if (t == typeof(string)) return (T)Convert.ChangeType(val, t);
            else if (t == typeof(ushort)) return (T)Convert.ChangeType(Convert.ToUInt16(val, 16), t);
            else if (t == typeof(int)) return (T)Convert.ChangeType(GetInt(val), t);
            else if (t == typeof(uint)) return (T)Convert.ChangeType(Convert.ToUInt32(val, 16), t);
            else if (t == typeof(double)) return (T)Convert.ChangeType(double.Parse(val, CultureInfo.InvariantCulture), t);
            else if (t == typeof(float)) return (T)Convert.ChangeType(float.Parse(val, CultureInfo.InvariantCulture), t);
            else if (t == typeof(bool)) return (T)Convert.ChangeType(string.IsNullOrWhiteSpace(val) || bool.Parse(val), t);

            Console.WriteLine($"Type of {t} is not supported by this method, returning default value: {def}...");

            return def;
        }

        public static bool HasAttribute(this XElement e, string name) => e.Attribute(name) != null;

        public static bool HasElement(this XElement e, string name) => e.Element(name) != null;

        public static bool HasRole(this SocketGuildUser user, string name) => user.Roles.Any(role => role.Name.Equals(name));

        public static bool IsAdmin(this SocketGuildUser user, IConfigurationRoot config) => user.Roles.Any(role => config["admin-roles"].Contains(role.Name));

        public static SocketTextChannel TextChannel(this DiscordSocketClient discord, ulong guildId, ulong channelId) => discord.GetGuild(guildId).GetTextChannel(channelId);

        public static string ToQuery(this IDictionary<string, string> collection, string address)
        {
            var sb = new StringBuilder($"{address}?");
            var i = 0;

            foreach (var entry in collection)
                sb.Append($"{entry.Key}={entry.Value}{(++i != collection.Count ? "&" : "")}");

            return sb.ToString();
        }

        public static ulong ToUnsignedLong(this string text) => ulong.Parse(text);
    }
}
