package kabam.rotmg.essences {

public class TalismanTierDesc {

    public var tier_:int;
    public var abilityLifeCost:Number;

    public function TalismanTierDesc(xml:XML) {
        this.tier_ = xml.@tier;
        this.abilityLifeCost = xml.AbilityLifeCost.@percentage;
    }
}
}

