using common;
using common.database;
using System;
using System.Linq;
using wServer.networking;
using wServer.core.net.handlers;

namespace wServer.core
{
    public class ConnectionInfo : IEquatable<ConnectionInfo>
    {
        public DbAccount Account;
        public Client Client;
        public string GUID;
        public byte[] Key;
        public string MapInfo;

        public ConnectionInfo(Client client, HelloData data)
        {
            Client = client;
            Account = client.Account;
            GUID = data.GUID;
            GameId = data.GameId;
            Key = data.Key;
            Reconnecting = !Key.SequenceEqual(Empty<byte>.Array);
            MapInfo = data.MapJSON;
            Time = DateTime.UtcNow.ToUnixTimestamp();
        }

        public int GameId { get; }
        public bool Reconnecting { get; }
        public long Time { get; }

        public bool Equals(ConnectionInfo other) => GUID.Equals(other.GUID);

        public override bool Equals(object obj) => obj == null ? false : obj is ConnectionInfo p ? Equals(p) : false;

        public override int GetHashCode() => GUID.GetHashCode();
    }
}
