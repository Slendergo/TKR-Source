using System.Collections.Generic;

namespace TKR.WorldServer.memory
{
    public sealed class ObjectPool<T> where T : IObjectPoolObject, new()
    {
        private readonly Queue<T> Container;

        public ObjectPool(int capacity)
        {
            Container = new Queue<T>(capacity);
            for (var i = 0; i < capacity; i++)
                Container.Enqueue(new T());
        }

        public T Rent()
        {
            T ret;
            if (Container.Count > 0)
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
