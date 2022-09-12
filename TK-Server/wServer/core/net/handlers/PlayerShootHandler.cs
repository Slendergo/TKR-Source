using common;
using wServer.core;
using wServer.core.objects;
using wServer.networking;
using wServer.networking.packets.outgoing;

namespace wServer.core.net.handlers
{
    public class PlayerShootHandler : IMessageHandler
    {
        public override MessageId MessageId => MessageId.PLAYERSHOOT;

        public override void Handle(Client client, NReader rdr, ref TickTime tickTime)
        {
            var time = rdr.ReadInt32();
            var bulletId = rdr.ReadInt32();
            var containerType = rdr.ReadInt32();
            var startingPosition = Position.Read(rdr);
            var angle = rdr.ReadSingle();

            var player = client.Player;
            if (!player.GameServer.Resources.GameData.Items.TryGetValue((ushort)containerType, out var item))
            {
                client.Disconnect("Attempting to shoot a invalid item");
                return;
            }

            var hasItemType = false;
            for(var i = 0; i < player.Inventory.Length; i++)
                if (player.Inventory[i] != null && player.Inventory[i].ObjectType == containerType)
                {
                    hasItemType = true;
                    break;
                }

            if (!hasItemType)
            {
                client.Disconnect("Attempting to shoot a item that they dont have");
                return;
            }

            if (item.ObjectType == player.Inventory[1].ObjectType)
            {
                if (player.World.DisableAbilities)
                    client.Disconnect("Attempting to activate ability in a disabled world");
                return; // todo
            }

            if (player.World.DisableShooting)
            {
                client.Disconnect("Attempting to shoot in a disabled world");
                return;
            }

            if(player.IsValidShoot(time, item.RateOfFire))
            {
                client.Disconnect("Attempting to shoot a item that they dont have");
                return;
            }

            var arcGap = item.ArcGap;
            for (var i = 0; i < item.NumProjectiles; i++)
            {
                var newBulletId = player.GetNextBulletId();
                var clientBulletId = (bulletId + i) % (0xFFFF - 0xFF);
                if (newBulletId != clientBulletId)
                {
                    client.Disconnect("bullet id desync");
                    return;
                }

                var prjDesc = item.Projectiles[0];
                var prj = player.PlayerShootProjectile(time, newBulletId, item.ObjectType, angle + arcGap * i, startingPosition, prjDesc);
                player.World.BroadcastIfVisibleExclude(new AllyShoot()
                {
                    OwnerId = player.Id,
                    Angle = prj.Angle,
                    ContainerType = item.ObjectType,
                    BulletId = prj.BulletId
                }, player, player);
                player.FameCounter.Shoot();
            }
        }
    }
}
