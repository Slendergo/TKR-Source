namespace wServer.core.objects
{
    public class Sign : StaticObject
    {
        public Sign(GameServer manager, ushort objType) : base(manager, objType, null, true, false, false)
        { }

        public override bool HitByProjectile(Entity shooter, Projectile projectile, TickTime time) => false;
    }
}
