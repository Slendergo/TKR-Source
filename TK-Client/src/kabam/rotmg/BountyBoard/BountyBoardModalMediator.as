package kabam.rotmg.BountyBoard {
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.sound.SoundEffectLibrary;

import flash.events.MouseEvent;

import kabam.lib.net.api.MessageProvider;
import kabam.lib.net.impl.SocketServer;
import kabam.rotmg.dialogs.control.CloseDialogsSignal;
import kabam.rotmg.messaging.impl.GameServerConnection;
import kabam.rotmg.messaging.impl.outgoing.bounty.BountyRequest;

import org.swiftsuspenders.Injector;

import robotlegs.bender.bundles.mvcs.Mediator;

public class BountyBoardModalMediator extends Mediator {

    [Inject]
    public var injector:Injector;
    [Inject]
    public var closeDialogs:CloseDialogsSignal;
    [Inject]
    public var view:BountyBoardModal;
    [Inject]
    public var socketServer:SocketServer;
    [Inject]
    public var messages:MessageProvider;

    private var gameSprite:GameSprite;

    override public function initialize():void {
        this.gameSprite = this.view.gs_;
        this.view.close.add(this.onCancel);
    }

    private function easyBounty(e:MouseEvent):void {
        var _local_1:BountyRequest;
        _local_1 = (this.messages.require(GameServerConnection.BOUNTYREQUEST) as BountyRequest);
        _local_1.BountyId = 1;
        this.socketServer.sendMessage(_local_1);
        this.view.onClose(e);
    }
    private function onCancel() : void
    {
        SoundEffectLibrary.play("button_click");
        this.closeDialogs.dispatch();
    }

}
}
