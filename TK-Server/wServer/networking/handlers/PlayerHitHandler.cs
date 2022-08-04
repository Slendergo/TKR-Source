using System.Linq;
using wServer.core;
using wServer.core.objects;
using wServer.networking.packets;
using wServer.networking.packets.incoming;

namespace wServer.networking.handlers
{
    internal class PlayerHitHandler : PacketHandlerBase<PlayerHit>
    {
        public override PacketId ID => PacketId.PLAYERHIT;

        protected override void HandlePacket(Client client, PlayerHit packet, ref TickTime time)
        {
            var player = client.Player;

            if (player == null || player.World == null)
                return;

            Handle(client.Player, time, packet.ObjectId, packet.BulletId);
        }

        private void Handle(Player player, TickTime time, int objectId, byte bulletId)
        {
            if (player?.World == null)
                return;

            var entity = player.World.GetEntity(objectId);

            var prj = entity != null
                ? ((IProjectileOwner)entity).Projectiles[bulletId]
                : player.World.Projectiles.Where(p => p.Value.ProjectileOwner.Self.Id == objectId).SingleOrDefault(p => p.Value.ProjectileId == bulletId).Value;

            if (prj == null)
                return;

            prj?.ForceHit(player, time);
        }
    }
}
