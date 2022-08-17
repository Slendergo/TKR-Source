using common;
using common.database;
using wServer.core;
using wServer.core.objects;
using wServer.core.objects.vendors;
using wServer.core.worlds.logic;
using wServer.networking;
using wServer.networking.packets;
using wServer.core.net.handlers;
using wServer.networking.packets.outgoing;

namespace wServer.core.net.handlers
{
    public sealed class BuyMessageHandler : IMessageHandler
    {
        public override MessageId MessageId => MessageId.BUY;

        public override void Handle(Client client, NReader rdr, ref TickTime time)
        {
            var objectId = rdr.ReadInt32();

            var player = client.Player;
            if (player?.World == null)
                return;

            var obj = player.World.GetEntity(objectId) as SellableObject;
            obj?.Buy(player);
        }
    }
}
