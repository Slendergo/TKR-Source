using common;
using common.resources;

namespace wServer.networking.packets.outgoing
{
    public class AoeData
    {
        public Position Pos { get; set; }
        public float Radius { get; set; }
        public int Damage { get; set; }
        public ConditionEffectIndex Effect { get; set; }
        public float Duration { get; set; }
        public ushort OrigType { get; set; }
        public ARGB Color { get; set; }

        public void Write(NWriter wtr)
        {
            Pos.Write(wtr);
            wtr.Write(Radius);
            wtr.Write((ushort)Damage);
            wtr.Write((byte)Effect);
            wtr.Write(Duration);
            wtr.Write(OrigType);
            Color.Write(wtr);
        }
    }
}
