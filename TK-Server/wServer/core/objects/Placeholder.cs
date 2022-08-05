namespace wServer.core.objects
{
    internal class Placeholder : StaticObject
    {
        public Placeholder(GameServer manager, int life) : base(manager, 0x070f, life, true, true, false) => SetDefaultSize(0);
    }
}
