package kabam.rotmg.SkillTree.components {
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.parameters.Parameters;
import com.company.assembleegameclient.ui.TextBox;
import com.company.assembleegameclient.ui.panels.ButtonPanel;

import flash.events.KeyboardEvent;
import flash.events.MouseEvent;

import kabam.rotmg.SkillTree.SkillTreeModal;

public class SkillTreePanel extends ButtonPanel {

    public function SkillTreePanel(gs:GameSprite) {
        super(gs, "Skill Tree", "Select Skills");
    }

    override protected function onButtonClick(evt:MouseEvent):void{
        if(!this.gs_.mui_.setHotkeysInput_ || !this.gs_.mui_.enablePlayerInput_){
            return;
        }
        this.gs_.mui_.setEnablePlayerInput(false); /* Disable player movement */
        this.gs_.mui_.setEnableHotKeysInput(false); /* Disable Hotkeys */
        this.openDialog.dispatch(new SkillTreeModal(this.gs_));
    }

    override protected function onKeyDown(evt:KeyboardEvent):void{
        if(evt.keyCode == Parameters.data_.interact && !TextBox.isInputtingText){
            if(!this.gs_.mui_.setHotkeysInput_ || !this.gs_.mui_.enablePlayerInput_){
                return;
            }
            this.gs_.mui_.setEnablePlayerInput(false); /* Disable player movement */
            this.gs_.mui_.setEnableHotKeysInput(false); /* Disable Hotkeys */
            this.openDialog.dispatch(new SkillTreeModal(this.gs_));
        }
    }

}
}
