using common.database;
using System;
using wServer.core.objects;

namespace wServer.core.miscfile.inventory
{
    public class InventoryDatas
    {
        private ItemData[] _datas;
        private SV<string>[] _datasValues;

        public InventoryDatas(IContainer container, ItemData[] datas)
        {
            _datasValues = new SV<string>[datas.Length];
            _datas = new ItemData[datas.Length];

            for (var i = 0; i < datas.Length; i++)
            {
                var sti = (int)StatDataType.InventoryData0 + i;
                if (i >= 12)
                    sti = (int)StatDataType.BackPackData0 + i - 12;

                _datasValues[i] = new SV<string>(container as Entity, (StatDataType)sti, datas[i]?.GetData() ?? "{}", container is Player && i > 3);
                _datas[i] = datas[i];
            }
        }

        public int Length => _datas.Length;

        public ItemData this[int index]
        {
            get => _datas[index];
            set
            {
                _datasValues[index].SetValue(value?.GetData() ?? "{}");
                _datas[index] = value;
            }
        }

        public ItemData[] GetDatas() => (ItemData[])_datas.Clone();

        public void SetDatas(ItemData[] datas)
        {
            if (datas.Length > Length)
                throw new InvalidOperationException("Item array must be <= the size of the initialized array.");

            for (var i = 0; i < datas.Length; i++)
            {
                _datasValues[i].SetValue(datas[i]?.GetData() ?? "{}");
                _datas[i] = datas[i];
            }
        }
    }
}
