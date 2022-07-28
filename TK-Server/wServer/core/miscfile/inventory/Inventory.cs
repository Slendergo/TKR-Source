using common.database;
using common.resources;
using System;
using System.Collections.Generic;
using System.Linq;
using wServer.core.miscfile.inventory;
using wServer.core.objects;

namespace wServer.core
{
    public class Inventory : IEnumerable<Item>
    {
        public InventoryItems _items;

        public InventoryDatas Data;

        private static readonly object TrySaveLock = new object();

        private object _invLock = new object();

        public Inventory(IContainer parent) : this(parent, new Item[20], new ItemData[20])
        { }

        public Inventory(IContainer parent, Item[] items, ItemData[] datas)
        {
            Parent = parent;

            _items = new InventoryItems(parent, items);

            Data = new InventoryDatas(parent, datas);
        }

        public event EventHandler<InventoryChangedEventArgs> InventoryChanged;

        public int Length => _items.Length;
        public IContainer Parent { get; }

        public Item this[int index]
        {
            get
            {
                using (TimedLock.Lock(_invLock))
                    return _items[index];
            }
            set
            {
                using (TimedLock.Lock(_invLock))
                    if (_items[index] != value)
                    {
                        var oItems = _items.GetItems();

                        _items[index] = value;

                        InventoryChanged?.Invoke(this, new InventoryChangedEventArgs(oItems, _items.GetItems()));
                    }
            }
        }

        public static bool Execute(params InventoryTransaction[] transactions)
        {
            using (TimedLock.Lock(TrySaveLock))
            {
                if (transactions.Any(tranaction => !tranaction.Validate()))
                    return false;

                foreach (var transcation in transactions)
                    transcation.Execute();

                return true;
            }
        }

        public static bool Revert(params InventoryTransaction[] transactions)
        {
            using (TimedLock.Lock(TrySaveLock))
            {
                if (transactions.Any(tranaction => !tranaction.Validate(true)))
                    return false;

                foreach (var transcation in transactions)
                    transcation.Revert();
                return true;
            }
        }

        public static bool DatExecute(params InventoryDataTransaction[] transactions)
        {
            using (TimedLock.Lock(TrySaveLock))
            {
                if (transactions.Any(transaction => !transaction.Validate()))
                    return false;

                foreach (var transaction in transactions)
                    transaction.Execute();
                return true;
            }
        }

        public static bool DatRevert(params InventoryDataTransaction[] transactions)
        {
            using (TimedLock.Lock(TrySaveLock))
            {
                if (transactions.Any(transaction => !transaction.Validate(true)))
                    return false;

                foreach (var trans in transactions)
                    trans.Revert();

                return true;
            }
        }

        public InventoryTransaction CreateTransaction() => new InventoryTransaction(Parent);

        public InventoryDataTransaction CreateDataTransaction() => new InventoryDataTransaction(Parent);

        public int GetAvailableInventorySlot(Item item)
        {
            using (TimedLock.Lock(_invLock))
            {
                if (Parent is Player plr)
                {
                    var playerDesc = plr.CoreServerManager.Resources.GameData
                        .Classes[plr.ObjectDesc.ObjectType];
                    for (var i = 0; i < 4; i++)
                        if (_items[i] == null && playerDesc.SlotTypes[i] == item.SlotType)
                            return i;

                    for (var i = 4; i < 12 || (plr.HasBackpack && i < plr.Inventory.Length); i++)
                        if (_items[i] == null)
                            return i;
                }
                else
                {
                    for (var i = 0; i < 8; i++)
                        if (_items[i] == null)
                            return i;
                }

                return -1;
            }
        }

        public IEnumerator<Item> GetEnumerator() => ((IEnumerable<Item>)_items.GetItems()).GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => _items.GetItems().GetEnumerator();

        public Item[] GetItems()
        {
            using (TimedLock.Lock(TrySaveLock))
            using (TimedLock.Lock(_invLock))
                return _items.GetItems();
        }

        public ushort[] GetItemTypes()
        {
            using (TimedLock.Lock(_invLock))
                return _items.GetItems().Select(_ => _?.ObjectType ?? 0xffff).ToArray();
        }

        public void SetItems(Item[] items)
        {
            using (TimedLock.Lock(_invLock))
            {
                var oItems = _items.GetItems();

                _items.SetItems(items);

                InventoryChanged?.Invoke(this, new InventoryChangedEventArgs(oItems, _items.GetItems()));
            }
        }

        public void SetDataItems(ItemData[] datas)
        {
            using(TimedLock.Lock(_invLock))
            {
                Data.SetDatas(datas);
            }
        }

        public Item[] ConvertObjectType2ItemArray(IEnumerable<ushort> a)
        {
            var gameData = (Parent as Entity).CoreServerManager.Resources.GameData;

            return a.Select(_ => (_ == 0xffff || !gameData.Items.ContainsKey(_)) ? null : gameData.Items[_]).ToArray();
        }
    }
}
