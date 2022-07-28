using common.database.extension;
using common.database.model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace common.database.xml.model
{
    public struct Legends
    {
        private IEnumerable<LegendEntry> _legends;
        private LegendType _type;

        public static async Task<Legends> SerializeAsync(RedisDb redis, LegendType type)
        {
            var timeSpan = type.ToField();
            var service = redis.GetLegendService;
            var infos = service[type];
            var entries = new List<LegendEntry>();

            for (var i = 0; i < infos.Length; i++)
            {
                var character = new CharacterModel();
                var name = await redis.Db.ReadAsync<string>($"account.{infos[i].AccountId}", "name");

                await character.InitAsync(redis.Db, infos[i].AccountId.ToString(), infos[i].CharacterId.ToString());

                entries.Add(await LegendEntry.SerializeAsync(redis, character, name));
            }

            return new Legends()
            {
                _type = type,
                _legends = entries
            };
        }

        public XElement ToXml() => new XElement("FameList", new XAttribute("timespan", _type.ToField()), _legends.Select(x => x.ToXml()));
    }
}
