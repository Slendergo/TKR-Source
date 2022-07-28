using common.resources;
using System.Xml.Linq;

namespace common.database.xml.model
{
    public struct ItemCosts
    {
        private static XElement ItemCostsXml;

        public static void Init(Resources resources)
        {
            var elem = new XElement("ItemCosts");

            foreach (var skin in resources.GameData.Skins.Values)
            {
                var ca = new XElement("ItemCost", skin.Cost);
                ca.Add(new XAttribute("type", skin.Type));

                elem.Add(ca);
            }

            ItemCostsXml = elem;
        }

        public static XElement ToXml() => ItemCostsXml;
    }
}
