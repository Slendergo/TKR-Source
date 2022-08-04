using common;
using System.Linq;
using wServer.core;
using wServer.core.objects;
using wServer.networking;

namespace wServer.core.net.handlers
{
    public class PlayerHitHandler : IMessageHandler
    {
        public override PacketId MessageId => PacketId.PLAYERHIT;

        public override void Handle(Client client, NReader rdr, ref TickTime tickTime)
        {
            var bulletId = rdr.ReadByte();
            var objectId = rdr.ReadInt32();

            var player = client.Player;
            if (player?.World == null)
                return;

            var entity = player.World.GetEntity(objectId);

            var prj = entity != null
                ? ((IProjectileOwner)entity).Projectiles[bulletId]
                : player.World.Projectiles.Where(p => p.Value.ProjectileOwner.Self.Id == objectId).SingleOrDefault(p => p.Value.ProjectileId == bulletId).Value;

            if (prj == null)
                return;

            prj?.ForceHit(player, tickTime);
        }
    }
}
