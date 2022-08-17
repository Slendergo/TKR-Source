package kabam.rotmg.essences.view.components {

import avm2.intrinsics.memory.casi32;

import com.company.assembleegameclient.ui.StatusBar;
import flash.display.Sprite;

public class EssenceGauge extends Sprite
{
    public var gague_:StatusBar;

    public function EssenceGauge()
    {
        this.gague_ = new StatusBar(64, 400, 0x428751, 5526612,"Ess", false, false, true, 32);
        this.addChild(this.gague_);
    }

    private var current_:int;
    private var max_:int;
    private var v_:int;

    public function update(current:int, max:int):void{
        this.v_ = this.current_;
        this.current_ = current;
        this.max_ = max;
    }

    public function draw():void
    {
        if(this.v_ != this.current_){
            this.v_ += this.v_ < this.current_ ? 1 : -1;
        }
        this.gague_.draw(this.v_, this.max_,0);
    }
}
}
