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
        private Random Rand = new Random();
        private readonly static List<MobDrops> ChestItems = new List<MobDrops>();

        public ChestLoot(params MobDrops[] drops) => ChestItems.AddRange(ChestItems);

        public IEnumerable<Item> CalculateItems(GameServer core, int min, int max)
        {
            var consideration = new List<LootDef>();
            foreach (var i in ChestItems)
                i.Populate(consideration);

            var retCount = Rand.Next(min, max);

            foreach (var i in consideration)
            {
                if (Rand.NextDouble() < i.Probabilty)
                {
                    yield return i.Item;
                    retCount--;
                }

                if (retCount == 0)
                    yield break;
            }
        }
    }

    public class Loot : List<MobDrops>
    {
        #region Configure DropRates

        public sealed class DropRates
        {
            public static double FRAGMENT_THRESHOLD;
            public static double FRAGMENTS;
            public static string[] FRAGMENTS_NAMES;

            public static double LG;
            public static double LG_THRESHOLD;

            public static double MT;
            public static double MT_THRESHOLD;

            public static double ORYX_ITEMS;
            public static string[] ORYX_ITEMS_NAMES;
            public static double ORYX_ITEMS_THRESHOLD;

            public static double LG_TALISMAN;
            public static string[] LG_TALISMAN_NAMES;
            public static double LG_TALISMAN_THRESHOLD;

            public static double MT_TALISMAN;
            public static string[] MT_TALISMAN_NAMES;
            public static double MT_TALISMAN_THRESHOLD;

            public static double HARD_BOUNTY;
            public static string[] HARD_BOUNTY_NAMES;
            public static double HARD_BOUNTY_THRESHOLD;
        }

        public static void ConfigureDropRates()
        {
            // configure drop rates
            DropRates.MT = 1.0 / 800d;
            DropRates.LG = 1.0 / 700d; // 1/700
            DropRates.FRAGMENTS = 1.0 / 700d;
            DropRates.ORYX_ITEMS = 1.0 / 1000d;
            DropRates.LG_TALISMAN = 1.0 / 1500d;
            DropRates.MT_TALISMAN = 1.0 / 1300d;
            DropRates.HARD_BOUNTY = 1.0 / 700d;
            //configure threshold
            DropRates.MT_THRESHOLD = 0.05; // 5%
            DropRates.LG_THRESHOLD = 0.03; // 3%
            DropRates.FRAGMENT_THRESHOLD = 0.05; // 5%
            DropRates.ORYX_ITEMS_THRESHOLD = 0.05; // 5%
            DropRates.LG_TALISMAN_THRESHOLD = 0.06; // 6%
            DropRates.MT_TALISMAN_THRESHOLD = 0.1; // 10%
            DropRates.HARD_BOUNTY_THRESHOLD = 0.5; // 10%

            // configure custom items
            DropRates.ORYX_ITEMS_NAMES = new[]
            {
                "Oryx's Armor of War",
                "Oryx's Broken Helm",
                "Shattered Horn of Oryx",
                "The Horn Breaker"
            };
            DropRates.FRAGMENTS_NAMES = new[]
            {
                "Fire Fragment",
                "Water Fragment",
                "Earth Fragment",
                "Wind Fragment"
            };
            DropRates.LG_TALISMAN_NAMES = new[]
            {
                "Gem of Life",
                "Severed Marble Hand",
                "Talisman of Luck",
                "Talisman of Mana"
            };
            DropRates.MT_TALISMAN_NAMES = new[]
            {
                "Cerberus's Right Claw",
                "Cerberus's Left Claw"
            };
            DropRates.HARD_BOUNTY_NAMES = new[]
            {
                "Raven's Head",
                "Dark Skeleton's Shield",
                "Thanatos's Garments",
                "Heart of the Beast",
                "Beast Gem",
                "Cerberus's Ribcage"
            };
        }

        #endregion Configure DropRates

        #region Utils

        /*  Brown 0,  Pink 1,   Purple 2, Gold 3,   Cyan 4,   Blue 5,   Orange 6, White 7,  Mythical 8, Eternal 9 */
        private static readonly ushort[] BAG_ID_TO_TYPE = new ushort[] { 0x0500, 0x0506, 0x0503, 0x0532, 0x0509, 0x050B, 0x0533, 0x050C, 0x5076, 0xa002 };
        /*  Brown 0,  Pink 1,   Purple 2, Gold 3,   Cyan 4,   Blue 5,   Orange 6, White 7,  Mythical 8, Eternal 9 */
        private static readonly ushort[] BOOSTED_BAG_ID_TO_TYPE = new ushort[] { 0x0534, 0x0535, 0x0536, 0x0537, 0x0538, 0x0539, 0x053b, 0x053a, 0x5077, 0xa003 };

        private static readonly int[] AbilityT = new int[] { 4, 5, 11, 12, 13, 15, 16, 18, 19, 20, 21, 22, 23, 25, };
        private static readonly int[] ArmorT = new int[] { 6, 7, 14, };
        private static readonly int[] RingT = new int[] { 9 };
        private static readonly int[] WeaponT = new int[] { 1, 2, 3, 8, 17, 24 };

        public static bool DropInSoulboundBag(Item item)
        {
            foreach (var weaponid in WeaponT)
                if (item.SlotType == weaponid)
                    if (item.Tier != null && item.Tier >= 8 && !(item.Tier <= 3))
                        return true;

            foreach (var abilityId in AbilityT)
                if (item.SlotType == abilityId)
                    if (item.Tier != null && item.Tier >= 3 && !(item.Tier <= 3))
                        return true;

            foreach (var armorId in ArmorT)
                if (item.SlotType == armorId)
                    if (item.Tier != null && item.Tier >= 8 && !(item.Tier <= 3))
                        return true;

            foreach (var ringId in RingT)
                if (item.SlotType == ringId)
                    if (item.Tier != null && item.Tier >= 3 && !(item.Tier <= 2))
                        return true;

            return false;
        }

        private static int ReturnBagType(Item i, int bType)
        {
            var bagType = bType;

            if (i.BagType > bagType) bagType = i.BagType;

            if (DropInSoulboundBag(i) == false && bagType < 1) bagType = 1; //pink

            if (DropInSoulboundBag(i) && bagType < 2) bagType = 2; //purple

            if (i.ObjectType == 0x5060 && bagType < 3) bagType = 3;

            if (i.Legendary && bagType < 7) bagType = 7;

            if ((i.Revenge || i.Mythical || i.Eternal) && bagType < 8) bagType = 8;

            return bagType;
        }

        public static bool CheckTalismanOfLuck(Player player, string item)
        {
            var talismanofLuck = player.Inventory.Where(i => i != null && i.ObjectId == item).FirstOrDefault();
            return talismanofLuck != default;
        }

        public static double CheckTalismans(Player player)
        {
            var talismans = player.Inventory.Count(i => i != null && i.ObjectId == "Talisman of Looting");
            return talismans == 1 ? (talismans * 0.02f) : (talismans * 0.02f) - (talismans * 0.005f);
        }

        public static double GetPlayerLootBoost(Player player)
        {
            if (player == null) 
                return 0;

            var core = player.GameServer;

            var db = core.Database;
            //var account = db.GetAccount(player.AccountId);
            //var guild = db.GetGuild(account.GuildId);

            var allLoot = 0.0;
            allLoot += player.LDBoostTime > 0 ? 0.1 : 0;
            allLoot += ((player.Node5TickMaj * 0.05) + (player.Node5TickMin * 0.025) + (player.Node5Med * 0.075) + (player.Node5Big > 1 ? 0.2 : player.Node5Big > 0 ? 0.1 : 0));
            allLoot += CheckTalismans(player);
            allLoot += CheckTalismanOfLuck(player, "Talisman of Luck") ? 0.2 : 0; //20%
            return allLoot;
        }

        private List<LootDef> GetEnemyClasifiedLoot(List<LootDef> list, Enemy enemy)
        {
            #region Rarity Enemies Loot

            var gameData = enemy.GameServer.Resources.GameData;
            var xmlitem = gameData.Items;
            var itemtoid = gameData.IdToObjectType;

            if (enemy.Legendary)
            {
                list.Add(new LootDef(xmlitem[itemtoid["Potion Dust"]], 0.25, 0.001));
                list.Add(new LootDef(xmlitem[itemtoid["Item Dust"]], 0.025, 0.001));
                list.Add(new LootDef(xmlitem[itemtoid["Miscellaneous Dust"]], 0.02, 0.001));
                list.Add(new LootDef(xmlitem[itemtoid["Special Dust"]], 0.003, 0.001));
            }
            else if (enemy.Epic)
            {
                list.Add(new LootDef(xmlitem[itemtoid["Potion Dust"]], 0.25, 0.001));
                list.Add(new LootDef(xmlitem[itemtoid["Item Dust"]], 0.015, 0.001));
                list.Add(new LootDef(xmlitem[itemtoid["Miscellaneous Dust"]], 0.01, 0.001));
                list.Add(new LootDef(xmlitem[itemtoid["Special Dust"]], 0.002, 0.001));
            }
            else if (enemy.Rare)
            {
                list.Add(new LootDef(xmlitem[itemtoid["Potion Dust"]], 0.5, 0.001));
                list.Add(new LootDef(xmlitem[itemtoid["Item Dust"]], 0.01, 0.001));
                list.Add(new LootDef(xmlitem[itemtoid["Miscellaneous Dust"]], 0.005, 0.001));
                list.Add(new LootDef(xmlitem[itemtoid["Special Dust"]], 0.001, 0.001));
            }

            #endregion Rarity Enemies Loot

            return list;
        }

        #endregion Utils

        public static readonly Random Rand = new Random();

        public Loot(params MobDrops[] drops) => AddRange(drops);

        public void Handle(Enemy enemy, TickTime time)
        {
            if (enemy.SpawnedByBehavior) return;

            var possDrops = new List<LootDef>();
            GetEnemyClasifiedLoot(possDrops, enemy);
            foreach (var i in this)
                i.Populate(possDrops);

            var privDrops = new Dictionary<Player, IList<Item>>();
            var pubDrops = new List<Item>();

            foreach (var i in possDrops)
            {
                if (i.Threshold <= 0 && DropInSoulboundBag(i.Item))
                {
                    i.Threshold = 0.01;
                    continue;
                }

                if (i.Threshold <= 0 && Rand.NextDouble() < i.Probabilty)
                    pubDrops.Add(i.Item);
            }
            ProcessPublicDrops(pubDrops, enemy);

            var playersAvaliable = enemy.DamageCounter.GetPlayerData();

            if (playersAvaliable == null) return;

            foreach (var tupPlayer in playersAvaliable)
            {
                var player = tupPlayer.Item1;
                var playerDamage = tupPlayer.Item2;

                if (player == null || player.World == null || player.Client == null)
                    continue;

                var percentageOfDamage = (Math.Round(100.0 * (playerDamage / (double)enemy.DamageCounter.TotalDamage), 4) / 100);
                var DamageBoost = player.Node5Big > 0 ? 0 : percentageOfDamage;
                var enemyRarityPercent = 
                    enemy.Legendary ? 0.05 : // 5%
                    enemy.Epic ? 0.025 : // 2.5 %
                    enemy.Rare ? 0.0125 : 0; // 1.25%
                
                var playerLootBoost = GetPlayerLootBoost(player) + enemyRarityPercent;

                //Console.WriteLine($"Loot Boost: {playerLootBoost}");

                if (enemy.ObjectDesc.Event)
                {
                    player.Stacks[0].Push(player.GameServer.Resources.GameData.Items[0xa22]);
                    player.Stacks[1].Push(player.GameServer.Resources.GameData.Items[0xa23]);
                }

                var drops = new List<Item>();

                foreach (var i in possDrops)
                {
                    var lootBoosts = i.Item.Potion ? i.Probabilty : 
                        i.Probabilty + (i.Probabilty * playerLootBoost);

                    //Console.WriteLine(i.Probabilty + " | " + (i.Probabilty + (i.Probabilty * playerLootBoost)));
                    var chance = Rand.NextDouble();

                    //if (i.Item.Tier == null)
                    //{
                    //    Console.WriteLine("Item: " + i.Item.ObjectId);
                    //    Console.WriteLine("Drop Chance: " + chance);
                    //    Console.WriteLine("Loot Boost: " + lootBoosts);
                    //    Console.WriteLine("Item Probability: " + i.Probabilty);
                    //    Console.WriteLine("Item Threshold: " + i.Threshold + "%");
                    //    Console.WriteLine("Damage Dealt: " + percentageOfDamage + "%");
                    //    Console.WriteLine("\n\n");
                    //}

                    if (i.Threshold >= 0 && i.Threshold < percentageOfDamage && chance < lootBoosts)
                        drops.Add(i.Item);
                }

                privDrops[player] = drops;
            }

            foreach (var priv in privDrops)
                if (priv.Value.Count > 0)
                    ProcessPrivateBags(enemy, priv.Value, enemy.GameServer, priv.Key);
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

            foreach (var i in loots)
            {
                bagType = ReturnBagType(i, bagType);

                var isEligible = i.Revenge || i.Mythical || i.Legendary;

                if (player != null && isEligible)
                {
                    var chat = core.ChatManager;
                    var world = player.World;
                    var isMythical = i.Revenge || i.Mythical;

                    player.Client.SendPacket(new GlobalNotification() { Text = isMythical ? "revloot" : "legloot" });

                    #region Discord Bot Message

                    if (player.Rank <= 60)
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
                                player.Rank,
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
                        msg.Append($" [{i.DisplayId ?? i.ObjectId}], by doing {Math.Round(100.0 * (enemy.DamageCounter.hitters[owners[0]] / (double)enemy.DamageCounter.TotalDamage), 0)}% damage!");
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
            container.Move(enemy.X + (float)((Rand.NextDouble() * 2 - 1) * 0.5), enemy.Y + (float)((Rand.NextDouble() * 2 - 1) * 0.5));
            container.SetDefaultSize(bagType >= 6 ? 120 : bagType >= 3 ? 90 : 70);
            enemy.World.EnterWorld(container);
        }

        private static void ProcessPublicDrops(List<Item> pubDrops, Enemy enemy)
        {
            var bagType = 0;
            var idx = 0;
            var items = new Item[8];
            foreach (var i in pubDrops)
            {
                bagType = ReturnBagType(i, bagType);
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
    }

    public class LootDef
    {
        public readonly Item Item;
        public double Probabilty;
        public double Threshold;

        public LootDef(Item item, double probabilty, double threshold)
        {
            Item = item;
            Probabilty = probabilty;
            Threshold = threshold;
        }
    }
}
