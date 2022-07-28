package kabam.rotmg.messaging.impl.outgoing {
import com.hurlant.util.asn1.parser.boolean;

import flash.utils.IDataOutput;

public class SmallSkillTree extends OutgoingMessage {

    public var skillNumber:int;
    public var removePoint:int;

    public function SmallSkillTree(_arg1:uint, _arg2:Function) {
        super(_arg1, _arg2);
    }

    override public function writeToOutput(_arg1:IDataOutput):void {
        _arg1.writeInt(this.skillNumber);
        _arg1.writeByte(this.removePoint);
    }

    override public function toString():String {
        return (formatToString("SMALLSKILLTREE", "skillNumber", "removePoint"));
    }


}
}
