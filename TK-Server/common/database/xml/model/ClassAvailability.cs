using common.database.model;
using common.resources;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace common.database.xml.model
{
    public class ClassAvailability
    {
        private static Dictionary<string, string> _classAvailability;
        private static Dictionary<ushort, string> _classes;

        public Dictionary<string, string> Classes { get; private set; }

        public static void Init(Resources resources)
        {
            _classes = resources.GameData.ObjectDescs.Values.Where(objDesc => objDesc.Player)
                .ToDictionary(objDesc => objDesc.ObjectType, objDesc => objDesc.ObjectId);
            _classAvailability = _classes.ToDictionary(@class => @class.Value, @class => "available");
        }

        public static async Task<ClassAvailability> SerializeAsync(RedisDb redis, Resources resources, AccountModel account)
        {
            var classes = _classAvailability.Keys.ToDictionary(id => id, id => _classAvailability[id]);
            var stats = new ClassStatsModel();

            await stats.InitAsync(redis.Db, account.Id.ToString());

            foreach (var type in resources.GameData.Classes.Keys)
            {
                var stat = stats[type];

                if (stat.IsNull)
                    continue;

                classes[_classes[type]] = "unrestricted";
            }

            return new ClassAvailability() { Classes = classes };
        }

        public XElement ToXml()
        {
            var elem = new XElement("ClassAvailabilityList");

            foreach (var @class in Classes.Keys)
            {
                var ca = new XElement("ClassAvailability", Classes[@class]);
                ca.Add(new XAttribute("id", @class));

                elem.Add(ca);
            }

            return elem;
        }
    }
}
