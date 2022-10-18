using TKR.Shared;
using TKR.Shared.resources;
using TKR.WorldServer.core.miscfile.datas;
using TKR.WorldServer.core.miscfile.structures;

namespace TKR.WorldServer.networking.packets.outgoing
{
    public sealed class AoeMessage : OutgoingMessage
    {
        private readonly Position Pos;
        private readonly float Radius;
        private readonly int Damage;
        private readonly ConditionEffectIndex Effect;
        private readonly float Duration;
        private readonly ushort OrigType;
        private readonly ARGB Color;

        public override MessageId MessageId => MessageId.AOE;

        public AoeMessage(Position pos, float radius, int damage, ConditionEffectIndex effect, float duration, ushort origType, ARGB color)
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
