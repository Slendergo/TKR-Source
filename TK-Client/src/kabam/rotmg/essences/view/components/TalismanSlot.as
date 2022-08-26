package kabam.rotmg.essences.view.components {
import avmplus.factoryXml;

import com.company.assembleegameclient.objects.ObjectLibrary;
import com.company.assembleegameclient.ui.Scrollbar;
import com.company.assembleegameclient.ui.StatusBar;
import com.company.assembleegameclient.ui.tooltip.EquipmentToolTip;
import com.company.assembleegameclient.ui.tooltip.PlayerToolTip;
import com.company.assembleegameclient.ui.tooltip.TextToolTip;
import com.company.assembleegameclient.util.TextureRedrawer;
import com.company.assembleegameclient.util.redrawers.GlowRedrawer;
import com.company.ui.SimpleText;
import com.company.util.AssetLibrary;
import com.company.util.MoreColorUtil;

import flash.display.Bitmap;
import flash.display.BitmapData;
import flash.display.Graphics;
import flash.display.Sprite;
import flash.events.Event;
import flash.events.MouseEvent;
import flash.filters.ColorMatrixFilter;
import flash.filters.DropShadowFilter;
import flash.geom.ColorTransform;
import flash.text.TextFieldAutoSize;
import flash.utils.Dictionary;
import flash.utils.getTimer;

import io.decagames.rotmg.ui.buttons.SliceScalingButton;
import io.decagames.rotmg.ui.defaults.DefaultLabelFormat;
import io.decagames.rotmg.ui.sliceScaling.SliceScalingBitmap;
import io.decagames.rotmg.ui.texture.TextureParser;

import kabam.rotmg.essences.TalismanProperties;

import kabam.rotmg.essences.view.EssenceView;

import org.hamcrest.text.re;

public class TalismanSlot extends Sprite {

    public var level_:int;
    public var tier_:int;
    public var current_:int;
    public var max_:int;

    private var view_:EssenceView;
    public var w_:int;
    public var h_:int;
    private var icon_:Bitmap;
    private var expBar_:StatusBar;

    private var sellButton:SliceScalingButton;
    private var sellButton2:SliceScalingButton;
    private var sellButton3:SliceScalingButton;
    private var sellButton4:SliceScalingButton;
    private var sellButton5:SliceScalingButton;

    private var upArrow_:Sprite;
    private var downArrow_:Sprite;
    private var lock_:Bitmap;
    public var levelIcon_:Sprite;

    private var lastUpdateTime_:int;
    private var change_:int;
    public var delta_:int;

    public var active_:Boolean
    public var editing_:Boolean;
    public var enabled_:Boolean;

    private var labelText_:SimpleText;
    private var levelText_:SimpleText;
    private var tooltipText_:SimpleText;

    private var toolTip_:TalismanToolTip;

    private var props_:TalismanProperties;

    public function TalismanSlot(props:TalismanProperties, w:int, h:int, view:EssenceView) {
        this.props_ = props;
        this.view_ = view;
        this.w_ = w;
        this.h_ = h;

        this.icon_ = new Bitmap();
        this.icon_.x = -4;
        this.icon_.y = 0;
        var iconType:int = ObjectLibrary.idToType_[this.props_.itemAssociatedWith_];
        this.icon_.bitmapData = ObjectLibrary.getRedrawnTextureFromType(iconType, 70, true);
        addChild(this.icon_)

        this.labelText_ = new SimpleText(12, 0xFFFFFF,false,100,0);
        this.labelText_.setBold(true);
        this.labelText_.htmlText = "<p align=\"center\">" + this.props_.name_ + "</p>";
        this.labelText_.autoSize = TextFieldAutoSize.CENTER;
        this.labelText_.wordWrap = true;
        this.labelText_.multiline = true;
        this.labelText_.filters = [new DropShadowFilter(0,0,0)];
        if(this.labelText_.height > 20){
            this.labelText_.x = 40;
            this.labelText_.y = 8;
        }else{
            this.labelText_.x = 44;
            this.labelText_.y = 16;
        }
        addChild(this.labelText_);

        this.expBar_ = new StatusBar(100, 12, 5931045,0x363636, "Lvl X", false, false, false, 10, false);
        this.expBar_.x = 30;
        this.expBar_.y = 50;
        this.expBar_.visible = false;
        addChild(this.expBar_);

        var arrowHeight:int = this.expBar_.width * 0.5;

        this.upArrow_ = getSprite(this.onUpArrow);
        Scrollbar.drawArrow(arrowHeight, this.expBar_.height, this.upArrow_.graphics);
        this.upArrow_.rotation = 0;
        this.upArrow_.x = this.expBar_.x + this.expBar_.w_ + this.upArrow_.width;
        this.upArrow_.y = this.expBar_.y + (this.expBar_.h_ / 2) ;
        this.upArrow_.visible = false;
        addChild(this.upArrow_);

        this.downArrow_ = getSprite(this.onDownArrow);
        Scrollbar.drawArrow(arrowHeight, this.expBar_.height, this.downArrow_.graphics);
        this.downArrow_.rotation = -180;
        this.downArrow_.x = this.expBar_.x - this.downArrow_.width;
        this.downArrow_.y = this.upArrow_.y;
        this.downArrow_.mouseEnabled = false;
        this.downArrow_.transform.colorTransform = MoreColorUtil.darkCT;
        this.downArrow_.visible = false;
        addChild(this.downArrow_);

        this.sellButton = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "generic_green_button"));
        this.sellButton.width = 96;
        this.sellButton.setLabel("Imbue", DefaultLabelFormat.defaultModalTitle);
        this.sellButton.x = this.w_ - (this.sellButton.width / 2) - 16;
        this.sellButton.y = this.h_ - (this.sellButton.height / 2) - 8;
        this.sellButton.scaleX = 0.6;
        this.sellButton.scaleY = 0.6;
        this.sellButton.visible = false;
        this.sellButton.addEventListener(MouseEvent.CLICK, this.onAddEssence);
        addChild(this.sellButton);

        this.sellButton2 = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "generic_green_button"));
        this.sellButton2.width = 96;
        this.sellButton2.setLabel("Upgrade", DefaultLabelFormat.defaultModalTitle);
        this.sellButton2.x = this.w_ - (this.sellButton2.width / 2) - 16;
        this.sellButton2.y = this.h_ - (this.sellButton2.height / 2) - 8;
        this.sellButton2.scaleX = 0.6;
        this.sellButton2.scaleY = 0.6;
        this.sellButton2.visible = false;
        this.sellButton2.addEventListener(MouseEvent.CLICK, this.onUpgrade);
        addChild(this.sellButton2);

        this.sellButton3 = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "generic_green_button"));
        this.sellButton3.width = 96;
        this.sellButton3.setLabel("Modify", DefaultLabelFormat.defaultModalTitle);
        this.sellButton3.x = this.w_ - (this.sellButton3.width / 2) - 16;
        this.sellButton3.y = this.h_ - (this.sellButton3.height / 2) - 8;
        this.sellButton3.scaleX = 0.6;
        this.sellButton3.scaleY = 0.6;
        this.sellButton3.addEventListener(MouseEvent.CLICK, this.onModifyClick);
        addChild(this.sellButton3);

        this.sellButton5 = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "generic_green_button"));
        this.sellButton5.width = 96;
        this.sellButton5.setLabel("Activate", DefaultLabelFormat.defaultModalTitle);
        this.sellButton5.scaleX = 0.6;
        this.sellButton5.scaleY = 0.6;
        this.sellButton5.x = (this.sellButton5.width / 2) - 15;
        this.sellButton5.y = this.h_ - (this.sellButton5.height / 2) - 15;
        this.sellButton5.addEventListener(MouseEvent.CLICK, this.onToggleState);
        addChild(this.sellButton5);

        this.sellButton4 = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "generic_green_button"));
        this.sellButton4.width = 96;
        this.sellButton4.setLabel("Back", DefaultLabelFormat.defaultModalTitle);
        this.sellButton4.scaleX = 0.6;
        this.sellButton4.scaleY = 0.6;
        this.sellButton4.x = (this.sellButton4.width / 2) - 15;
        this.sellButton4.y = this.h_ - (this.sellButton4.height / 2) - 15;
        this.sellButton4.addEventListener(MouseEvent.CLICK, this.onCancelEditing);
        this.sellButton4.visible = false;
        addChild(this.sellButton4);

        this.levelIcon_ = new Sprite();
        addChild(this.levelIcon_);

        this.levelText_ = new SimpleText(10, 0xFFFFFF,false,16,0);
        this.levelText_.setBold(true);
        this.levelText_.htmlText = "0";
        this.levelText_.autoSize = TextFieldAutoSize.CENTER;
        this.levelText_.filters = [new DropShadowFilter(0,0,0)];
        this.levelText_.x = 32;
        this.levelText_.y = 29;
        this.levelIcon_.addChild(this.levelText_);

        this.tooltipText_ = new SimpleText(12, 0xFFFFFF,false,76, 76);
        this.tooltipText_.setBold(true);
        this.tooltipText_.htmlText = "<p align=\"center\">Overview</p>";
        this.tooltipText_.autoSize = TextFieldAutoSize.CENTER;
        this.tooltipText_.wordWrap = true;
        this.tooltipText_.multiline = true;
        this.tooltipText_.mouseEnabled = true;
        this.tooltipText_.filters = [new DropShadowFilter(0,0,0)];
        this.tooltipText_.x = (this.w_ / 2) - (76 / 2);
        this.tooltipText_.y = 47;
        this.tooltipText_.addEventListener(MouseEvent.MOUSE_OVER, this.onMouseOver);
        this.tooltipText_.addEventListener(MouseEvent.MOUSE_OUT, this.onMouseOut);
        addChild(this.tooltipText_);

        this.lock_ = new Bitmap(TextureRedrawer.redraw(AssetLibrary.getImageFromSet("lofiInterface2",5),120,true,0));
        this.lock_.x = (this.w_ / 2) - (this.lock_.width / 2);
        this.lock_.y = (this.h_ / 2) - (this.lock_.height / 2) + 18;
        this.lock_.transform.colorTransform = new ColorTransform(1, 1, 0.0)
        this.lock_.visible = false;
        addChild(this.lock_);

        this.draw();
    }

    private function onMouseOver(event:MouseEvent) : void
    {
        this.removeTooltip();
        this.toolTip_ = new TalismanToolTip(this.level_, this.props_);
        stage.addChild(this.toolTip_);
    }

    private function onMouseOut(event:MouseEvent) : void
    {
        this.removeTooltip();
    }

    private function removeTooltip() : void
    {
        if(this.toolTip_ != null)
        {
            if(this.toolTip_.parent != null)
            {
                this.toolTip_.parent.removeChild(this.toolTip_);
            }
            this.toolTip_ = null;
        }
    }

    private function onAddEssence(event:MouseEvent):void
    {
        var remainder:int = this.view_.player.essence_ - this.delta_;
        if(remainder < 0) {
            return;
        }
        this.view_.addEssence.dispatch(this.props_.type_, this.delta_);

        if(this.delta_ == this.max_){
            this.delta_ = 0;
        }
    }

    private function onUpgrade(event:MouseEvent):void
    {
        this.view_.tierUp.dispatch(this.props_.type_, this.delta_);
        if(this.delta_ == this.max_){
            this.delta_ = 0;
        }
    }

    private function onCancelEditing(event:MouseEvent):void
    {
        if(!this.editing_) {
            return;
        }
        this.delta_ = 0;
        this.enable();
        this.view_.editSlot(null);
    }

    private function onModifyClick(event:MouseEvent):void
    {
        if(!this.enabled_ || this.editing_) {
            return;
        }
        this.view_.editSlot(this);
    }

    private function onToggleState(event:MouseEvent):void
    {
        if(!this.active_){
            this.view_.enable.dispatch(this.props_.type_);
        }
        else{
            this.view_.disable.dispatch(this.props_.type_);
        }
    }

    private function onUpArrow(event:MouseEvent):void
    {
        addEventListener(Event.ENTER_FRAME, this.onArrowFrame);
        addEventListener(MouseEvent.MOUSE_UP, this.onArrowUp);
        this.lastUpdateTime_ = getTimer();
        this.change_ = 1;
    }

    private function onDownArrow(event:MouseEvent):void
    {
        addEventListener(Event.ENTER_FRAME, this.onArrowFrame);
        addEventListener(MouseEvent.MOUSE_UP, this.onArrowUp);
        this.lastUpdateTime_ = getTimer();
        this.change_ = -1;
    }

    private function onArrowUp(event:Event) : void
    {
        removeEventListener(Event.ENTER_FRAME, this.onArrowFrame);
        removeEventListener(MouseEvent.MOUSE_UP, this.onArrowUp);
    }

    private function onArrowFrame(event:Event) : void
    {
        var time:int = getTimer();
        var dt:Number = (time - this.lastUpdateTime_) / 1000;
        var dist:int = this.max_ * dt * this.change_;

        this.delta_ = Math.min(this.view_.player.essence_, this.delta_ + dist);

        this.delta_ = Math.min(this.delta_, this.max_ - this.current_);
        this.delta_ = Math.max(this.delta_, 0);

        this.lastUpdateTime_ = time;
    }

    private function getSprite(downFunction:Function):Sprite
    {
        var sprite:Sprite = new Sprite();
        sprite.addEventListener(MouseEvent.MOUSE_DOWN, downFunction);
        sprite.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver);
        sprite.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut);
        return sprite;
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

    public function setActive(active:Boolean):void{
        this.active_ = active;
        this.sellButton5.setLabel(this.active_ ? "Disable" : "Enable", DefaultLabelFormat.defaultModalTitle);
    }

    public function setLevel(level:int):void {
        this.level_ = level;
        this.levelText_.htmlText = level.toString();
        this.levelText_.updateMetrics();
        if(level >= 0 && level < 10){
            this.levelText_.x = 32;
            this.levelText_.y = 29;
        }else{
            this.levelText_.x = 29;
            this.levelText_.y = 29;
        }
    }

    public function setTier(tier:int):void {
        this.tier_ = tier;
    }

    public function enable():void {
        this.enabled_ = true;
        this.editing_ = false;
        this.transform.colorTransform = MoreColorUtil.identity;
        modifyVisibilities();
    }

    public function disable():void {
        this.enabled_ = false;
        this.editing_ = false;
        this.transform.colorTransform = MoreColorUtil.darkCT;
        modifyVisibilities();
    }

    public function edit():void {
        this.enabled_ = false;
        this.editing_ = true;
        this.transform.colorTransform = MoreColorUtil.identity;
        modifyVisibilities();
    }

    public function modifyVisibilities():void {

        var isMaxTier = this.tier_ == (this.props_.tiers_.length - 1);
        var hasReachedGoal = this.current_ + this.delta_ == this.max_;
        var isMaxLevel = this.level_ == this.props_.maxLevels_;
        var isMaxLevelTier = isMaxLevel && hasReachedGoal;

        this.upArrow_.mouseEnabled = !isMaxLevelTier && !hasReachedGoal;
        this.downArrow_.mouseEnabled = !isMaxLevelTier && this.delta_ != 0;
        this.upArrow_.transform.colorTransform = !this.upArrow_.mouseEnabled ? MoreColorUtil.darkCT : MoreColorUtil.identity;
        this.downArrow_.transform.colorTransform = !this.downArrow_.mouseEnabled ? MoreColorUtil.darkCT : MoreColorUtil.identity;

        if(this.editing_) {
            this.sellButton2.visible = isMaxTier ? false : isMaxLevelTier;
            this.sellButton.visible = !(isMaxTier && isMaxLevel) && !this.sellButton2.visible;
            this.sellButton3.visible = false;
            this.sellButton5.visible = false;
            this.expBar_.visible = !(isMaxTier && isMaxLevel);
            this.lock_.visible = false;
            this.upArrow_.visible = this.expBar_.visible;
            this.downArrow_.visible = this.expBar_.visible;
            this.sellButton4.visible = true;
            this.tooltipText_.visible = false;
            return;
        }

        if(this.enabled_){
            this.sellButton.visible = false;
            this.sellButton2.visible = false;
            this.sellButton3.visible = !(isMaxTier && isMaxLevel);
            this.sellButton5.visible = true;
            this.expBar_.visible = false;
            this.lock_.visible = false;
            this.upArrow_.visible = false;
            this.downArrow_.visible = false;
            this.sellButton4.visible = false;
            this.tooltipText_.visible = true;
            this.tooltipText_.mouseEnabled = true;
            return;
        }

        this.sellButton.visible = false;
        this.sellButton2.visible = false;
        this.sellButton3.visible = false;
        this.sellButton5.visible = false;
        this.expBar_.visible = false;
        this.lock_.visible = true;
        this.upArrow_.visible = false;
        this.downArrow_.visible = false;
        this.sellButton4.visible = false;
        this.tooltipText_.visible = true;
        this.tooltipText_.mouseEnabled = false;
    }


    public function draw():void
    {
        if(this.toolTip_ != null)  {
            this.toolTip_.draw();
        }

        var colorActiveOutline:uint = 0x0062ff;
        var colorEnabledOutline:uint = 0x7dc76b;
        var colorDisabledOutline:uint = 0xc76b6b;
        var colorEditingOutline:uint = 0xa39c1a;

        var g:Graphics = this.graphics;

        g.clear();
        g.lineStyle(2,this.active_ ? colorActiveOutline : this.editing_ ? colorEditingOutline : this.enabled_ ? colorEnabledOutline : colorDisabledOutline);
        g.beginFill(0x545454, 1);
        g.drawRoundRect(4, 4, this.w_- 4, this.h_ - 4,16,16);
        g.endFill();

        g.lineStyle(1,0);
        g.beginFill(this.tier_ == 0 ? 0x808080 : this.tier_ == 1 ? 0xD6C416 : 0x7B091C,0.7);
        g.drawRoundRect(8, 8, 32, 32, 16, 16);
        g.endFill();

        if(this.editing_) {
            this.expBar_.setBarColor(this.tier_ == 0 ? 0x808080 : this.tier_ == 1 ? 0xD6C416 : 0xD45069);
            this.expBar_.draw(this.current_ + this.delta_, this.max_, 0);
        } else {
            g.lineStyle(1,0);
            g.beginFill(0x454545);
            g.drawRoundRect((this.w_ / 2) - (76 / 2), 47, 76, 20, 16, 16);
            g.endFill();
        }

        g = this.levelIcon_.graphics;
        g.clear();
        g.lineStyle(1 ,0, 0.9);
        g.beginFill(0x808080, 0.9);
        g.drawCircle(36, 36, 9);
        g.endFill();
    }
}
}
