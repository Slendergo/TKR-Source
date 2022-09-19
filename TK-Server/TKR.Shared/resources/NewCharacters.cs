using System.Xml.Linq;
using TKR.Shared;

namespace TKR.Shared.resources
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
