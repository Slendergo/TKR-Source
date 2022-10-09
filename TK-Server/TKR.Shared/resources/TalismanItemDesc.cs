using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace TKR.Shared.resources
{
    public sealed class TalismanItemDesc
    {
        public readonly bool Common;
        public readonly bool Legendary;
        public readonly bool Mythic;
        public readonly bool OnlyOne;
        public readonly List<TalismanItemProvidesDesc> Provides;

        public TalismanItemDesc(XElement elem)
        {
            Common = elem.HasElement("Common");
            Legendary = elem.HasElement("Legendary");
            Mythic = elem.HasElement("Mythic");
            OnlyOne = elem.HasElement("OnlyOne");

            Provides = new List<TalismanItemProvidesDesc>();
            foreach(var e in elem.Elements("Provides"))
                Provides.Add(new TalismanItemProvidesDesc(e));
        }
    }

    [Flags]
    public enum TalismanEffectType : byte
    {
        None = 0,
        WeakImmunity = 1,
        CallToArms = 2,
        PartyOfOne = 3,
        PocketChange = 4,
        StunImmunity = 5,
        LuckOfTheIrish = 6,
        KnownAfterDeath = 7
    }

    public sealed class TalismanItemProvidesDesc
    {
        public readonly TalismanEffectType Effect;

        public TalismanItemProvidesDesc(XElement elem)
        {
            if(elem.HasAttribute("effect", out var effect))
                Effect = (TalismanEffectType)Enum.Parse(typeof(TalismanEffectType), effect.Value.Replace(" ", ""));
        }
    }
}
