package kabam.rotmg.ui.view {
import com.company.assembleegameclient.objects.GameObject;
import com.company.assembleegameclient.util.TextureRedrawer;
import com.company.assembleegameclient.util.redrawers.GlowRedrawer;
import com.company.util.AssetLibrary;
import com.company.util.MoreColorUtil;

import flash.display.Bitmap;
import flash.display.BitmapData;
import flash.display.Sprite;
import flash.filters.DropShadowFilter;
import flash.geom.ColorTransform;
import flash.utils.getTimer;

public class BossHealthBar extends Sprite {

    private var background:Bitmap;
    private var foreground:Bitmap;
    private var mask_:Bitmap;
    private var portrait_:Bitmap;
    private var go_:GameObject;
    private var timeSinceNull_:int;

    public function BossHealthBar() {
        portrait_ = new Bitmap();
        portrait_.filters = [new DropShadowFilter(0, 0, 0, 0.5, 12, 12)];
        foreground = new Bitmap(AssetLibrary.getImageFromSet("BossBar", 0));
        background = new Bitmap(AssetLibrary.getImageFromSet("BossBar", 2));
        mask_ = new Bitmap(AssetLibrary.getImageFromSet("BossBar", 4));

        background.mask = mask_;

        addChild(mask_);
        addChild(foreground);
        addChild(background);
        addChild(portrait_);

        mouseEnabled = false;
        mouseChildren = false;

        timeSinceNull_ = getTimer();
    }

    private function getPortrait(go:GameObject):BitmapData
    {
        var portraitTexture:BitmapData = go.props_.portrait_ != null ? go.props_.portrait_.getTexture() : go.texture_;
        var size:int = 8 / portraitTexture.width * 100;
        return GlowRedrawer.outlineGlow(TextureRedrawer.resize(portraitTexture, go.mask_, size, true, go.tex1Id_, go.tex2Id_), 0, 0);
    }

    public function setGameObject(go:GameObject):void
    {
        if(go_ == go){
            return;
        }

        go_ = go;

        var isNull:Boolean = go == null;
        if(!isNull) {
            portrait_.bitmapData = isNull ? null : getPortrait(go);

            foreground.x = portrait_.width + 2;
            foreground.y = portrait_.height / 2 - this.foreground.height / 2;

            background.x = foreground.x;
            background.y = foreground.y + 2;

            mask_.x = background.x;
            mask_.y = background.y + 2;
            visible = true;
        }
    }

    public function draw():void {
        if(go_ == null){
            visible = false;
            mask_.width = background.width;
            return;
        }

        if(go_.isInvulnerable()) {
            background.transform.colorTransform = MoreColorUtil.blueCT;
        }else{
            background.transform.colorTransform = MoreColorUtil.redCT; //fix this "non null" error in console
        }

        mask_.width = (go_.rtHp_ / go_.maxHP_) * background.width;
    }
}
}
