using System;

namespace wServer.core
{
    public sealed class ReconInfo
    {
        public byte[] Key;

        public ReconInfo(int dest, byte[] key, DateTime timeout)
        {
            Destination = dest;
            Key = key;
            Timeout = timeout;
        }

        public int Destination { get; }
        public DateTime Timeout { get; }
    }
}
