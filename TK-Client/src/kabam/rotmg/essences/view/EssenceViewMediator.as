package kabam.rotmg.essences.view {
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

public class EssenceViewMediator extends Mediator {

    [Inject]
    public var view:EssenceView;

    [Inject]
    public var closeDialogs:CloseDialogsSignal;

    override public function initialize():void
    {
        this.view.close.add(this.onCancel);
        this.view.addEssence.add(this.onAddEssence);
        this.view.tierUp.add(this.onTierUp);
        this.view.enable.add(this.onEnable);
        this.view.disable.add(this.onDisable);
    }

    override public function destroy():void
    {
        this.view.close.remove(this.onCancel);
        this.view.addEssence.remove(this.onAddEssence);
        this.view.tierUp.remove(this.onTierUp);
        this.view.enable.remove(this.onEnable);
        this.view.disable.remove(this.onDisable);
    }

    public function onAddEssence(type:int, amount:int):void {
        GameServerConnection.instance.talismanAction(TalismanEssenceAction.ADD_ESSENCE, type, amount);
    }

    public function onTierUp(type:int, cost:int):void {
        GameServerConnection.instance.talismanAction(TalismanEssenceAction.TIER_UP, type, cost);
    }

    public function onEnable(type:int):void {
        GameServerConnection.instance.talismanAction(TalismanEssenceAction.ENABLE, type, 0);
        this.view.player.activateTalisman(type);
    }

    public function onDisable(type:int):void {
        GameServerConnection.instance.talismanAction(TalismanEssenceAction.DISABLE, type, 0);
        this.view.player.deactivateTalisman(type);
    }

    private function onCancel():void {
        this.view.gs_.mui_.setEnablePlayerInput(true); /* Disable player movement */
        this.view.gs_.mui_.setEnableHotKeysInput(true); /* Disable Hotkeys */
        SoundEffectLibrary.play("button_click");
        this.closeDialogs.dispatch();
    }
}
}