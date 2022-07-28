using common.database.model;
using common.resources;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace common.database.xml.model
{
    public struct MaxClassLevelList
    {
        private static List<ushort> Classes;

        private ClassStatsModel _classStats;

        public static void Init(Resources resources) => Classes = resources.GameData.ObjectDescs.Values.Where(objDesc => objDesc.Player).Select(objDesc => objDesc.ObjectType).ToList();

        public static async Task<MaxClassLevelList> SerializeAsync(RedisDb redis, AccountModel model)
        {
            var stats = new ClassStatsModel();

            await stats.InitAsync(redis.Db, model.Id.ToString());

            return new MaxClassLevelList() { _classStats = stats };
        }

        public XElement ToXml()
        {
            var elem = new XElement("MaxClassLevelList");

            foreach (var type in Classes)
            {
                var ca = new XElement("MaxClassLevel");
                ca.Add(new XAttribute("maxLevel", _classStats[type].BestLevel));
                ca.Add(new XAttribute("classType", type));

                elem.Add(ca);
            }

            return elem;
        }
    }
}
