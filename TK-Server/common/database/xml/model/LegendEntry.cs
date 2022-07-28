using common.database.model;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace common.database.xml.model
{
    public struct LegendEntry
    {
        public int AccountId { get; private set; }
        public int CharId { get; private set; }
        public ushort[] Equipment { get; private set; }
        public string Name { get; private set; }
        public ushort ObjectType { get; private set; }
        public int Skin { get; private set; }
        public int Tex1 { get; private set; }
        public int Tex2 { get; private set; }
        public int TotalFame { get; private set; }

        public static async Task<LegendEntry> SerializeAsync(RedisDb redis, CharacterModel model, string name)
        {
            var death = new DeathModel();

            await death.InitAsync(redis.Db, model.AccountId.ToString(), model.Id.ToString());

            return new LegendEntry()
            {
                AccountId = model.AccountId,
                CharId = model.Id,
                Name = name,
                ObjectType = model.ObjectType,
                Tex1 = model.Texture1,
                Tex2 = model.Texture2,
                Skin = model.Skin,
                Equipment = model.Items,
                TotalFame = death.TotalFame
            };
        }

        public XElement ToXml() => new XElement("FameListElem",
            new XAttribute("accountId", AccountId),
            new XAttribute("charId", CharId),
            new XElement("Name", Name),
            new XElement("ObjectType", ObjectType),
            new XElement("Tex1", Tex1),
            new XElement("Tex2", Tex2),
            new XElement("Texture", Skin),
            new XElement("Equipment", Equipment.Select(x => (short)x).ToArray().ToCommaSepString()),
            new XElement("TotalFame", TotalFame)
        );
    }
}
