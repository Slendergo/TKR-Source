#define PRIVATE_TESTING

using common;
using common.database;
using NLog;
using System.Linq;
using wServer.core;
using wServer.networking.packets;
using wServer.networking.packets.incoming;
using wServer.networking.packets.outgoing;

namespace wServer.networking.handlers
{
    internal class HelloHandler : PacketHandlerBase<Hello>
    {
        private const int minDonorRank = 10;

        private new static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public override PacketId ID => PacketId.HELLO;

        protected override void HandlePacket(Client client, Hello packet) => Handle(client, packet);

        private void Handle(Client client, Hello packet)
        {
            // validate connection eligibility and get acc info
            var acc = VerifyConnection(client, packet);

            if (acc == null)
                return;

            var cManager = Program.CoreServerManager;
            var config = cManager.ServerConfig;

            // first check: admin server
            if (config.serverInfo.adminOnly && !acc.Admin && !cManager.IsWhitelisted(acc.AccountId))
            {
                client.SendFailure("You must be whitelisted to join this server.", Failure.MessageNoDisconnect);
                return;
            }

            // second check: donor server
            if (config.serverSettings.donorOnly && acc.Rank < minDonorRank)
            {
                client.SendFailure($"You must be with rank {minDonorRank} on your account to jooin this server.", Failure.MessageNoDisconnect);
                return;
            }

            // log ip
            cManager.Database.LogAccountByIp(client.IpAddress, acc.AccountId);
            acc.IP = client.IpAddress;
            acc.FlushAsync();

            client.Account = acc;

            cManager.ConnectionManager.AddConnection(new ConnectionInfo(client, packet));
        }

        private DbAccount VerifyConnection(Client client, Hello packet)
        {
            var cManager = Program.CoreServerManager;
            var config = cManager.ServerConfig;

#if !DEBUG
            var version = config.serverSettings.version;

            if (packet.BuildVersion == null)
                return null;

            if (!version.Equals(packet.BuildVersion))
            {
                client.SendFailure(version, Failure.ClientUpdateNeeded);
                return null;
            }
#endif

            var db = cManager.Database;
            var s1 = db.Verify(packet.GUID, packet.Password, out DbAccount acc);

            if (s1 == DbLoginStatus.AccountNotExists)
            {
                var s2 = db.Register(packet.GUID, packet.Password, true, out acc);

                if (s2 != RegisterStatus.OK)
                {
                    client.SendFailure("Bad Login", Failure.MessageWithDisconnect);
                    return null;
                }
            }
            if (s1 == DbLoginStatus.InvalidCredentials)
            {
                client.SendFailure("Email or Password is incorrect.", Failure.MessageWithDisconnect);
                return null;
            }

            if (acc.Banned)
            {
                client.SendFailure("Account banned.", Failure.MessageWithDisconnect);
                return null;
            }

            if (db.IsIpBanned(client.IpAddress))
            {
                client.SendFailure("IP banned.", Failure.MessageWithDisconnect);
                return null;
            }

            return acc;
        }
    }
}
