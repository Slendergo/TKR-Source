using TKR.WorldServer.core.miscfile.thread;

namespace TKR.WorldServer.core.objects
{
    public class Pet : Entity, IPlayer
    {
        public Pet(GameServer manager, Player player, ushort objType) : base(manager, objType) => PlayerOwner = player;

        public Player PlayerOwner { get; set; }

        public void Damage(int dmg, Entity src) { }
        public override bool HitByProjectile(Projectile projectile, TickTime time) => false;

        public bool IsVisibleToEnemy() => false;
    }
}
