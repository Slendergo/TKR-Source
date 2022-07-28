using common;
using common.database;
using System;
using System.Linq;
using wServer.networking;
using wServer.networking.packets.incoming;

namespace wServer.core
{
    public class ConnectionInfo : IEquatable<ConnectionInfo>
    {
        public DbAccount Account;
        public Client Client;
        public string GUID;
        public byte[] Key;
        public string MapInfo;

        public ConnectionInfo(Client client, Hello hello)
        {
            Client = client;
            Account = client.Account;
            GUID = hello.GUID;
            GameId = hello.GameId;
            Key = hello.Key;
            Reconnecting = !Key.SequenceEqual(Empty<byte>.Array);
            MapInfo = hello.MapJSON;
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
