using common.resources;
using NLog;
using System;
using wServer.core.objects;
using wServer.networking.packets;
using wServer.networking.packets.incoming;
using wServer.networking.packets.outgoing;

namespace wServer.networking.handlers
{
    internal class PlayerShootHandler : PacketHandlerBase<PlayerShoot>
    {
        public override PacketId ID => PacketId.PLAYERSHOOT;

        protected override void HandlePacket(Client client, PlayerShoot packet) 
        {
            var player = client.Player;

            if (!player.CoreServerManager.Resources.GameData.Items.TryGetValue(packet.ContainerType, out Item item))
            {
                player.DropNextRandom();
                return;
            }

            if (item == player.Inventory[1])
                return; // ability shoot handled by useitem

            // validate
            var result = player.ValidatePlayerShoot(item, packet.Time);
            if (result != PlayerShootStatus.OK)
            {
                //CheatLog.Info($"PlayerShoot validation failure ({player.Name}:{player.AccountId}): {result}");
                player.DropNextRandom();
                return;
            }

            // create projectile and show other players
            var prjDesc = item.Projectiles[0]; //Assume only one
            var prj = player.PlayerShootProjectile(packet.BulletId, prjDesc, item.ObjectType, packet.Time, packet.StartingPos, packet.Angle);

            player.Owner.EnterWorld(prj);
            player.Owner.BroadcastIfVisibleExclude(new AllyShoot()
            {
                OwnerId = player.Id,
                Angle = packet.Angle,
                ContainerType = packet.ContainerType,
                BulletId = packet.BulletId
            }, player, player, PacketPriority.Low);
            player.FameCounter.Shoot(prj);
        }
    }
}
