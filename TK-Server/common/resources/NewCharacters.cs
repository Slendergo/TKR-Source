using System.Xml.Linq;

namespace common.resources
{
    public class NewCharacters
    {
        public readonly int Level;
        public readonly bool Maxed;

        public NewCharacters(XElement e)
        {
            Maxed = e.HasElement("Maxed");
            Level = e.GetValue("Level", 1);
        }
    }
}
