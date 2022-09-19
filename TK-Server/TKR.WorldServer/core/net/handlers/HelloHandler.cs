using NLog;
using TKR.Shared;
using TKR.Shared.database;
using TKR.Shared.database.account;
using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.networking;
using TKR.WorldServer.networking.packets.outgoing;

namespace TKR.WorldServer.core.net.handlers
{

    public class HelloData
    {
        public string BuildVersion { get; set; }
        public int GameId { get; set; }
        public string GUID { get; set; }
        //public string Hash { get; set; }
        public byte[] Key { get; set; }
        public int KeyTime { get; set; }
        public string MapJSON { get; set; }
        public string Password { get; set; }

        public HelloData(NReader rdr)
        {
            BuildVersion = rdr.ReadUTF();
            GameId = rdr.ReadInt32();
            if (BuildVersion.Contains("-bot"))
            {
                BuildVersion = BuildVersion.Replace("-bot", "");
                GUID = rdr.ReadUTF();
                Password = rdr.ReadUTF();
            }
            else
            {
                GUID = RSA.Instance.Decrypt(rdr.ReadUTF());
                Password = RSA.Instance.Decrypt(rdr.ReadUTF());
            }
            KeyTime = rdr.ReadInt32();
            Key = rdr.ReadBytes(rdr.ReadInt16());
            MapJSON = rdr.Read32UTF();
        }
    }

    public class HelloHandler : IMessageHandler
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public override MessageId MessageId => MessageId.HELLO;

        public override void Handle(Client client, NReader rdr, ref TickTime tickTime)
        {
            var helloData = new HelloData(rdr);

            // validate connection eligibility and get acc info
            var acc = VerifyConnection(client, helloData);
            if (acc == null)
                return;

            var cManager = client.GameServer;
            var config = cManager.Configuration;

            var rank = new DbRank(acc.Database, acc.AccountId);

            // first check: admin server
            if (config.serverInfo.adminOnly && !rank.IsAdmin && !cManager.IsWhitelisted(acc.AccountId))
            {
                client.SendFailure("You must be whitelisted to join this server.", FailureMessage.MessageNoDisconnect);
                return;
            }

            // second check: supporter server
            if (config.serverSettings.supporterOnly && rank.Rank < RankingType.Supporter3)
            {
                client.SendFailure($"You must be a supporter join this server.", FailureMessage.MessageNoDisconnect);
                return;
            }

            // log ip
            cManager.Database.LogAccountByIp(client.IpAddress, acc.AccountId);
            acc.IP = client.IpAddress;
            acc.FlushAsync();

            client.Account = acc;
            client.Rank = rank;

            cManager.ConnectionManager.AddConnection(new ConnectionInfo(client, helloData));
        }

        private DbAccount VerifyConnection(Client client, HelloData helloData)
        {
            var cManager = client.GameServer;
            var config = cManager.Configuration;

#if !DEBUG
            var version = config.serverSettings.version;

            if (helloData.BuildVersion == null)
                return null;

            if (!version.Equals(helloData.BuildVersion))
            {
                client.SendFailure(version, FailureMessage.ClientUpdateNeeded);
                return null;
            }
#endif

            var db = cManager.Database;
            var s1 = db.Verify(helloData.GUID, helloData.Password, out DbAccount acc);

            if (s1 == DbLoginStatus.AccountNotExists)
            {
                client.SendFailure("Bad Login", FailureMessage.MessageWithDisconnect);
                return null;
            }

            if (s1 == DbLoginStatus.InvalidCredentials)
            {
                client.SendFailure("Email or Password is incorrect.", FailureMessage.MessageWithDisconnect);
                return null;
            }

            if (acc.Banned)
            {
                client.SendFailure("Account banned.", FailureMessage.MessageWithDisconnect);
                return null;
            }

            if (db.IsIpBanned(client.IpAddress))
            {
                client.SendFailure("IP banned.", FailureMessage.MessageWithDisconnect);
                return null;
            }

            return acc;
        }
    }
}
