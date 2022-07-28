using System;
using wServer.core;
using wServer.core.objects;

namespace wServer.logic.behaviors
{
    internal class JumpToRandomOffset : CycleBehavior
    {
        private readonly int maxX;
        private readonly int maxY;
        private readonly int minX;
        private readonly int minY;

        public JumpToRandomOffset(int minX, int maxX, int minY, int maxY)
        {
            this.minX = minX;
            this.maxX = maxX;
            this.minY = minY;
            this.maxY = maxY;
        }

        protected override void OnStateEntry(Entity host, TickData time, ref object state) => host.Move(host.X + Random.Next(minX, maxX), host.Y + Random.Next(minY, maxY));

        protected override void TickCore(Entity host, TickData time, ref object state)
        { }
    }
}
