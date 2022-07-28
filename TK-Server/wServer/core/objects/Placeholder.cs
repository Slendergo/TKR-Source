namespace wServer.core.objects
{
    internal class Placeholder : StaticObject
    {
        public Placeholder(CoreServerManager manager, int life) : base(manager, 0x070f, life, true, true, false) => SetDefaultSize(0);
    }
}
