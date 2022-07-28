using System.Xml.Linq;

namespace common.resources
{
    public class UnlockClass
    {
        public readonly uint? Cost;
        public readonly ushort? Level;
        public readonly ushort? Type;

        public UnlockClass(XElement e)
        {
            var n = e.Element("UnlockLevel");

            if (n != null && n.HasAttribute("type") && n.HasAttribute("level"))
            {
                Type = n.GetAttribute<ushort>("type");
                Level = n.GetAttribute<ushort>("level");
            }

            n = e.Element("UnlockCost");

            if (n != null)
                Cost = (uint)int.Parse(n.Value);
        }
    }
}
