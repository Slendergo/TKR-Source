using common;
using common.resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace EyeOfTheMetis
{
    public sealed class MetisDb : Database
    {
        /// <summary>
        /// Special credits to contributors that gave me an updated list:
        /// Old ( @TLC#1911 ) and Kaoran ( @Kaoran#6254 ) .
        /// </summary>
        private static readonly List<string> MostValueableItemsName = new List<string>()
        {
            "Cannon", "Wind Carrier", "Earth's Power", "Rod of Fire", "The Ancient Wand of Fury",
            "Ruthven's Fury", "Tiki's Breastplate", "Mushroom's Cloth", "Magicians Hide",
            "Sentinel's Seal", "Geb's Hand", "Bow of the Havens", "Twilight Skull",
            "Shield of The Ancient's", "Talisman of Luck", "Talisman of Mana", "Gem of Life",
            "Marble's Hand", "The Horn Breaker", "Ghost Robe", "Cape of Septavius",
            "Megamoth's Scepter", "Broken Oryx Helm", "Broken Oryx Armor", "Oryx's Horn",
            "Lodius", "Bloody Katana", "Staff of Unholy Sacrifice", "Dagger of Flaming Fury",
            "Omnipotence Ring", "Strange Poison", "Sword of the Colossus", "Velocity Bow",
            "Magical Lodestone", "Breastplate of New Life", "Sourcestone", "Marble Seal",
            "Quiver of the Shadows", "Bow of the Void", "Skull of Corrupted Souls",
            "Ritual Robe", "Bloodshed Ring", "Armor of Nil", "Colossus Essence",
            "Tome of Universal Theory", "Wand of Pain", "Orb of the Fire Element Egg",
            "Queen's Prism"
        };

        public List<Item> AllMostValuableItems { get; private set; }
        public List<AccountData> AllAccountDatas { get; private set; }

        public static ManualResetEvent Loading;

        public MetisDb(Action<string, bool> log)
            : base(
                  new Resources("./resources", null,
                      (current, total, from, breakline)
                      => GetProgressInfo(log, from, current, total, breakline)),
                  new ServerConfig() { dbInfo = new DbInfo() },
                  true,
                  Loading = new ManualResetEvent(false)
            )
        {
        }

        public void SerializeBigData(Action<string> log)
        {
            AllMostValuableItems = GetAllMostValuableItems().ToList();

            log.Invoke(FormatEntry("most valuable items", AllMostValuableItems.Count));

            AllAccountDatas = GetAllAccountDatas(log);

            log.Invoke(FormatEntry("account datas", AllAccountDatas.Count));
        }

        public string TagToTierName(Item item)
            => item.Legendary ? "LG" : item.Revenge ? "RG" : "??";

        private IEnumerable<Item> GetAllMostValuableItems()
        {
            var ids = _resources.GameData.IdToObjectType;
            var items = _resources.GameData.Items;
            var objectTypes = new List<ushort>();

            for (var i = 0; i < MostValueableItemsName.Count; i++)
                objectTypes.Add(ids[MostValueableItemsName[i]]);

            for (var j = 0; j < objectTypes.Count; j++)
                yield return items[objectTypes[j]];
        }

        private List<AccountData> GetAllAccountDatas(Action<string> log)
        {
            var allAccounts = Enumerable.Range(1, int.Parse(_db.StringGet("nextAccId")));
            var current = 1;
            var total = allAccounts.Count();
            void logger(float cur, bool breakline)
                => GetProgressInfo((message, writeType)
                => log.Invoke(message), "AccountDatas <AsyncTask>", cur, total, breakline);

            var allAccountDatas = allAccounts.AsParallel().AsOrdered().Select(accountId =>
            {
                current = accountId;
                logger(current, current == total);

                var account = new DbAccount(_db, accountId, null, true);
                var characters = GetAliveCharacters(account)
                    .Select(characterId => new DbChar(account, characterId, true))
                    .ToList();
                var vault = new DbVault(account, true);

                return new AccountData(account, characters, vault);
            }).ToList();

            return allAccountDatas;
        }

        #region "MetisDb Utils"

        private static void GetProgressInfo(
            Action<string, bool> log,
            string from,
            float current,
            float total,
            bool breakline
            )
            => log.Invoke(
                $"[{(current / total).ToString("00.00%")}]\t" +
                $"{current} of {total} pending - " +
                $"loaded: {from}{(breakline ? "\n\n" : "")}",
                true
            );

        private static string FormatEntry(string prefix, int total)
            => string.Format(
                " - {0}:\n\t{1} entr{2}",
                prefix, total, total > 1 ? "ies" : "y"
            );

        #endregion "MetisDb Utils"
    }
}