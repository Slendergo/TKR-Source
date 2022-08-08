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
        public int GameId { get; }
        public bool Reconnecting { get; }
        public long Time { get; }

        public DbAccount Account { get; }
        public Client Client { get; }
        public string GUID { get; }
        public byte[] Key { get; }
        public string MapInfo { get; }

        public ConnectionInfo(Client client, HelloData data)
        {
            Client = client;
            Account = client.Account;
            GUID = data.GUID;
            GameId = data.GameId;
            Key = data.Key;
            Reconnecting = !Key.SequenceEqual(ArrayUtils<byte>.Empty);
            MapInfo = data.MapJSON;
            Time = DateTime.UtcNow.ToUnixTimestamp();
        }

        public bool Equals(ConnectionInfo other) => GUID.Equals(other.GUID);

        public override bool Equals(object obj) => obj != null && obj is ConnectionInfo p && Equals(p);

        public override int GetHashCode() => GUID.GetHashCode();
    }
}
