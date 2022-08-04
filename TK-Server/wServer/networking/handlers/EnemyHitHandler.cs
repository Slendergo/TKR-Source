using common.resources;
using wServer.core;
using wServer.core.objects;
using wServer.networking.packets;
using wServer.networking.packets.incoming;

namespace wServer.networking.handlers
{
    internal class EnemyHitHandler : PacketHandlerBase<EnemyHit>
    {
        public override PacketId ID => PacketId.ENEMYHIT;

        protected override void HandlePacket(Client client, EnemyHit packet, ref TickTime time) => Handle(client.Player, time, packet);

        private void Handle(Player player, TickTime time, EnemyHit pkt)
        {
            var entity = player?.World?.GetEntity(pkt.TargetId);

            if (entity?.World == null || entity.HasConditionEffect(ConditionEffects.Invulnerable))
                return;

            if (player.HasConditionEffect(ConditionEffects.Hidden))
                return;

            var prj = (player as IProjectileOwner).Projectiles[pkt.BulletId];

            if (prj == null)
                return;

            prj?.ForceHit(entity, time);
        }
    }
}
