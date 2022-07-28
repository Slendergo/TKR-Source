package kabam.rotmg.Forge {

import flash.events.MouseEvent;

import kabam.rotmg.dialogs.control.CloseDialogsSignal;

import robotlegs.bender.bundles.mvcs.Mediator;

public class ForgeModalMediator extends Mediator {

    [Inject]
    public var closeDialogs:CloseDialogsSignal;
    [Inject]
    public var view:ForgeModal;


    override public function initialize() : void
    {
        this.view.close.add(this.onCancel);
        this.view.forgeButton.addEventListener(MouseEvent.CLICK, this.onForge);
    }

    override public function destroy() : void
    {
        this.view.gs_.mui_.setEnablePlayerInput(true); /* Disable player movement */
        this.view.gs_.mui_.setEnableHotKeysInput(true); /* Disable Hotkeys */
        this.view.close.remove(this.onCancel);
        this.view.forgeButton.removeEventListener(MouseEvent.CLICK, this.onForge);
    }

    private function onForge(event:MouseEvent) : void
    {
        this.view.gs_.gsc_.acceptFusion(this.view.itemForgeTile.getIncludedItems());
        this.closeDialogs.dispatch();
    }

    private function onCancel() : void
    {
        this.view.gs_.mui_.setEnablePlayerInput(true); /* Disable player movement */
        this.view.gs_.mui_.setEnableHotKeysInput(true); /* Disable Hotkeys */
        this.closeDialogs.dispatch();
    }


}
}//package kabam.rotmg.pets.view.components
