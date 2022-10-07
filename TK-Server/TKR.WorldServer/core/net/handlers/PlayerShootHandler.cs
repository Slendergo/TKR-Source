using TKR.Shared;
using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.networking;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.networking.packets.outgoing;
using TKR.WorldServer.core.miscfile.structures;
using System;
using TKR.WorldServer.core.worlds;
using TKR.WorldServer.logic;
using TKR.Shared.resources;

namespace TKR.WorldServer.core.net.handlers
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
            if (!player.GameServer.Resources.GameData.Items.TryGetValue((ushort)containerType, out _))
            {
                client.Disconnect("Attempting to shoot a invalid item", true);
                return;
            }

            var slot = -1;
            for (var i = 0; i < 2; i++)
                if (player.Inventory[i] != null && player.Inventory[i].ObjectType == containerType)
                {
                    slot = i;
                    break;
                }

            if (slot == -1)
            {
                client.Disconnect("Attempting to shoot a item that they dont have", true);
                return;
            }

            var newBulletId = player.GetNextBulletId();
            var clientBulletId = bulletId % 0xFF;
            if (newBulletId != clientBulletId)
            {
                System.Console.WriteLine($"DESYNC PROJECTILES: {player.Name} Class: {player.ObjectDesc.DisplayId ?? player.ObjectDesc.ObjectId}", true);
                return;
            }

            // todo rate of fire checks

            if (player.Inventory[slot] == null || player.Inventory[slot].ObjectType != containerType)
            {
                client.Disconnect($"Invalid item: {(slot == 0 ? "Weapon" : "Ability")} {player.Inventory[slot].ObjectType} != {containerType}", true);
                return;
            }

            if (slot == 0 && player.World.DisableShooting)
            {
                client.Disconnect("Attempting to shoot in a disabled world", true);
                return;
            }

            if (slot == 1 && player.World.DisableAbilities)
            {
                client.Disconnect("Attempting to activate ability in a disabled world", true);
                return;
            }

            var item = player.Inventory[slot];
            var prjDesc = item.Projectiles[0];
            var prj = player.PlayerShootProjectile(time, newBulletId, item.ObjectType, angle, startingPosition, prjDesc);
            player.World.AddProjectile(prj);

            var allyShoot = new AllyShoot(prj.ProjectileId, player.Id, item.ObjectType, angle);
            player.World.BroadcastIfVisibleExclude(allyShoot, player, player);
            player.FameCounter.Shoot();
        }
    }
}
