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
        private static readonly List<ISellableItem> Abilities = new List<ISellableItem>
        {
            //TIER 3
            new ShopItem("Cloak of the Night Thief", 500),
            new ShopItem("Elvencraft Quiver", 500),
            new ShopItem("Scorching Blast Spell", 500),
            new ShopItem("Tome of Rejuvenation", 500),
            new ShopItem("Red Iron Helm", 500),
            new ShopItem("Reinforced Shield", 500),
            new ShopItem("Seal of the Aspirant", 500),
            new ShopItem("Stingray Poison", 500),
            new ShopItem("Soul Siphon Skull", 500),
            new ShopItem("Savage Trap", 500),
            new ShopItem("Neutralization Orb", 500),
            new ShopItem("Hallucination Prism", 500),
            new ShopItem("Arcblast Scepter", 500),
            new ShopItem("Silver Star", 500),
            //TIER 4
            new ShopItem("Cloak of the Red Agent", 500),
            new ShopItem("Magesteel Quiver", 500),
            new ShopItem("Destruction Sphere Spell", 500),
            new ShopItem("Tome of Renewing", 500),
            new ShopItem("Steel Helm", 500),
            new ShopItem("Golden Shield", 500),
            new ShopItem("Seal of the Divine", 500),
            new ShopItem("Felwasp Toxin", 500),
            new ShopItem("Essence Tap Skull", 500),
            new ShopItem("Demonhunter Trap", 500),
            new ShopItem("Timelock Orb", 500),
            new ShopItem("Prism of Figments", 500),
            new ShopItem("Cloudflash Scepter", 500),
            new ShopItem("Wind Circle", 500)
        };

        private static readonly List<ISellableItem> Armor = new List<ISellableItem>
        {
            new ShopItem("Robe of the Illusionist", 50),
            new ShopItem("Robe of the Grand Sorcerer", 500),
            new ShopItem("Studded Leather Armor", 50),
            new ShopItem("Hydra Skin Armor", 500),
            new ShopItem("Mithril Armor", 50),
            new ShopItem("Acropolis Armor", 500)
        };

        private static readonly List<ISellableItem> Consumables = new List<ISellableItem>
        {
            new ShopItem("XP Booster 20 min", 100),
            new ShopItem("Backpack", 300),
            new ShopItem("Loot Drop Potion", 400),
            new ShopItem("Magic Potion", 5),
            new ShopItem("Health Potion", 5),
            new ShopItem("Item Dust", 150),
            new ShopItem("Potion Dust", 150),
            new ShopItem("Miscellaneous Dust", 250),
            new ShopItem("Special Dust", 1500),
            new ShopItem("Char Slot Unlocker", 1000),
            new ShopItem("Vault Chest Unlocker", 200),
            new ShopItem("Magic Paper", 0)
        };

        private static readonly List<ISellableItem> KeysGold = new List<ISellableItem>
        {
            new ShopItem("Strange Library Key", 200),
			new ShopItem("Lost Halls Key", 200),
			new ShopItem("Davy's Key", 150),
            new ShopItem("The Crawling Depths Key", 150),
            new ShopItem("Shatters Key", 200),
            new ShopItem("Ocean Trench Key", 150),
            new ShopItem("Tomb of the Ancients Key", 100),
            new ShopItem("Deadwater Docks Key", 150),
            new ShopItem("Woodland Labyrinth Key", 150),
        };

        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private static readonly List<ISellableItem> Miscellaneous = new List<ISellableItem>
        {
            new ShopItem("XP Booster 20 min", 10),
            new ShopItem("Backpack", 30),
            new ShopItem("Loot Drop Potion", 40),
            new ShopItem("Item Dust", 15),
            new ShopItem("Potion Dust", 15),
            new ShopItem("Miscellaneous Dust", 25),
            new ShopItem("Special Dust", 100),
            new ShopItem("Char Slot Unlocker", 60),
            new ShopItem("Vault Chest Unlocker", 60),
            new ShopItem("Cocktail of Mana", 150),
            new ShopItem("Cocktail of Life", 150),
            new ShopItem("Cocktail of Attack", 120),
            new ShopItem("Cocktail of Defense", 120),
            new ShopItem("Cocktail of Speed", 120),
            new ShopItem("Cocktail of Dexterity", 120),
            new ShopItem("Cocktail of Wisdom", 120),
            new ShopItem("Cocktail of Vitality", 120)
        };

        private static readonly List<ISellableItem> PurchasableFame = new List<ISellableItem>
        {
            new ShopItem("50 Fame", 50),
            new ShopItem("100 Fame", 100),
            new ShopItem("500 Fame", 500),
            new ShopItem("1000 Fame", 1000),
            new ShopItem("5000 Fame", 5000)
        };

        private static readonly List<ISellableItem> Rings = new List<ISellableItem>
        {
            //TIER 3
            new ShopItem("Ring of Superior Attack", 500),
            new ShopItem("Ring of Superior Defense", 500),
            new ShopItem("Ring of Superior Speed", 500),
            new ShopItem("Ring of Superior Dexterity", 500),
            new ShopItem("Ring of Superior Vitality", 500),
            new ShopItem("Ring of Superior Wisdom", 500),
            new ShopItem("Ring of Superior Health", 500),
            new ShopItem("Ring of Superior Magic", 500),
            //TIER 4
            new ShopItem("Ring of Paramount Attack", 500),
            new ShopItem("Ring of Paramount Defense", 500),
            new ShopItem("Ring of Paramount Speed", 500),
            new ShopItem("Ring of Paramount Dexterity", 500),
            new ShopItem("Ring of Paramount Vitality", 500),
            new ShopItem("Ring of Paramount Wisdom", 500),
            new ShopItem("Ring of Paramount Health", 500),
            new ShopItem("Ring of Paramount Magic", 500)
        };

        private static readonly List<ISellableItem> Small_Cloths = new List<ISellableItem>
        {
            new ShopItem("Small Futuristic Cloth", 10),
            new ShopItem("Small Red Dragon Scale Cloth", 10),
            new ShopItem("Small Midnight Dragon Scale Cloth", 10),
            new ShopItem("Small Ivory Dragon Scale Cloth", 10),
            new ShopItem("Small Green Dragon Scale Cloth", 10),
            new ShopItem("Small Blue Dragon Scale Cloth", 10),
            new ShopItem("Small Jester Argyle Cloth", 10),
            new ShopItem("Small Heavy Chainmail Cloth", 10),
            new ShopItem("Small Flame Cloth", 10),
            new ShopItem("Small Alchemist Cloth", 10),
            new ShopItem("Small Blue Camo Cloth", 10),
            new ShopItem("Small Clanranald Cloth", 10),
            new ShopItem("Small Hibiscus Beach Wrap Cloth", 10),
            new ShopItem("Small Leopard Print Cloth", 10),
            new ShopItem("Small Relief Cloth", 10),
            new ShopItem("Small Spring Cloth", 10),
            new ShopItem("Small Zebra Print Cloth", 10),
            new ShopItem("Small American Flag Cloth", 10),
            new ShopItem("Small Celtic Knot Cloth", 10),
            new ShopItem("Small Colored Egg Cloth", 10),
            new ShopItem("Small Intense Clovers Cloth", 10),
            new ShopItem("Small Mosaic Cloth", 10),
            new ShopItem("Small Spooky Cloth", 10),
            new ShopItem("Small Sunburst Cloth", 10),
            new ShopItem("Small White Diamond Cloth", 10),
            new ShopItem("Small Skull Cloth", 10),
            new ShopItem("Small Pink Sparkly Cloth", 10),
            new ShopItem("Small Heart Cloth", 10),
            new ShopItem("Small Dark Blue Stripe Cloth", 10),
            new ShopItem("Small Brown Lined Cloth", 10),
            new ShopItem("Small Starry Cloth", 10)
        };

        private static readonly List<ISellableItem> Special = new List<ISellableItem>
        {
            new ShopItem("Amulet of Resurrection", 50000)
        };

        private static readonly List<ISellableItem> Store1 = new List<ISellableItem>
        {
            new ShopItem("Pirate Cave Key", 25),
            new ShopItem("Spider Den Key", 25),
            new ShopItem("Undead Lair Key", 50),
            new ShopItem("Sprite World Key", 50),
            new ShopItem("Abyss of Demons Key", 50),
            new ShopItem("Snake Pit Key", 50),
            new ShopItem("Beachzone Key", 50),
            new ShopItem("Lab Key", 50),
            new ShopItem("Totem Key", 50),
            new ShopItem("Manor Key", 80),
            new ShopItem("Candy Key", 100),
            new ShopItem("Cemetery Key", 150),
            new ShopItem("Davy's Key", 200),
            new ShopItem("Ocean Trench Key", 300),
            new ShopItem("Tomb of the Ancients Key", 400)
        };

        private static readonly List<ISellableItem> Weapons = new List<ISellableItem>
        {
            //TIER 8
            new ShopItem("Fire Dagger", 500),
            new ShopItem("Golden Bow", 500),
            new ShopItem("Staff of Horror", 500),
            new ShopItem("Wand of Death", 500),
            new ShopItem("Ravenheart Sword", 500),
            new ShopItem("Demon Edge", 500),
            //TIER 9
            new ShopItem("Ragetalon Dagger", 500),
            new ShopItem("Verdant Bow", 500),
            new ShopItem("Staff of Necrotic Arcana", 500),
            new ShopItem("Wand of Deep Sorcery", 500),
            new ShopItem("Dragonsoul Sword", 500),
            new ShopItem("Jewel Eye Katana", 500)
        };

        public static void Initialize(CoreServerManager manager)
        {
            InitDyes(manager);

            foreach (var shop in Shops)
            {
                var shopItems = shop.Value.Item1;

                if (shopItems == null)
                    continue;

                foreach (var shopItem in shopItems.OfType<ShopItem>())
                    if (!manager.Resources.GameData.IdToObjectType.TryGetValue(shopItem.Name, out ushort id))
                        Log.Warn("Item name: {0}, not found.", shopItem.Name);
                    else
                        shopItem.SetItem(id);
            }
        }

        public static readonly Dictionary<TileRegion, Tuple<List<ISellableItem>, CurrencyType, int>> Shops = new Dictionary<TileRegion, Tuple<List<ISellableItem>, CurrencyType, int>>()
        {
            { TileRegion.Store_1, new Tuple<List<ISellableItem>, CurrencyType, int>(Weapons, CurrencyType.Fame, 0) },
            { TileRegion.Store_2, new Tuple<List<ISellableItem>, CurrencyType, int>(Abilities, CurrencyType.Fame, 0) },
            { TileRegion.Store_3, new Tuple<List<ISellableItem>, CurrencyType, int>(Armor, CurrencyType.Fame, 0) },
            { TileRegion.Store_4, new Tuple<List<ISellableItem>, CurrencyType, int>(Rings, CurrencyType.Fame, 0) },
            { TileRegion.Store_6, new Tuple<List<ISellableItem>, CurrencyType, int>(PurchasableFame, CurrencyType.Fame, 0) },
            { TileRegion.Store_7, new Tuple<List<ISellableItem>, CurrencyType, int>(Consumables, CurrencyType.Fame, 0) },
            { TileRegion.Store_8, new Tuple<List<ISellableItem>, CurrencyType, int>(Special, CurrencyType.Fame, 0) },
            { TileRegion.Store_17, new Tuple<List<ISellableItem>, CurrencyType, int>(KeysGold, CurrencyType.Gold, 0) },
            { TileRegion.Store_19, new Tuple<List<ISellableItem>, CurrencyType, int>(Miscellaneous, CurrencyType.Gold, 0) }
        };

        private static void InitDyes(CoreServerManager manager)
        {
            var d1 = new List<ISellableItem>();
            var d2 = new List<ISellableItem>();
            var c1 = new List<ISellableItem>();

            foreach (var i in manager.Resources.GameData.Items.Values)
            {
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
                        c1.Add(new ShopItem(i.ObjectId, 10));

                    continue;
                }
            }

            Shops[TileRegion.Store_15] = new Tuple<List<ISellableItem>, CurrencyType, int>(d1, CurrencyType.Gold, 0);
            Shops[TileRegion.Store_16] = new Tuple<List<ISellableItem>, CurrencyType, int>(d2, CurrencyType.Gold, 0);
            Shops[TileRegion.Store_18] = new Tuple<List<ISellableItem>, CurrencyType, int>(c1, CurrencyType.Gold, 0);
        }
    }
}
