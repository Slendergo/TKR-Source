using System;

namespace wServer.core
{
    public class DbEventArgs : EventArgs
    {
        public DbEventArgs(string message) => Message = message;

        public string Message { get; private set; }
    }
}
