namespace common.isc.data
{
    public sealed class ServerInfo
    {
        public string address { get; set; } = "127.0.0.1";
        public bool adminOnly { get; set; } = false;
        public int baseFameRequirement { get; set; } = 0;
        public string bindAddress { get; set; } = "127.0.0.1";
        public bool debug { get; set; } = false;
        public string instanceId { get; set; } = "";
        public float latitude { get; set; } = 0;
        public float longitude { get; set; } = 0;
        public int maxPlayers { get; set; } = 0;
        public string name { get; set; } = "Test Server";
        public PlayerList playerList { get; set; } = new PlayerList();
        public int players { get; set; } = 0;
        public int port { get; set; } = 2050;
        public int realmCount { get; set; } = 0;
        public ServerType type { get; set; } = ServerType.Account;

        public bool IsJustStarted() => players == 0 && maxPlayers == 0;
    }
}
