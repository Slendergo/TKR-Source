using TKR.Shared.resources;
using TKR.WorldServer.core.net.stats;
using TKR.WorldServer.core.objects.inventory;

namespace TKR.WorldServer.core.objects
{
    public partial class Player
    {
        private StatTypeValue<int> _talismanEffects;

        public int TalismanEffects 
        {
            get => _talismanEffects.GetValue(); 
            set => _talismanEffects.SetValue(value);
        }

        public bool HasTalismanEffect(TalismanEffectType talismanEffectType) =>  (TalismanEffects & (1 << ((int)talismanEffectType - 1))) != 0;
    
        public void RecalculateTalismanEffects()
        {
            if (HasTalismanEffect(TalismanEffectType.WeakImmunity))
                RemoveCondition(ConditionEffectIndex.Weak);
            if (HasTalismanEffect(TalismanEffectType.StunImmunity))
                RemoveCondition(ConditionEffectIndex.Stunned);

            TalismanEffects = 0;
            for (var i = 20; i < 28; i++ )
            {
                var item = Inventory[i];
                if (item == null || item.TalismanItemDesc == null || item.SlotType != InventoryConstants.TALISMAN_SLOT_TYPE)
                    continue;

                foreach(var providesDesc in item.TalismanItemDesc.Provides)
                {
                    var talismanEffectType = providesDesc.Effect;
                    TalismanEffects |= (1 << ((int)providesDesc.Effect - 1));
                }
            }
        }

        protected override bool CanApplyCondition(ConditionEffectIndex effect)
        {
            if (effect == ConditionEffectIndex.Weak && HasTalismanEffect(TalismanEffectType.WeakImmunity))
                return false;
            if (effect == ConditionEffectIndex.Stunned && HasTalismanEffect(TalismanEffectType.StunImmunity))
                return false;
            return base.CanApplyCondition(effect);
        }
    }
}
