using System.Xml.Linq;

namespace wServer.core.objects
{
    public class Wall : StaticObject
    {
        public Wall(CoreServerManager manager, ushort objType, XElement node) : base(manager, objType, GetHP(node), true, false, true)
        { }
    }
}
