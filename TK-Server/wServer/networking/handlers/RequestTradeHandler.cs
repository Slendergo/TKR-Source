using wServer.networking.packets;
using wServer.networking.packets.incoming;

namespace wServer.networking.handlers
{
    internal class RequestTradeHandler : PacketHandlerBase<RequestTrade>
    {
        public override PacketId ID => PacketId.REQUESTTRADE;

        protected override void HandlePacket(Client client, RequestTrade packet) => Handle(client, packet);

        private void Handle(Client client, RequestTrade packet)
        {
            if (client.Player == null || IsTest(client)) return;

            if (client.Player.Stars < 2 && client.Player.Rank < 10)
            {
                client.Player.SendHelp("To use this feature you need 2 stars or D-1 rank.");

                return;
            }

            client.Player.RequestTrade(packet.Name);
        }
    }
}
