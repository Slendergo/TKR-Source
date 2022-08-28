package kabam.rotmg.Engine.view {
import kabam.rotmg.Engine.view.*;
import kabam.rotmg.PotionStorage.*;

import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.sound.SoundEffectLibrary;

import flash.events.Event;
import flash.events.MouseEvent;

import kabam.lib.net.api.MessageProvider;
import kabam.lib.net.impl.SocketServer;
import kabam.rotmg.dialogs.control.CloseDialogsSignal;
import kabam.rotmg.messaging.impl.GameServerConnection;
import kabam.rotmg.messaging.impl.outgoing.UsePotion;
import kabam.rotmg.messaging.impl.outgoing.potionStorage.PotionStorage;
import kabam.rotmg.messaging.impl.outgoing.talisman.TalismanEssenceAction;

import org.swiftsuspenders.Injector;

import robotlegs.bender.bundles.mvcs.Mediator;

public class EngineViewMediator extends Mediator {

    [Inject]
    public var view:EngineView;

    [Inject]
    public var closeDialogs:CloseDialogsSignal;

    override public function initialize():void
    {
        this.view.close.add(this.onCancel);
        this.view.fuelButton.addEventListener(MouseEvent.CLICK, this.onFuel);
    }

    override public function destroy():void
    {
        this.view.gs_.mui_.setEnablePlayerInput(true); /* Disable player movement */
        this.view.gs_.mui_.setEnableHotKeysInput(true); /* Disable Hotkeys */
        this.view.close.remove(this.onCancel);
        this.view.fuelButton.removeEventListener(MouseEvent.CLICK, this.onFuel);
    }

    private function onFuel(event:MouseEvent) : void
    {
        this.view.gs_.gsc_.acceptFuel(this.view.itemEngineTile.getIncludedItems());
        this.closeDialogs.dispatch();
    }

    private function onCancel():void {
        this.view.gs_.mui_.setEnablePlayerInput(true); /* Disable player movement */
        this.view.gs_.mui_.setEnableHotKeysInput(true); /* Disable Hotkeys */
        SoundEffectLibrary.play("button_click");
        this.closeDialogs.dispatch();
    }
}
}