using wServer.core.worlds;

namespace wServer.core.objects
{
    public class Pet : Entity, IPlayer
    {
        public Pet(GameServer manager, Player player, ushort objType) : base(manager, objType) => PlayerOwner = player;

        public Player PlayerOwner { get; set; }

        public void Damage(int dmg, Entity src)
        {
        }

        public override bool HitByProjectile(Projectile projectile, TickTime time) => false;

        public override void Tick(ref TickTime time)
        {
            if (PlayerOwner == null)
                World.LeaveWorld(this);
        }

        public bool IsVisibleToEnemy() => false;
    }
}
