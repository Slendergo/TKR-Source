package kabam.rotmg.essences.view.components {
import com.company.assembleegameclient.ui.Scrollbar;
import com.company.assembleegameclient.ui.StatusBar;
import com.company.assembleegameclient.ui.tooltip.TextToolTip;
import com.company.util.AssetLibrary;

import flash.display.Bitmap;
import flash.display.BitmapData;
import flash.display.Graphics;
import flash.display.Sprite;
import flash.utils.Dictionary;

import io.decagames.rotmg.ui.buttons.SliceScalingButton;
import io.decagames.rotmg.ui.defaults.DefaultLabelFormat;
import io.decagames.rotmg.ui.texture.TextureParser;

public class TalismanSlot extends Sprite {

    public static const MAX_LEVEL = 10;
    public static const MAX_TIER = 3;

    public var slot_:int;
    public var level_:int;
    public var tier_:int;
    public var current_:int;
    public var max_:int;

    public var w_:int;
    public var h_:int;
    public var icon_:Bitmap;
    public var expBar_:StatusBar;

    private var upArrow_:Sprite;
    private var downArrow_:Sprite;

    private static var icons_:Dictionary;

    public function TalismanSlot(w:int, h:int, slot:int) {
        this.w_ = w;
        this.h_ = h;
        this.slot_ = slot;

        this.icon_ = new Bitmap();
        this.icon_.scaleX = 4;
        this.icon_.scaleY = 4;
        this.icon_.x = 8;
        this.icon_.y = 8;
        addChild(this.icon_);

        this.expBar_ = new StatusBar(100, 12, 5931045,3163155, "Lvl X", false, false, false, 10);
        this.expBar_.x = 30;
        this.expBar_.y = 50;
        addChild(this.expBar_);

        var arrowHeight:int = this.expBar_.width * 0.5;

        this.upArrow_ = new Sprite();
        Scrollbar.drawArrow(arrowHeight, this.expBar_.height, this.upArrow_.graphics);
        this.upArrow_.rotation = 0;
        this.upArrow_.x = this.expBar_.width / 2;
        this.upArrow_.y = arrowHeight / 2;
        addChild(this.upArrow_);

        var downArrow:BitmapData = AssetLibrary.getImageFromSet("lofiInterface", 54);
        var downArrowBitmap:Bitmap = new Bitmap(downArrow);

//        this.remove_ = new Sprite();
//        this.remove_.rotation = 270;
//        this.remove_.addChild(downArrowBitmap);
//        addChild(this.remove_);
    }

    public function setExp(exp:int):void {
        this.current_ = exp;
    }

    public function setExpGoal(expGoal:int):void {
        this.max_ = expGoal;
    }

    public function setLevel(level:int):void
    {
        this.level_ = level;
    }

    public function setTier(tier:int):void{
        this.tier_ = tier;
    }

    private function getIconFromTypeAndTier():BitmapData
    {
        if(icons_ == null){
            icons_ = new Dictionary();

            for(var i = 0; i < 32; i++) {
                if(icons_[i] == null) {
                    icons_[i] = new Vector.<BitmapData>();
                }

                var icon = AssetLibrary.getImageFromSet("lofiObj2", 49);
                icons_[i][0] = icon;

                icon = AssetLibrary.getImageFromSet("lofiObj2", 50);
                icons_[i][1] = icon;
            }
        }

        return icons_[this.slot_][this.tier_];
    }

    public function draw():void
    {
        this.icon_.bitmapData = getIconFromTypeAndTier();

        var lvlText:String = "Lvl " + this.level_;
        if(this.level_ == MAX_LEVEL){
            lvlText = "Max";
        }
        this.expBar_.labelText_.text = lvlText;
        this.expBar_.labelText_.updateMetrics();

        this.expBar_.draw(this.current_, this.max_, 0);

        var g:Graphics = this.graphics;
        g.clear();
        g.lineStyle(1,0);
        g.beginFill(0x545454, 1);
        g.drawRoundRect(4, 4, this.w_- 4, this.h_ - 4,16,16);
        g.endFill();

        g.lineStyle(1,0);
        g.beginFill(0x454545,0.7);
        g.drawRoundRect(8, 8, 32, 32, 16, 16);
        g.endFill();
    }
}
}
