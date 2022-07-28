package kabam.rotmg.BountyBoard.SubscriptionUI {
import com.company.assembleegameclient.sound.SoundEffectLibrary;

import kabam.rotmg.dialogs.control.CloseDialogsSignal;

import robotlegs.bender.bundles.mvcs.Mediator;

public class SubscriptionUIMediator extends Mediator {

    [Inject]
    public var closeDialogs:CloseDialogsSignal;
    [Inject]
    public var view:SubscriptionUI;


    override public function initialize():void {
        this.view.close.add(this.onCancel);
    }


    private function onCancel() : void
    {
        SoundEffectLibrary.play("button_click");
        this.closeDialogs.dispatch();
    }

}
}
