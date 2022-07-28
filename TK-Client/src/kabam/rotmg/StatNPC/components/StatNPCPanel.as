package kabam.rotmg.StatNPC.components {
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.parameters.Parameters;
import com.company.assembleegameclient.ui.TextBox;
import com.company.assembleegameclient.ui.panels.ButtonPanel;

import flash.events.KeyboardEvent;
import flash.events.MouseEvent;

import kabam.rotmg.StatNPC.StatNPCModal;

public class StatNPCPanel extends ButtonPanel{


    public function StatNPCPanel(gs:GameSprite) {
        super(gs, "Magician", "Start to Upgrade");
    }

    override protected function onKeyDown(evt:KeyboardEvent):void{
        if(evt.keyCode == Parameters.data_.interact && !TextBox.isInputtingText){
            this.openDialog.dispatch(new StatNPCModal(this.gs_));
        }
    }

    override protected function onButtonClick(evt:MouseEvent):void{
        this.openDialog.dispatch(new StatNPCModal(this.gs_));
    }

}
}
