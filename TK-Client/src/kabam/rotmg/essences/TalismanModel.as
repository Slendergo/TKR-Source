package kabam.rotmg.essences {
import kabam.rotmg.messaging.impl.data.TalismanData;

public class TalismanModel
{
    public var type_:int;
    public var level_:int;
    public var xp_:int;
    public var goal_:int;
    public var tier_:int;

    public function TalismanModel(data:TalismanData){
        this.type_ = data.type_;
        this.level_ = data.type_;
        this.xp_ = data.xp_;
        this.goal_ = data.goal_;
        this.tier_ = data.tier_;
    }
}
}
