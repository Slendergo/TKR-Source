using wServer.core;
using wServer.networking.packets;
using wServer.networking.packets.incoming;

namespace wServer.networking.handlers
{
    internal class PongHandler : PacketHandlerBase<Pong>
    {
        public override PacketId ID => PacketId.PONG;

        protected override void HandlePacket(Client client, Pong packet, ref TickTime time) => client?.Player?.Pong(time, packet);
    }
}
