using TKR.Shared.resources;
using System;
using System.Collections.Generic;
using TKR.WorldServer.utils;

namespace TKR.WorldServer.core.objects.inventory
{
    public sealed class ItemDustPools
    {
        public List<WeightedCollection<Item>> Pools { get; private set; } = new List<WeightedCollection<Item>>();

        public void AddPool(List<KeyValuePair<Item, int>> items)
        {
            var weightedCollection = new WeightedCollection<Item>();
            foreach (var item in items)
                weightedCollection.AddItem(item.Key, item.Value);
            Pools.Add(weightedCollection);
        }

        public Item GetRandom(Random random)
        {
            var pool = Pools[random.Next(Pools.Count)];
            return pool.GetRandom(random);
        }
    }
}
