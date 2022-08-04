using wServer.core;
using wServer.networking.packets;
using wServer.networking.packets.incoming;

namespace wServer.networking.handlers
{
    internal class CancelTradeHandler : PacketHandlerBase<CancelTrade>
    {
        public override PacketId ID => PacketId.CANCELTRADE;

        protected override void HandlePacket(Client client, CancelTrade packet, ref TickTime time)
        {
            Handle(client, packet);
        }

        private void Handle(Client client, CancelTrade packet)
        {
            var player = client.Player;
            if (player == null || IsTest(client))
                return;

            player.CancelTrade();
        }
    }
}
