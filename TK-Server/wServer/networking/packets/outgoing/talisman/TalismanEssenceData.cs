using common;
using System.Collections.Generic;

namespace wServer.networking.packets.outgoing.talisman
{
    public struct TalismanData
    {
        public byte Type { get; private set; }
        public byte Level { get; private set; }
        public int CurrentXP { get; private set; }
        public int ExpGoal { get; private set; }
        public byte Tier { get; private set; }

        public TalismanData(byte type, byte level, int xp, int goal, byte tier)
        {
            Type = type;
            Level = level;
            CurrentXP = xp;
            ExpGoal = goal;
            Tier = tier;
        }

        public void Write(NWriter wtr)
        {
            wtr.Write(Type);
            wtr.Write(Level);
            wtr.Write(CurrentXP);
            wtr.Write(ExpGoal);
            wtr.Write(Tier);
        }
    }

    public class TalismanEssenceData : OutgoingMessage
    {
        public List<TalismanData> Talismans { get; private set; } = new List<TalismanData>();

        public override MessageId MessageId => MessageId.TALISMAN_ESSENCE_DATA;

        protected override void Write(NWriter wtr)
        {
            wtr.Write((short)Talismans.Count);
            foreach(var talisman in Talismans)
                talisman.Write(wtr);
        }
    }
}
