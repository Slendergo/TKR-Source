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

        protected override void HandlePacket(Client client, PlayerHit packet)
        {
            var player = client.Player;

            if (player == null || player.Owner == null)
                return;

            player.AddPendingAction(t => Handle(client.Player, t, packet.ObjectId, packet.BulletId));
        }

        private void Handle(Player player, TickData time, int objectId, byte bulletId)
        {
            if (player?.Owner == null)
                return;

            var entity = player.Owner.GetEntity(objectId);

            var prj = entity != null
                ? ((IProjectileOwner)entity).Projectiles[bulletId]
                : player.Owner.Projectiles.Where(p => p.Value.ProjectileOwner.Self.Id == objectId).SingleOrDefault(p => p.Value.ProjectileId == bulletId).Value;

            if (prj == null)
                return;

            prj?.ForceHit(player, time);
        }
    }
}
