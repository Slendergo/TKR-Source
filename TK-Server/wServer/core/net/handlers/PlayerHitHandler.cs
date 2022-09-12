using common;
using wServer.networking;

namespace wServer.core.net.handlers
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

            var prj = player.GetProjectile(objectId, bulletId);
            if (prj == null)
            {
                System.Console.WriteLine("Null Projectile PlayerHit");
                return;
            }

            var shooter = player.World.GetEntity(objectId);
            prj?.HitEntity(shooter, player, tickTime);

            System.Console.WriteLine("PLAYER HIT");
        }
    }
}
