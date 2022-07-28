using common.database.model;
using System;
using System.Linq;
using System.Xml.Linq;

namespace common.database.xml.model
{
    public struct Character
    {
        public int Attack { get; private set; }
        public int CharacterId { get; private set; }
        public int CurrentFame { get; private set; }
        public bool Dead { get; private set; }
        public int Defense { get; private set; }
        public int Dexterity { get; private set; }
        public ushort[] Equipment { get; private set; }
        public int Exp { get; private set; }
        public bool HasBackpack { get; private set; }
        public int HealthStackCount { get; private set; }
        public int HitPoints { get; private set; }
        public int HpRegen { get; private set; }
        public int Level { get; private set; }
        public int MagicPoints { get; private set; }
        public int MagicStackCount { get; private set; }
        public int MaxHitPoints { get; private set; }
        public int MaxMagicPoints { get; private set; }
        public int MpRegen { get; private set; }
        public ushort ObjectType { get; private set; }
        public FameStats PCStats { get; private set; }
        public int Skin { get; private set; }
        public int Speed { get; private set; }
        public int Tex1 { get; private set; }
        public int Tex2 { get; private set; }

        public static Character Serialize(CharacterModel character, bool dead) => new Character()
        {
            CharacterId = character.Id,
            ObjectType = character.ObjectType,
            Level = character.Level,
            Exp = character.Experience,
            CurrentFame = character.Fame,
            Equipment = character.Items,
            MaxHitPoints = character.Stats[0],
            MaxMagicPoints = character.Stats[1],
            Attack = character.Stats[2],
            Defense = character.Stats[3],
            Speed = character.Stats[4],
            Dexterity = character.Stats[5],
            HpRegen = character.Stats[6],
            MpRegen = character.Stats[7],
            HitPoints = character.HP,
            MagicPoints = character.MP,
            Tex1 = character.Texture1,
            Tex2 = character.Texture2,
            Skin = character.Skin,
            PCStats = FameStats.Read(character.FameStats),
            HealthStackCount = character.HealthPotions,
            MagicStackCount = character.MagicPotions,
            Dead = dead,
            HasBackpack = character.HasBackpack
        };

        public XElement ToXml() => new XElement("Char",
            new XAttribute("id", CharacterId),
            new XElement("ObjectType", ObjectType),
            new XElement("Level", Level),
            new XElement("Exp", Exp),
            new XElement("CurrentFame", CurrentFame),
            new XElement("Equipment", Equipment.Select(x => (short)x).ToArray().ToCommaSepString()),
            new XElement("MaxHitPoints", MaxHitPoints),
            new XElement("HitPoints", HitPoints),
            new XElement("MaxMagicPoints", MaxMagicPoints),
            new XElement("MagicPoints", MagicPoints),
            new XElement("Attack", Attack),
            new XElement("Defense", Defense),
            new XElement("Speed", Speed),
            new XElement("Dexterity", Dexterity),
            new XElement("HpRegen", HpRegen),
            new XElement("MpRegen", MpRegen),
            new XElement("Tex1", Tex1),
            new XElement("Tex2", Tex2),
            new XElement("Texture", Skin),
            new XElement("PCStats", Convert.ToBase64String(PCStats.Write())),
            new XElement("HealthStackCount", HealthStackCount),
            new XElement("MagicStackCount", MagicStackCount),
            new XElement("Dead", Dead),
            new XElement("HasBackpack", HasBackpack ? "1" : "0")
        );
    }
}
