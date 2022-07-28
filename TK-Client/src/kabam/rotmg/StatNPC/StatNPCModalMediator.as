package kabam.rotmg.StatNPC {

import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.sound.SoundEffectLibrary;

import kabam.lib.net.api.MessageProvider;
import kabam.lib.net.impl.SocketServer;
import kabam.rotmg.dialogs.control.CloseDialogsSignal;
import kabam.rotmg.dialogs.control.OpenDialogSignal;
import kabam.rotmg.messaging.impl.GameServerConnection;
import kabam.rotmg.messaging.impl.outgoing.UpgradeStat;

import robotlegs.bender.bundles.mvcs.Mediator;

public class StatNPCModalMediator extends Mediator {

    [Inject]
    public var closeDialogs:CloseDialogsSignal;
    [Inject]
    public var view:StatNPCModal;
    [Inject]
    public var socketServer:SocketServer;
    [Inject]
    public var messages:MessageProvider;
    [Inject]
    public var openDialog:OpenDialogSignal;

    private var gameSprite:GameSprite;


    override public function initialize():void{
        this.gameSprite = this.view.gs_;
        this.view.upgrade.add(this.onCall);
        this.view.close.add(this.onCancel);
        this.view.info.add(this.onInfo);
    }


    override public function destroy() : void
    {
        this.view.upgrade.remove(this.onCancel);
        this.view.close.remove(this.onCancel);
    }

    private function onInfo() : void
    {
        this.openDialog.dispatch(new StatNPCInfo(this.gameSprite));
    }

    private function onCall() : void
    {
        if(this.view.player.baseStat >= 10 || this.view.checkNextCost() > this.view.player.fame_){
            return;
        }
        this.view.player.baseStat += 1;
        var packet:UpgradeStat;
        packet = (this.messages.require(GameServerConnection.UPGRADESTAT) as UpgradeStat);
        this.socketServer.sendMessage(packet);
        this.closeDialogs.dispatch();
    }

    private function onCancel() : void
    {
        SoundEffectLibrary.play("button_click");
        this.closeDialogs.dispatch();
    }
}
}
