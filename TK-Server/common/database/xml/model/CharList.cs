using common.database.model;
using common.resources;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace common.database.xml.model
{
    public struct CharList
    {
        public Account Account { get; private set; }
        public Character[] Characters { get; private set; }
        public ClassAvailability ClassesAvailable { get; private set; }
        public double? Lat { get; set; }
        public double? Long { get; set; }
        public MaxClassLevelList MaxLevelList { get; private set; }
        public int MaxNumChars { get; private set; }
        public int NextCharId { get; private set; }
        public IEnumerable<ServerItem> Servers { get; set; }

        public static async Task<CharList> SerializeAsync(RedisDb redis, Resources resources, AccountModel account)
        {
            var ids = await account.AliveCharacterIdsAsync();
            var characters = new List<Character>();

            for (var i = 0; i < ids.Length; i++)
            {
                var character = new CharacterModel();

                await character.InitAsync(redis.Db, account.Id.ToString(), ids[i].ToString());

                if (character.EntryExist)
                    characters.Add(Character.Serialize(character, false));
            }

            return new CharList()
            {
                Characters = characters.ToArray(),
                NextCharId = account.NextCharId,
                MaxNumChars = account.MaxCharSlot,
                Account = await Account.SerializeAsync(redis, resources, account),
                ClassesAvailable = await ClassAvailability.SerializeAsync(redis, resources, account),
                MaxLevelList = await MaxClassLevelList.SerializeAsync(redis, account)
            };
        }

        public XElement ToXml() => new XElement("Chars",
            new XAttribute("nextCharId", NextCharId),
            new XAttribute("maxNumChars", MaxNumChars),
            Characters.Select(x => x.ToXml()),
            Account.ToXml(),
            ClassesAvailable.ToXml(),
            new XElement("News"),
            new XElement("Servers", Servers.Select(x => x.ToXml())),
            Lat == null ? null : new XElement("Lat", Lat),
            Long == null ? null : new XElement("Long", Long),
            (Account.Skins.Length > 0) ? new XElement("OwnedSkins", Account.Skins.ToCommaSepString()) : null,
            ItemCosts.ToXml(),
            MaxLevelList.ToXml()
        );
    }
}
