package kabam.rotmg.messaging.impl.outgoing {
import flash.utils.IDataOutput;

public class BigSkillTree extends OutgoingMessage {

    public var skillNumber:int;

    public function BigSkillTree(_arg1:uint, _arg2:Function) {
        super(_arg1, _arg2);
    }

    override public function writeToOutput(_arg1:IDataOutput):void {
        _arg1.writeInt(this.skillNumber);
    }

    override public function toString():String {
        return (formatToString("BIGSKILLTREE", "skillNumber"));
    }


}
}
