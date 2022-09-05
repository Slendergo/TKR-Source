using common;
using common.database;
using common.discord;
using common.resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.core;
using wServer.core.objects;
using wServer.networking.packets.outgoing;

namespace wServer.logic.loot
{
    public class ChestLoot
    {
        private readonly static List<MobDrops> ChestItems = new List<MobDrops>();

        public ChestLoot(params MobDrops[] drops) => ChestItems.AddRange(ChestItems);

        public IEnumerable<Item> CalculateItems(GameServer core, Random random, int min, int max)
        {
            var consideration = new List<LootDef>();
            foreach (var i in ChestItems)
                i.Populate(consideration);

            var retCount = random.Next(min, max);

            foreach (var i in consideration)
            {
                if (random.NextDouble() < i.Probabilty)
                {
                    yield return core.Resources.GameData.Items[core.Resources.GameData.IdToObjectType[i.Item]];
                    retCount--;
                }

                if (retCount == 0)
                    yield break;
            }
        }
    }

    public class Loot : List<MobDrops>
    {
        #region Utils

        /*  Brown 0,  Pink 1,   Purple 2, Gold 3,   Cyan 4,   Blue 5,   Orange 6, White 7,  Mythical 8, Eternal 9 */
        private static readonly ushort[] BAG_ID_TO_TYPE = new ushort[] { 0x0500, 0x0506, 0x0503, 0x0532, 0x0509, 0x050B, 0x0533, 0x050C, 0x5076, 0xa002 };
        /*  Brown 0,  Pink 1,   Purple 2, Gold 3,   Cyan 4,   Blue 5,   Orange 6, White 7,  Mythical 8, Eternal 9 */
        private static readonly ushort[] BOOSTED_BAG_ID_TO_TYPE = new ushort[] { 0x0534, 0x0535, 0x0536, 0x0537, 0x0538, 0x0539, 0x053b, 0x053a, 0x5077, 0xa003 };
        
        private static readonly int[] AbilityT = new int[] { 4, 5, 11, 12, 13, 15, 16, 18, 19, 20, 21, 22, 23, 25, };
        private static readonly int[] ArmorT = new int[] { 6, 7, 14, };
        private static readonly int[] RingT = new int[] { 9 };
        private static readonly int[] WeaponT = new int[] { 1, 2, 3, 8, 17, 24 };

        public static bool DropsInSoulboundBag(ItemType type, int tier)
        {
            if (type == ItemType.Ring)
                if (tier >= 2)
                    return true;
            if (type == ItemType.Ability)
                if (tier > 2)
                    return true;
            return tier > 6;
        }

        // slotType
        // tier
        // item
        private static Dictionary<ItemType, Dictionary<int, List<Item>>> Items = new Dictionary<ItemType, Dictionary<int, List<Item>>>();

        public List<Item> GetItems(ItemType itemType, int tier)
        {
            if (Items.TryGetValue(itemType, out var keyValuePairs))
                if (keyValuePairs.TryGetValue(tier, out var items))
                    return items;
            return null;
        }

        public static void Initialize(GameServer gameServer)
        {
            // get all tiers

            var allItems = gameServer.Resources.GameData.Items;
            foreach (var item in allItems.Values)
            {
                var itemType = TierLoot.SlotTypesToItemType(item.SlotType);
                if (!Items.TryGetValue(itemType, out var dict))
                    Items[itemType] = dict = new Dictionary<int, List<Item>>();
                if (!dict.TryGetValue(item.Tier, out var items))
                    Items[itemType][item.Tier] = items = new List<Item>();
                items.Add(item);
            }

            //GetSlotTypes

            Items = Items.OrderBy(_ => _.Key).ToDictionary(_ => _.Key, _ => _.Value);
        }

        public static double GetPlayerLootBoost(Player player)
        {
            if (player == null) 
                return 0;
            var allLoot = 0.0;
            switch (player.GameServer.WorldManager.Nexus.EngineStage)
            {
                case 1: allLoot += 0.25; break;
                case 2: allLoot += 0.5; break;
                case 3: allLoot += 1.0; break;
                default: break;
            }
            allLoot += player.LDBoostTime > 0 ? 0.1 : 0;
            allLoot += player.TalismanLootBoost;
            if(player.TalismanLootBoostPerPlayer != 0.0 && player.World.Players.Count != 1)
                allLoot += (player.TalismanLootBoostPerPlayer * (player.World.Players.Count - 1));
            allLoot += player.TalismanCanOnlyGetWhiteBags ? 0.5 : 0;
            return allLoot;
        }

        private List<LootDef> GetEnemyClasifiedLoot(List<LootDef> list, Enemy enemy)
        {
            var gameData = enemy.GameServer.Resources.GameData;
            var xmlitem = gameData.Items;
            var itemtoid = gameData.IdToObjectType;

            if (enemy.Legendary)
            {
                list.Add(new LootDef("Glowing Shard", 0.20, 0.001)); // 8%
                list.Add(new LootDef("Glowing Shard", 0.20, 0.001)); // 8%
                list.Add(new LootDef("Glowing Shard", 0.20, 0.001)); // 8%
                list.Add(new LootDef("Potion Dust", 0.08, 0.001)); // 8%
                list.Add(new LootDef("Potion Dust", 0.08, 0.001)); // 8%
                list.Add(new LootDef("Item Dust", 0.12, 0.001)); // 12%
                list.Add(new LootDef("Miscellaneous Dust", 0.05, 0.001)); // 5% 
                list.Add(new LootDef("Special Dust", 0.003, 0.001)); // 3%
            }
            else if (enemy.Epic)
            {
                list.Add(new LootDef("Glowing Shard", 0.10, 0.001)); // 8%
                list.Add(new LootDef("Glowing Shard", 0.10, 0.001)); // 8%
                list.Add(new LootDef("Potion Dust", 0.05, 0.001)); // 5%
                list.Add(new LootDef("Item Dust", 0.08, 0.001)); //8%
                list.Add(new LootDef("Miscellaneous Dust", 0.02, 0.001)); //2%
                list.Add(new LootDef("Special Dust", 0.001, 0.001)); // 1%
            }
            else if (enemy.Rare)
            {
                list.Add(new LootDef("Glowing Shard", 0.08, 0.001)); // 8%
                list.Add(new LootDef("Potion Dust", 0.03, 0.001));//3%
                list.Add(new LootDef("Item Dust", 0.05, 0.001)); //5%
                list.Add(new LootDef("Miscellaneous Dust", 0.01, 0.001)); //1%
                list.Add(new LootDef("Special Dust", 0.0005, 0.001)); //0.5%
            }

            return list;
        }

        #endregion Utils

        public Loot(params MobDrops[] drops) => AddRange(drops);

        public void Handle(Enemy enemy, TickTime time)
        {
            if (enemy.SpawnedByBehavior)
                return;

            var possDrops = new List<LootDef>();
            GetEnemyClasifiedLoot(possDrops, enemy);
            foreach (var i in this)
                i.Populate(possDrops);

            var pubDrops = new List<Item>();

            foreach (var i in possDrops)
            {
                var chance = enemy.World.Random.NextDouble();
                if (i.ItemType == ItemType.None)
                {
                    // we treat item names as soulbound never public loot
                    continue;
                }

                if (DropsInSoulboundBag(i.ItemType, i.Tier))
                    continue;

                if (i.Threshold <= 0 && chance < i.Probabilty)
                {
                    var items = GetItems(i.ItemType, i.Tier);
                    var chosenTieredItem = items[enemy.World.Random.Next(items.Count)];
                    pubDrops.Add(chosenTieredItem);
                }
            }

            ProcessPublicDrops(pubDrops, enemy);

            var playersAvaliable = enemy.DamageCounter.GetPlayerData();
            if (playersAvaliable == null)
                return;

            var privDrops = new Dictionary<Player, IList<Item>>();
            foreach (var tupPlayer in playersAvaliable)
            {
                var player = tupPlayer.Item1;
                var playerDamage = tupPlayer.Item2;

                if (player.TalismanCantGetLoot)
                    continue;

                if (player == null || player.World == null || player.Client == null)
                    continue;

                var percentageOfDamage = (Math.Round(100.0 * (playerDamage / (double)enemy.DamageCounter.TotalDamage), 4) / 100);
                if (percentageOfDamage >= 0.001) // 0.01%
                {
                    if (enemy.ObjectDesc.Encounter)
                    {
                        var essenceToGive = player.World.Random.Next(350, 500);
                        if (essenceToGive > 0)
                            player.GiveEssence(essenceToGive);
                    }
                    else if (enemy.ObjectDesc.Quest)
                    {
                        if (enemy.ObjectDesc.Level >= 18)
                        {
                            var essenceToGive = player.World.Random.Next(100, 250);
                            if (essenceToGive > 0)
                                player.GiveEssence(essenceToGive);
                        }
                        else if (enemy.ObjectDesc.Level >= 15)
                        {
                            var essenceToGive = player.World.Random.Next(75, 100);
                            if (essenceToGive > 0)
                                player.GiveEssence(essenceToGive);
                        }
                        else if (enemy.ObjectDesc.Level >= 5)
                        {
                            var essenceToGive = player.World.Random.Next(1, 50);
                            if (essenceToGive > 0)
                                player.GiveEssence(essenceToGive);
                        }
                    }
                }

                var playerLootBoost = GetPlayerLootBoost(player);

                //Console.WriteLine($"Loot Boost: {playerLootBoost}");

                if (enemy.ObjectDesc.Event)
                {
                    player.Stacks[0].Push(player.GameServer.Resources.GameData.Items[0xa22]);
                    player.Stacks[1].Push(player.GameServer.Resources.GameData.Items[0xa23]);
                }

                var gameData = enemy.GameServer.Resources.GameData;

                var drops = new List<Item>();
                foreach (var i in possDrops)
                {
                    var c = enemy.World.Random.NextDouble();

                    var probability = i.Probabilty + (i.Probabilty * playerLootBoost);

                    if (i.Threshold >= 0 && i.Threshold < percentageOfDamage)
                    {
                        Item item = null;
                        if (i.ItemType != ItemType.None)
                        {
                            var items = GetItems(i.ItemType, i.Tier);
                            if (items != null)
                                item = enemy.World.Random.NextLength(items);
                        }
                        else
                        {
                            if (!gameData.IdToObjectType.TryGetValue(i.Item, out var type))
                                continue;

                            if (!gameData.Items.TryGetValue(type, out item))
                                continue;
                        }

                        if (item == null)
                        {
                            player.SendError($"There was a error calculating the item roll for item: {i.Item}, please report this [#1]");
                            continue;
                        }

                        var isEligible = item.Revenge || item.Mythical || item.Legendary;
                        if (isEligible)
                        {
                            var chance = Math.Round(1 / probability, 2);
                            var roll = Math.Round(c / probability, 2);

                            if (roll > chance * 0.8)
                                player.SendInfo($"You have rolled: {roll}/{chance} for: {item.DisplayId ?? item.ObjectId}");
                        }

                        if (c >= probability)
                            continue;

                        if (item == null)
                        {
                            player.SendError($"There was a error giving u the item: {i.Item}, please report this [#2]");
                            continue;
                        }

                        drops.Add(item);
                    }
                }

                privDrops[player] = drops;
            }

            foreach (var priv in privDrops)
                if (priv.Value.Count > 0)
                    ProcessPrivateBags(enemy, priv.Value, enemy.GameServer, priv.Key);
        }

        private static void ProcessPublicDrops(List<Item> drops, Enemy enemy)
        {
            var bagType = 0;
            var idx = 0;
            var items = new Item[8];
            foreach (var i in drops)
            {
                if (i.BagType > bagType)
                    bagType = i.BagType;

                items[idx] = i;
                idx++;
                if (idx == 8)
                {
                    DropBag(enemy, new int[] { }, bagType, items, false);
                    idx = 0;
                    items = new Item[8];
                    bagType = 0;
                }
            }
            if (idx > 0)
                DropBag(enemy, new int[] { }, bagType, items, false);
        }

        private static void ProcessPrivateBags(Enemy enemy, IEnumerable<Item> loots, GameServer core, params Player[] owners)
        {
            var player = owners[0] ?? null;
            var idx = 0;
            var bagType = 0;
            var items = new Item[8];
            var boosted = false;

            if (owners.Count() == 1 && GetPlayerLootBoost(player) > 1.0)
                boosted = true;

            if (player.TalismanCanOnlyGetWhiteBags)
            {
                var isWhiteBag = false;
                foreach (var i in loots)
                {
                    if (i.BagType >= 7)
                    {
                        isWhiteBag = true;
                        break;
                    }
                }

                if (!isWhiteBag)
                    return;
            }

            foreach (var i in loots)
            {
                if (i.BagType > bagType)
                    bagType = i.BagType;

                var isEligible = i.Revenge || i.Mythical || i.Legendary;
                if (player != null && isEligible)
                {
                    var chat = core.ChatManager;
                    var world = player.World;
                    var isMythical = i.Revenge || i.Mythical;

                    player.Client.SendPacket(new GlobalNotification() { Text = isMythical ? "revloot" : "legloot" });

                    #region Discord Bot Message

                    if (!player.IsAdmin)
                    {
                        var discord = core.Configuration.discordIntegration;
                        var players = world.Players.Count(p => p.Value.Client != null);

                        try
                        {
                            var builder = discord.MakeLootBuilder(
                                core.Configuration.serverInfo,
                                player.World.IsRealm ? player.World.DisplayName : player.World.IdName,
                                players,
                                world.MaxPlayers,
                                world.InstanceType == WorldResourceInstanceType.Dungeon,
                                isMythical ? "Mythical" : "Legendary",
                                isMythical ? discord.mtBagImage : discord.lgBagImage,
                                isMythical ? discord.mtImage : discord.lgImage,
                                player.Name,
                                player.Client.Rank.Rank,
                                player.Stars,
                                i.ObjectId,
                                player.ObjectDesc.ObjectId,
                                player.Level,
                                player.Fame,
                                player.GetMaxedStats()
                            );

                            if (discord.CanSendLootNotification(player.Stars, player.ObjectDesc.ObjectId.ToLower()) && builder.HasValue)
#pragma warning disable
                                discord.SendWebhook(discord.webhookLootEvent, builder.Value);
#pragma warning restore
                        }
                        catch (Exception ex) 
                        {
                            Console.WriteLine($"Failed to call discord.MakeLootBuilder {ex}");
                        }
                    }

                    #endregion Discord Bot Message

                    if (player != null)
                    {
                        //<LootNotifier> [PlayerName] has obtained a <Legendary/Revenge/Mythical> Item [ItemName], with [PercentageOfDamage]% damage dealt!
                        var msg = new StringBuilder($"[{player.Client.Account.Name}] has obtained ");
                        if (i.Revenge)
                            msg.Append("a Revenge");
                        else if (i.Legendary)
                            msg.Append("a Legendary");
                        else if (i.Mythical)
                            msg.Append("a Mythical");
                        else if (i.Eternal)
                            msg.Append("an Eternal");

                        var hitters = enemy.DamageCounter.GetHitters();
                        msg.Append($" [{i.DisplayId ?? i.ObjectId}], by doing {Math.Round(100.0 * (hitters[owners[0]] / (double)enemy.DamageCounter.TotalDamage), 0)}% damage!");
                        chat.AnnounceLoot(msg.ToString());
                    }
                }

                items[idx] = i;
                idx++;

                if (idx == 8)
                {
                    DropBag(enemy, owners.Select(x => x.AccountId).ToArray(), bagType, items, boosted);
                    items = new Item[8];
                    idx = 0;
                    bagType = 0;
                }
            }

            if (idx > 0)
                DropBag(enemy, owners.Select(x => x.AccountId).ToArray(), bagType, items, boosted);
        }

        private static void DropBag(Enemy enemy, int[] owners, int bagType, Item[] items, bool boosted)
        {
            ushort bag = BAG_ID_TO_TYPE[0];
            if (bagType > 0)
                bag = BAG_ID_TO_TYPE[bagType];

            // Boosted bags
            if (boosted)
                bag = BOOSTED_BAG_ID_TO_TYPE[bagType];

            var container = new Container(enemy.GameServer, bag, 1500 * 60, true);

            for (int j = 0; j < 8; j++)
            {
                if (items[j] != null && items[j].Quantity > 0 && items[j].QuantityLimit > 0)
                    container.Inventory.Data[j] = new ItemData()
                    {
                        Stack = items[j].Quantity,
                        MaxStack = items[j].QuantityLimit
                    };
                container.Inventory[j] = items[j];
            }

            container.BagOwners = owners;
            container.Move(enemy.X + (float)((enemy.World.Random.NextDouble() * 2 - 1) * 0.5), enemy.Y + (float)((enemy.World.Random.NextDouble() * 2 - 1) * 0.5));
            container.SetDefaultSize(bagType >= 6 ? 120 : bagType >= 3 ? 90 : 70);
            enemy.World.EnterWorld(container);
        }
    }

    public class LootDef
    {
        public string Item;
        public double Probabilty;
        public double Threshold;
        public int Tier;
        public ItemType ItemType;

        public LootDef(string item, double probabilty, double threshold, int tier = -1, ItemType itemType = ItemType.None)
        {
            Item = item;
            Probabilty = probabilty;
            Threshold = threshold;
            Tier = tier;
            ItemType = itemType;
        }
    }
}
