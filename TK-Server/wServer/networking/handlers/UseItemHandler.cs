using wServer.core;
using wServer.core.objects;
using wServer.networking.packets;
using wServer.networking.packets.incoming;

namespace wServer.networking.handlers
{
    internal class UseItemHandler : PacketHandlerBase<UseItem>
    {
        public override PacketId ID => PacketId.USEITEM;

        protected override void HandlePacket(Client client, UseItem packet, ref TickTime time) => Handle(client.Player, time, packet);

        private void Handle(Player player, TickTime time, UseItem packet)
        {
            if (player?.World == null)
                return;

            player.UseItem(time, packet.SlotObject.ObjectId, packet.SlotObject.SlotId, packet.ItemUsePos, packet.SellMaxed);
        }
    }
}
