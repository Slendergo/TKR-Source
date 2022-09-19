using TKR.Shared.resources;
using System;
using System.Collections.Generic;
using TKR.WorldServer.core;
using TKR.WorldServer.core.miscfile.stats;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.core.worlds;

namespace TKR.WorldServer.core.objects
{
    public abstract class Character : Entity
    {
        public int HP { get => _hp.GetValue(); set => _hp.SetValue(value); }
        public int MaximumHP { get => _maximumHP.GetValue(); set => _maximumHP.SetValue(value); }

        private SV<int> _hp;
        private SV<int> _maximumHP;

        private static Random R = new Random();//temp fix

        protected Character(GameServer manager, ushort objType) : base(manager, objType)
        {
            _hp = new SV<int>(this, StatDataType.HP, 0);
            _maximumHP = new SV<int>(this, StatDataType.MaximumHP, 0);

            if (ObjectDesc != null)
            {
                if (ObjectDesc.SizeStep != 0)
                {
                    var step = R.Next(0, (ObjectDesc.MaxSize - ObjectDesc.MinSize) / ObjectDesc.SizeStep + 1) * ObjectDesc.SizeStep;
                    SetDefaultSize(ObjectDesc.MinSize + step);
                }
                else
                    SetDefaultSize(ObjectDesc.MinSize);

                SetConditions();

                HP = ObjectDesc.MaxHP;
                MaximumHP = HP;
            }
        }

        protected override void ExportStats(IDictionary<StatDataType, object> stats, bool isOtherPlayer)
        {
            base.ExportStats(stats, isOtherPlayer);
            stats[StatDataType.HP] = HP;
            if (!(this is Player))
                stats[StatDataType.MaximumHP] = MaximumHP;
        }

        private void SetConditions()
        {
            if (ObjectDesc.Invincible)
                ApplyPermanentConditionEffect(ConditionEffectIndex.Invincible);

            if (ObjectDesc.ArmorBreakImmune)
                ApplyPermanentConditionEffect(ConditionEffectIndex.ArmorBreakImmune);

            if (ObjectDesc.CurseImmune)
                ApplyPermanentConditionEffect(ConditionEffectIndex.CurseImmune);

            if (ObjectDesc.DazedImmune)
                ApplyPermanentConditionEffect(ConditionEffectIndex.DazedImmune);

            if (ObjectDesc.ParalyzeImmune)
                ApplyPermanentConditionEffect(ConditionEffectIndex.ParalyzeImmune);

            if (ObjectDesc.PetrifyImmune)
                ApplyPermanentConditionEffect(ConditionEffectIndex.PetrifyImmune);

            if (ObjectDesc.SlowedImmune)
                ApplyPermanentConditionEffect(ConditionEffectIndex.SlowedImmune);

            if (ObjectDesc.StasisImmune)
                ApplyPermanentConditionEffect(ConditionEffectIndex.StasisImmune);

            if (ObjectDesc.StunImmune)
                ApplyPermanentConditionEffect(ConditionEffectIndex.StunImmune);
        }
    }
}
