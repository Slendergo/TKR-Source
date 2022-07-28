package kabam.rotmg.messaging.impl.outgoing {
import flash.utils.IDataOutput;

public class Options extends OutgoingMessage {

    public var allyShots:Boolean;

    public function Options(_arg1:uint, _arg2:Function) {
        super(_arg1, _arg2);
    }

    override public function writeToOutput(_arg1:IDataOutput):void {
        _arg1.writeBoolean(this.allyShots);
    }

    override public function toString():String {
        return (formatToString("OPTIONS", "allyShots"));
    }
}
}
