﻿using System.Xml.Linq;
using TKR.Shared;

namespace TKR.Shared.resources
{
    public class ConditionEffect
    {
        public int DurationMS;
        public ConditionEffectIndex Effect;

        //public ConditionEffect()
        //{ }

        public ConditionEffect(ConditionEffectIndex effect, int duration)
        {
            Effect = effect;
            DurationMS = duration;
        }

        public ConditionEffect(XElement e)
        {
            Effect = Utils.GetEffect(e.Value);
            DurationMS = (int)(e.GetAttribute<float>("duration") * 1000.0f);
        }
    }
}
