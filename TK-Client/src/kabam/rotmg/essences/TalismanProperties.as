package kabam.rotmg.essences {
import flash.utils.Dictionary;

public class TalismanProperties
{
    public var type_:int;
    public var name_:String;
    public var itemAssociatedWith_:String;
    public var baseUpgradeCost_:int;
    public var tierUpgradeCost_:int;
    public var costMultiplier_:Number;
    public var maxLevels_:int;
    public var source_:XML;
    public var tiers_:Dictionary;

    public function TalismanProperties(xml:XML)
    {
        this.type_ = int(xml.@type);
        this.name_ = xml.Name;
        this.itemAssociatedWith_ = xml.ItemAssociatedWith;
        this.baseUpgradeCost_ = int(xml.BaseUpgradeCost);
        this.tierUpgradeCost_ = int(xml.TierUpgradeCost);
        this.costMultiplier_ = Number(xml.CostMultiplier);
        this.maxLevels_ = int(xml.MaxLevels);
        this.source_ = xml;

        this.tiers_ = new Dictionary();
        for each(var tier:XML in xml.Tier)
        {
            var desc:TalismanTierDesc = new TalismanTierDesc(tier);
            this.tiers_[desc.tier_] = desc;
        }
    }
}
}

