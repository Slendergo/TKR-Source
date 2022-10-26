using System.Xml.Linq;
using TKR.Shared.resources;
using TKR.WorldServer.core.net.stats;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.core.structures;

namespace TKR.WorldServer.core.activates
{
    public sealed class IncrementStatActivate : Activate
    {
        private readonly int StatType;
        private readonly int Amount;

        public IncrementStatActivate(Player host, XElement options, bool returnsBool)
            : base(host, options, returnsBool)
        {
            StatType = GetIntArgument(options, "stat", -1);
            Amount = GetIntArgument(options, "amount", 0);
        }

        public override bool ExecuteBool(Item item, ref Position usePosition)
        {
            var statIndex = StatsManager.GetStatIndex((StatDataType)StatType);
            var statInfo = Host.GameServer.Resources.GameData.Classes[Host.ObjectType].Stats;

            var baseAmount = Host.Stats.Base[statIndex];
            var amount = baseAmount + Amount;
            var max = statInfo[statIndex].MaxValue;

            if (baseAmount >= max)
            {
                Host.SendError("You're maxed!");
                return false;
            }

            if (amount > max)
                amount = max;
            Host.Stats.Base[statIndex] = amount;
            return true;
        }
    }
}