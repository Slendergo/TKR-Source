using common.database.model;
using common.resources;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace common.database.xml.model
{
    public struct Stats
    {
        private Dictionary<ushort, ClassStatsEntry> entries;
        public int BestCharFame { get; private set; }
        public int Fame { get; private set; }
        public int TotalFame { get; private set; }
        public ClassStatsEntry this[ushort objType] => entries[objType];

        public static Stats Serialize(Resources resources, AccountModel account, ClassStatsModel stats)
        {
            var ret = new Stats()
            {
                TotalFame = account.FameHistory,
                Fame = account.Fame,
                entries = new Dictionary<ushort, ClassStatsEntry>(),
                BestCharFame = 0
            };

            foreach (var type in resources.GameData.Classes.Keys)
            {
                var entry = ClassStatsEntry.Serialize(stats[type], type);

                if (entry.BestFame > ret.BestCharFame)
                    ret.BestCharFame = entry.BestFame;

                ret.entries[type] = entry;
            }

            return ret;
        }

        public XElement ToXml() => new XElement("Stats",
            entries.Values.Select(x => x.ToXml()),
            new XElement("BestCharFame", BestCharFame),
            new XElement("TotalFame", TotalFame),
            new XElement("Fame", Fame)
        );
    }
}
