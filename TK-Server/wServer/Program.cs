using CA.Threading.Tasks;
using common;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using wServer.core;
using wServer.utils;

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
