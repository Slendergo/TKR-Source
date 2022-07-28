using wServer.core.objects;
using wServer.core.objects.vendors;
using wServer.networking.packets;
using wServer.networking.packets.incoming;

namespace wServer.networking.handlers
{
    internal class BuyHandler : PacketHandlerBase<Buy>
    {
        public override PacketId ID => PacketId.BUY;

        protected override void HandlePacket(Client client, Buy packet)
        {
            //client.Manager.Logic.AddPendingAction(t => Handle(client.Player, packet.ObjectId));
            Handle(client.Player, packet.ObjectId);
        }

        private void Handle(Player player, int objId)
        {
            if (player?.Owner == null)
                return;

            var obj = player.Owner.GetEntity(objId) as SellableObject;
            obj?.Buy(player);
        }
    }
}
