package kabam.rotmg.Forge.components {

import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.objects.GameObject;
import com.company.assembleegameclient.parameters.Parameters;
import com.company.assembleegameclient.ui.TextBox;
import com.company.assembleegameclient.ui.panels.ButtonPanel;

import flash.events.KeyboardEvent;
import flash.events.MouseEvent;

import kabam.rotmg.Forge.ForgeModal;

public class ForgePanel extends ButtonPanel {


    private var gm_:GameObject;

    public function ForgePanel(arg1:GameSprite, gm:GameObject) {
        super(arg1, "Forge", "Start Forging");
        this.gm_ = gm;
    }

    override protected function onButtonClick(evt:MouseEvent):void{
        if(!this.gs_.mui_.setHotkeysInput_ || !this.gs_.mui_.enablePlayerInput_){
            return;
        }
        this.gs_.mui_.setEnablePlayerInput(false); /* Disable player movement */
        this.gs_.mui_.setEnableHotKeysInput(false); /* Disable Hotkeys */
        this.openDialogNoModal.dispatch(new ForgeModal(this.gs_, this.gm_));
    }

    override protected function onKeyDown(evt:KeyboardEvent):void{
        if(evt.keyCode == Parameters.data_.interact && !TextBox.isInputtingText){
            if(!this.gs_.mui_.setHotkeysInput_ || !this.gs_.mui_.enablePlayerInput_){
                return;
            }
            this.gs_.mui_.setEnablePlayerInput(false); /* Disable player movement */
            this.gs_.mui_.setEnableHotKeysInput(false); /* Disable Hotkeys */
            this.openDialogNoModal.dispatch(new ForgeModal(this.gs_, this.gm_));
        }
    }

}
}
