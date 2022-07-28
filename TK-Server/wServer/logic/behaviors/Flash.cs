using wServer.core;
using wServer.core.objects;
using wServer.networking;
using wServer.networking.packets.outgoing;

namespace wServer.logic.behaviors
{
    internal class Flash : Behavior
    {
        private readonly uint color;
        private float flashPeriod;
        private int flashRepeats;

        public Flash(uint color, double flashPeriod, int flashRepeats)
        {
            this.color = color;
            this.flashPeriod = (float)flashPeriod;
            this.flashRepeats = flashRepeats;
        }

        protected override void OnStateEntry(Entity host, TickData time, ref object state) => host.Owner.BroadcastIfVisible(new ShowEffect()
        { EffectType = EffectType.Flashing, Pos1 = new Position() { X = flashPeriod, Y = flashRepeats }, TargetObjectId = host.Id, Color = new ARGB(color) }, host, PacketPriority.Low);

        protected override void TickCore(Entity host, TickData time, ref object state)
        { }
    }
}
