using TKR.WorldServer.core;

namespace TKR.WorldServer
{
    public sealed class Program
    {
        private static void Main(string[] args)
        {
            new GameServer(args).Run();
        }
    }
}
