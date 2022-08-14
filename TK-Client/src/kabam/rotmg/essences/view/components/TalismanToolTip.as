package kabam.rotmg.essences.view.components {

import com.company.assembleegameclient.objects.ObjectLibrary;
import com.company.assembleegameclient.objects.Player;
import com.company.assembleegameclient.ui.GameObjectListItem;
import com.company.assembleegameclient.ui.GuildText;
import com.company.assembleegameclient.ui.LineBreakDesign;
import com.company.assembleegameclient.ui.RankText;
import com.company.assembleegameclient.ui.StatusBar;
import com.company.assembleegameclient.ui.panels.itemgrids.EquippedGrid;
import com.company.assembleegameclient.ui.tooltip.ToolTip;
import com.company.assembleegameclient.util.FilterUtil;
import com.company.ui.SimpleText;
import com.company.util.BitmapUtil;

import flash.display.Bitmap;

import flash.display.BitmapData;
import flash.filters.DropShadowFilter;

import kabam.rotmg.essences.TalismanProperties;
import kabam.rotmg.messaging.impl.data.StatData;

public class TalismanToolTip extends ToolTip
{
    private static const MAX_WIDTH:int = 230;

    private var props_:TalismanProperties;

    private var icon_:Bitmap;
    private var titleText_:SimpleText;
    private var line1_:LineBreakDesign;
    private var effects:Vector.<Effect>;
    private var explainText_:SimpleText;
    private var line2_:LineBreakDesign;

    public function TalismanToolTip(leveL:int, props:TalismanProperties)
    {
        this.props_ = props;

        super( 0x363636, 1.0, 0x9B9B9B, 1.0);

        var objectType:int = ObjectLibrary.idToType_[this.props_.itemAssociatedWith_];

        var texture:BitmapData = ObjectLibrary.getRedrawnTextureFromType(objectType,60,true,true, 5);
        texture = BitmapUtil.cropToBitmapData(texture,4,4,texture.width - 8,texture.height - 8);
        this.icon_ = new Bitmap(texture);
        addChild(icon_);

        this.titleText_ = new SimpleText(16, 16777215, false, MAX_WIDTH - this.icon_.width - 4 - 30, 0);
        this.titleText_.setBold(true);
        this.titleText_.wordWrap = true;
        this.titleText_.text = ObjectLibrary.typeToDisplayId_[objectType];
        this.titleText_.filters = [new DropShadowFilter(0, 0, 0, 0.5, 12, 12)];
        this.titleText_.x = icon_.width + 4;
        this.titleText_.y = icon_.height / 2 - this.titleText_.actualHeight_ / 2;
        this.titleText_.updateMetrics();
        addChild(this.titleText_);

        this.line1_ = new LineBreakDesign(MAX_WIDTH - 12,0);
        addChild(this.line1_);

        this.effects = new Vector.<Effect>();
//        this.effects.push(new Effect("Base Cost", this.props_.baseUpgradeCost_.toString()));
//        this.effects.push(new Effect("Cost Multiplier", this.props_.costMultiplier_.toString()));
//        this.effects.push(new Effect("Max Level", this.props_.maxLevels_.toString()));

        var tierXML:XML = null;
        for each(tierXML in this.props_.source_.Tier)
        {
            var i:int = tierXML.@tier;

            this.effects.push(new Effect((i == 0 ? "Regular Tier" : i == 1 ? "Legendary Tier" : "Mythic Tier"), "").setColor(i == 0 ? "#acacac" : i == 1 ? "#D6C416" : "#d45069"));

            var xml:XML = null;
            for each(xml in tierXML.StatType){

                var scaleType:String = "";
                if(xml.@scale == "perLevel") {
                    scaleType = " Per Level"
                }

                var symbol:String = "+";
                if(xml.@amount < 0){
                    symbol = "";
                }

                this.effects.push(new Effect("" , symbol + xml.@amount + " " + StatData.statToName(xml.@type) + scaleType));
            }

            xml = null;
            for each(xml in tierXML.ImmuneTo){
                this.effects.push(new Effect("", "Immune to " + xml.@effect));
            }

            xml = null;
            for each(xml in tierXML.LootBoost){
                var scaleType:String = "";
                if(xml.@scale == "perLevel") {
                    scaleType = " Per Level"
                }
                var symbol:String = "+";
                if(xml.@amount < 0){
                    symbol = "";
                }
                this.effects.push(new Effect("", symbol +xml.@amount + "% Loot Boost" + scaleType));
            }

            this.effects.push(new Effect("",""));
        }

        this.explainText_ = new SimpleText(14,11776947,false,MAX_WIDTH - this.icon_.width - 4,0);
        this.explainText_.wordWrap = true;
        this.explainText_.htmlText = BuildEffectsHTML(this.effects);
        this.explainText_.useTextDimensions();
        this.explainText_.filters = FilterUtil.getTextOutlineFilter();
        addChild(this.explainText_);

        this.line2_ = new LineBreakDesign(MAX_WIDTH - 12,0);
        addChild(this.line2_);
    }

    private function BuildEffectsHTML(effects:Vector.<Effect>) : String
    {
        var effect:Effect = null;
        var textColor:String = null;
        var nameColor:String = null;
        var html:String = "";
        var first:Boolean = true;
        for each(effect in effects)
        {
            textColor = "#FFFF8F";
            if(!first)
            {
                html = html + "\n";
            }
            else
            {
                first = false;
            }
            nameColor = effect.color_ != "" ? effect.color_ : null;
            if(effect.name_ != "")
            {
                html = html + ("<font color=\"" + nameColor + "\">" + effect.name_ + "</font>" + ": ");
            }
            html = html + ("<font color=\"" + textColor + "\">" + effect.value_ + "</font>");
        }
        return html;
    }

    override protected function alignUI():void
    {
        this.titleText_.x = (this.icon_.width + 4);
        this.titleText_.y = ((this.icon_.height / 2) - (this.titleText_.height / 2));

        this.line1_.x = 8;
        this.line1_.y = ((this.titleText_.y + this.titleText_.height)) + 16;

        this.explainText_.x = 8;
        this.explainText_.y = ((this.line1_.y + this.line1_.height)) + 8;

        this.line2_.x = 8;
        this.line2_.y = ((this.explainText_.y + this.explainText_.height)) + 8;
    }

    override protected function position() : void
    {
        if(stage == null)  {
            return;
        }

        if(stage.mouseX < stage.stageWidth / 2)
        {
            x = stage.mouseX + 12;
        }
        else
        {
            x = stage.mouseX - width - 1;
        }
        if(x < 12)
        {
            x = 12;
        }
        if(stage.mouseY < stage.stageHeight / 3)
        {
            y = stage.mouseY + 12;
        }
        else
        {
            y = stage.mouseY - height - 1;
        }
        if(y < 12)
        {
            y = 12;
        }
    }

    override public function draw() : void
    {
        super.draw();
    }
}
}

class Effect
{
    public var name_:String;
    public var value_:String;
    public var color_:String = "#B3B3B3";

    function Effect(name:String, value:String)
    {
        this.name_ = name;
        this.value_ = value;
    }

    public function setColor(_arg1:String):Effect {
        this.color_ = _arg1;
        return (this);
    }

}