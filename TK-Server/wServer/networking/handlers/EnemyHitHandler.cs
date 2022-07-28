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

        protected override void HandlePacket(Client client, EnemyHit packet) => client?.Player?.AddPendingAction(t => Handle(client.Player, t, packet));

        private void Handle(Player player, TickData time, EnemyHit pkt)
        {
            var entity = player?.Owner?.GetEntity(pkt.TargetId);

            if (entity?.Owner == null || entity.HasConditionEffect(ConditionEffects.Invulnerable))
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
