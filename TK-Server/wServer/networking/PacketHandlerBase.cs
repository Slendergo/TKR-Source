using NLog;
using wServer.core.worlds.logic;
using wServer.networking.packets;
using wServer.networking.packets.incoming;

namespace wServer.networking
{
    internal interface IPacketHandler
    {
        PacketId ID { get; }

        void Handle(Client client, IncomingMessage packet);
    }

    internal abstract class PacketHandlerBase<T> : IPacketHandler where T : IncomingMessage
    {
        protected static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public abstract PacketId ID { get; }

        public void Handle(Client client, IncomingMessage packet) => HandlePacket(client, (T)packet);

        protected abstract void HandlePacket(Client client, T packet);

        protected bool IsAvailable(Client client)
        {
            if (client == null || client.Account == null || client.Player == null || client.Player.Owner == null || IsTest(client))
                return false;

            return true;
        }

        protected bool IsEnabledOrIsVipMarket(Client client)
        {
            var config = Program.CoreServerManager.ServerConfig;

            if (!config.serverSettings.marketEnabled)
            {
                client.Player.SendError("Market not Enabled.");
                return false;
            }

            if (config.serverInfo.adminOnly)
            {
                if (!Program.CoreServerManager.IsWhitelisted(client.Player.AccountId) || client.Player?.Rank < 110)
                {
                    client.Player.SendError("Admin Only, you need to be Whitelisted to use this.");
                    return false;
                }
            }
            else
            {
                if (!client.Player.CanUseThisFeature(core.objects.Player.GenericRank.VIP))
                {
                    client.Player.SendError("You can't use this Feature.");
                    return false;
                }
            }

            return true;
        }

        protected bool IsTest(Client cli) => cli?.Player?.Owner is Test;
    }
}
