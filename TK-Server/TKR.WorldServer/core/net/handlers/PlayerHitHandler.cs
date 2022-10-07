using System;
using TKR.Shared;
using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.networking;

namespace TKR.WorldServer.core.net.handlers
{
    public class PlayerHitHandler : IMessageHandler
    {
        public override MessageId MessageId => MessageId.PLAYERHIT;

        public override void Handle(Client client, NReader rdr, ref TickTime tickTime)
        {
            var bulletId = rdr.ReadInt32();
            var objectId = rdr.ReadInt32();

            var player = client.Player;
            if (player?.World == null)
                return;

            var projectile = player.World.GetProjectile(objectId, bulletId);
            if (projectile == null)
            {
                Console.WriteLine($"[PlayerHit -> projectile == null] {player.Id} -> {player.Name} | {bulletId}");
                return;
            }

            if (projectile.HasElapsed(tickTime.TotalElapsedMs))
            {
                Console.WriteLine($"[PlayerHit -> HasElapsed] {player.Id} -> {player.Name} | {bulletId}");
                return;
            }

            if (projectile.HasAlreadyHitTarget(player.Id))
                Console.WriteLine($"[PlayerHit -> Projectile.TryHit] {player.Id} -> {player.Name} Already hit with projectile | Potential Client Modification");
            else
                player.HitByProjectile(projectile, ref tickTime);
            projectile.RegisterHit(player.Id);
        }
    }
}
