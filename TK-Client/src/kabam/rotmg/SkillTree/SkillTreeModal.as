package kabam.rotmg.SkillTree {
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.objects.Player;
import com.company.assembleegameclient.ui.tooltip.BigSkillTreeToolTip;
import com.company.assembleegameclient.ui.tooltip.SkillTreeToolTip;
import com.company.assembleegameclient.ui.tooltip.ToolTip;
import com.company.ui.SimpleText;

import flash.display.Bitmap;

import flash.events.Event;

import flash.events.MouseEvent;
import flash.filters.GlowFilter;

import io.decagames.rotmg.ui.buttons.SliceScalingButton;
import io.decagames.rotmg.ui.defaults.DefaultLabelFormat;

import io.decagames.rotmg.ui.popups.header.PopupHeader;

import io.decagames.rotmg.ui.popups.modal.ModalPopup;
import io.decagames.rotmg.ui.sliceScaling.SliceScalingBitmap;
import io.decagames.rotmg.ui.texture.TextureParser;

import kabam.rotmg.SkillTree.images.bigSkills.BigAttSkill;

import kabam.rotmg.SkillTree.images.bigSkills.BigDefSkill;

import kabam.rotmg.SkillTree.images.bigSkills.BigDexSkill;

import kabam.rotmg.SkillTree.images.bigSkills.BigHPRegenSkill;

import kabam.rotmg.SkillTree.images.bigSkills.BigLifeSkill;
import kabam.rotmg.SkillTree.images.bigSkills.BigLootSkill;
import kabam.rotmg.SkillTree.images.bigSkills.BigMPRegenSkill;
import kabam.rotmg.SkillTree.images.bigSkills.BigManaSkill;
import kabam.rotmg.SkillTree.images.bigSkills.BigRoFSkill;
import kabam.rotmg.SkillTree.images.bigSkills.BigSpdSkill;
import kabam.rotmg.SkillTree.images.bigSkills.BigVitSkill;
import kabam.rotmg.SkillTree.images.bigSkills.BigWisSkill;

import kabam.rotmg.SkillTree.images.smallSkills.SmallAttSkill;
import kabam.rotmg.SkillTree.images.smallSkills.SmallDefSkill;
import kabam.rotmg.SkillTree.images.smallSkills.SmallDexSkill;
import kabam.rotmg.SkillTree.images.smallSkills.SmallHPRegenSkill;

import kabam.rotmg.SkillTree.images.smallSkills.SmallLifeSkill;
import kabam.rotmg.SkillTree.images.smallSkills.SmallLootSkill;
import kabam.rotmg.SkillTree.images.smallSkills.SmallMPRegenSkill;
import kabam.rotmg.SkillTree.images.smallSkills.SmallManaSkill;
import kabam.rotmg.SkillTree.images.smallSkills.SmallRoFSkill;
import kabam.rotmg.SkillTree.images.smallSkills.SmallSpdSkill;
import kabam.rotmg.SkillTree.images.smallSkills.SmallVitSkill;
import kabam.rotmg.SkillTree.images.smallSkills.SmallWisSkill;
import kabam.rotmg.classes.model.CharacterClass;
import kabam.rotmg.classes.model.ClassesModel;

import kabam.rotmg.core.StaticInjectorContext;
import kabam.rotmg.core.signals.ShowTooltipSignal;
import kabam.rotmg.game.model.GameModel;

import org.osflash.signals.Signal;

public class SkillTreeModal extends ModalPopup{

    internal var gs_:GameSprite;
    internal var quitButton:SliceScalingButton;
    internal var saveButton:SliceScalingButton;
    internal var saveButtonDecor:SliceScalingBitmap;
    internal var player:Player;
    internal var text_:String;

    internal var pointsText:SimpleText;
    internal var smallLifeSkill:Bitmap;
    internal var smallManaSkill:Bitmap;
    internal var smallAttSkill:Bitmap;
    internal var smallDefSkill:Bitmap;
    internal var smallSpdSkill:Bitmap;
    internal var smallDexSkill:Bitmap;
    internal var smallVitSkill:Bitmap;
    internal var smallWisSkill:Bitmap;
    internal var smallHPRegenSkill:Bitmap;
    internal var smallMPRegenSkill:Bitmap;
    internal var smallRoFSkill:Bitmap;
    internal var smallLootSkill:Bitmap;
    internal var bigLifeSkill:Bitmap;
    internal var bigManaSkill:Bitmap;
    internal var bigAttSkill:Bitmap;
    internal var bigDefSkill:Bitmap;
    internal var bigSpdSkill:Bitmap;
    internal var bigDexSkill:Bitmap;
    internal var bigVitSkill:Bitmap;
    internal var bigWisSkill:Bitmap;
    internal var bigHPRegenSkill:Bitmap;
    internal var bigMPRegenSkill:Bitmap;
    internal var bigRoFSkill:Bitmap;
    internal var bigLootSkill:Bitmap;

    internal var smallSkill1:SliceScalingButton;
    internal var smallSkill1ToolTip:ToolTip;
    internal var smallSkill1Level:SimpleText;
    internal var smallSkill2:SliceScalingButton;
    internal var smallSkill2ToolTip:ToolTip;
    internal var smallSkill2Level:SimpleText;
    internal var smallSkill3:SliceScalingButton;
    internal var smallSkill3ToolTip:ToolTip;
    internal var smallSkill3Level:SimpleText;
    internal var smallSkill4:SliceScalingButton;
    internal var smallSkill4ToolTip:ToolTip;
    internal var smallSkill4Level:SimpleText;
    internal var smallSkill5:SliceScalingButton;
    internal var smallSkill5ToolTip:ToolTip;
    internal var smallSkill5Level:SimpleText;
    internal var smallSkill6:SliceScalingButton;
    internal var smallSkill6ToolTip:ToolTip;
    internal var smallSkill6Level:SimpleText;
    internal var smallSkill7:SliceScalingButton;
    internal var smallSkill7ToolTip:ToolTip;
    internal var smallSkill7Level:SimpleText;
    internal var smallSkill8:SliceScalingButton;
    internal var smallSkill8ToolTip:ToolTip;
    internal var smallSkill8Level:SimpleText;
    internal var smallSkill9:SliceScalingButton;
    internal var smallSkill9ToolTip:ToolTip;
    internal var smallSkill9Level:SimpleText;
    internal var smallSkill10:SliceScalingButton;
    internal var smallSkill10ToolTip:ToolTip;
    internal var smallSkill10Level:SimpleText;
    internal var smallSkill11:SliceScalingButton;
    internal var smallSkill11ToolTip:ToolTip;
    internal var smallSkill11Level:SimpleText;
    internal var smallSkill12:SliceScalingButton;
    internal var smallSkill12ToolTip:ToolTip;
    internal var smallSkill12Level:SimpleText;

    internal var bigSkill1:SliceScalingButton;
    internal var bigSkill1ToolTip:ToolTip;
    internal var bigSkill2:SliceScalingButton;
    internal var bigSkill2ToolTip:ToolTip;
    internal var bigSkill3:SliceScalingButton;
    internal var bigSkill3ToolTip:ToolTip;
    internal var bigSkill4:SliceScalingButton;
    internal var bigSkill4ToolTip:ToolTip;
    internal var bigSkill5:SliceScalingButton;
    internal var bigSkill5ToolTip:ToolTip;
    internal var bigSkill6:SliceScalingButton;
    internal var bigSkill6ToolTip:ToolTip;
    internal var bigSkill7:SliceScalingButton;
    internal var bigSkill7ToolTip:ToolTip;
    internal var bigSkill8:SliceScalingButton;
    internal var bigSkill8ToolTip:ToolTip;
    internal var bigSkill9:SliceScalingButton;
    internal var bigSkill9ToolTip:ToolTip;
    internal var bigSkill10:SliceScalingButton;
    internal var bigSkill10ToolTip:ToolTip;
    internal var bigSkill11:SliceScalingButton;
    internal var bigSkill11ToolTip:ToolTip;
    internal var bigSkill12:SliceScalingButton;
    internal var bigSkill12ToolTip:ToolTip;

    internal var close:Signal = new Signal();
    internal var clicked:Signal = new Signal();


    public function SkillTreeModal(gs:GameSprite) {
        this.gs_ = gs;
        super(700, 500, "Skill Tree");
        this.x = 27.5;
        this.y = 22.5;
        this.player = StaticInjectorContext.getInjector().getInstance(GameModel).player;
        this.quitButton = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI","close_button"));
        this.header.addButton(this.quitButton, PopupHeader.RIGHT_BUTTON);
        this.quitButton.addEventListener(MouseEvent.CLICK,this.onClose);

        this.pointsText = new SimpleText(30, 0xFFFFFF, false, 300);
        this.pointsText.setText("Points: " + this.player.points + "/40");
        this.pointsText.x = 50;
        this.pointsText.y = 20;
        this.pointsText.addEventListener(Event.ENTER_FRAME, this.onEnterFrame);
        addChild(this.pointsText);

        this.saveButtonDecor = SliceScalingBitmap(TextureParser.instance.getSliceScalingBitmap("UI", "main_button_decoration_dark", 155));
        this.saveButtonDecor.x = 130.5;
        this.saveButtonDecor.y = 446;
        this.saveButtonDecor.scaleX = 0.9;
        this.saveButtonDecor.scaleY = 0.9;
        addChild(this.saveButtonDecor);
        this.saveButton = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "generic_green_button"));
        this.saveButton.width = 101.5;
        this.saveButton.setLabel("Save", DefaultLabelFormat.defaultModalTitle);
        this.saveButton.x = 150;
        this.saveButton.y = 450;
        addChild(this.saveButton);


        this.smallSkill1 = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "skilltree_box", 100));
        this.smallSkill1.scaleX = 0.75;
        this.smallSkill1.scaleY = 0.75;
        this.smallSkill1.x = 8;
        this.smallSkill1.y = 79;
        this.smallSkill1.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver1);
        this.smallSkill1.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut1);
        this.smallLifeSkill = new SmallLifeSkill();
        this.smallLifeSkill.x = this.smallSkill1.x + 13;
        this.smallLifeSkill.y = this.smallSkill1.y + 13;
        this.smallSkill1Level = new SimpleText(15, 0xFFFFFF, false);
        this.smallSkill1Level.setText(this.player.smallSkill1 + "").setBold(true);
        this.smallSkill1Level.x = this.smallSkill1.x + 3;
        this.smallSkill1Level.y = this.smallSkill1.y + 3;
        this.smallSkill1Level.addEventListener(Event.ENTER_FRAME, this.onEnterFrame1);
        addChild(this.smallSkill1);
        addChild(this.smallLifeSkill);
        addChild(this.smallSkill1Level);
        this.smallSkill2 = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "skilltree_box", 100));
        this.smallSkill2.scaleX = 0.75;
        this.smallSkill2.scaleY = 0.75;
        this.smallSkill2.x = 86;
        this.smallSkill2.y = 79;
        this.smallSkill2.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver2);
        this.smallSkill2.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut2);
        this.smallManaSkill = new SmallManaSkill();
        this.smallManaSkill.x = this.smallSkill2.x + 13;
        this.smallManaSkill.y = this.smallSkill2.y + 13;
        this.smallSkill2Level = new SimpleText(15, 0xFFFFFF).setBold(true).setText(this.player.smallSkill2.toString());
        this.smallSkill2Level.x = this.smallSkill2.x + 3;
        this.smallSkill2Level.y = this.smallSkill2.y + 3;
        this.smallSkill2Level.addEventListener(Event.ENTER_FRAME, this.onEnterFrame2);
        addChild(this.smallSkill2);
        addChild(this.smallManaSkill);
        addChild(this.smallSkill2Level);
        this.smallSkill3 = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "skilltree_box", 100));
        this.smallSkill3.scaleX = 0.75;
        this.smallSkill3.scaleY = 0.75;
        this.smallSkill3.x = 164;
        this.smallSkill3.y = 79;
        this.smallSkill3.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver3);
        this.smallSkill3.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut3);
        this.smallAttSkill = new SmallAttSkill();
        this.smallAttSkill.x = this.smallSkill3.x + 13;
        this.smallAttSkill.y = this.smallSkill3.y + 13;
        this.smallSkill3Level = new SimpleText(15, 0xFFFFFF).setBold(true).setText(this.player.smallSkill3.toString());
        this.smallSkill3Level.x = this.smallSkill3.x + 3;
        this.smallSkill3Level.y = this.smallSkill3.y + 3;
        this.smallSkill3Level.addEventListener(Event.ENTER_FRAME, this.onEnterFrame3);
        addChild(this.smallSkill3);
        addChild(this.smallAttSkill);
        addChild(this.smallSkill3Level);
        this.smallSkill4 = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "skilltree_box", 100));
        this.smallSkill4.scaleX = 0.75;
        this.smallSkill4.scaleY = 0.75;
        this.smallSkill4.x = 8;
        this.smallSkill4.y = 159;
        this.smallSkill4.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver4);
        this.smallSkill4.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut4);
        this.smallDefSkill = new SmallDefSkill();
        this.smallDefSkill.x = this.smallSkill4.x + 13;
        this.smallDefSkill.y = this.smallSkill4.y + 13;
        this.smallSkill4Level = new SimpleText(15, 0xFFFFFF).setBold(true).setText(this.player.smallSkill4.toString());
        this.smallSkill4Level.x = this.smallSkill4.x + 3;
        this.smallSkill4Level.y = this.smallSkill4.y + 3;
        this.smallSkill4Level.addEventListener(Event.ENTER_FRAME, this.onEnterFrame4);
        addChild(this.smallSkill4);
        addChild(this.smallDefSkill);
        addChild(this.smallSkill4Level);
        this.smallSkill5 = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "skilltree_box", 100));
        this.smallSkill5.scaleX = 0.75;
        this.smallSkill5.scaleY = 0.75;
        this.smallSkill5.x = 86;
        this.smallSkill5.y = 159;
        this.smallSkill5.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver5);
        this.smallSkill5.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut5);
        this.smallSpdSkill = new SmallSpdSkill();
        this.smallSpdSkill.x = this.smallSkill5.x + 13;
        this.smallSpdSkill.y = this.smallSkill5.y + 13;
        this.smallSkill5Level = new SimpleText(15, 0xFFFFFF).setBold(true).setText(this.player.smallSkill5.toString());
        this.smallSkill5Level.x = this.smallSkill5.x + 3;
        this.smallSkill5Level.y = this.smallSkill5.y + 3;
        this.smallSkill5Level.addEventListener(Event.ENTER_FRAME, this.onEnterFrame5);
        addChild(this.smallSkill5);
        addChild(this.smallSpdSkill);
        addChild(this.smallSkill5Level);
        this.smallSkill6 = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "skilltree_box", 100));
        this.smallSkill6.scaleX = 0.75;
        this.smallSkill6.scaleY = 0.75;
        this.smallSkill6.x = 164;
        this.smallSkill6.y = 159;
        this.smallSkill6.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver6);
        this.smallSkill6.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut6);
        this.smallDexSkill = new SmallDexSkill();
        this.smallDexSkill.x = this.smallSkill6.x + 13;
        this.smallDexSkill.y = this.smallSkill6.y + 13;
        this.smallSkill6Level = new SimpleText(15, 0xFFFFFF).setBold(true).setText(this.player.smallSkill6.toString());
        this.smallSkill6Level.x = this.smallSkill6.x + 3;
        this.smallSkill6Level.y = this.smallSkill6.y + 3;
        this.smallSkill6Level.addEventListener(Event.ENTER_FRAME, this.onEnterFrame6);
        addChild(this.smallSkill6);
        addChild(this.smallDexSkill);
        addChild(this.smallSkill6Level);
        this.smallSkill7 = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "skilltree_box", 100));
        this.smallSkill7.scaleX = 0.75;
        this.smallSkill7.scaleY = 0.75;
        this.smallSkill7.x = 8;
        this.smallSkill7.y = 239;
        this.smallSkill7.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver7);
        this.smallSkill7.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut7);
        this.smallVitSkill = new SmallVitSkill();
        this.smallVitSkill.x = this.smallSkill7.x + 13;
        this.smallVitSkill.y = this.smallSkill7.y + 13;
        this.smallSkill7Level = new SimpleText(15, 0xFFFFFF).setBold(true).setText(this.player.smallSkill7.toString());
        this.smallSkill7Level.x = this.smallSkill7.x + 3;
        this.smallSkill7Level.y = this.smallSkill7.y + 3;
        this.smallSkill7Level.addEventListener(Event.ENTER_FRAME, this.onEnterFrame7);
        addChild(this.smallSkill7);
        addChild(this.smallVitSkill);
        addChild(this.smallSkill7Level);
        this.smallSkill8 = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "skilltree_box", 100));
        this.smallSkill8.scaleX = 0.75;
        this.smallSkill8.scaleY = 0.75;
        this.smallSkill8.x = 86;
        this.smallSkill8.y = 239;
        this.smallSkill8.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver8);
        this.smallSkill8.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut8);
        this.smallWisSkill = new SmallWisSkill();
        this.smallWisSkill.x = this.smallSkill8.x + 13;
        this.smallWisSkill.y = this.smallSkill8.y + 13;
        this.smallSkill8Level = new SimpleText(15, 0xFFFFFF).setBold(true).setText(this.player.smallSkill8.toString());
        this.smallSkill8Level.x = this.smallSkill8.x + 3;
        this.smallSkill8Level.y = this.smallSkill8.y + 3;
        this.smallSkill8Level.addEventListener(Event.ENTER_FRAME, this.onEnterFrame8);
        addChild(this.smallSkill8);
        addChild(this.smallWisSkill);
        addChild(this.smallSkill8Level);
        this.smallSkill9 = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "skilltree_box", 100));
        this.smallSkill9.scaleX = 0.75;
        this.smallSkill9.scaleY = 0.75;
        this.smallSkill9.x = 164;
        this.smallSkill9.y = 239;
        this.smallSkill9.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver9);
        this.smallSkill9.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut9);
        this.smallHPRegenSkill = new SmallHPRegenSkill();
        this.smallHPRegenSkill.x = this.smallSkill9.x + 13;
        this.smallHPRegenSkill.y = this.smallSkill9.y + 13;
        this.smallSkill9Level = new SimpleText(15, 0xFFFFFF).setBold(true).setText(this.player.smallSkill9.toString());
        this.smallSkill9Level.x = this.smallSkill9.x + 3;
        this.smallSkill9Level.y = this.smallSkill9.y + 3;
        this.smallSkill9Level.addEventListener(Event.ENTER_FRAME, this.onEnterFrame9);
        addChild(this.smallSkill9);
        addChild(this.smallHPRegenSkill);
        addChild(this.smallSkill9Level);
        this.smallSkill10 = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "skilltree_box", 100));
        this.smallSkill10.scaleX = 0.75;
        this.smallSkill10.scaleY = 0.75;
        this.smallSkill10.x = 8;
        this.smallSkill10.y = 319;
        this.smallSkill10.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver10);
        this.smallSkill10.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut10);
        this.smallMPRegenSkill = new SmallMPRegenSkill();
        this.smallMPRegenSkill.x = this.smallSkill10.x + 13;
        this.smallMPRegenSkill.y = this.smallSkill10.y + 13;
        this.smallSkill10Level = new SimpleText(15, 0xFFFFFF).setBold(true).setText(this.player.smallSkill10.toString());
        this.smallSkill10Level.x = this.smallSkill10.x + 3;
        this.smallSkill10Level.y = this.smallSkill10.y + 3;
        this.smallSkill10Level.addEventListener(Event.ENTER_FRAME, this.onEnterFrame10);
        addChild(this.smallSkill10);
        addChild(this.smallMPRegenSkill);
        addChild(this.smallSkill10Level);
        this.smallSkill11 = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "skilltree_box", 100));
        this.smallSkill11.scaleX = 0.75;
        this.smallSkill11.scaleY = 0.75;
        this.smallSkill11.x = 86;
        this.smallSkill11.y = 319;
        this.smallSkill11.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver11);
        this.smallSkill11.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut11);
        this.smallRoFSkill = new SmallRoFSkill();
        this.smallRoFSkill.x = this.smallSkill11.x + 13;
        this.smallRoFSkill.y = this.smallSkill11.y + 13;
        this.smallSkill11Level = new SimpleText(15, 0xFFFFFF).setBold(true).setText(this.player.smallSkill11.toString());
        this.smallSkill11Level.x = this.smallSkill11.x + 3;
        this.smallSkill11Level.y = this.smallSkill11.y + 3;
        this.smallSkill11Level.addEventListener(Event.ENTER_FRAME, this.onEnterFrame11);
        addChild(this.smallSkill11);
        addChild(this.smallRoFSkill);
        addChild(this.smallSkill11Level);
        this.smallSkill12 = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "skilltree_box", 100));
        this.smallSkill12.scaleX = 0.75;
        this.smallSkill12.scaleY = 0.75;
        this.smallSkill12.x = 164;
        this.smallSkill12.y = 319;
        this.smallSkill12.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver12);
        this.smallSkill12.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut12);
        this.smallLootSkill = new SmallLootSkill();
        this.smallLootSkill.x = this.smallSkill12.x + 13;
        this.smallLootSkill.y = this.smallSkill12.y + 13;
        this.smallSkill12Level = new SimpleText(15, 0xFFFFFF).setBold(true).setText(this.player.smallSkill12.toString());
        this.smallSkill12Level.x = this.smallSkill12.x + 3;
        this.smallSkill12Level.y = this.smallSkill12.y + 3;
        this.smallSkill12Level.addEventListener(Event.ENTER_FRAME, this.onEnterFrame12);
        addChild(this.smallSkill12);
        addChild(this.smallLootSkill);
        addChild(this.smallSkill12Level);

        this.bigSkill1 = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "skilltree_box", 100));
        this.bigSkill1.x = 587;
        this.bigSkill1.y = 25;
        this.bigSkill1.disabled = true;
        this.bigSkill1.alpha = 0.5;
        this.bigSkill1.addEventListener(Event.ENTER_FRAME, this.onEnterFrameBig1);
        this.bigSkill1.addEventListener(MouseEvent.ROLL_OVER, this.onRollOverBig1);
        this.bigSkill1.addEventListener(MouseEvent.ROLL_OUT, this.onRollOutBig1);
        this.bigLifeSkill = new BigLifeSkill();
        this.bigLifeSkill.x = this.bigSkill1.x + 5;
        this.bigLifeSkill.y = this.bigSkill1.y + 7;
        this.bigLifeSkill.alpha = 0.5;
        addChild(this.bigSkill1);
        addChild(this.bigLifeSkill);
        this.bigSkill2 = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "skilltree_box", 100));
        this.bigSkill2.x = 484;
        this.bigSkill2.y = 25;
        this.bigSkill2.disabled = true;
        this.bigSkill2.alpha = 0.5;
        this.bigSkill2.addEventListener(Event.ENTER_FRAME, this.onEnterFrameBig2);
        this.bigSkill2.addEventListener(MouseEvent.ROLL_OVER, this.onRollOverBig2);
        this.bigSkill2.addEventListener(MouseEvent.ROLL_OUT, this.onRollOutBig2);
        this.bigManaSkill = new BigManaSkill();
        this.bigManaSkill.x = this.bigSkill2.x + 5;
        this.bigManaSkill.y = this.bigSkill2.y + 7;
        this.bigManaSkill.alpha = 0.5;
        addChild(this.bigSkill2);
        addChild(this.bigManaSkill);
        this.bigSkill3 = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "skilltree_box", 100));
        this.bigSkill3.x = 381;
        this.bigSkill3.y = 25;
        this.bigSkill3.disabled = true;
        this.bigSkill3.alpha = 0.5;
        this.bigSkill3.addEventListener(Event.ENTER_FRAME, this.onEnterFrameBig3);
        this.bigSkill3.addEventListener(MouseEvent.ROLL_OVER, this.onRollOverBig3);
        this.bigSkill3.addEventListener(MouseEvent.ROLL_OUT, this.onRollOutBig3);
        this.bigAttSkill = new BigAttSkill();
        this.bigAttSkill.x = this.bigSkill3.x + 5;
        this.bigAttSkill.y = this.bigSkill3.y + 7;
        this.bigAttSkill.alpha = 0.5;
        addChild(this.bigSkill3);
        addChild(this.bigAttSkill);
        this.bigSkill4 = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "skilltree_box", 100));
        this.bigSkill4.x = 587;
        this.bigSkill4.y = 131;
        this.bigSkill4.disabled = true;
        this.bigSkill4.alpha = 0.5;
        this.bigSkill4.addEventListener(Event.ENTER_FRAME, this.onEnterFrameBig4);
        this.bigSkill4.addEventListener(MouseEvent.ROLL_OVER, this.onRollOverBig4);
        this.bigSkill4.addEventListener(MouseEvent.ROLL_OUT, this.onRollOutBig4);
        this.bigDefSkill = new BigDefSkill();
        this.bigDefSkill.x = this.bigSkill4.x + 5;
        this.bigDefSkill.y = this.bigSkill4.y + 7;
        this.bigDefSkill.alpha = 0.5;
        addChild(this.bigSkill4);
        addChild(this.bigDefSkill);
        this.bigSkill5 = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "skilltree_box", 100));
        this.bigSkill5.x = 484;
        this.bigSkill5.y = 131;
        this.bigSkill5.disabled = true;
        this.bigSkill5.alpha = 0.5;
        this.bigSkill5.addEventListener(Event.ENTER_FRAME, this.onEnterFrameBig5);
        this.bigSkill5.addEventListener(MouseEvent.ROLL_OVER, this.onRollOverBig5);
        this.bigSkill5.addEventListener(MouseEvent.ROLL_OUT, this.onRollOutBig5);
        this.bigSpdSkill = new BigSpdSkill();
        this.bigSpdSkill.x = this.bigSkill5.x + 5;
        this.bigSpdSkill.y = this.bigSkill5.y + 7;
        this.bigSpdSkill.alpha = 0.5;
        addChild(this.bigSkill5);
        addChild(this.bigSpdSkill);
        this.bigSkill6 = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "skilltree_box", 100));
        this.bigSkill6.x = 381;
        this.bigSkill6.y = 131;
        this.bigSkill6.disabled = true;
        this.bigSkill6.alpha = 0.5;
        this.bigSkill6.addEventListener(Event.ENTER_FRAME, this.onEnterFrameBig6);
        this.bigSkill6.addEventListener(MouseEvent.ROLL_OVER, this.onRollOverBig6);
        this.bigSkill6.addEventListener(MouseEvent.ROLL_OUT, this.onRollOutBig6);
        this.bigDexSkill = new BigDexSkill();
        this.bigDexSkill.x = this.bigSkill6.x + 5;
        this.bigDexSkill.y = this.bigSkill6.y + 7;
        this.bigDexSkill.alpha = 0.5;
        addChild(this.bigSkill6);
        addChild(this.bigDexSkill);
        this.bigSkill7 = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "skilltree_box", 100));
        this.bigSkill7.x = 587;
        this.bigSkill7.y = 237;
        this.bigSkill7.disabled = true;
        this.bigSkill7.alpha = 0.5;
        this.bigSkill7.addEventListener(Event.ENTER_FRAME, this.onEnterFrameBig7);
        this.bigSkill7.addEventListener(MouseEvent.ROLL_OVER, this.onRollOverBig7);
        this.bigSkill7.addEventListener(MouseEvent.ROLL_OUT, this.onRollOutBig7);
        this.bigVitSkill = new BigVitSkill();
        this.bigVitSkill.x = this.bigSkill7.x + 5;
        this.bigVitSkill.y = this.bigSkill7.y + 7;
        this.bigVitSkill.alpha = 0.5;
        addChild(this.bigSkill7);
        addChild(this.bigVitSkill);
        this.bigSkill8 = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "skilltree_box", 100));
        this.bigSkill8.x = 484;
        this.bigSkill8.y = 237;
        this.bigSkill8.disabled = true;
        this.bigSkill8.alpha = 0.5;
        this.bigSkill8.addEventListener(Event.ENTER_FRAME, this.onEnterFrameBig8);
        this.bigSkill8.addEventListener(MouseEvent.ROLL_OVER, this.onRollOverBig8);
        this.bigSkill8.addEventListener(MouseEvent.ROLL_OUT, this.onRollOutBig8);
        this.bigWisSkill = new BigWisSkill();
        this.bigWisSkill.x = this.bigSkill8.x + 5;
        this.bigWisSkill.y = this.bigSkill8.y + 7;
        this.bigWisSkill.alpha = 0.5;
        addChild(this.bigSkill8);
        addChild(this.bigWisSkill);
        this.bigSkill9 = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "skilltree_box", 100));
        this.bigSkill9.x = 381;
        this.bigSkill9.y = 237;
        this.bigSkill9.disabled = true;
        this.bigSkill9.alpha = 0.5;
        this.bigSkill9.addEventListener(Event.ENTER_FRAME, this.onEnterFrameBig9);
        this.bigSkill9.addEventListener(MouseEvent.ROLL_OVER, this.onRollOverBig9);
        this.bigSkill9.addEventListener(MouseEvent.ROLL_OUT, this.onRollOutBig9);
        this.bigHPRegenSkill = new BigHPRegenSkill();
        this.bigHPRegenSkill.x = this.bigSkill9.x + 5;
        this.bigHPRegenSkill.y = this.bigSkill9.y + 7;
        this.bigHPRegenSkill.alpha = 0.5;
        addChild(this.bigSkill9);
        addChild(this.bigHPRegenSkill);
        this.bigSkill10 = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "skilltree_box", 100));
        this.bigSkill10.x = 587;
        this.bigSkill10.y = 343;
        this.bigSkill10.disabled = true;
        this.bigSkill10.alpha = 0.5;
        this.bigMPRegenSkill = new BigMPRegenSkill();
        this.bigMPRegenSkill.x = this.bigSkill10.x + 5;
        this.bigMPRegenSkill.y = this.bigSkill10.y + 7;
        this.bigMPRegenSkill.alpha = 0.5;
        this.bigSkill10.addEventListener(Event.ENTER_FRAME, this.onEnterFrameBig10);
        this.bigSkill10.addEventListener(MouseEvent.ROLL_OVER, this.onRollOverBig10);
        this.bigSkill10.addEventListener(MouseEvent.ROLL_OUT, this.onRollOutBig10);
        addChild(this.bigSkill10);
        addChild(this.bigMPRegenSkill);
        this.bigSkill11 = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "skilltree_box", 100));
        this.bigSkill11.x = 484;
        this.bigSkill11.y = 343;
        this.bigSkill11.disabled = true;
        this.bigSkill11.alpha = 0.5;
        this.bigSkill11.addEventListener(Event.ENTER_FRAME, this.onEnterFrameBig11);
        this.bigSkill11.addEventListener(MouseEvent.ROLL_OVER, this.onRollOverBig11);
        this.bigSkill11.addEventListener(MouseEvent.ROLL_OUT, this.onRollOutBig11);
        this.bigRoFSkill = new BigRoFSkill();
        this.bigRoFSkill.x = this.bigSkill11.x + 5;
        this.bigRoFSkill.y = this.bigSkill11.y + 7;
        this.bigRoFSkill.alpha = 0.5;
        addChild(this.bigSkill11);
        addChild(this.bigRoFSkill);
        this.bigSkill12 = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "skilltree_box", 100));
        this.bigSkill12.x = 381;
        this.bigSkill12.y = 343;
        this.bigSkill12.disabled = true;
        this.bigSkill12.alpha = 0.5;
        this.bigSkill12.addEventListener(Event.ENTER_FRAME, this.onEnterFrameBig12);
        this.bigSkill12.addEventListener(MouseEvent.ROLL_OVER, this.onRollOverBig12);
        this.bigSkill12.addEventListener(MouseEvent.ROLL_OUT, this.onRollOutBig12);
        this.bigLootSkill = new BigLootSkill();
        this.bigLootSkill.x = this.bigSkill12.x + 5;
        this.bigLootSkill.y = this.bigSkill12.y + 7;
        this.bigLootSkill.alpha = 0.5;
        addChild(this.bigSkill12);
        addChild(this.bigLootSkill);
    }

    private function onRollOverBig1(arg1:MouseEvent):void
    {
        if(bigSkill1ToolTip != null)
        {
            if(bigSkill1ToolTip.parent != null)
            {
                bigSkill1ToolTip.parent.removeChild(bigSkill1ToolTip);
            }
        }
        bigSkill1ToolTip = new BigSkillTreeToolTip(
                "Big Skill of Life",
                "Unlocked With: \n+5 Small Skill of Life\n+3 Small Skill of Health Regeneration\n+2 Small Skill of Vitality",
                20, "Positive Beneficts: \n+40% Life",
                80, "Negative Beneficts: \n-30% Wisdom\n-15% Vitality\n-10% Rate of Fire",
                110);
        StaticInjectorContext.getInjector().getInstance(ShowTooltipSignal).dispatch(bigSkill1ToolTip);
    }

    private function onRollOutBig1(param1:MouseEvent) : void
    {
        if(bigSkill1ToolTip && bigSkill1ToolTip.parent)
        {
            bigSkill1ToolTip.parent.removeChild(bigSkill1ToolTip);
        }
    }

    private function onRollOverBig2(arg1:MouseEvent):void
    {
        if(bigSkill2ToolTip != null)
        {
            if(bigSkill2ToolTip.parent != null)
            {
                bigSkill2ToolTip.parent.removeChild(bigSkill2ToolTip);
            }
        }
        bigSkill2ToolTip = new BigSkillTreeToolTip(
                "Big Skill of Mana",
                "Unlocked With: \n+5 Small Skill of Mana\n+3 Small Skill of Mana Regeneration\n+2 Small Skill of Wisdom",
                20,
                "Positive Beneficts: \n+50% Mana\n+25% Mana Regeneration",
                80,
                "Negative Beneficts: \n-15% Wisdom\nTalisman of Mana don't work.",
                125);
        StaticInjectorContext.getInjector().getInstance(ShowTooltipSignal).dispatch(bigSkill2ToolTip);
    }

    private function onRollOutBig2(param1:MouseEvent) : void
    {
        if(bigSkill2ToolTip && bigSkill2ToolTip.parent)
        {
            bigSkill2ToolTip.parent.removeChild(bigSkill2ToolTip);
        }
    }

    private function onRollOverBig3(arg1:MouseEvent):void
    {
        if(bigSkill3ToolTip != null)
        {
            if(bigSkill3ToolTip.parent != null)
            {
                bigSkill3ToolTip.parent.removeChild(bigSkill3ToolTip);
            }
        }
        bigSkill3ToolTip = new BigSkillTreeToolTip("Big Skill of Attack",
                "Unlocked With: \n+5 Small Skill of Attack\n+3 Small Skill of Dexterity",
                20,
                "Positive Beneficts: \n+15% Attack\n+10 Attack",
                65,
                "Negative Beneficts: \n-10% Speed\n-10% Vitality\n-10% Life",
                110);
        StaticInjectorContext.getInjector().getInstance(ShowTooltipSignal).dispatch(bigSkill3ToolTip);
    }

    private function onRollOutBig3(param1:MouseEvent) : void
    {
        if(bigSkill3ToolTip && bigSkill3ToolTip.parent)
        {
            bigSkill3ToolTip.parent.removeChild(bigSkill3ToolTip);
        }
    }

    private function onRollOverBig4(arg1:MouseEvent):void
    {
        if(bigSkill4ToolTip != null)
        {
            if(bigSkill4ToolTip.parent != null)
            {
                bigSkill4ToolTip.parent.removeChild(bigSkill4ToolTip);
            }
        }
        bigSkill4ToolTip = new BigSkillTreeToolTip("Big Skill of Defense",
                "Unlocked With: \n+5 Small Skill Defense\n+3 Small Skill Life",
                20,
                "Positive Beneficts: \n+40% Defense",
                65,
                "Negative Beneficts: \n-20% Speed\n-5% Rate of Fire",
                100);
        StaticInjectorContext.getInjector().getInstance(ShowTooltipSignal).dispatch(bigSkill4ToolTip);
    }

    private function onRollOutBig4(param1:MouseEvent) : void
    {
        if(bigSkill4ToolTip && bigSkill4ToolTip.parent)
        {
            bigSkill4ToolTip.parent.removeChild(bigSkill4ToolTip);
        }
    }

    private function onRollOverBig5(arg1:MouseEvent):void
    {
        if(bigSkill5ToolTip != null)
        {
            if(bigSkill5ToolTip.parent != null)
            {
                bigSkill5ToolTip.parent.removeChild(bigSkill5ToolTip);
            }
        }
        bigSkill5ToolTip = new BigSkillTreeToolTip("Big Skill of Speed",
                "Unlocked With: \n+5 Small Skill of Speed\n+3 Small Skill of Defense\n+2 Small Skill of Vitality",
                20,
                "Positive Beneficts: \n+25% Speed\n+10 Vitality",
                80,
                "Negative Beneficts: \n-5% Defense\n-10% Mana",
                125);
        StaticInjectorContext.getInjector().getInstance(ShowTooltipSignal).dispatch(bigSkill5ToolTip);
    }

    private function onRollOutBig5(param1:MouseEvent) : void
    {
        if(bigSkill5ToolTip && bigSkill5ToolTip.parent)
        {
            bigSkill5ToolTip.parent.removeChild(bigSkill5ToolTip);
        }
    }

    private function onRollOverBig6(arg1:MouseEvent):void
    {
        if(bigSkill6ToolTip != null)
        {
            if(bigSkill6ToolTip.parent != null)
            {
                bigSkill6ToolTip.parent.removeChild(bigSkill6ToolTip);
            }
        }
        bigSkill6ToolTip = new BigSkillTreeToolTip("Big Skill of Dexterity",
                "Unlocked With: \n+5 Small Skill of Dexterity\n+2 Small Skill of Attack\n+1 Rate of Fire",
                20,
                "Positive Beneficts: \n+15% Dexterity\n+10 Dexterity",
                80,
                "Negative Beneficts: \n-15% Defense\n-10% Wisdom\n-10% Mana",
                125);
        StaticInjectorContext.getInjector().getInstance(ShowTooltipSignal).dispatch(bigSkill6ToolTip);
    }

    private function onRollOutBig6(param1:MouseEvent) : void
    {
        if(bigSkill6ToolTip && bigSkill6ToolTip.parent)
        {
            bigSkill6ToolTip.parent.removeChild(bigSkill6ToolTip);
        }
    }

    private function onRollOverBig7(arg1:MouseEvent):void
    {
        if(bigSkill7ToolTip != null)
        {
            if(bigSkill7ToolTip.parent != null)
            {
                bigSkill7ToolTip.parent.removeChild(bigSkill7ToolTip);
            }
        }
        bigSkill7ToolTip = new BigSkillTreeToolTip("Big Skill of Vitality", "Unlocked With: \n+5 Small Skill of Vitality\n+3 Small Skill of Health Regeneration", 20, "Positive Beneficts: \n+20% Vitality\n+10% Health Regeneration", 65, "Negative Beneficts: \n-5% Life", 110);
        StaticInjectorContext.getInjector().getInstance(ShowTooltipSignal).dispatch(bigSkill7ToolTip);
    }

    private function onRollOutBig7(param1:MouseEvent) : void
    {
        if(bigSkill7ToolTip && bigSkill7ToolTip.parent)
        {
            bigSkill7ToolTip.parent.removeChild(bigSkill7ToolTip);
        }
    }

    private function onRollOverBig8(arg1:MouseEvent):void
    {
        if(bigSkill8ToolTip != null)
        {
            if(bigSkill8ToolTip.parent != null)
            {
                bigSkill8ToolTip.parent.removeChild(bigSkill8ToolTip);
            }
        }
        bigSkill8ToolTip = new BigSkillTreeToolTip("Big Skill of Wisdom", "Unlocked With: \n+5 Small Skill of Wisdom\n+3 Small Skill of Mana Regeneration", 20, "Positive Beneficts: \n+20% Wisdom\n+10% Mana Regeneration", 65, "Negative Beneficts: \n-10% Mana\nWisdom Scaling reduced by 10%.", 110);
        StaticInjectorContext.getInjector().getInstance(ShowTooltipSignal).dispatch(bigSkill8ToolTip);
    }

    private function onRollOutBig8(param1:MouseEvent) : void
    {
        if(bigSkill8ToolTip && bigSkill8ToolTip.parent)
        {
            bigSkill8ToolTip.parent.removeChild(bigSkill8ToolTip);
        }
    }

    private function onRollOverBig9(arg1:MouseEvent):void
    {
        if(bigSkill9ToolTip != null)
        {
            if(bigSkill9ToolTip.parent != null)
            {
                bigSkill9ToolTip.parent.removeChild(bigSkill9ToolTip);
            }
        }
        bigSkill9ToolTip = new BigSkillTreeToolTip(
                "Big Skill of Health Regeneration",
                "Unlocked With: \n+5 Small Skill of Health Regeneration\n+3 Small Skill of Life",
                20,
                "Positive Beneficts: \n+35% Health Regeneration\n+5% Vitality",
                65,
                "Negative Beneficts: \n-5% Life\nHealth Potions effectiveness is reduced by 25%.",
                110,
                275);
        StaticInjectorContext.getInjector().getInstance(ShowTooltipSignal).dispatch(bigSkill9ToolTip);
    }

    private function onRollOutBig9(param1:MouseEvent) : void
    {
        if(bigSkill9ToolTip && bigSkill9ToolTip.parent)
        {
            bigSkill9ToolTip.parent.removeChild(bigSkill9ToolTip);
        }
    }

    private function onRollOverBig10(arg1:MouseEvent):void
    {
        if(bigSkill10ToolTip != null)
        {
            if(bigSkill10ToolTip.parent != null)
            {
                bigSkill10ToolTip.parent.removeChild(bigSkill10ToolTip);
            }
        }
        bigSkill10ToolTip = new BigSkillTreeToolTip("Big Skill of Mana Regeneration", "Unlocked With: \n+5 Small Skill of Mana Regeneration\n+3 Small Skill of Mana", 20, "Positive Beneficts: \n+20% Mana Regeneration\n+10% Wisdom", 65, "Negative Beneficts: \n-10% Mana\nMana Potions only recover 50 MP instahead of 100 MP.", 110, 275);
        StaticInjectorContext.getInjector().getInstance(ShowTooltipSignal).dispatch(bigSkill10ToolTip);
    }

    private function onRollOutBig10(param1:MouseEvent) : void
    {
        if(bigSkill10ToolTip && bigSkill10ToolTip.parent)
        {
            bigSkill10ToolTip.parent.removeChild(bigSkill10ToolTip);
        }
    }

    private function onRollOverBig11(arg1:MouseEvent):void
    {
        if(bigSkill11ToolTip != null)
        {
            if(bigSkill11ToolTip.parent != null)
            {
                bigSkill11ToolTip.parent.removeChild(bigSkill11ToolTip);
            }
        }
        bigSkill11ToolTip = new BigSkillTreeToolTip(
                "Big Skill of Rate of Fire",
                "Unlocked With: \n+5 Small Skill of Rate of Fire\n+3 Small Skill of Dexterity",
                20,
                "Positive Beneficts: \n+30% Rate of Fire (Multiplicative, not Additive)",
                65,
                "Negative Beneficts: \nImmunity to: Talisman of Looting, Talisman of Luck",
                110);
        StaticInjectorContext.getInjector().getInstance(ShowTooltipSignal).dispatch(bigSkill11ToolTip);
    }

    private function onRollOutBig11(param1:MouseEvent) : void
    {
        if(bigSkill11ToolTip && bigSkill11ToolTip.parent)
        {
            bigSkill11ToolTip.parent.removeChild(bigSkill11ToolTip);
        }
    }

    private function onRollOverBig12(arg1:MouseEvent):void
    {
        if(bigSkill12ToolTip != null)
        {
            if(bigSkill12ToolTip.parent != null)
            {
                bigSkill12ToolTip.parent.removeChild(bigSkill12ToolTip);
            }
        }
        bigSkill12ToolTip = new BigSkillTreeToolTip("Big Skill of Looting", "Unlocked With: \n+5 Small Skill of Looting\n+3 Small Skill of Rate of Fire", 20, "Positive Beneficts: \n+20% Loot Boost", 65, "Negative Beneficts: \nYou can’t do Damage Boost (Your loot isn't Boosted with damage)", 100);
        StaticInjectorContext.getInjector().getInstance(ShowTooltipSignal).dispatch(bigSkill12ToolTip);
    }

    private function onRollOutBig12(param1:MouseEvent) : void
    {
        if(bigSkill12ToolTip && bigSkill12ToolTip.parent)
        {
            bigSkill12ToolTip.parent.removeChild(bigSkill12ToolTip);
        }
    }

    private function onEnterFrame1(arg1:Event):void
    {
        if(this.smallSkill1Level != null){
            if(this.smallSkill1Level.parent != null){
                this.smallSkill1Level.parent.removeChild(this.smallSkill1Level);
            }
        }
        if(this.player.smallSkill1 >= 5 || this.player.points <= 0){
            this.smallSkill1.disabled = true;
            this.smallSkill1.alpha = 0.5;
            this.smallLifeSkill.alpha = 0.5;
        }
        this.smallSkill1Level.setText(this.player.smallSkill1 + "").setColor(0xFFFFFF).setSize(15).setBold(true);
        addChild(this.smallSkill1Level);
    }

    private function onEnterFrame2(arg1:Event):void
    {
        if(this.smallSkill2Level != null){
            if(this.smallSkill2Level.parent != null){
                this.smallSkill2Level.parent.removeChild(this.smallSkill2Level);
            }
        }
        if(this.player.smallSkill2 >= 5 || this.player.points <= 0){
            this.smallSkill2.disabled = true;
            this.smallSkill2.alpha = 0.5;
            this.smallManaSkill.alpha = 0.5;
        }
        this.smallSkill2Level.setText(this.player.smallSkill2 + "").setColor(0xFFFFFF).setSize(15).setBold(true);
        addChild(this.smallSkill2Level);
    }

    private function onEnterFrame3(arg1:Event):void
    {
        if(this.smallSkill3Level != null){
            if(this.smallSkill3Level.parent != null){
                this.smallSkill3Level.parent.removeChild(this.smallSkill3Level);
            }
        }
        if(this.player.smallSkill3 >= 5 || this.player.points <= 0){
            this.smallSkill3.disabled = true;
            this.smallSkill3.alpha = 0.5;
            this.smallAttSkill.alpha = 0.5;
        }
        this.smallSkill3Level.setText(this.player.smallSkill3 + "").setColor(0xFFFFFF).setSize(15).setBold(true);
        addChild(this.smallSkill3Level);
    }

    private function onEnterFrame4(arg1:Event):void
    {
        if(this.smallSkill4Level != null){
            if(this.smallSkill4Level.parent != null){
                this.smallSkill4Level.parent.removeChild(this.smallSkill4Level);
            }
        }
        if(this.player.smallSkill4 >= 5 || this.player.points <= 0){
            this.smallSkill4.disabled = true;
            this.smallSkill4.alpha = 0.5;
            this.smallDefSkill.alpha = 0.5;
        }
        this.smallSkill4Level.setText(this.player.smallSkill4 + "").setColor(0xFFFFFF).setSize(15).setBold(true);
        addChild(this.smallSkill4Level);
    }

    private function onEnterFrame5(arg1:Event):void
    {
        if(this.smallSkill5Level != null){
            if(this.smallSkill5Level.parent != null){
                this.smallSkill5Level.parent.removeChild(this.smallSkill5Level);
            }
        }
        if(this.player.smallSkill5 >= 5 || this.player.points <= 0){
            this.smallSkill5.disabled = true;
            this.smallSkill5.alpha = 0.5;
            this.smallSpdSkill.alpha = 0.5;
        }
        this.smallSkill5Level.setText(this.player.smallSkill5 + "").setColor(0xFFFFFF).setSize(15).setBold(true);
        addChild(this.smallSkill5Level);
    }

    private function onEnterFrame6(arg1:Event):void
    {
        if(this.smallSkill6Level != null){
            if(this.smallSkill6Level.parent != null){
                this.smallSkill6Level.parent.removeChild(this.smallSkill6Level);
            }
        }
        if(this.player.smallSkill6 >= 5 || this.player.points <= 0){
            this.smallSkill6.disabled = true;
            this.smallSkill6.alpha = 0.5;
            this.smallDexSkill.alpha = 0.5;
        }
        this.smallSkill6Level.setText(this.player.smallSkill6 + "").setColor(0xFFFFFF).setSize(15).setBold(true);
        addChild(this.smallSkill6Level);
    }

    private function onEnterFrame7(arg1:Event):void
    {
        if(this.smallSkill7Level != null){
            if(this.smallSkill7Level.parent != null){
                this.smallSkill7Level.parent.removeChild(this.smallSkill7Level);
            }
        }
        if(this.player.smallSkill7 >= 5 || this.player.points <= 0){
            this.smallSkill7.disabled = true;
            this.smallSkill7.alpha = 0.5;
            this.smallVitSkill.alpha = 0.5;
        }
        this.smallSkill7Level.setText(this.player.smallSkill7 + "").setColor(0xFFFFFF).setSize(15).setBold(true);
        addChild(this.smallSkill7Level);
    }

    private function onEnterFrame8(arg1:Event):void
    {
        if(this.smallSkill8Level != null){
            if(this.smallSkill8Level.parent != null){
                this.smallSkill8Level.parent.removeChild(this.smallSkill8Level);
            }
        }
        if(this.player.smallSkill8 >= 5 || this.player.points <= 0){
            this.smallSkill8.disabled = true;
            this.smallSkill8.alpha = 0.5;
            this.smallWisSkill.alpha = 0.5;
        }
        this.smallSkill8Level.setText(this.player.smallSkill8 + "").setColor(0xFFFFFF).setSize(15).setBold(true);
        addChild(this.smallSkill8Level);
    }

    private function onEnterFrame9(arg1:Event):void
    {
        if(this.smallSkill9Level != null){
            if(this.smallSkill9Level.parent != null){
                this.smallSkill9Level.parent.removeChild(this.smallSkill9Level);
            }
        }
        if(this.player.smallSkill9 >= 5 || this.player.points <= 0){
            this.smallSkill9.disabled = true;
            this.smallSkill9.alpha = 0.5;
            this.smallHPRegenSkill.alpha = 0.5;
        }
        this.smallSkill9Level.setText(this.player.smallSkill9 + "").setColor(0xFFFFFF).setSize(15).setBold(true);
        addChild(this.smallSkill9Level);
    }

    private function onEnterFrame10(arg1:Event):void
    {
        if(this.smallSkill10Level != null){
            if(this.smallSkill10Level.parent != null){
                this.smallSkill10Level.parent.removeChild(this.smallSkill10Level);
            }
        }
        if(this.player.smallSkill10 >= 5 || this.player.points <= 0){
            this.smallSkill10.disabled = true;
            this.smallSkill10.alpha = 0.5;
            this.smallMPRegenSkill.alpha = 0.5;
        }
        this.smallSkill10Level.setText(this.player.smallSkill10 + "").setColor(0xFFFFFF).setSize(15).setBold(true);
        addChild(this.smallSkill10Level);
    }

    private function onEnterFrame11(arg1:Event):void
    {
        if(this.smallSkill11Level != null){
            if(this.smallSkill11Level.parent != null){
                this.smallSkill11Level.parent.removeChild(this.smallSkill11Level);
            }
        }
        if(this.player.smallSkill11 >= 5 || this.player.points <= 0){
            this.smallSkill11.disabled = true;
            this.smallSkill11.alpha = 0.5;
            this.smallRoFSkill.alpha = 0.5;
        }
        this.smallSkill11Level.setText(this.player.smallSkill11 + "").setColor(0xFFFFFF).setSize(15).setBold(true);
        addChild(this.smallSkill11Level);
    }

    private function onEnterFrame12(arg1:Event):void
    {
        if(this.smallSkill12Level != null){
            if(this.smallSkill12Level.parent != null){
                this.smallSkill12Level.parent.removeChild(this.smallSkill12Level);
            }
        }
        if(this.player.smallSkill12 >= 5 || this.player.points <= 0){
            this.smallSkill12.disabled = true;
            this.smallSkill12.alpha = 0.5;
            this.smallLootSkill.alpha = 0.5;
        }
        this.smallSkill12Level.setText(this.player.smallSkill12 + "").setColor(0xFFFFFF).setSize(15).setBold(true);
        addChild(this.smallSkill12Level);
    }

    private function onEnterFrameBig1(arg1:Event) : void {
        if(this.checkBigSkills() && !this.player.bigSkill1){
            this.bigSkill1.alpha = 0.5;
            this.bigLifeSkill.alpha = 0.5;
            this.bigSkill1.disabled = true;
            return;
        }
        if(this.player.smallSkill1 >= 5 && this.player.smallSkill9 >= 3 && this.player.smallSkill7 >= 2){
            this.bigSkill1.alpha = 1;
            this.bigLifeSkill.alpha = 1;
            this.bigSkill1.disabled = false;
            if(this.player.bigSkill1){
                this.bigSkill1.filters = [new GlowFilter(0x4DFDFD)];
                this.bigSkill1.disabled = true;
            }
        }
    }

    private function onEnterFrameBig2(arg1:Event) : void {
        if(this.checkBigSkills() && !this.player.bigSkill2){
            this.bigSkill2.alpha = 0.5;
            this.bigManaSkill.alpha = 0.5;
            this.bigSkill2.disabled = true;
            return;
        }
        if(this.player.smallSkill2 >= 5 && this.player.smallSkill10 >= 3 && this.player.smallSkill8 >= 2){
            this.bigSkill2.alpha = 1;
            this.bigManaSkill.alpha = 1;
            this.bigSkill2.disabled = false;
            if(this.player.bigSkill2){
                this.bigSkill2.filters = [new GlowFilter(0xFFFF05)];
                this.bigSkill2.disabled = true;
            }
        }
    }

    private function onEnterFrameBig3(arg1:Event) : void {
        if(this.checkBigSkills() && !this.player.bigSkill3){
            this.bigSkill3.alpha = 0.5;
            this.bigAttSkill.alpha = 0.5;
            this.bigSkill3.disabled = true;
            return;
        }
        if(this.player.smallSkill3 >= 5 && this.player.smallSkill6 >= 3){
            this.bigSkill3.alpha = 1;
            this.bigAttSkill.alpha = 1;
            this.bigSkill3.disabled = false;
            if(this.player.bigSkill3){
                this.bigSkill3.filters = [new GlowFilter(0xCA0FC8)];
                this.bigSkill3.disabled = true;
            }
        }
    }

    private function onEnterFrameBig4(arg1:Event) : void {
        if(this.checkBigSkills() && !this.player.bigSkill4){
            this.bigSkill4.alpha = 0.5;
            this.bigDefSkill.alpha = 0.5;
            this.bigSkill4.disabled = true;
            return;
        }
        if(this.player.smallSkill4 >= 5 && this.player.smallSkill1 >= 3){
            this.bigSkill4.alpha = 1;
            this.bigDefSkill.alpha = 1;
            this.bigSkill4.disabled = false;
            if(this.player.bigSkill4){
                this.bigSkill4.filters = [new GlowFilter(0xB9C5C5)];
                this.bigSkill4.disabled = true;
            }
        }
    }

    private function onEnterFrameBig5(arg1:Event) : void {
        if(this.checkBigSkills() && !this.player.bigSkill5){
            this.bigSkill5.alpha = 0.5;
            this.bigSpdSkill.alpha = 0.5;
            this.bigSkill5.disabled = true;
            return;
        }
        if(this.player.smallSkill5 >= 5 && this.player.smallSkill4 >= 3 && this.player.smallSkill7 >= 2){
            this.bigSkill5.alpha = 1;
            this.bigSpdSkill.alpha = 1;
            this.bigSkill5.disabled = false;
            if(this.player.bigSkill5){
                this.bigSkill5.filters = [new GlowFilter(0x51FC4E)];
                this.bigSkill4.disabled = true;
            }
        }
    }

    private function onEnterFrameBig6(arg1:Event) : void {
        if(this.checkBigSkills() && !this.player.bigSkill6){
            this.bigSkill6.alpha = 0.5;
            this.bigDexSkill.alpha = 0.5;
            this.bigSkill6.disabled = true;
            return;
        }
        if(this.player.smallSkill6 >= 5 && this.player.smallSkill3 >= 2 && this.player.smallSkill11 >= 1){
            this.bigSkill6.alpha = 1;
            this.bigDexSkill.alpha = 1;
            this.bigSkill6.disabled = false;
            if(this.player.bigSkill6){
                this.bigSkill6.filters = [new GlowFilter(0xFDA546)];
                this.bigSkill6.disabled = true;
            }
        }
    }

    private function onEnterFrameBig7(arg1:Event) : void {
        if(this.checkBigSkills() && !this.player.bigSkill7){
            this.bigSkill7.alpha = 0.5;
            this.bigVitSkill.alpha = 0.5;
            this.bigSkill7.disabled = true;
            return;
        }
        if(this.player.smallSkill7 >= 5 && this.player.smallSkill9 >= 3){
            this.bigSkill7.alpha = 1;
            this.bigVitSkill.alpha = 1;
            this.bigSkill7.disabled = false;
            if(this.player.bigSkill7){
                this.bigSkill7.filters = [new GlowFilter(0xEA2424)];
                this.bigSkill7.disabled = true;
            }
        }
    }

    private function onEnterFrameBig8(arg1:Event) : void {
        if(this.checkBigSkills() && !this.player.bigSkill8){
            this.bigSkill8.alpha = 0.5;
            this.bigWisSkill.alpha = 0.5;
            this.bigSkill8.disabled = true;
            return;
        }
        if(this.player.smallSkill8 >= 5 && this.player.smallSkill10 >= 3){
            this.bigSkill8.alpha = 1;
            this.bigWisSkill.alpha = 1;
            this.bigSkill8.disabled = false;
            if(this.player.bigSkill8){
                this.bigSkill8.filters = [new GlowFilter(0x4A92FE)];
                this.bigSkill8.disabled = true;
            }
        }
    }

    private function onEnterFrameBig9(arg1:Event) : void {
        if(this.checkBigSkills() && !this.player.bigSkill9){
            this.bigSkill9.alpha = 0.5;
            this.bigSkill9.disabled = true;
            return;
        }
        if(this.player.smallSkill9 >= 5 && this.player.smallSkill1 >= 3){
            this.bigSkill9.alpha = 1;
            this.bigHPRegenSkill.alpha = 1;
            this.bigSkill9.disabled = false;
            if(this.player.bigSkill9){
                this.bigSkill9.filters = [new GlowFilter(0xE61E1E)];
                this.bigSkill9.disabled = true;
            }
        }
    }

    private function onEnterFrameBig10(arg1:Event) : void {
        if(this.checkBigSkills() && !this.player.bigSkill10){
            this.bigSkill10.alpha = 0.5;
            this.bigSkill10.disabled = true;
            return;
        }
        if(this.player.smallSkill10 >= 5 && this.player.smallSkill2 >= 3){
            this.bigSkill10.alpha = 1;
            this.bigMPRegenSkill.alpha = 1;
            this.bigSkill10.disabled = false;
            if(this.player.bigSkill10){
                this.bigSkill10.filters = [new GlowFilter(0x1D61E6)];
                this.bigSkill10.disabled = true;
            }
        }
    }

    private function onEnterFrameBig11(arg1:Event) : void {
        if(this.checkBigSkills() && !this.player.bigSkill11){
            this.bigSkill11.alpha = 0.5;
            this.bigSkill11.disabled = true;
            return;
        }
        if(this.player.smallSkill11 >= 5 && this.player.smallSkill6 >= 3){
            this.bigSkill11.alpha = 1;
            this.bigRoFSkill.alpha = 1;
            this.bigSkill11.disabled = false;
            if(this.player.bigSkill11){
                this.bigSkill11.filters = [new GlowFilter(0x89D412)];
                this.bigSkill11.disabled = true;
            }
        }
    }

    private function onEnterFrameBig12(arg1:Event) : void {
        if(this.checkBigSkills() && !this.player.bigSkill12){
            this.bigSkill12.alpha = 0.5;
            this.bigLootSkill.alpha = 0.5;
            this.bigSkill12.disabled = true;
            return;
        }
        if(this.player.smallSkill12 >= 5 && this.player.smallSkill11 >= 3){
            this.bigSkill12.alpha = 1;
            this.bigLootSkill.alpha = 1;
            this.bigSkill12.disabled = false;
            if(this.player.bigSkill12){
                this.bigSkill12.filters = [new GlowFilter(0x74C01B)];
                this.bigSkill12.disabled = true;
            }
        }
    }


    internal function MaxText(player:Player):int //changed it to return a number
    {
        var maxed:int = 0;
        var classes:ClassesModel = StaticInjectorContext.getInjector().getInstance(ClassesModel);
        var charType:CharacterClass = classes.getCharacterClass(player.objectType_); //Imports
        if(Math.max(player.maxHP_ - player.maxHPBoost_ - player.maxHPMax_, 0) >= 50) //life
        {
            maxed += 2;
        }
        if(Math.max(player.maxMP_ - player.maxMPBoost_ - player.maxMPMax_, 0) >= 50) //mana
        {
            maxed += 2;
        }
        if(Math.max(player.attack_ - player.attackBoost_ - player.attackMax_, 0) >= 10) //attack
        {
            maxed += 2;
        }
        if(Math.max(player.defense_ - player.defenseBoost_ - player.defenseMax_, 0) >= 10) //defense
        {
            maxed += 2;
        }
        if(Math.max(player.dexterity_ - player.dexterityBoost_ - player.dexterityMax_, 0) >= 10) //dexterity
        {
            maxed += 2;
        }
        if(Math.max(player.speed_ - player.speedBoost_ - player.speedMax_, 0) >= 10) //speed
        {
            maxed += 2;
        }
        if(Math.max(player.vitality_ - player.vitalityBoost_ - player.vitalityMax_, 0) >= 10) //vit
        {
            maxed += 2;
        }
        if(Math.max(player.wisdom_ - player.wisdomBoost_ - player.wisdomMax_, 0) >= 10) //wis
        {
            maxed += 2;
        }
        //Removed useless if statements and added +2 to each maxed.

        return maxed; // return the number
    }


    internal function checkBigSkills():Boolean{
        var local1:int = 0;
        if(this.player.bigSkill1){
            local1 += 1;
        }
        if(this.player.bigSkill2){
            local1 += 1;
        }
        if(this.player.bigSkill3){
            local1 += 1;
        }
        if(this.player.bigSkill4){
            local1 += 1;
        }
        if(this.player.bigSkill5){
            local1 += 1;
        }
        if(this.player.bigSkill6){
            local1 += 1;
        }
        if(this.player.bigSkill7){
            local1 += 1;
        }
        if(this.player.bigSkill8){
            local1 += 1;
        }
        if(this.player.bigSkill9){
            local1 += 1;
        }
        if(this.player.bigSkill10){
            local1 += 1;
        }
        if(this.player.bigSkill11){
            local1 += 1;
        }
        if(this.player.bigSkill12){
            local1 += 1;
        }

        var maxStats:int = MaxText(this.player); //get the maxed stats of the player
        var local2:Boolean = local1 >= 4;
        if(maxStats >= 16) //basically, 16/16
                local2 = local1 >= 5; //this boolean will be true if the player already have 5 slots ocupped, else, will return false
        return local2;
    }

    private function onRollOver1(arg1:MouseEvent):void
    {
        if(smallSkill1ToolTip != null)
        {
            if(smallSkill1ToolTip.parent != null)
            {
                smallSkill1ToolTip.parent.removeChild(smallSkill1ToolTip);
            }
        }
        smallSkill1ToolTip = new SkillTreeToolTip("Small Skill of Life ", "+2% Life", 150);
        StaticInjectorContext.getInjector().getInstance(ShowTooltipSignal).dispatch(smallSkill1ToolTip);
    }

    private function onRollOut1(param1:MouseEvent) : void
    {
        if(smallSkill1ToolTip && smallSkill1ToolTip.parent)
        {
            smallSkill1ToolTip.parent.removeChild(smallSkill1ToolTip);
        }
    }

    private function onRollOver2(arg1:MouseEvent):void
    {
        if(smallSkill2ToolTip != null)
        {
            if(smallSkill2ToolTip.parent != null)
            {
                smallSkill2ToolTip.parent.removeChild(smallSkill2ToolTip);
            }
        }
        smallSkill2ToolTip = new SkillTreeToolTip("Small Skill of Mana ", "+2% Mana", 150);
        StaticInjectorContext.getInjector().getInstance(ShowTooltipSignal).dispatch(smallSkill2ToolTip);
    }

    private function onRollOut2(param1:MouseEvent) : void
    {
        if(smallSkill2ToolTip && smallSkill2ToolTip.parent)
        {
            smallSkill2ToolTip.parent.removeChild(smallSkill2ToolTip);
        }
    }

    private function onRollOver3(arg1:MouseEvent):void
    {
        if(smallSkill3ToolTip != null)
        {
            if(smallSkill3ToolTip.parent != null)
            {
                smallSkill3ToolTip.parent.removeChild(smallSkill3ToolTip);
            }
        }
        smallSkill3ToolTip = new SkillTreeToolTip("Small Skill of Attack ", "+2% Attack", 150);
        StaticInjectorContext.getInjector().getInstance(ShowTooltipSignal).dispatch(smallSkill3ToolTip);
    }

    private function onRollOut3(param1:MouseEvent) : void
    {
        if(smallSkill3ToolTip && smallSkill3ToolTip.parent)
        {
            smallSkill3ToolTip.parent.removeChild(smallSkill3ToolTip);
        }
    }

    private function onRollOver4(arg1:MouseEvent):void
    {
        if(smallSkill4ToolTip != null)
        {
            if(smallSkill4ToolTip.parent != null)
            {
                smallSkill4ToolTip.parent.removeChild(smallSkill4ToolTip);
            }
        }
        smallSkill4ToolTip = new SkillTreeToolTip("Small Skill of Defense ", "+2% Defense", 175);
        StaticInjectorContext.getInjector().getInstance(ShowTooltipSignal).dispatch(smallSkill4ToolTip);
    }

    private function onRollOut4(param1:MouseEvent) : void
    {
        if(smallSkill4ToolTip && smallSkill4ToolTip.parent)
        {
            smallSkill4ToolTip.parent.removeChild(smallSkill4ToolTip);
        }
    }

    private function onRollOver5(arg1:MouseEvent):void
    {
        if(smallSkill5ToolTip != null)
        {
            if(smallSkill5ToolTip.parent != null)
            {
                smallSkill5ToolTip.parent.removeChild(smallSkill5ToolTip);
            }
        }
        smallSkill5ToolTip = new SkillTreeToolTip("Small Skill of Speed ", "+2% Speed");
        StaticInjectorContext.getInjector().getInstance(ShowTooltipSignal).dispatch(smallSkill5ToolTip);
    }

    private function onRollOut5(param1:MouseEvent) : void
    {
        if(smallSkill5ToolTip && smallSkill5ToolTip.parent)
        {
            smallSkill5ToolTip.parent.removeChild(smallSkill5ToolTip);
        }
    }

    private function onRollOver6(arg1:MouseEvent):void
    {
        if(smallSkill6ToolTip != null)
        {
            if(smallSkill6ToolTip.parent != null)
            {
                smallSkill6ToolTip.parent.removeChild(smallSkill6ToolTip);
            }
        }
        smallSkill6ToolTip = new SkillTreeToolTip("Small Skill of Dexterity ", "+2% Dexterity", 175);
        StaticInjectorContext.getInjector().getInstance(ShowTooltipSignal).dispatch(smallSkill6ToolTip);
    }

    private function onRollOut6(param1:MouseEvent) : void
    {
        if(smallSkill6ToolTip && smallSkill6ToolTip.parent)
        {
            smallSkill6ToolTip.parent.removeChild(smallSkill6ToolTip);
        }
    }

    private function onRollOver7(arg1:MouseEvent):void
    {
        if(smallSkill7ToolTip != null)
        {
            if(smallSkill7ToolTip.parent != null)
            {
                smallSkill7ToolTip.parent.removeChild(smallSkill7ToolTip);
            }
        }
        smallSkill7ToolTip = new SkillTreeToolTip("Small Skill of Vitality ", "+2% Vitality");
        StaticInjectorContext.getInjector().getInstance(ShowTooltipSignal).dispatch(smallSkill7ToolTip);
    }

    private function onRollOut7(param1:MouseEvent) : void
    {
        if(smallSkill7ToolTip && smallSkill7ToolTip.parent)
        {
            smallSkill7ToolTip.parent.removeChild(smallSkill7ToolTip);
        }
    }

    private function onRollOver8(arg1:MouseEvent):void
    {
        if(smallSkill8ToolTip != null)
        {
            if(smallSkill8ToolTip.parent != null)
            {
                smallSkill8ToolTip.parent.removeChild(smallSkill8ToolTip);
            }
        }
        smallSkill8ToolTip = new SkillTreeToolTip("Small Skill of Wisdom ", "+2% Wisdom", 175);
        StaticInjectorContext.getInjector().getInstance(ShowTooltipSignal).dispatch(smallSkill8ToolTip);
    }

    private function onRollOut8(param1:MouseEvent) : void
    {
        if(smallSkill8ToolTip && smallSkill8ToolTip.parent)
        {
            smallSkill8ToolTip.parent.removeChild(smallSkill8ToolTip);
        }
    }

    private function onRollOver9(arg1:MouseEvent):void
    {
        if(smallSkill9ToolTip != null)
        {
            if(smallSkill9ToolTip.parent != null)
            {
                smallSkill9ToolTip.parent.removeChild(smallSkill9ToolTip);
            }
        }
        smallSkill9ToolTip = new SkillTreeToolTip("Small Skill of HP Regeneration ", "+2% Health Regeneration", 225);
        StaticInjectorContext.getInjector().getInstance(ShowTooltipSignal).dispatch(smallSkill9ToolTip);
    }

    private function onRollOut9(param1:MouseEvent) : void
    {
        if(smallSkill9ToolTip && smallSkill9ToolTip.parent)
        {
            smallSkill9ToolTip.parent.removeChild(smallSkill9ToolTip);
        }
    }

    private function onRollOver10(arg1:MouseEvent):void
    {
        if(smallSkill10ToolTip != null)
        {
            if(smallSkill10ToolTip.parent != null)
            {
                smallSkill10ToolTip.parent.removeChild(smallSkill10ToolTip);
            }
        }
        smallSkill10ToolTip = new SkillTreeToolTip("Small Skill of MP Regeneration ", "+2% Mana Regeneration", 225);
        StaticInjectorContext.getInjector().getInstance(ShowTooltipSignal).dispatch(smallSkill10ToolTip);
    }

    private function onRollOut10(param1:MouseEvent) : void
    {
        if(smallSkill10ToolTip && smallSkill10ToolTip.parent)
        {
            smallSkill10ToolTip.parent.removeChild(smallSkill10ToolTip);
        }
    }

    private function onRollOver11(arg1:MouseEvent):void
    {
        if(smallSkill11ToolTip != null)
        {
            if(smallSkill11ToolTip.parent != null)
            {
                smallSkill11ToolTip.parent.removeChild(smallSkill11ToolTip);
            }
        }
        smallSkill11ToolTip = new SkillTreeToolTip("Small Skill of RoF ", "+2% Rate of Fire");
        StaticInjectorContext.getInjector().getInstance(ShowTooltipSignal).dispatch(smallSkill11ToolTip);
    }

    private function onRollOut11(param1:MouseEvent) : void
    {
        if(smallSkill11ToolTip && smallSkill11ToolTip.parent)
        {
            smallSkill11ToolTip.parent.removeChild(smallSkill11ToolTip);
        }
    }

    private function onRollOver12(arg1:MouseEvent):void
    {
        if(smallSkill12ToolTip != null)
        {
            if(smallSkill12ToolTip.parent != null)
            {
                smallSkill12ToolTip.parent.removeChild(smallSkill12ToolTip);
            }
        }
        smallSkill12ToolTip = new SkillTreeToolTip("Small Skill of Looting ", "+2% Looting", 175);
        StaticInjectorContext.getInjector().getInstance(ShowTooltipSignal).dispatch(smallSkill12ToolTip);
    }

    private function onRollOut12(param1:MouseEvent) : void
    {
        if(smallSkill12ToolTip && smallSkill12ToolTip.parent)
        {
            smallSkill12ToolTip.parent.removeChild(smallSkill12ToolTip);
        }
    }

    private function onEnterFrame(arg1:Event) : void
    {
        var local1:int = 0;
        if(this.player.maxedLife){
            local1 += 5;
        }
        if(this.player.maxedMana){
            local1 += 5;
        }
        if(this.player.maxedAtt){
            local1 += 5;
        }
        if(this.player.maxedDef){
            local1 += 5;
        }
        if(this.player.maxedSpd){
            local1 += 5;
        }
        if(this.player.maxedDex){
            local1 += 5;
        }
        if(this.player.maxedVit){
            local1 += 5;
        }
        if(this.player.maxedWis){
            local1 += 5;
        }
        if(this.player.smallSkill1 >= 5){
            this.player.smallSkill1 = 5;
        }
        if(this.player.smallSkill2 >= 5){
            this.player.smallSkill2 = 5;
        }
        if(this.player.smallSkill3 >= 5){
            this.player.smallSkill3 = 5;
        }
        if(this.player.smallSkill4 >= 5){
            this.player.smallSkill4 = 5;
        }
        if(this.player.smallSkill5 >= 5){
            this.player.smallSkill5 = 5;
        }
        if(this.player.smallSkill6 >= 5){
            this.player.smallSkill6 = 5;
        }
        if(this.player.smallSkill7 >= 5){
            this.player.smallSkill7 = 5;
        }
        if(this.player.smallSkill8 >= 5){
            this.player.smallSkill8 = 5;
        }
        if(this.player.smallSkill9 >= 5){
            this.player.smallSkill9 = 5;
        }
        if(this.player.smallSkill10 >= 5){
            this.player.smallSkill10 = 5;
        }
        if(this.player.smallSkill11 >= 5){
            this.player.smallSkill11 = 5;
        }
        if(this.player.smallSkill12 >= 5){
            this.player.smallSkill12 = 5;
        }
        if(local1 > this.player.smallSkill1 + this.player.smallSkill2 + this.player.smallSkill3 + this.player.smallSkill4 + this.player.smallSkill5 + this.player.smallSkill6 + this.player.smallSkill7 + this.player.smallSkill8 + this.player.smallSkill9 + this.player.smallSkill10 + this.player.smallSkill11 + this.player.smallSkill12){
            this.player.points = local1 - (this.player.smallSkill1 + this.player.smallSkill2 + this.player.smallSkill3 + this.player.smallSkill4 + this.player.smallSkill5 + this.player.smallSkill6 + this.player.smallSkill7 + this.player.smallSkill8 + this.player.smallSkill9 + this.player.smallSkill10 + this.player.smallSkill11 + this.player.smallSkill12);
        }
        if(this.pointsText != null){
            if(this.pointsText.parent != null){
                this.pointsText.parent.removeChild(this.pointsText);
            }
        }
        this.pointsText.setText("Points: " + this.player.points + "/40").setSize(30).setColor(0xFFFFFF);
        addChild(this.pointsText);
    }

    private function onClose(arg1:Event) : void
    {
        this.close.dispatch();
    }

}
}
