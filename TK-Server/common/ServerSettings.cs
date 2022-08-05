using System;
using System.Linq;

namespace common
{
    public class ServerSettings
    {
        public bool donorOnly { get; set; } = false;
        public string logFolder { get; set; } = "undefined";
        public int maxConnections { get; set; } = 0;
        public int maxPlayers { get; set; } = 0;
        public int realms { get; set; } = 2;
        public string resourceFolder { get; set; } = "undefined";
        public int restartTime { get; set; } = 0;
        public string version { get; set; } = "undefined";
        public int[] whitelist { get; set; } = new int[] { };
    }
}
