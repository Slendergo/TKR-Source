using System.Xml.Linq;

namespace common.resources
{
    public class NewCharacters
    {
        public readonly int Level;

        public NewCharacters(XElement e)
        {
            Level = e.GetValue("Level", 1);
        }
    }
}
