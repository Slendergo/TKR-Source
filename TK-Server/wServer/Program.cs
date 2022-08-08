using System;
using System.Collections.Generic;
using wServer.core;

namespace wServer
{
    public sealed class Program
    {
        private static void Main(string[] args)
        {
            new GameServer(args).Run();
        }
    }
}
