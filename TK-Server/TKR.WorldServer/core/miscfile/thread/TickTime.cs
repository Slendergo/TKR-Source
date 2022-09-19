namespace TKR.WorldServer.core.miscfile.thread
{
    public struct TickTime
    {
        public int LogicTime;
        public int ElapsedMsDelta;
        public long TickCount;
        public long TotalElapsedMs;
        public float DeltaTime => ElapsedMsDelta * 0.001f;
        public float BehaviourTickTime => DeltaTime * 5.0f;
    }
}
