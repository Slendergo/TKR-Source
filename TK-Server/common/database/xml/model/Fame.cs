using common.database.info;
using common.database.model;
using common.resources;
using System;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace common.database.xml.model
{
    public class Fame
    {
        public FameBonusInfo[] Bonuses { get; private set; }
        public Character Character { get; private set; }
        public DateTime DeathTime { get; private set; }
        public bool FirstBorn { get; private set; }
        public string Killer { get; private set; }
        public string Name { get; private set; }
        public FameStats Stats { get; private set; }
        public int TotalFame { get; private set; }

        public static async Task<Fame> SerializeAsync(RedisDb redis, Resources resources, CharacterModel character, string name)
        {
            var death = new DeathModel();

            await death.InitAsync(redis.Db, character.AccountId.ToString(), character.Id.ToString());

            if (!death.EntryExist)
                return null;

            var stats = FameStats.Read(character.FameStats);

            return new Fame()
            {
                Name = name,
                Character = Character.Serialize(character, true),
                Stats = stats,
                Bonuses = stats.CalculateBonusesFull(resources.GameData, character, death.IsFirstBorn),
                TotalFame = death.TotalFame,
                FirstBorn = death.IsFirstBorn,
                DeathTime = death.DeathTime,
                Killer = death.Killer
            };
        }

        public XElement ToXml() => new XElement("Fame",
            GetCharElem(),
            new XElement("BaseFame", Character.CurrentFame),
            new XElement("TotalFame", TotalFame),
            new XElement("Shots", Stats.Shots),
            new XElement("ShotsThatDamage", Stats.ShotsThatDamage),
            new XElement("SpecialAbilityUses", Stats.SpecialAbilityUses),
            new XElement("TilesUncovered", Stats.TilesUncovered),
            new XElement("Teleports", Stats.Teleports),
            new XElement("PotionsDrunk", Stats.PotionsDrunk),
            new XElement("MonsterKills", Stats.MonsterKills),
            new XElement("MonsterAssists", Stats.MonsterAssists),
            new XElement("GodKills", Stats.GodKills),
            new XElement("GodAssists", Stats.GodAssists),
            new XElement("CubeKills", Stats.CubeKills),
            new XElement("OryxKills", Stats.OryxKills),
            new XElement("QuestsCompleted", Stats.QuestsCompleted),
            new XElement("PirateCavesCompleted", Stats.PirateCavesCompleted),
            new XElement("UndeadLairsCompleted", Stats.UndeadLairsCompleted),
            new XElement("AbyssOfDemonsCompleted", Stats.AbyssOfDemonsCompleted),
            new XElement("SnakePitsCompleted", Stats.SnakePitsCompleted),
            new XElement("SpiderDensCompleted", Stats.SpiderDensCompleted),
            new XElement("SpriteWorldsCompleted", Stats.SpriteWorldsCompleted),
            new XElement("LevelUpAssists", Stats.LevelUpAssists),
            new XElement("MinutesActive", Stats.MinutesActive),
            new XElement("TombsCompleted", Stats.TombsCompleted),
            new XElement("TrenchesCompleted", Stats.TrenchesCompleted),
            new XElement("JunglesCompleted", Stats.JunglesCompleted),
            new XElement("ManorsCompleted", Stats.ManorsCompleted),
            Bonuses.Select(_ =>
                new XElement("Bonus",
                    new XAttribute("id", _.Name),
                    new XAttribute("desc", _.Description),
                    _.Bonus
                )
            ),
            new XElement("CreatedOn", DeathTime.ToUnixTimestamp()),
            new XElement("KilledBy", Killer)
        );

        private XElement GetCharElem()
        {
            var ret = Character.ToXml();
            ret.Add(new XElement("Account", new XElement("Name", Name)));

            return ret;
        }
    }
}
