using NLog;
using wServer.core;
using wServer.core.worlds.logic;
using wServer.networking.packets;
using wServer.core.net.handlers;

namespace wServer.networking
{
    internal interface IPacketHandler
    {
        PacketId MessageId { get; }

        void Handle(Client client, IMessageHandler packet, ref TickTime time);
    }

    internal abstract class PacketHandlerBase<T> : IPacketHandler where T : IMessageHandler
    {
        protected static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public abstract PacketId MessageId { get; }

        public void Handle(Client client, IMessageHandler packet, ref TickTime time) => HandlePacket(client, (T)packet, ref time);

        protected abstract void HandlePacket(Client client, T packet, ref TickTime time);

    }
}
