package kabam.rotmg.essences.view.components {
import com.company.assembleegameclient.misc.DefaultLabelFormat;
import com.company.assembleegameclient.ui.StatusBar;
import com.company.ui.SimpleText;

import flash.display.Sprite;
import flash.filters.DropShadowFilter;

import io.decagames.rotmg.ui.buttons.SliceScalingButton;
import io.decagames.rotmg.ui.texture.TextureParser;

public class EssenceGauge extends Sprite
{
    public var gague_:StatusBar;

    private var maxCapacity_:int;
    private var currentCapacity_:int;

    public function EssenceGauge()
    {
        this.gague_ = new StatusBar(64, 352, 0x428751, 5526612,"Ess", false, false, true, 32);
        this.addChild(this.gague_);
    }

    public function setCapacity(amount:int):void
    {
        this.currentCapacity_ = amount;
    }

    public function setMaxCapacity(amount:int):void
    {
        this.maxCapacity_ = amount;
    }

    public function draw():void
    {
        this.gague_.draw(this.currentCapacity_, this.maxCapacity_,0);
    }
}
}
