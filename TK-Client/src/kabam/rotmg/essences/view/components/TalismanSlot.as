package kabam.rotmg.essences.view.components {
import com.company.assembleegameclient.ui.Scrollbar;
import com.company.assembleegameclient.ui.StatusBar;
import com.company.assembleegameclient.ui.tooltip.TextToolTip;
import com.company.util.AssetLibrary;

import flash.display.Bitmap;
import flash.display.BitmapData;
import flash.display.Graphics;
import flash.display.Sprite;
import flash.events.MouseEvent;
import flash.geom.ColorTransform;
import flash.utils.Dictionary;

import io.decagames.rotmg.ui.buttons.SliceScalingButton;
import io.decagames.rotmg.ui.defaults.DefaultLabelFormat;
import io.decagames.rotmg.ui.sliceScaling.SliceScalingBitmap;
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

        this.upArrow_ = getSprite(this.onUpArrow);
        Scrollbar.drawArrow(arrowHeight, this.expBar_.height, this.upArrow_.graphics);
        this.upArrow_.rotation = 0;
        this.upArrow_.x = this.expBar_.x + this.expBar_.w_ + this.upArrow_.width;
        this.upArrow_.y = this.expBar_.y + (this.expBar_.h_ / 2) ;//- (this.upArrow_.height / 2);
        addChild(this.upArrow_);

        this.downArrow_ = getSprite(this.onUpArrow);
        Scrollbar.drawArrow(arrowHeight, this.expBar_.height, this.downArrow_.graphics);
        this.downArrow_.rotation = -180;
        this.downArrow_.x = this.expBar_.x - this.downArrow_.width;
        this.downArrow_.y = this.upArrow_.y;
        addChild(this.downArrow_);

        this.sellButton = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "generic_green_button"));
        this.sellButton.width = 96;
        this.sellButton.setLabel("Imbue", DefaultLabelFormat.defaultModalTitle);
        this.sellButton.x = this.w_ - (this.sellButton.width / 2) - 16;
        this.sellButton.y = this.h_ - (this.sellButton.height / 2) - 8;
        this.sellButton.scaleX = 0.6;
        this.sellButton.scaleY = 0.6;
        addChild(this.sellButton);
    }
    private var sellButton:SliceScalingButton;

    private function getSprite(downFunction:Function):Sprite
    {
        var sprite:Sprite = new Sprite();
        sprite.addEventListener(MouseEvent.MOUSE_DOWN,downFunction);
        sprite.addEventListener(MouseEvent.ROLL_OVER,this.onRollOver);
        sprite.addEventListener(MouseEvent.ROLL_OUT,this.onRollOut);
        return sprite;
    }

    private function onUpArrow(event:MouseEvent) : void
    {

    }

    private function onRollOver(event:MouseEvent) : void
    {
        var sprite:Sprite = event.target as Sprite;
        sprite.transform.colorTransform = new ColorTransform(1,0.8627,0.5216);
    }

    private function onRollOut(event:MouseEvent) : void
    {
        var sprite:Sprite = event.target as Sprite;
        sprite.transform.colorTransform = new ColorTransform(1,1,1);
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
