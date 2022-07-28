namespace wServer.core.objects
{
    public class Sign : StaticObject
    {
        public Sign(CoreServerManager manager, ushort objType) : base(manager, objType, null, true, false, false)
        { }

        public override bool HitByProjectile(Projectile projectile, TickData time) => false;
    }
}
