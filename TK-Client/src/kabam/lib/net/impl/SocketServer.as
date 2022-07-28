package kabam.lib.net.impl {
import com.company.assembleegameclient.parameters.Parameters;
import com.hurlant.crypto.hash.SHA1;
import com.hurlant.crypto.symmetric.ICipher;

import flash.events.Event;
import flash.events.IOErrorEvent;
import flash.events.ProgressEvent;
import flash.events.SecurityErrorEvent;
import flash.net.Socket;
import flash.utils.ByteArray;

import kabam.lib.net.api.MessageProvider;

import org.osflash.signals.Signal;

public class SocketServer {
    public static const MESSAGE_LENGTH_SIZE_IN_BYTES:int = 4;

    public const connected:Signal = new Signal();
    public const closed:Signal = new Signal();
    public const error:Signal = new Signal(String);

    private const unsentPlaceholder:Message = new Message(0);
    private const data:ByteArray = new ByteArray();
    private const sha1:SHA1 = new SHA1();

    private static function logError(message:Message = null, error:Error = null):void {
        trace("Socket-Server Protocol Error!" + (!message ? " Unknown message." : ""));

        if (message)
            trace("\t- Message " + message.id + ": " + message.toString());

        if (error != null) {
            trace("Error:");
            trace("\t- ID: " + error.errorID);
            trace("\t- Name: " + error.name);
            trace("\t- Message: " + error.message);
            trace("\t- StackTrace: ");
            trace(error.getStackTrace());
        }
    }

    private static function parseString(error:String, arguments:Array):String {
        var count:int = arguments.length;

        for (var i:int = 0; i < count; i++)
            error = error.replace("{" + i + "}", arguments[i]);

        return error;
    }

    public function SocketServer() {
        this.head = this.unsentPlaceholder;
        this.tail = this.unsentPlaceholder;
    }

    [Inject]
    public var messages:MessageProvider;
    [Inject]
    public var socket:Socket;
    private var server:String;
    private var port:int;
    private var head:Message;
    private var tail:Message;
    private var messageLen:int = -1;

    public function connect(address:String, port:int):void {
        this.server = address;
        this.port = port;

        this.addListeners();

        this.messageLen = -1;
        this.socket.connect(address, port);
    }

    public function disconnect():void {
        if (this.socket.connected)
            this.socket.close();

        this.removeListeners();

        this.closed.dispatch();
    }

    public function sendMessage(message:Message):void {
        this.tail.next = message;
        this.tail = message;
        this.socket.connected && this.sendPendingMessages();
    }

    private function addListeners():void {
        this.socket.addEventListener(Event.CONNECT, this.onConnect);
        this.socket.addEventListener(Event.CLOSE, this.onClose);
        this.socket.addEventListener(ProgressEvent.SOCKET_DATA, this.onSocketData);
        this.socket.addEventListener(IOErrorEvent.IO_ERROR, this.onIOError);
        this.socket.addEventListener(SecurityErrorEvent.SECURITY_ERROR, this.onSecurityError);
    }

    private function removeListeners():void {
        this.socket.removeEventListener(Event.CONNECT, this.onConnect);
        this.socket.removeEventListener(Event.CLOSE, this.onClose);
        this.socket.removeEventListener(ProgressEvent.SOCKET_DATA, this.onSocketData);
        this.socket.removeEventListener(IOErrorEvent.IO_ERROR, this.onIOError);
        this.socket.removeEventListener(SecurityErrorEvent.SECURITY_ERROR, this.onSecurityError);
    }

    private function sendPendingMessages():void {
        var temp:Message = this.head.next;
        var msg:Message = temp;

        if (!this.socket.connected)
            return;

        var i:Number = 0;

        while (msg) {
            this.data.position = 0;
            this.data.length = 0;

            var buffer:ByteArray = msg.write(this.data);

            this.data.position = 0;

            this.socket.writeInt(this.data.bytesAvailable + 5);
            this.socket.writeByte(msg.id);
            this.socket.writeBytes(this.data);

            temp = msg;
            msg = msg.next;
            temp.consume();
            i++;
        }

        if (i > 0)
            this.socket.flush();

        this.unsentPlaceholder.next = null;
        this.unsentPlaceholder.prev = null;
        this.head = (this.tail = this.unsentPlaceholder);
    }

    private function onConnect(event:Event):void {
        this.connected.dispatch();
    }

    private function onClose(event:Event):void {
        this.closed.dispatch();
    }

    private function onIOError(event:IOErrorEvent):void {
        var errMsg:String;

        if (event.errorID === 2031)
            errMsg = "The server is offline right now, retrying to connect...";
        else
            errMsg = parseString("Socket-Server IO Error: {0}", [event.text]);

        this.error.dispatch(errMsg);

        trace(errMsg);

        this.closed.dispatch();
    }

    private function onSecurityError(event:SecurityErrorEvent):void {
        var errMsg:String = parseString("Socket-Server Security: {0}. Please open port {1} in your firewall and/or router settings and try again", [event.text, Parameters.PORT]);
        this.error.dispatch(errMsg);

        trace(errMsg);

        this.closed.dispatch();
    }

    private function onSocketData(_:ProgressEvent = null):void {
        var messageId:uint;
        var message:Message;
        var errorMessage:String;

        while (true) {
            if (this.socket === null || !this.socket.connected)
                break;

            if (this.messageLen === -1) {
                if (this.socket.bytesAvailable < MESSAGE_LENGTH_SIZE_IN_BYTES)
                    break;

                try {
                    this.messageLen = this.socket.readInt();
                }
                catch (error:Error) {
                    errorMessage = parseString("Socket-Server Data Error: {0}\n{1}", [error.name, error.message]);

                    this.error.dispatch(errorMessage);

                    trace(errorMessage);

                    this.messageLen = -1;
                    return;
                }
            }

            if (this.socket.bytesAvailable < this.messageLen - MESSAGE_LENGTH_SIZE_IN_BYTES)
                break;

            messageId = this.socket.readUnsignedByte();
            message = this.messages.require(messageId);

            this.data.position = 0;
            this.data.length = 0;
            if (this.messageLen - 5 > 0)
                this.socket.readBytes(this.data, 0, this.messageLen - 5);

            this.data.position = 0;

            this.messageLen = -1;

            if (!message) {
                logError();
                return;
            }

            try {
                message.read(this.data);
                if(messageId != 12)
                    trace("Reading Message: " + message + ", ID: " + messageId);
            }
            catch (error:Error) {
                logError(message, error);
                return;
            }

            message.consume();

            this.sendPendingMessages();
        }
    }
}
}
