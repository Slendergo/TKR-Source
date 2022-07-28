using wServer.core.worlds;

namespace wServer.core.objects
{
    public class Pet : Entity, IPlayer
    {
        public Pet(CoreServerManager manager, Player player, ushort objType) : base(manager, objType) => PlayerOwner = player;

        public Player PlayerOwner { get; set; }

        public void Damage(int dmg, Entity src)
        { }

        public override bool HitByProjectile(Projectile projectile, TickData time) => false;

        public override void Init(World owner)
        {
            base.Init(owner);

            Size = 200;
        }

        public bool IsVisibleToEnemy() => false;
    }
}
