using common.resources;
using System.Collections.Generic;
using wServer.networking.packets.outgoing;

namespace wServer.core.objects
{
    public sealed class PendingShootAcknowledgement
    {
        private EnemyShoot EnemyShoot { get; }
        private ServerPlayerShoot ServerPlayerShoot { get; }

        public PendingShootAcknowledgement(EnemyShoot enemyShoot)
        {
            EnemyShoot = enemyShoot;
        }

        public PendingShootAcknowledgement(ServerPlayerShoot serverPlayerShoot)
        {
            ServerPlayerShoot = serverPlayerShoot;
        }

        public KeyValuePair<int, List<Projectile>> Acknowledge(GameServer gameServer, int time)
        {
            var ret = new List<Projectile>();
            ProjectileDesc xmlProjectile;

            if (EnemyShoot != null)
            {
                var containerXMLObject = gameServer.Resources.GameData.ObjectDescs[(ushort)EnemyShoot.ObjectType];
                xmlProjectile = containerXMLObject.Projectiles[EnemyShoot.BulletType];

                for (var i = 0; i < EnemyShoot.NumShots; i++)
                {
                    var angle = (float)(EnemyShoot.Angle + EnemyShoot.AngleInc * i);
                    var bulletId = (EnemyShoot.BulletId + i) % 0xFFFF;
                    ret.Add(new Projectile(time, EnemyShoot.ObjectId, bulletId, EnemyShoot.ObjectType, angle, EnemyShoot.StartingPos.X, EnemyShoot.StartingPos.Y, EnemyShoot.Damage, xmlProjectile));
                }
                return new KeyValuePair<int, List<Projectile>>(EnemyShoot.ObjectId, ret);
            }

            var containerItem = gameServer.Resources.GameData.Items[(ushort)ServerPlayerShoot.ObjectType];
            xmlProjectile = containerItem.Projectiles[0];

            ret.Add(new Projectile(time, ServerPlayerShoot.ObjectId, ServerPlayerShoot.BulletId, ServerPlayerShoot.ObjectType, ServerPlayerShoot.Angle, ServerPlayerShoot.StartingPos.X, ServerPlayerShoot.StartingPos.Y, ServerPlayerShoot.Damage, xmlProjectile));
            return new KeyValuePair<int, List<Projectile>>(ServerPlayerShoot.ObjectId, ret);
        }
    }

    public partial class Player
    {
        public Queue<PendingShootAcknowledgement> PendingShootAcks { get; } = new Queue<PendingShootAcknowledgement>();

        public void AddPendingEnemyShoot(EnemyShoot enemyShoot)
        {
            var shootAck = new PendingShootAcknowledgement(enemyShoot);
            PendingShootAcks.Enqueue(shootAck);
        }

        public void AddPendingServerPlayerShoot(ServerPlayerShoot serverPlayerShoot)
        {
            var shootAck = new PendingShootAcknowledgement(serverPlayerShoot);
            PendingShootAcks.Enqueue(shootAck);
        }
    }
}
