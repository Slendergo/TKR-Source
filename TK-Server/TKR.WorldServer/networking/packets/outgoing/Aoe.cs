using TKR.Shared;
using TKR.Shared.resources;
using TKR.WorldServer.core.miscfile.datas;
using TKR.WorldServer.core.miscfile.structures;

namespace TKR.WorldServer.networking.packets.outgoing
{
    public class Aoe : OutgoingMessage
    {
        public Position Pos { get; set; }
        public float Radius { get; set; }
        public int Damage { get; set; }
        public ConditionEffectIndex Effect { get; set; }
        public float Duration { get; set; }
        public ushort OrigType { get; set; }
        public ARGB Color { get; set; }

        public override MessageId MessageId => MessageId.AOE;

        public Aoe(Position pos, float radius, int damage, ConditionEffectIndex effect, float duration, ushort origType, ARGB color)
        {
            Pos = pos;
            Radius = radius;
            Damage = damage;
            Effect = effect;
            Duration = duration;
            OrigType = origType;
            Color = color;
        }

        public override void Write(NetworkWriter wtr)
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
