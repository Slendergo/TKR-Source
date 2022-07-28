using common.database;
using common.resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using wServer.core.objects;
using wServer.core.objects.containers;
using wServer.core.objects.vendors;
using wServer.networking;
using wServer.networking.packets.outgoing;

namespace wServer.core.worlds.logic
{
    public class Vault : World
    {
        private Client _client;
        private LinkedList<Container> vaults;

        public Vault(ProtoWorld proto, Client client = null) : base(proto)
        {
            if (client != null)
            {
                _client = client;

                AccountId = _client.Account.AccountId;
                vaults = new LinkedList<Container>();
            }

            IsDungeon = false;
        }

        public int AccountId { get; private set; }

        public void AddChest(Entity original)
        {
            var vaultChest = new DbVaultSingle(_client.Account, _client.Account.VaultCount - 1);
            var con = new Container(_client.CoreServerManager, 0x0504, null, false, vaultChest)
            {
                BagOwners = new int[] { _client.Account.AccountId }
            };
            con.Inventory.SetItems(con.Inventory.ConvertObjectType2ItemArray(vaultChest.Items));
            con.Inventory.SetDataItems(vaultChest.ItemDatas);
            con.Inventory.InventoryChanged += (sender, e) => SaveChest(((Inventory)sender).Parent);
            con.Move(original.X, original.Y);

            LeaveWorld(original);
            EnterWorld(con);

            vaults.AddFirst(con);
        }

        public override bool AllowedAccess(Client client) => base.AllowedAccess(client) && AccountId == client.Account.AccountId;

        public override void LeaveWorld(Entity entity)
        {
            base.LeaveWorld(entity);

            if (entity.ObjectType != 0x0744 && entity.ObjectType != 0xa011)
                return;

            var objType = entity.ObjectType == 0x0744 ? 0x0743 : 0xa012;
            var x = new StaticObject(_client.CoreServerManager, (ushort)objType, null, true, false, false) { Size = 65 };
            x.Move(entity.X, entity.Y);

            EnterWorld(x);

            if (_client.Account.Gifts.Length <= 0)
                _client.SendPacket(new GlobalNotification
                {
                    Text = "giftChestEmpty"
                });
        }

        protected override void Init()
        {
            if (IsLimbo)
                return;

            FromWorldMap(new MemoryStream(Manager.Resources.Worlds[Name].wmap[0]));
            InitVault();
        }

        private void InitVault()
        {
            var vaultChestPosition = new List<IntPoint>();
            var giftChestPosition = new List<IntPoint>();
            var specialChestPosition = new List<IntPoint>();

            var spawn = new IntPoint(0, 0);
            var w = Map.Width;
            var h = Map.Height;

            for (var y = 0; y < h; y++)
                for (var x = 0; x < w; x++)
                {
                    var tile = Map[x, y];

                    switch (tile.Region)
                    {
                        case TileRegion.Spawn:
                            spawn = new IntPoint(x, y);
                            break;

                        case TileRegion.Vault:
                            vaultChestPosition.Add(new IntPoint(x, y));
                            break;

                        case TileRegion.Gifting_Chest:
                            giftChestPosition.Add(new IntPoint(x, y));
                            break;

                        case TileRegion.Special_Chest:
                            specialChestPosition.Add(new IntPoint(x, y));
                            break;
                    }
                }

            vaultChestPosition.Sort((x, y) => Comparer<int>.Default.Compare((x.X - spawn.X) * (x.X - spawn.X) + (x.Y - spawn.Y) * (x.Y - spawn.Y), (y.X - spawn.X) * (y.X - spawn.X) + (y.Y - spawn.Y) * (y.Y - spawn.Y)));
            giftChestPosition.Sort((x, y) => Comparer<int>.Default.Compare((x.X - spawn.X) * (x.X - spawn.X) + (x.Y - spawn.Y) * (x.Y - spawn.Y), (y.X - spawn.X) * (y.X - spawn.X) + (y.Y - spawn.Y) * (y.Y - spawn.Y)));
            specialChestPosition.Sort((x, y) => Comparer<int>.Default.Compare((x.X - spawn.X) * (x.X - spawn.X) + (x.Y - spawn.Y) * (x.Y - spawn.Y), (y.X - spawn.X) * (y.X - spawn.X) + (y.Y - spawn.Y) * (y.Y - spawn.Y)));

            for (var i = 0; i < _client.Account.VaultCount && vaultChestPosition.Count > 0; i++)
            {
                var vaultChest = new DbVaultSingle(_client.Account, i);
                var con = new Container(_client.CoreServerManager, 0x0504, null, false, vaultChest)
                {
                    BagOwners = new int[] { _client.Account.AccountId },
                    Size = 65
                };
                con.Inventory.SetItems(con.Inventory.ConvertObjectType2ItemArray(vaultChest.Items));
                con.Inventory.SetDataItems(vaultChest.ItemDatas);
                con.Inventory.InventoryChanged += (sender, e) => SaveChest(((Inventory)sender).Parent);
                con.Move(vaultChestPosition[0].X + 0.5f, vaultChestPosition[0].Y + 0.5f);

                EnterWorld(con);

                vaultChestPosition.RemoveAt(0);
                vaults.AddFirst(con);
            }
            foreach (var i in vaultChestPosition)
            {
                var x = new ClosedVaultChest(_client.CoreServerManager, 0x0505) { Size = 65 };
                x.Move(i.X + 0.5f, i.Y + 0.5f);

                EnterWorld(x);
            }

            var gifts = _client.Account.Gifts.ToList();
            while (gifts.Count > 0 && giftChestPosition.Count > 0)
            {
                var c = Math.Min(8, gifts.Count);
                var items = gifts.GetRange(0, c);

                gifts.RemoveRange(0, c);

                if (c < 8)
                    items.AddRange(Enumerable.Repeat(ushort.MaxValue, 8 - c));

                var con = new GiftChest(_client.CoreServerManager, 0x0744, null, false)
                {
                    BagOwners = new int[] { _client.Account.AccountId },
                    Size = 65
                };
                con.Inventory.SetItems(con.Inventory.ConvertObjectType2ItemArray(items));
                con.Move(giftChestPosition[0].X + 0.5f, giftChestPosition[0].Y + 0.5f);

                EnterWorld(con);

                giftChestPosition.RemoveAt(0);
            }
            foreach (var i in giftChestPosition)
            {
                var x = new StaticObject(_client.CoreServerManager, 0x0743, null, true, false, false) { Size = 65 };
                x.Move(i.X + 0.5f, i.Y + 0.5f);

                EnterWorld(x);
            }

            GenerateSpecialChests(specialChestPosition);
        }

        private void GenerateSpecialChests(List<IntPoint> specialChestPosition)
        {
            for (var i = 0; i < 6; i++)
            {
                var specialVault = new DbSpecialVault(_client.Account, i);
                if (!specialVault.GetItems())
                    continue;
                var con = new SpecialChest(_client.CoreServerManager, 0xa011, null, false, specialVault)
                {
                    BagOwners = new int[] { _client.Account.AccountId },
                    Size = 65
                };
                con.Inventory.SetItems(con.Inventory.ConvertObjectType2ItemArray(specialVault.Items));
                con.Inventory.SetDataItems(specialVault.ItemDatas);
                con.Inventory.InventoryChanged += (sender, e) => SaveChest(((Inventory)sender).Parent);
                con.Move(specialChestPosition[0].X + 0.5f, specialChestPosition[0].Y + 0.5f);

                EnterWorld(con);
                //Console.WriteLine($"Chest With Items, Pos: X: {specialChestPosition[0].X}, Y: {specialChestPosition[0].Y}");
                specialChestPosition.RemoveAt(0);
            }
            foreach (var i in specialChestPosition)
            {
                var x = new StaticObject(_client.CoreServerManager, 0xa012, null, true, false, false) { Size = 65 };
                x.Move(i.X + 0.5f, i.Y + 0.5f);

                EnterWorld(x);
                //Console.WriteLine($"Empty Chest, Pos: X: {i.X}, Y: {i.Y}");
            }
        }

        private void SaveChest(IContainer chest)
        {
            var dbLink = chest?.DbLink;

            if (dbLink == null)
                return;

            dbLink.Items = chest.Inventory.GetItemTypes();
            dbLink.ItemDatas = chest.Inventory.Data.GetDatas();
            dbLink.FlushAsync();
        }
    }
}
