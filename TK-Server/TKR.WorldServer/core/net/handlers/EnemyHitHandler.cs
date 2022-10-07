using TKR.Shared;
using TKR.Shared.resources;
using System;
using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.networking;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.utils;

namespace TKR.WorldServer.core.net.handlers
{
    public class EnemyHitHandler : IMessageHandler
    {
        public override MessageId MessageId => MessageId.ENEMYHIT;

        public override void Handle(Client client, NReader rdr, ref TickTime tickTime)
        {
            var time = rdr.ReadInt32();
            var bulletId = rdr.ReadInt32();
            var targetId = rdr.ReadInt32();
            var killed = rdr.ReadBoolean();

            var player = client.Player;
            if (player.World == null)
                return;

            if (player.HasConditionEffect(ConditionEffectIndex.Hidden))
                return;

            var entity = player.World.GetEntity(targetId);
            if (entity == null)
                return;


            var projectile = player.World.GetProjectile(player.Id, bulletId);
            if (projectile == null)
            {
                Console.WriteLine($"[prj == null] {player.Id} -> {player.Name} | {bulletId}");
                return;
            }

            if (projectile.HasElapsed(player.C2STime(time)))
            {
                Console.WriteLine($"[HasElapsed] {player.Id} -> {player.Name} | {bulletId}");
                return;
            }

            if (!projectile.HasAlreadyHitTarget(targetId))
            {
                entity.HitByProjectile(projectile, ref tickTime);
                projectile.SuccessfulHit(targetId);
            }
            else
                Console.WriteLine($"[EnemyHit -> Projectile.TryHit] {player.Id} -> {player.Name} Already hit with projectile | Potential Client Modification");

            if (entity is StaticObject)
            {
                if (killed && (entity as StaticObject).HP > 0)
                    player.PlayerUpdate.ForceAdd(entity);
            }
            else if (entity is Enemy)
                 if (killed && (entity as Enemy).HP > 0)
                    player.PlayerUpdate.ForceAdd(entity);

            if (player.HasTalismanEffect(TalismanEffectType.InsatiableThirst))
            {
                var totalLife = 0.00;// player.Stats[0] * 0.1;
                var totalMana = player.Stats[1] * 0.01;

                if (totalLife > 0)
                    Player.HealDiscrete(player, (int)totalLife, false);
                if (totalMana > 0)
                    Player.HealDiscrete(player, (int)totalMana, true);
            }
        }
    }
}
