package kabam.rotmg.BountyBoard {
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.objects.GameObject;
import com.company.assembleegameclient.objects.Player;
import com.company.assembleegameclient.parameters.Parameters;
import com.company.ui.SimpleText;
import com.gskinner.motion.GTween;

import flash.display.Graphics;
import flash.display.Shape;
import flash.events.Event;

import flash.events.MouseEvent;
import flash.filters.ColorMatrixFilter;
import flash.filters.GlowFilter;
import flash.text.TextField;
import flash.text.TextFieldAutoSize;

import io.decagames.rotmg.ui.buttons.SliceScalingButton;
import io.decagames.rotmg.ui.defaults.DefaultLabelFormat;

import io.decagames.rotmg.ui.popups.header.PopupHeader;

import io.decagames.rotmg.ui.popups.modal.ModalPopup;
import io.decagames.rotmg.ui.texture.TextureParser;

import kabam.rotmg.BountyBoard.SubscriptionUI.SubscriptionUI;

import kabam.rotmg.BountyBoard.assets.EasyBountyUIImage;
import kabam.rotmg.core.StaticInjectorContext;
import kabam.rotmg.game.model.AddTextLineVO;
import kabam.rotmg.game.model.GameModel;
import kabam.rotmg.game.signals.AddTextLineSignal;

import org.osflash.signals.Signal;

public class BountyBoardModal extends ModalPopup{

    internal var gs_:GameSprite;
    internal var quitButton:SliceScalingButton;
    internal var player:Player;
    internal var text_:String;

    internal var close:Signal = new Signal();
    internal var clicked:Signal = new Signal();

    private var easyBountyContainer:Shape;
    private var hardBountyContainer:Shape;
    private var hellBountyContainer:Shape;
    private var godlyBountyContainer:Shape;

    public var easyBountyButton:SliceScalingButton;
    public var hardBountyButton:SliceScalingButton;
    public var hellBountyButton:SliceScalingButton;
    public var godlyBountyButton:SliceScalingButton;

    private var easyBountyTitle:SimpleText;
    private var hardBountyTitle:SimpleText;
    private var hellBountyTitle:SimpleText;
    private var godlyBountyTitle:SimpleText;

    private var easyBountyDescription:SimpleText;
    private var hardBountyDescription:SimpleText;
    private var hellBountyDescription:SimpleText;
    private var godlyBountyDescription:SimpleText;

    private var easyRecommendSize:SimpleText;
    private var hardRecommendSize:SimpleText;
    private var hellRecommendSize:SimpleText;
    private var godlyRecommendSize:SimpleText;

    private var easyCost:SimpleText;
    private var hardCost:SimpleText;
    private var hellCost:SimpleText;
    private var godlyCost:SimpleText;

    private var easyBountyImage:EasyBountyUIImage;

    private var addTextLine:AddTextLineSignal;

    public var gameObject:GameObject;

    public function BountyBoardModal(gs:GameSprite, gm:GameObject) {
        this.gs_ = gs;
        this.gameObject = gm;
        super(700, 500, "Guild Bounties");
        this.alpha = 0;
        new GTween(this, 0.2, {"alpha": 1});
        this.x = 27.5;
        this.y = 22.5;
        this.addTextLine = StaticInjectorContext.getInjector().getInstance(AddTextLineSignal);
        this.player = StaticInjectorContext.getInjector().getInstance(GameModel).player;
        this.quitButton = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI","close_button"));
        this.header.addButton(this.quitButton, PopupHeader.RIGHT_BUTTON);
        this.quitButton.addEventListener(MouseEvent.CLICK,this.onClose);

        //CONTAINERS
        this.easyBountyContainer = new Shape();
        var local0:Graphics = this.easyBountyContainer.graphics;
        var thickness:Number = 2;
        var color:Number = 0x00ff00;
        local0.clear();
        local0.lineStyle(thickness, color);
        local0.beginFill(0x013220, 0.8);
        local0.drawRoundRect(60, 10, 270, 220,5,5);
        local0.endFill();
        //shadow with glow filter
        this.easyBountyContainer.filters = [new GlowFilter(0, 1, 10, 10, 1, 3)];
        addChild(this.easyBountyContainer);

        this.hardBountyContainer = new Shape();
        var local1:Graphics = this.hardBountyContainer.graphics;
        var color11:Number = 0x0000ff;
        local1.clear();
        local1.lineStyle(thickness, color11);
        local1.beginFill(0x00008b , 0.8);
        local1.drawRoundRect(360, 10, 270, 220,5,5);
        local1.endFill();
        //shadow with glow filter
        this.hardBountyContainer.filters = [new GlowFilter(0, 1, 10, 10, 1, 3)];
        addChild(this.hardBountyContainer);

        this.hellBountyContainer = new Shape();
        var local2:Graphics = this.hellBountyContainer.graphics;
        var color2:Number = 0xff0000;
        local2.clear();
        local2.lineStyle(thickness, color2);
        local2.beginFill(0x8B0000, 0.8);
        local2.drawRoundRect(60, 260, 270, 220,5,5);
        local2.endFill();
        //shadow with glow filter
        this.hellBountyContainer.filters = [new GlowFilter(0, 1, 10, 10, 1, 3)];
        addChild(this.hellBountyContainer);

        this.godlyBountyContainer = new Shape();
        var local3:Graphics = this.godlyBountyContainer.graphics;
        var color3:Number = 0x6a0dad;
        local3.clear();
        local3.lineStyle(thickness, color3);
        local3.beginFill(0x301934, 0.8);
        local3.drawRoundRect(360, 260, 270, 220,5,5);
        local3.endFill();
        //shadow with glow filter
        this.godlyBountyContainer.filters = [new GlowFilter(0, 1, 10, 10, 1, 3)];
        addChild(this.godlyBountyContainer);

        //UI IMAGES
        this.easyBountyImage = new EasyBountyUIImage();
        this.easyBountyImage.x= 90;
        this.easyBountyImage.y= 50;
        this.easyBountyImage.width = easyBountyImage.measuredWidth*2;
        this.easyBountyImage.height = easyBountyImage.measuredHeight*2;
        this.easyBountyImage.filters =  [ new ColorMatrixFilter([0.2086, 0.5094, 0.0720, 0, 0, 0.2086, 0.5094, 0.0720, 0, 0, 0.2086, 0.5094, 0.0720, 0, 0, 0, 0, 0, 1, 0]) ];
       //this.easyBountyImage.filters =  [ new ColorMatrixFilter([0, 0, 0, 0, 0, 0, 0 ,0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0]) ];
        addChild(this.easyBountyImage);
        //BUTTONS
        this.easyBountyButton = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI","generic_green_button", 155));
        this.easyBountyButton.setLabel("Begin", DefaultLabelFormat.defaultModalTitle);
        this.easyBountyButton.width = 110;
        this.easyBountyButton.x = 140;
        this.easyBountyButton.y = 180;
        this.easyBountyButton.addEventListener(MouseEvent.CLICK, this.showEasySubscription);
        addChild(this.easyBountyButton);

        this.hardBountyButton = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI","generic_green_button", 155));
        this.hardBountyButton.setLabel("Begin", DefaultLabelFormat.defaultModalTitle);
        this.hardBountyButton.width = 110;
        this.hardBountyButton.x = 440;
        this.hardBountyButton.y = 180;
        this.hardBountyButton.addEventListener(MouseEvent.CLICK, this.showHardSubscription);
        addChild(this.hardBountyButton);

        this.hellBountyButton = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI","generic_green_button", 155));
        this.hellBountyButton.setLabel("Begin", DefaultLabelFormat.defaultModalTitle);
        this.hellBountyButton.width = 110;
        this.hellBountyButton.x = 140;
        this.hellBountyButton.y = 430;
        this.hellBountyButton.addEventListener(MouseEvent.CLICK, this.showHellSubscription);
        addChild(this.hellBountyButton);

        this.godlyBountyButton = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI","generic_green_button", 155));
        this.godlyBountyButton.setLabel("Begin", DefaultLabelFormat.defaultModalTitle);
        this.godlyBountyButton.width = 110;
        this.godlyBountyButton.x = 440;
        this.godlyBountyButton.y = 430;
        this.godlyBountyButton.addEventListener(MouseEvent.CLICK, this.showGodlySubscription);
        addChild(this.godlyBountyButton);

        //TITLE TEXTS
        this.easyBountyTitle = new SimpleText(18, 0xFFFFFF,false, 230,40,false);
        this.easyBountyTitle.filters = [new GlowFilter(0,1,2,2,10,1)];
        this.easyBountyTitle.x = 100;
        this.easyBountyTitle.y = 30;
        this.easyBountyTitle.text = "SUBDUE THE SUCCUBUS";
        addChild(this.easyBountyTitle);

        this.hardBountyTitle = new SimpleText(18, 0xFFFFFF,false, 230,40,false);
        this.hardBountyTitle.filters = [new GlowFilter(0,1,2,2,10,1)];
        this.hardBountyTitle.x = 420;
        this.hardBountyTitle.y = 30;
        this.hardBountyTitle.text = "WELCOME TO HELL";
        addChild(this.hardBountyTitle);
        this.hellBountyTitle = new SimpleText(18, 0xFFFFFF,false, 230,40,false);
        this.hellBountyTitle.filters = [new GlowFilter(0,1,2,2,10,1)];
        this.hellBountyTitle.x = 100;
        this.hellBountyTitle.y = 280;
        this.hellBountyTitle.text = "A DATE WITH THE DEVIL";
        addChild(this.hellBountyTitle);

        this.godlyBountyTitle = new SimpleText(18, 0xFFFFFF,false, 230,40,false);
        this.godlyBountyTitle.filters = [new GlowFilter(0,1,2,2,10,1)];
        this.godlyBountyTitle.x = 390;
        this.godlyBountyTitle.y = 280;
        this.godlyBountyTitle.text = "THE PRESENCE OF THE KING";
        addChild(this.godlyBountyTitle);
        //DESCRIPTION TEXTS
        this.easyBountyDescription = new SimpleText(14, 0xFFFFFF,false, 230,140,false);
        this.easyBountyDescription.wordWrap = true;
        this.easyBountyDescription.filters = [new GlowFilter(0,1,2,2,10,1)];
        this.easyBountyDescription.x = 80;
        this.easyBountyDescription.y = 60;
        this.easyBountyDescription.autoSize = TextFieldAutoSize.CENTER;
        this.easyBountyDescription.text = "Help.. Anybody who sees this... we may not have much time left... there is a seductive creature in the Bloodlust Caves... we are trapped here, helpless to the creature... This creature seems to be a sort of Succubus... Heroes, I implore thee to help us!";
        addChild(this.easyBountyDescription);
        this.hardBountyDescription = new SimpleText(14, 0xFFFFFF,false, 230,140,false);
        this.hardBountyDescription.wordWrap = true;
        this.hardBountyDescription.filters = [new GlowFilter(0,1,2,2,10,1)];
        this.hardBountyDescription.x = 375;
        this.hardBountyDescription.y = 60;
        this.hardBountyDescription.autoSize = TextFieldAutoSize.CENTER;
        this.hardBountyDescription.text = "To anyone listening to this tape... it is already too late for me. Beware of the Six Guardians, their temper... The smell of blood, iron and fire... The sounds of chains is overbeari- Oh god... It's here, the mad dog, oHH gOd.. AAAGhghhAHHHHhhghh-!";
        addChild(this.hardBountyDescription);
        this.hellBountyDescription = new SimpleText(14, 0xFFFFFF,false, 230,140,false);
        this.hellBountyDescription.wordWrap = true;
        this.hellBountyDescription.filters = [new GlowFilter(0,1,2,2,10,1)];
        this.hellBountyDescription.x = 80;
        this.hellBountyDescription.y = 320;
        this.hellBountyDescription.autoSize = TextFieldAutoSize.CENTER;
        this.hellBountyDescription.text = "Heroes, news of you defeating the Great Demon Hound and It's Master has spread... Hades request an audience with you... I don't recommend making him wait... good luck.";
        addChild(this.hellBountyDescription);
        this.godlyBountyDescription = new SimpleText(14, 0xFFFFFF,false, 230,140,false);
        this.godlyBountyDescription.wordWrap = true;
        this.godlyBountyDescription.filters = [new GlowFilter(0,1,2,2,10,1)];
        this.godlyBountyDescription.x = 375;
        this.godlyBountyDescription.y = 320;
        this.godlyBountyDescription.autoSize = TextFieldAutoSize.CENTER;
        this.godlyBountyDescription.text = "The Great Monarch in the Sky, The Monkey King, has awakened from his milliennial slumber and sits upon the golden throne in Heaven. He fears no one and thirsts for blood, so he invites anyone to challenge him. Heroes, I wish you luck!";
        addChild(this.godlyBountyDescription);
        //RECOMMENDED TEXTS
        this.easyRecommendSize = new SimpleText(12, 0xFFFF00,false, 230,140,false);
        this.easyRecommendSize.wordWrap = true;
        this.easyRecommendSize.filters = [new GlowFilter(0,1,2,2,10,1)];
        this.easyRecommendSize.x = 90;
        this.easyRecommendSize.y = 160;
        this.easyRecommendSize.text = "Recommended Raid Size: 15-20 Members.";
        addChild(this.easyRecommendSize);
        this.hardRecommendSize = new SimpleText(12, 0xFFFF00,false, 230,140,false);
        this.hardRecommendSize.wordWrap = true;
        this.hardRecommendSize.filters = [new GlowFilter(0,1,2,2,10,1)];
        this.hardRecommendSize.x = 390;
        this.hardRecommendSize.y = 160;
        this.hardRecommendSize.text = "Recommended Raid Size: 50-55 Members.";
        addChild(this.hardRecommendSize);
        this.hellRecommendSize = new SimpleText(12, 0xFFFF00,false, 230,140,false);
        this.hellRecommendSize.wordWrap = true;
        this.hellRecommendSize.filters = [new GlowFilter(0,1,2,2,10,1)];
        this.hellRecommendSize.x = 90;
        this.hellRecommendSize.y = 420;
        this.hellRecommendSize.text = "Recommended Raid Size: 70-75 Members.";
        addChild(this.hellRecommendSize);
        this.godlyRecommendSize = new SimpleText(12, 0xFFFF00,false, 230,140,false);
        this.godlyRecommendSize.wordWrap = true;
        this.godlyRecommendSize.filters = [new GlowFilter(0,1,2,2,10,1)];
        this.godlyRecommendSize.x = 390;
        this.godlyRecommendSize.y = 420;
        this.godlyRecommendSize.text = "Recommended Raid Size: 999+ Members.";
        addChild(this.godlyRecommendSize);
        //COSTS
        this.easyCost = new SimpleText(12, 0xFFFF00, false, 230,140,false);
        this.easyCost.wordWrap =true;
        this.easyCost.filters = [new GlowFilter(0,1,2,2,10,1)];
        this.easyCost.x = 150;
        this.easyCost.y = 215;
        this.easyCost.text = "Cost: 10,000 Guild Fame per Raid.";
        addChild(this.easyCost);

        this.hardCost = new SimpleText(12, 0xFFFF00, false, 230,140,false);
        this.hardCost.wordWrap =true;
        this.hardCost.filters = [new GlowFilter(0,1,2,2,10,1)];
        this.hardCost.x = 460;
        this.hardCost.y = 215;
        this.hardCost.text = "Cost: 50,000 Guild Fame per Raid.";
        addChild(this.hardCost);

        this.hellCost = new SimpleText(12, 0xFFFF00, false, 230,140,false);
        this.hellCost.wordWrap =true;
        this.hellCost.filters = [new GlowFilter(0,1,2,2,10,1)];
        this.hellCost.x = 150;
        this.hellCost.y = 465;
        this.hellCost.text = "Cost: 75,000 Guild Fame per Raid.";
        addChild(this.hellCost);

        this.godlyCost = new SimpleText(12, 0xFFFF00, false, 230,140,false);
        this.godlyCost.wordWrap =true;
        this.godlyCost.filters = [new GlowFilter(0,1,2,2,10,1)];
        this.godlyCost.x = 455;
        this.godlyCost.y = 465;
        this.godlyCost.text = "Cost: 150,000 Guild Fame per Raid.";
        addChild(this.godlyCost);
    }
    public function onClose(arg1:Event) : void
    {
        this.close.dispatch();
    }

    private function showEasySubscription(e:MouseEvent):void {
        this.openDialog.dispatch(new SubscriptionUI("Preparation For: Easy Bounty", this.gs_, 1, this.gameObject));
    }

    private function showHardSubscription(e:MouseEvent):void {
        this.openDialog.dispatch(new SubscriptionUI("Preparation For: Hard Bounty", this.gs_, 2, this.gameObject));
    }

    private function showHellSubscription(e:MouseEvent):void {
        this.openDialog.dispatch(new SubscriptionUI("Preparation For: Hell Bounty", this.gs_, 3, this.gameObject));
    }

    private function showGodlySubscription(e:MouseEvent):void {
        this.openDialog.dispatch(new SubscriptionUI("Preparation For: Godly Bounty", this.gs_, 4, this.gameObject));
    }

}
}
