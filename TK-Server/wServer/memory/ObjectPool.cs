using System.Collections.Generic;
using wServer.core.objects;
using wServer.core.worlds;
using wServer.core.worlds.logic;

namespace wServer.memory
{
    public interface IObjectPoolObject
    {
        void Reset();
    }

    public sealed class ObjectPools
    {
        private const int MAX_PROJECTILE_COUNT_PER_ENTITY = 256;

        public readonly ObjectPool<Projectile> Projectiles;

        public ObjectPools(World world)
        {
            Projectiles = new ObjectPool<Projectile>(world is TestWorld ? MAX_PROJECTILE_COUNT_PER_ENTITY : world.DisableShooting ? 0 : world.MaxPlayers * MAX_PROJECTILE_COUNT_PER_ENTITY);
        }
    }

    public sealed class ObjectPool<T> where T : IObjectPoolObject, new()
    {
        private readonly Queue<T> Container;

        public ObjectPool(int capacity)
        {
            Container = new Queue<T>(capacity);
            for(var i = 0; i < capacity; i++)
                Container.Enqueue(new T());
        }

        public T Rent()
        {
            T ret;
            if(Container.Count > 0)
            {
                ret = Container.Dequeue();
                return ret;
            }
            ret = new T();
            return ret;
        }

        public void Return(T rental)
        {
            rental.Reset();
            Container.Enqueue(rental);
        }
    }
}
