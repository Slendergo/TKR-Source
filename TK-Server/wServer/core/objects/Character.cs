using common.resources;
using System;
using System.Collections.Generic;
using wServer.core.worlds;

namespace wServer.core.objects
{
    public abstract class Character : Entity
    {
        public int GlowEnemy { get => _glowcolor.GetValue(); set => _glowcolor.SetValue(value); }
        public int HP { get => _hp.GetValue(); set => _hp.SetValue(value); }
        public int MaximumHP { get => _maximumHP.GetValue(); set => _maximumHP.SetValue(value); }

        private SV<int> _glowcolor;
        private SV<int> _hp;
        private SV<int> _maximumHP;

        private static Random R = new Random();//temp fix

        protected Character(GameServer manager, ushort objType) : base(manager, objType)
        {
            _hp = new SV<int>(this, StatDataType.HP, 0);
            _maximumHP = new SV<int>(this, StatDataType.MaximumHP, 0);
            _glowcolor = new SV<int>(this, StatDataType.GlowEnemy, 0);

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

        protected override void ExportStats(IDictionary<StatDataType, object> stats)
        {
            stats[StatDataType.HP] = HP;

            if (!(this is Player))
                stats[StatDataType.MaximumHP] = MaximumHP;

            stats[StatDataType.GlowEnemy] = GlowEnemy;

            base.ExportStats(stats);
        }

        private void SetConditions()
        {
            if (ObjectDesc.Invincible)
                ApplyConditionEffect(ConditionEffectIndex.Invincible);

            if (ObjectDesc.ArmorBreakImmune)
                ApplyConditionEffect(ConditionEffectIndex.ArmorBreakImmune);

            if (ObjectDesc.CurseImmune)
                ApplyConditionEffect(ConditionEffectIndex.CurseImmune);

            if (ObjectDesc.DazedImmune)
                ApplyConditionEffect(ConditionEffectIndex.DazedImmune);

            if (ObjectDesc.ParalyzeImmune)
                ApplyConditionEffect(ConditionEffectIndex.ParalyzeImmune);

            if (ObjectDesc.PetrifyImmune)
                ApplyConditionEffect(ConditionEffectIndex.PetrifyImmune);

            if (ObjectDesc.SlowedImmune)
                ApplyConditionEffect(ConditionEffectIndex.SlowedImmune);

            if (ObjectDesc.StasisImmune)
                ApplyConditionEffect(ConditionEffectIndex.StasisImmune);

            if (ObjectDesc.StunImmune)
                ApplyConditionEffect(ConditionEffectIndex.StunImmune);
        }
    }
}
