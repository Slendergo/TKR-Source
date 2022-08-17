using common;
using System.Linq;
using wServer.core;
using wServer.core.objects;
using wServer.networking;

namespace wServer.core.net.handlers
{
    public class PlayerHitHandler : IMessageHandler
    {
        public override MessageId MessageId => MessageId.PLAYERHIT;

        public override void Handle(Client client, NReader rdr, ref TickTime tickTime)
        {
            var bulletId = rdr.ReadByte();
            var objectId = rdr.ReadInt32();

            var player = client.Player;
            if (player?.World == null)
                return;

            var prj = player.World.GetProjectile(objectId, bulletId);
            if (prj == null)
                return;
            prj?.ForceHit(player, tickTime);
        }
    }
}
