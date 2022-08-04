using wServer.core;
using wServer.core.objects;
using wServer.networking.packets;
using wServer.networking.packets.incoming;

namespace wServer.networking.handlers
{
    internal class TeleportHandler : PacketHandlerBase<Teleport>
    {
        public override PacketId ID => PacketId.TELEPORT;

        protected override void HandlePacket(Client client, Teleport packet, ref TickTime time)
        {
            var player = client.Player;

            if (player == null || player.World == null)
                return;

            Handle(client.Player, time, packet.ObjectId);
        }

        private void Handle(Player player, TickTime time, int objId)
        {
            if (player == null || player.World == null)
                return;

            player.Teleport(time, objId);
        }
    }
}
