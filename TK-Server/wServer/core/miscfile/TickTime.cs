namespace wServer.core
{
    public struct TickTime
    {
        public int LogicTime;
        public int ElaspedMsDelta;
        public long TickCount;
        public long TotalElapsedMs;
        public float DeltaTime => ElaspedMsDelta / 1000.0f;
        public float BehaviourTickTime => (ElaspedMsDelta / 1000.0f) * 5.0f;
    }
}
