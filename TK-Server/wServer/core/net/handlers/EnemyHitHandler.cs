using common;
using common.resources;
using wServer.core.objects;
using wServer.networking;

namespace wServer.core.net.handlers
{
    public class EnemyHitHandler : IMessageHandler
    {
        public override PacketId MessageId => PacketId.ENEMYHIT;

        public override void Handle(Client client, NReader rdr, ref TickTime tickTime)
        {
            var time = rdr.ReadInt32();
            var bulletId = rdr.ReadByte();
            var targetId = rdr.ReadInt32();
            var killed = rdr.ReadBoolean();
            var itemType = rdr.ReadUInt16();


            var player = client.Player;
            var entity = player?.World?.GetEntity(targetId);

            if (entity?.World == null || entity.HasConditionEffect(ConditionEffects.Invulnerable))
                return;

            if (player.HasConditionEffect(ConditionEffects.Hidden))
                return;

            var prj = (player as IProjectileOwner).Projectiles[bulletId];

            if (prj == null)
                return;

            prj?.ForceHit(entity, tickTime);
        }
    }
}
