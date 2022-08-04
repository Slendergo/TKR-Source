using common;
using wServer.core;
using wServer.networking;

namespace wServer.core.net.handlers
{
    public class TeleportHandler : IMessageHandler
    {
        public override PacketId MessageId => PacketId.TELEPORT;

        public override void Handle(Client client, NReader rdr, ref TickTime tickTime)
        {
            var objectId = rdr.ReadInt32();

            var player = client.Player;
            if (player == null || player.World == null)
                return;

            player.Teleport(tickTime, objectId);
        }
    }
}
