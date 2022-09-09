package kabam.rotmg.essences {

public class TalismanTierDesc {

    public var tier_:int;
    public var abilityLifeCost:Number;
    public var damageIsAveraged_:Boolean;
    public var shotsPierce_:Boolean;

    public function TalismanTierDesc(xml:XML) {
        this.tier_ = xml.@tier;
        this.abilityLifeCost = xml.AbilityLifeCost.@percentage;
        this.damageIsAveraged_ = xml.DamageIsAverage;
        this.shotsPierce_ = xml.ShotsPierceArmour;
    }
}
}

