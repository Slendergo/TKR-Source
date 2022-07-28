package kabam.rotmg.BountyBoard.SubscriptionUI.playerList
{
import com.company.assembleegameclient.objects.ObjectLibrary;
import com.company.assembleegameclient.objects.Player;
import com.company.assembleegameclient.util.GuildUtil;
import com.company.assembleegameclient.util.TextureRedrawer;
import com.company.ui.SimpleText;

import flash.display.Bitmap;
import flash.display.BitmapData;
import flash.display.Sprite;
import flash.events.Event;
import flash.events.MouseEvent;
import flash.filters.DropShadowFilter;

import io.decagames.rotmg.ui.buttons.SliceScalingButton;
import io.decagames.rotmg.ui.texture.TextureParser;

import kabam.rotmg.BountyBoard.SubscriptionUI.SubscriptionUI;

import kabam.rotmg.classes.model.CharacterClass;

import kabam.rotmg.classes.model.ClassesModel;

import kabam.rotmg.core.StaticInjectorContext;

public class BountyPlayerListLine extends Sprite
{

    public static const WIDTH:int = 756;

    public static const HEIGHT:int = 28;


    /* Char Image */
    private var characterImage_:Bitmap;

    /* Class Text */
    private var classText_:SimpleText;

    /* Maxed Stats */
    private var maxedText_:SimpleText;

    /* Name Text */
    private var nameText_:SimpleText;
    public var name_:String;

    /* Rank Image */
    private var rankIcon_:Bitmap;

    /* On/Off Button */
    private var enableButton_:SliceScalingButton;

    /*Info that we will send after */
    public var playerId:int;
    public var playerAccepted:Boolean = true;

    public var place:int;

    public var ui:SubscriptionUI;

    function BountyPlayerListLine(place:int, player:Player, UI:SubscriptionUI, isMe:Boolean)
    {
        super();
        this.place = place;
        this.ui = UI;
        this.name_ = player.name_;
        this.playerId = player.objectId_;
        var textColor:uint = 11776947;
        if(isMe)
        {
            textColor = 16564761;
        }

        /* Character Image */
        var icon:BitmapData = TextureRedrawer.redraw(player.getPortrait2(),20,true,0,true);
        this.characterImage_ = new Bitmap(icon);
        this.characterImage_.x = 0;
        this.characterImage_.y = HEIGHT / 2 - this.characterImage_.height / 2;
        addChild(this.characterImage_);

        /* Class Text */
        this.classText_ = new SimpleText(18,textColor,false,0,0);
        this.classText_.setBold(true);
        this.classText_.text = ObjectLibrary.typeToDisplayId_[player.objectType_];
        this.classText_.useTextDimensions();
        this.classText_.filters = [new DropShadowFilter(0,0,0,1,8,8)];
        this.classText_.x = this.characterImage_.x + 50;
        this.classText_.y = HEIGHT / 2 - this.classText_.height / 2;
        addChild(this.classText_);

        /* Name Text */
        this.nameText_ = new SimpleText(18,textColor,false,0,0);
        this.nameText_.setBold(true);
        this.nameText_.text = this.name_;
        this.nameText_.useTextDimensions();
        this.nameText_.filters = [new DropShadowFilter(0,0,0,1,8,8)];
        this.nameText_.x = this.classText_.x + 75;
        this.nameText_.y = HEIGHT / 2 - this.nameText_.height / 2;
        addChild(this.nameText_);

        /* Maxed Text */
        var maxedStat:int = MaxText(player);
        this.maxedText_ = new SimpleText(18,0x1aff1a,false,0,0);
        this.maxedText_.setBold(true);
        this.maxedText_.text = maxedStat + "/8";
        if(player.upgraded_)
        {
            this.maxedText_.text = maxedStat + "/16";
        }
        this.maxedText_.useTextDimensions();
        this.maxedText_.filters = [new DropShadowFilter(0,0,0,1,8,8)];
        this.maxedText_.x = this.nameText_.x + 115;
        this.maxedText_.y = HEIGHT / 2 - this.maxedText_.height / 2;
        addChild(this.maxedText_);

        /* Rank Image */
        this.rankIcon_ = new Bitmap(GuildUtil.rankToIcon(player.guildRank_,20));
        this.rankIcon_.x = this.maxedText_.x + 50;
        this.rankIcon_.y = HEIGHT / 2 - this.rankIcon_.height / 2;
        addChild(this.rankIcon_);

        /* On/Off Button */
        this.enableButton_ = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "tier_claimed"));
        this.enableButton_.x = 425;
        this.enableButton_.y = HEIGHT / 2 - this.enableButton_.height / 2;
        this.enableButton_.addEventListener(MouseEvent.CLICK, this.checkAccepted);
        addChild(this.enableButton_);

    }

    public function checkAccepted(evt:Event) : void
    {
        if(this.playerAccepted)
        {
            ui.checkDisablePlayer(place);
            this.enableButton_.changeBitmap("tier_claimed_empty");
            playerAccepted = false;
        }
        else
        {
            var check:Boolean = ui.checkEnablePlayer(place);
            if(check)
            {
                this.enableButton_.changeBitmap("tier_claimed");
                playerAccepted = true; 
            }
        }
    }

    public function disable() : void
    {
        this.enableButton_.changeBitmap("tier_claimed_empty");
        playerAccepted = false;
    }

    public function enable() : void
    {
        this.enableButton_.changeBitmap("tier_claimed");
        playerAccepted = true;
    }

    private function MaxText(player:Player):int
    {
        var classes:ClassesModel = StaticInjectorContext.getInjector().getInstance(ClassesModel);
        var maxed:int = 0;
        var charType:CharacterClass = classes.getCharacterClass(player.objectType_);
        if(player.hp_ >= charType.hp.max)
        {
            maxed++;
        }
        if(player.hp_ >= charType.hp.max + 50)
        {
            maxed++;
        }
        if(player.mp_ >= charType.mp.max)
        {
            maxed++;
        }
        if(player.mp_ >= charType.mp.max + 50)
        {
            maxed++;
        }
        if(player.attack_ >= charType.attack.max)
        {
            maxed++;
        }
        if(player.attack_ >= charType.attack.max + 10)
        {
            maxed++;
        }
        if(player.defense_ >= charType.defense.max)
        {
            maxed++;
        }
        if(player.defense_ >= charType.defense.max + 10)
        {
            maxed++;
        }
        if(player.wisdom_ >= charType.mpRegeneration.max)
        {
            maxed++;
        }
        if(player.wisdom_ >= charType.mpRegeneration.max + 10)
        {
            maxed++;
        }
        if(player.vitality_ >= charType.hpRegeneration.max)
        {
            maxed++;
        }
        if(player.vitality_ >= charType.hpRegeneration.max + 10)
        {
            maxed++;
        }
        if(player.dexterity_ >= charType.dexterity.max)
        {
            maxed++;
        }
        if(player.dexterity_ >= charType.dexterity.max + 10)
        {
            maxed++;
        }
        if(player.speed_ >= charType.speed.max)
        {
            maxed++;
        }
        if(player.speed_ >= charType.speed.max + 10)
        {
            maxed++;
        }
        return maxed;
    }

}
}
