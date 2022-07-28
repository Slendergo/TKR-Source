using System.Xml.Linq;

namespace common.resources
{
    public class ConditionEffect
    {
        public int DurationMS;
        public ConditionEffectIndex Effect;

        public ConditionEffect()
        { }

        public ConditionEffect(XElement e)
        {
            Effect = Utils.GetEffect(e.Value);
            DurationMS = (int)(e.GetAttribute<float>("duration") * 1000.0f);
        }
    }
}
