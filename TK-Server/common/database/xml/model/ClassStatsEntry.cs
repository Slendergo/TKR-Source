using common.database.info;
using System.Xml.Linq;

namespace common.database.xml.model
{
    public struct ClassStatsEntry
    {
        public int BestFame { get; private set; }
        public int BestLevel { get; private set; }
        public ushort ObjectType { get; private set; }

        public static ClassStatsEntry Serialize(ClassStatInfo info, ushort type) => new ClassStatsEntry()
        {
            ObjectType = type,
            BestLevel = info.BestLevel,
            BestFame = info.BestFame
        };

        public XElement ToXml() => new XElement("ClassStats",
            new XAttribute("objectType", ObjectType.To4Hex()),
            new XElement("BestLevel", BestLevel),
            new XElement("BestFame", BestFame)
        );
    }
}
