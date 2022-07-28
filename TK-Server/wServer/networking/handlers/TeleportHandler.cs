using wServer.core;
using wServer.core.objects;
using wServer.networking.packets;
using wServer.networking.packets.incoming;

namespace wServer.networking.handlers
{
    internal class TeleportHandler : PacketHandlerBase<Teleport>
    {
        public override PacketId ID => PacketId.TELEPORT;

        protected override void HandlePacket(Client client, Teleport packet)
        {
            var player = client.Player;

            if (player == null || player.Owner == null)
                return;

            player.AddPendingAction(t => Handle(client.Player, t, packet.ObjectId));
        }

        private void Handle(Player player, TickData time, int objId)
        {
            if (player == null || player.Owner == null)
                return;

            player.Teleport(time, objId);
        }
    }
}
