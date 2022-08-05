using common;
using wServer.core;
using wServer.core.objects;
using wServer.networking;
using wServer.networking.packets.outgoing;

namespace wServer.core.net.handlers
{
    public class PlayerShootHandler : IMessageHandler
    {
        public int Time { get; set; }
        public byte BulletId { get; set; }
        public ushort ContainerType { get; set; }
        public Position StartingPos { get; set; }
        public float Angle { get; set; }

        public override PacketId MessageId => PacketId.PLAYERSHOOT;

        public override void Handle(Client client, NReader rdr, ref TickTime tickTime)
        {
            Time = rdr.ReadInt32();
            BulletId = rdr.ReadByte();
            ContainerType = rdr.ReadUInt16();
            StartingPos = Position.Read(rdr);
            Angle = rdr.ReadSingle();

            var player = client.Player;

            if (!player.GameServer.Resources.GameData.Items.TryGetValue(ContainerType, out var item))
            {
                player.DropNextRandom();
                return;
            }

            if (item == player.Inventory[1])
                return; // ability shoot handled by useitem

            // validate
            var result = player.ValidatePlayerShoot(item, Time);
            if (result != PlayerShootStatus.OK)
            {
                //CheatLog.Info($"PlayerShoot validation failure ({player.Name}:{player.AccountId}): {result}");
                player.DropNextRandom();
                return;
            }

            // create projectile and show other players
            var prjDesc = item.Projectiles[0]; //Assume only one
            var prj = player.PlayerShootProjectile(BulletId, prjDesc, item.ObjectType, Time, StartingPos, Angle);

            player.World.EnterWorld(prj);
            player.World.BroadcastIfVisibleExclude(new AllyShoot()
            {
                OwnerId = player.Id,
                Angle = Angle,
                ContainerType = ContainerType,
                BulletId = BulletId
            }, player, player, PacketPriority.Low);
            player.FameCounter.Shoot(prj);
        }
    }
}
