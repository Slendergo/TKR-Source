package kabam.rotmg.Engine.view.components {

import com.company.assembleegameclient.ui.StatusBar;
import flash.display.Sprite;

public class FuelGauge extends Sprite
{
    public var s1gague_:StatusBar;
    public var s2gague_:StatusBar;
    public var s3gague_:StatusBar;

    public function FuelGauge()
    {
        //fac710=off, 12cdd4=on
        this.s1gague_ = new StatusBar(64, 400, 0x428751, 5526612,"Stage 1", false, false, true, 32);
        this.addChild(this.s1gague_);
        this.s2gague_ = new StatusBar(64, 400, 0x428751, 5526612,"Stage 2", false, false, true, 32);
        this.s2gague_.x = this.s1gague_.width + 32;
        this.addChild(this.s2gague_);
        this.s3gague_ = new StatusBar(64, 400, 0x428751, 5526612,"Stage 3", false, false, true, 32);
        this.s3gague_.x = this.s2gague_.x + this.s2gague_.width + 32;
        this.addChild(this.s3gague_);
    }

    public function draw(current:int):void
    {
        var count1:int = Math.max(0, Math.min(current, 100));
        var count2:int = Math.max(0, Math.min(current - 100, 250));
        var count3:int = Math.max(0, Math.min(current - 350, 500));
        this.s1gague_.setMaxText(count1 >= 100 ? "Reached" : null);
        this.s2gague_.setMaxText(count2 >= 250 ? "Reached" : null);
        this.s3gague_.setMaxText(count3 >= 500 ? "Reached" : null);
        this.s1gague_.setBarColor(count1 >= 100 ? 0x12cdd4 : 0xfac710)
        this.s2gague_.setBarColor(count2 >= 250 ? 0x12cdd4 : 0xfac710)
        this.s3gague_.setBarColor(count3 >= 500 ? 0x12cdd4 : 0xfac710)
        this.s1gague_.draw(count1, 100,0);
        this.s2gague_.draw(count2, 250,0);
        this.s3gague_.draw(count3, 500,0);
    }
}
}
