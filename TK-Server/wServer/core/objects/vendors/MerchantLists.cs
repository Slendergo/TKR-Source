using common.resources;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace wServer.core.objects.vendors
{
    public interface ISellableItem
    {
        int Count { get; }
        ushort ItemId { get; }
        int Price { get; }
    }

    internal static class MerchantLists
    {

        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        
        private static readonly List<ISellableItem> Consumables = new List<ISellableItem>
        {
            new ShopItem("Magic Potion", 1),
            new ShopItem("Special Dust", 5000),
            new ShopItem("Health Potion", 1),
            new ShopItem("XP Booster 20 min", 100),
            new ShopItem("Loot Drop Potion", 200),
            new ShopItem("Item Dust", 175),
            new ShopItem("Backpack", 250),
            new ShopItem("Char Slot Unlocker", 300),
            new ShopItem("Vault Chest Unlocker", 300),
            new ShopItem("Loot Drop Potion", 200)
        };

        private static readonly List<ISellableItem> KeysFame = new List<ISellableItem>
        {
            new ShopItem("Strange Library Key", 500),
            new ShopItem("Cemetery Key", 500),
            new ShopItem("Lost Halls Key", 1000),
			new ShopItem("Davy's Key", 500),
            new ShopItem("The Crawling Depths Key", 500),
            new ShopItem("Shatters Key", 1000),
            new ShopItem("Ocean Trench Key", 500),
            new ShopItem("Tomb of the Ancients Key", 1000),
            new ShopItem("Deadwater Docks Key", 500),
            new ShopItem("Woodland Labyrinth Key", 500),
            new ShopItem("Spider Den Key", 500),
            new ShopItem("Undead Lair Key", 500),
            new ShopItem("Sprite World Key", 500),
            new ShopItem("Abyss of Demons Key", 500),
            new ShopItem("Snake Pit Key", 500),
            new ShopItem("Beachzone Key", 500),
            new ShopItem("Lab Key", 500),
            new ShopItem("Totem Key", 500),
            new ShopItem("Manor Key", 500),
            new ShopItem("Candy Key", 500),
            new ShopItem("Cemetery Key", 500),
            new ShopItem("Davy's Key", 500),
            new ShopItem("Ocean Trench Key", 500),
            new ShopItem("Tomb of the Ancients Key", 1000)
        };

        private static readonly List<ISellableItem> PurchasableFame = new List<ISellableItem>
        {
            new ShopItem("50 Fame", 50),
            new ShopItem("100 Fame", 100),
            new ShopItem("500 Fame", 500),
            new ShopItem("1000 Fame", 1000),
            new ShopItem("5000 Fame", 5000)
        };

        private static readonly List<ISellableItem> Special = new List<ISellableItem>
        {
            new ShopItem("Glowing Talisman", 200),
            new ShopItem("Glowing Talisman", 200),
            new ShopItem("Glowing Talisman", 200),
            new ShopItem("Glowing Talisman", 200)
        };

        public static void Initialize(GameServer gameServer)
        {
            InitDyes(gameServer);

            foreach (var shop in Shops)
            {
                var shopItems = shop.Value.Item1;

                if (shopItems == null)
                    continue;

                foreach (var shopItem in shopItems.OfType<ShopItem>())
                    if (!gameServer.Resources.GameData.IdToObjectType.TryGetValue(shopItem.Name, out ushort id))
                        Log.Warn("Item name: {0}, not found.", shopItem.Name);
                    else
                        shopItem.SetItem(id);
            }
        }

        public static readonly Dictionary<TileRegion, Tuple<List<ISellableItem>, CurrencyType, int>> Shops = new Dictionary<TileRegion, Tuple<List<ISellableItem>, CurrencyType, int>>()
        {
            { TileRegion.Store_1, new Tuple<List<ISellableItem>, CurrencyType, int>(PurchasableFame, CurrencyType.Fame, 0) },
            { TileRegion.Store_2, new Tuple<List<ISellableItem>, CurrencyType, int>(Consumables, CurrencyType.Fame, 0) },
            { TileRegion.Store_3, new Tuple<List<ISellableItem>, CurrencyType, int>(Special, CurrencyType.Gold, 0) },
            { TileRegion.Store_4, new Tuple<List<ISellableItem>, CurrencyType, int>(KeysFame, CurrencyType.Fame, 0) }
        };

        private static void InitDyes(GameServer gameServer)
        {
            var d1 = new List<ISellableItem>();
            var d2 = new List<ISellableItem>();
            var c1 = new List<ISellableItem>();
            var c2 = new List<ISellableItem>();
            var petGenerators = new List<ISellableItem>();
            var supporterPetGenerators = new List<ISellableItem>();

            foreach (var i in gameServer.Resources.GameData.Items.Values)
            {
                if (i.InvUse && i.ObjectId.Contains("Generator"))
                {
                    if (i.DonorItem)
                    {
                        supporterPetGenerators.Add(new ShopItem(i.ObjectId, 1000));
                        continue;
                    }
                    petGenerators.Add(new ShopItem(i.ObjectId, 1250));
                    continue;
                }

                if (!i.Class.Equals("Dye"))
                    continue;

                if (i.Texture1 != 0)
                {
                    if (i.ObjectId.Contains("Cloth") && i.ObjectId.Contains("Large"))
                        d1.Add(new ShopItem(i.ObjectId, 25));

                    if (i.ObjectId.Contains("Dye") && i.ObjectId.Contains("Clothing"))
                        c1.Add(new ShopItem(i.ObjectId, 25));

                    continue;
                }

                if (i.Texture2 != 0)
                {
                    if (i.ObjectId.Contains("Cloth") && i.ObjectId.Contains("Small"))
                        d2.Add(new ShopItem(i.ObjectId, 10));
                    if (i.ObjectId.Contains("Dye") && i.ObjectId.Contains("Accessory"))
                        c2.Add(new ShopItem(i.ObjectId, 10));

                    continue;
                }
            }

            Shops[TileRegion.Store_5] = new Tuple<List<ISellableItem>, CurrencyType, int>(petGenerators, CurrencyType.Fame, 0);
            Shops[TileRegion.Store_10] = new Tuple<List<ISellableItem>, CurrencyType, int>(supporterPetGenerators, CurrencyType.Gold, 0);

            Shops[TileRegion.Store_6] = new Tuple<List<ISellableItem>, CurrencyType, int>(d1, CurrencyType.Gold, 0);
            Shops[TileRegion.Store_7] = new Tuple<List<ISellableItem>, CurrencyType, int>(d2, CurrencyType.Gold, 0);
            Shops[TileRegion.Store_8] = new Tuple<List<ISellableItem>, CurrencyType, int>(c1, CurrencyType.Gold, 0);
            Shops[TileRegion.Store_9] = new Tuple<List<ISellableItem>, CurrencyType, int>(c2, CurrencyType.Gold, 0);

        }
    }
}
