using common.database;
using System.Linq;

namespace common
{
    public abstract class RInventory : RedisObject
    {
        public string Field { get; protected set; }
        public string DataField { get; protected set; }

        public ushort[] Items { get => GetValue<ushort[]>(Field) ?? Enumerable.Repeat((ushort)0xffff, 20).ToArray(); set => SetValue(Field, value); }
        public ItemData[] ItemDatas
        {
            get => GetValue<ItemData[]>(DataField) ?? new ItemData[20];
            set => SetValue(DataField, value);
        }
    }
}
