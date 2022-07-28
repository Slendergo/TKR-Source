package kabam.lib.net.impl {
import flash.utils.ByteArray;
import flash.utils.IDataInput;
import flash.utils.IDataOutput;

import kabam.rotmg.core.StaticInjectorContext;

public class Message {
    private const BUFFER:ByteArray = StaticInjectorContext.injector.getInstance(ByteArray, "ContextBuffer") as ByteArray;

    public function Message(id:uint, callback:Function = null, setBuffer:Boolean = false) {
        this.id = id;
        this.isCallback = callback != null;
        this.callback = callback;
        this.setBuffer = setBuffer;
    }

    public var pool:MessagePool;
    public var prev:Message;
    public var next:Message;
    public var id:uint;
    public var callback:Function;

    private var isCallback:Boolean;
    private var setBuffer:Boolean;

    public function parseFromInput(data:IDataInput):void {
    }

    public function writeToOutput(data:IDataOutput):void {
    }

    public function read(data:IDataInput):void {
        this.parseFromInput(data);
    }

    public function write(data:ByteArray):ByteArray {
        this.writeToOutput(data);

        return this.setBuffer ? BUFFER : null;
    }

    public function toString():String {
        return this.formatToString("MESSAGE", "id");
    }

    public function consume():void {
        this.isCallback && this.callback(this);
        this.prev = null;
        this.next = null;
        this.pool.append(this);
    }

    protected function formatToString(name:String, ...args):String {
        var str:String = "[" + name;
        for (var i:int = 0; i < args.length; i++) {
            str = str + (" " + args[i] + "=\"" + this[args[i]] + "\"");
        }
        str = str + "]";
        return str;
    }
}
}
