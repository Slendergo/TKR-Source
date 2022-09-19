using TKR.Shared;
using TKR.WorldServer.core.miscfile;
using TKR.WorldServer.core.miscfile.datas;
using TKR.WorldServer.core.miscfile.structures;

namespace TKR.WorldServer.networking.packets.outgoing
{
    public class ShowEffect : OutgoingMessage
    {
        public EffectType EffectType { get; set; }
        public int TargetObjectId { get; set; }
        public Position Pos1 { get; set; }
        public Position Pos2 { get; set; }
        public ARGB Color { get; set; }
        public int Duration { get; set; }
        public ushort ObjectType { get; set; }

        public override MessageId MessageId => MessageId.SHOWEFFECT;

        protected override void Write(NWriter wtr)
        {
            wtr.Write((byte)EffectType);
            wtr.Write(TargetObjectId);
            Pos1.Write(wtr);
            Pos2.Write(wtr);
            Color.Write(wtr);
            wtr.Write(Duration);
            wtr.Write(ObjectType);
        }
    }
}
