package kabam.rotmg.essences.view.components {

import com.company.assembleegameclient.ui.StatusBar;
import flash.display.Sprite;

public class EssenceGauge extends Sprite
{
    public var gague_:StatusBar;

    public function EssenceGauge()
    {
        this.gague_ = new StatusBar(64, 352, 0x428751, 5526612,"Ess", false, false, true, 32);
        this.addChild(this.gague_);
    }

    public function draw(current:int, max:int):void
    {
        this.gague_.draw(current, max,0);
    }
}
}
