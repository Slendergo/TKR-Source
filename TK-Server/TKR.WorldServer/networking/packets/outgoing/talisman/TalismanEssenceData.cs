using System.Collections.Generic;
using TKR.Shared;
using TKR.Shared.database.talisman;

namespace TKR.WorldServer.networking.packets.outgoing.talisman
{
    public sealed class TalismanData
    {
        public byte Type { get; set; }
        public byte Level { get; set; }
        public int CurrentXP { get; set; }
        public int ExpGoal { get; set; }
        public byte Tier { get; set; }
        public bool Active { get; set; }

        public TalismanData(DbTalismanEntry entry)
        {
            Type = entry.Type;
            Level = entry.Level;
            CurrentXP = entry.Exp;
            ExpGoal = entry.Goal;
            Tier = entry.Tier;
        }

        public TalismanData(byte type, byte level, int xp, int goal, byte tier, bool active)
        {
            Type = type;
            Level = level;
            CurrentXP = xp;
            ExpGoal = goal;
            Tier = tier;
            Active = active;
        }

        public void Write(NWriter wtr)
        {
            wtr.Write(Type);
            wtr.Write(Level);
            wtr.Write(CurrentXP);
            wtr.Write(ExpGoal);
            wtr.Write(Tier);
            wtr.Write(Active);
        }
    }

    public class TalismanEssenceData : OutgoingMessage
    {
        public int Essence { get; set; }
        public int EssenceCap { get; set; }

        public List<TalismanData> Talismans { get; private set; } = new List<TalismanData>();

        public override MessageId MessageId => MessageId.TALISMAN_ESSENCE_DATA;

        protected override void Write(NWriter wtr)
        {
            wtr.Write(Essence);
            wtr.Write(EssenceCap);
            wtr.Write((short)Talismans.Count);
            foreach (var talisman in Talismans)
                talisman.Write(wtr);
        }
    }
}
