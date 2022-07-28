package kabam.rotmg.messaging.impl.outgoing.newSkillTree {
import flash.utils.IDataOutput;

import kabam.rotmg.messaging.impl.outgoing.OutgoingMessage;

public class NewSkillTree extends OutgoingMessage {

    public var statType_:int;
    public var interactionType_:int;

    public function NewSkillTree(_arg1:uint, _arg2:Function) {
        super(_arg1, _arg2);
    }

    override public function writeToOutput(_arg1:IDataOutput):void {
        _arg1.writeByte(this.statType_);
        _arg1.writeInt(this.interactionType_);
    }

    override public function toString():String {
        return (formatToString("NewSkillTree", "statType_", "interactionType_"));
    }


}
}
