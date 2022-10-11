using TKR.Shared;
using TKR.Shared.database;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.core.worlds.logic;
using TKR.WorldServer.networking.packets;
using TKR.WorldServer.networking.packets.outgoing;
using TKR.WorldServer.core.objects.vendors;
using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.networking;

namespace TKR.WorldServer.core.net.handlers
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

            if (obj == null)
                player.Client.SendPacket(new BuyResultMessage
                {
                    Result = 1,
                    ResultString = $"Purchase Error: {BuyResult.TransactionFailed.GetDescription()}"
                });
        }
    }
}
