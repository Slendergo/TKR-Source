namespace wServer.core.objects
{
    internal class GuildHallPortal : StaticObject
    {
        public GuildHallPortal(CoreServerManager manager, ushort objType, int? life) : base(manager, objType, life, false, true, false)
        { }
    }
}
