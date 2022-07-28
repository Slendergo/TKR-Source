using wServer.networking.packets;
using wServer.networking.packets.incoming;

namespace wServer.networking.handlers
{
    internal class OptionsHandler : PacketHandlerBase<Options>
    {
        public override PacketId ID => PacketId.OPTIONS;

        protected override void HandlePacket(Client client, Options packet) => client.DisableAllyShoot = packet.AllyShots;
    }
}
