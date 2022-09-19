using System.Xml.Linq;

namespace TKR.WorldServer.core.objects
{
    public class Wall : StaticObject
    {
        public Wall(GameServer manager, ushort objType, int? hp) : base(manager, objType, hp, true, false, true)
        {
        }
    }
}
