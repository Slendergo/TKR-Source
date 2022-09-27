package kabam.rotmg.ui.view {
import com.company.assembleegameclient.objects.GameObject;
import com.company.assembleegameclient.util.TextureRedrawer;
import com.company.assembleegameclient.util.redrawers.GlowRedrawer;
import com.company.ui.SimpleText;
import com.company.util.AssetLibrary;
import com.company.util.MoreColorUtil;

import flash.display.Bitmap;
import flash.display.BitmapData;
import flash.display.Sprite;
import flash.filters.DropShadowFilter;
import flash.geom.ColorTransform;
import flash.utils.flash_proxy;
import flash.utils.getTimer;

public class BossHealthBar extends Sprite {

    private var background2:Bitmap;
    private var background:Bitmap;
    private var foreground:Bitmap;
    private var mask_:Bitmap;
    private var portrait_:Bitmap;
    private var go_:GameObject;
    private var timeSinceNull_:int;
    private var hpText_:SimpleText

    public function BossHealthBar() {
        portrait_ = new Bitmap();
        portrait_.filters = [new DropShadowFilter(0, 0, 0, 0.5, 12, 12)];
        foreground = new Bitmap(AssetLibrary.getImageFromSet("BossBar", 0));
        background2 = new Bitmap(AssetLibrary.getImageFromSet("BossBar", 2));
        background = new Bitmap(AssetLibrary.getImageFromSet("BossBar", 2));
        mask_ = new Bitmap(AssetLibrary.getImageFromSet("BossBar", 4));

        background.mask = mask_;

        hpText_ = new SimpleText(12,16777215,false,0,0);
        hpText_.setBold(true);
        hpText_.filters = [new DropShadowFilter(0,0,0)];

        addChild(mask_);
        addChild(foreground);
        addChild(background2);
        addChild(background);
        addChild(portrait_);
        addChild(hpText_);

        mask_.scaleX = 1.5;
        mask_.scaleY = 1.5;
        foreground.scaleX = mask_.scaleX;
        foreground.scaleY = mask_.scaleY;
        background2.scaleX = mask_.scaleX;
        background2.scaleY = mask_.scaleY;
        background.scaleX = mask_.scaleX;
        background.scaleY = mask_.scaleY;

        mouseEnabled = false;
        mouseChildren = false;

        timeSinceNull_ = getTimer();
    }

    private function getPortrait(go:GameObject):BitmapData {
        var portraitTexture:BitmapData = go.props_.portrait_ != null ? go.props_.portrait_.getTexture() : go.texture_;
        var size:int = 8 / portraitTexture.width * 100;
        return GlowRedrawer.outlineGlow(TextureRedrawer.resize(portraitTexture, go.mask_, size, true, go.tex1Id_, go.tex2Id_), 0, 0);
    }

    public function setGameObject(go:GameObject):void {
        if (go_ == go) {
            return;
        }

        go_ = go;

        var isNull:Boolean = go == null;
        if (!isNull) {
            portrait_.bitmapData = isNull ? null : getPortrait(go);

            foreground.x = portrait_.width + 2;
            foreground.y = portrait_.height / 2 - this.foreground.height / 2;

            background.x = foreground.x;
            background.y = foreground.y + 2;

            background2.x = foreground.x;
            background2.y = foreground.y + 2;

            mask_.x = background.x;
            mask_.y = background.y + 2;
            visible = true;
        }
    }

    public function draw():void {
        if (go_ == null) {
            visible = false;
            mask_.width = background.width;
            return;
        }

        hpText_.text = go_.hp_ + "/" + go_.maxHP_;
        hpText_.updateMetrics();

        hpText_.x = foreground.x + foreground.width / 2 - hpText_.width / 2;
        hpText_.y = foreground.y + foreground.height / 2 - hpText_.height / 2 - 2;

        if (go_.isInvulnerable()) {
            background.transform.colorTransform = new ColorTransform(50 / 255, 100 / 255, 190 / 255);
            background2.transform.colorTransform = new ColorTransform(50 / 255, 100 / 255, 190 / 255, 0.5);
        } else if (go_.isArmored() && !go_.isArmorBroken()) {
            background.transform.colorTransform = new ColorTransform(85 / 255, 60 / 255, 50 / 255);
            background2.transform.colorTransform = new ColorTransform(85 / 255, 60 / 255, 50 / 255, 0.5);
        } else {
            background.transform.colorTransform = new ColorTransform(0, 1, 0);
            background2.transform.colorTransform = new ColorTransform(0, 1, 0, 0.5);
        }

        if(go_.hp_ > go_.rtHp_)
            go_.rtHp_ = go_.hp_;
        mask_.width = (go_.rtHp_ / go_.maxHP_) * background.width;
    }
}
}