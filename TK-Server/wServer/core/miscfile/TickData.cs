namespace wServer.core
{
    public struct TickData
    {
        public int ElaspedMsDelta;
        public long TickCount;
        public long TotalElapsedMs;
        public float DeltaTime => ElaspedMsDelta / 1000.0f;
    }
}
