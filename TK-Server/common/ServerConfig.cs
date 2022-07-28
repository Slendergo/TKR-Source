using common.database;
using common.discord;
using common.isc.data;
using Newtonsoft.Json;
using System.IO;

namespace common
{
    public class ServerConfig
    {
        public DbInfo dbInfo { get; set; } = new DbInfo();
        public ServerInfo serverInfo { get; set; } = new ServerInfo();
        public ServerSettings serverSettings { get; set; } = new ServerSettings();
        public DiscordIntegration discordIntegration { get; set; } = new DiscordIntegration();

        public static ServerConfig ReadFile(string fileName)
        {
            using (var r = new StreamReader(fileName))
                return ReadJson(r.ReadToEnd());
        }

        public static ServerConfig ReadJson(string json) => JsonConvert.DeserializeObject<ServerConfig>(json);
    }
}
