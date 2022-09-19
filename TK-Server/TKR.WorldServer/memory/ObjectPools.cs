using TKR.WorldServer.core.objects;
using TKR.WorldServer.core.worlds;
using TKR.WorldServer.core.worlds.logic;

namespace TKR.WorldServer.memory
{
    public sealed class ObjectPools
    {
        private const int MAX_PROJECTILE_COUNT_PER_ENTITY = 256;

        public readonly ObjectPool<Projectile> Projectiles;

        public ObjectPools(World world)
        {
            Projectiles = new ObjectPool<Projectile>(world is TestWorld ? MAX_PROJECTILE_COUNT_PER_ENTITY : world.DisableShooting ? 0 : world.MaxPlayers * MAX_PROJECTILE_COUNT_PER_ENTITY);
        }
    }
}
