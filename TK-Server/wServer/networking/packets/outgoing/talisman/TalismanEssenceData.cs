using common;
using common.database.info;
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

        public TalismanData(DbTalismanEntry entry)
        {
            Type = entry.Type;
            Level = entry.Level;
            CurrentXP = entry.Exp;
            ExpGoal = entry.Goal;
            Tier = entry.Tier;
        }

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
        public int Essence { get; set; }
        public int EssenceCap { get;  set; }

        public List<TalismanData> Talismans { get; private set; } = new List<TalismanData>();

        public override MessageId MessageId => MessageId.TALISMAN_ESSENCE_DATA;

        protected override void Write(NWriter wtr)
        {
            wtr.Write(Essence);
            wtr.Write(EssenceCap);
            wtr.Write((short)Talismans.Count);
            foreach(var talisman in Talismans)
                talisman.Write(wtr);
        }
    }
}
