package kabam.rotmg.NewSkillTree {
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.objects.Player;
import com.company.assembleegameclient.ui.tooltip.BigSkillTreeToolTip;
import com.company.assembleegameclient.ui.tooltip.SkillTreeToolTip;
import com.company.assembleegameclient.ui.tooltip.ToolTip;
import com.company.ui.SimpleText;

import flash.display.Bitmap;
import flash.display.Graphics;
import flash.display.Sprite;

import flash.events.Event;

import flash.events.MouseEvent;
import flash.filters.GlowFilter;

import io.decagames.rotmg.ui.buttons.SliceScalingButton;
import io.decagames.rotmg.ui.defaults.DefaultLabelFormat;

import io.decagames.rotmg.ui.popups.header.PopupHeader;

import io.decagames.rotmg.ui.popups.modal.ModalPopup;
import io.decagames.rotmg.ui.sliceScaling.SliceScalingBitmap;
import io.decagames.rotmg.ui.texture.TextureParser;

import kabam.rotmg.NewSkillTree.images.AddPoint;
import kabam.rotmg.NewSkillTree.images.RemovePoint;
import kabam.rotmg.classes.model.CharacterClass;
import kabam.rotmg.classes.model.ClassesModel;

import kabam.rotmg.core.StaticInjectorContext;
import kabam.rotmg.core.signals.ShowTooltipSignal;
import kabam.rotmg.game.model.GameModel;

import org.osflash.signals.Signal;

public class SkillTreeModal extends ModalPopup{

    //region Original
    internal var gs_:GameSprite;
    internal var quitButton:SliceScalingButton;
    internal var saveButton:SliceScalingButton;
    internal var saveButtonDecor:SliceScalingBitmap;
    internal var player:Player;
    internal var text_:String;

    internal var pointsText:SimpleText;

    internal var tree1Tick1ToolTip:ToolTip;
    internal var tree1Tick2ToolTip:ToolTip;
    internal var tree1Tick3ToolTip:ToolTip;
    internal var tree1Tick4ToolTip:ToolTip;
    internal var tree1Tick5ToolTip:ToolTip;
    internal var tree1Tick6ToolTip:ToolTip;
    internal var tree2Tick1ToolTip:ToolTip;
    internal var tree2Tick2ToolTip:ToolTip;
    internal var tree2Tick3ToolTip:ToolTip;
    internal var tree2Tick4ToolTip:ToolTip;
    internal var tree2Tick5ToolTip:ToolTip;
    internal var tree2Tick6ToolTip:ToolTip;
    internal var tree3Tick1ToolTip:ToolTip;
    internal var tree3Tick2ToolTip:ToolTip;
    internal var tree3Tick3ToolTip:ToolTip;
    internal var tree3Tick4ToolTip:ToolTip;
    internal var tree3Tick5ToolTip:ToolTip;
    internal var tree3Tick6ToolTip:ToolTip;
    internal var tree4Tick1ToolTip:ToolTip;
    internal var tree4Tick2ToolTip:ToolTip;
    internal var tree4Tick3ToolTip:ToolTip;
    internal var tree4Tick4ToolTip:ToolTip;
    internal var tree4Tick5ToolTip:ToolTip;
    internal var tree4Tick6ToolTip:ToolTip;
    internal var tree5Tick1ToolTip:ToolTip;
    internal var tree5Tick2ToolTip:ToolTip;
    internal var tree5Tick3ToolTip:ToolTip;
    internal var tree5Tick4ToolTip:ToolTip;
    internal var tree5Tick5ToolTip:ToolTip;
    internal var tree5Tick6ToolTip:ToolTip;

    internal var close:Signal = new Signal();
    //endregion
    internal var addBitmap:Bitmap;
    internal var removeBitmap:Bitmap;
    internal var tree1Add:SliceScalingButton;
    internal var tree1Remove:SliceScalingButton;
    internal var tree1Points:SimpleText;
    internal var tree2Add:SliceScalingButton;
    internal var tree2Remove:SliceScalingButton;
    internal var tree2Points:SimpleText;
    internal var tree3Add:SliceScalingButton;
    internal var tree3Remove:SliceScalingButton;
    internal var tree3Points:SimpleText;
    internal var tree4Add:SliceScalingButton;
    internal var tree4Remove:SliceScalingButton;
    internal var tree4Points:SimpleText;
    internal var tree5Add:SliceScalingButton;
    internal var tree5Remove:SliceScalingButton;
    internal var tree5Points:SimpleText;

    internal var bg_:Sprite;
    internal var pointsBg:Sprite;
    internal var tree1tick1_:Sprite, tree1tick2_:Sprite, tree1tick3_:Sprite, tree1tick4_:Sprite, tree1tick5_:Sprite, tree1tick6_:Sprite, tree1tick7_:Sprite, tree1tick8_:Sprite, tree1tick9_:Sprite, tree1tick10_:Sprite, tree1tick11_:Sprite, tree1tick12_:Sprite, tree1tick13_:Sprite, tree1tick14_:Sprite, tree1tick15_:Sprite, tree1tick16_:Sprite, tree1tick17_:Sprite, tree1tick18_:Sprite, tree1tick19_:Sprite, tree1tick20_:Sprite, tree2tick1_:Sprite, tree2tick2_:Sprite, tree2tick3_:Sprite, tree2tick4_:Sprite, tree2tick5_:Sprite, tree2tick6_:Sprite, tree2tick7_:Sprite, tree2tick8_:Sprite, tree2tick9_:Sprite, tree2tick10_:Sprite, tree2tick11_:Sprite, tree2tick12_:Sprite, tree2tick13_:Sprite, tree2tick14_:Sprite, tree2tick15_:Sprite, tree2tick16_:Sprite, tree2tick17_:Sprite, tree2tick18_:Sprite, tree2tick19_:Sprite, tree2tick20_:Sprite, tree3tick1_:Sprite, tree3tick2_:Sprite, tree3tick3_:Sprite, tree3tick4_:Sprite, tree3tick5_:Sprite, tree3tick6_:Sprite, tree3tick7_:Sprite, tree3tick8_:Sprite, tree3tick9_:Sprite, tree3tick10_:Sprite, tree3tick11_:Sprite, tree3tick12_:Sprite, tree3tick13_:Sprite, tree3tick14_:Sprite, tree3tick15_:Sprite, tree3tick16_:Sprite, tree3tick17_:Sprite, tree3tick18_:Sprite, tree3tick19_:Sprite, tree3tick20_:Sprite, tree4tick1_:Sprite, tree4tick2_:Sprite, tree4tick3_:Sprite, tree4tick4_:Sprite, tree4tick5_:Sprite, tree4tick6_:Sprite, tree4tick7_:Sprite, tree4tick8_:Sprite, tree4tick9_:Sprite, tree4tick10_:Sprite, tree4tick11_:Sprite, tree4tick12_:Sprite, tree4tick13_:Sprite, tree4tick14_:Sprite, tree4tick15_:Sprite, tree4tick16_:Sprite, tree4tick17_:Sprite, tree4tick18_:Sprite, tree4tick19_:Sprite, tree4tick20_:Sprite, tree5tick1_:Sprite, tree5tick2_:Sprite, tree5tick3_:Sprite, tree5tick4_:Sprite, tree5tick5_:Sprite, tree5tick6_:Sprite, tree5tick7_:Sprite, tree5tick8_:Sprite, tree5tick9_:Sprite, tree5tick10_:Sprite, tree5tick11_:Sprite, tree5tick12_:Sprite, tree5tick13_:Sprite, tree5tick14_:Sprite, tree5tick15_:Sprite, tree5tick16_:Sprite, tree5tick17_:Sprite, tree5tick18_:Sprite, tree5tick19_:Sprite, tree5tick20_:Sprite;

    private var tree1Label_:SimpleText;
    private var tree2Label_:SimpleText;
    private var tree3Label_:SimpleText;
    private var tree4Label_:SimpleText;
    private var tree5Label_:SimpleText;
    private var pointsLabel:SimpleText;

    internal var glowFilter:GlowFilter = new GlowFilter(0xffffff, 0.9, 4, 4, 20, 1);


    public function SkillTreeModal(gs:GameSprite) {
        this.gs_ = gs;
        this.tree1tick1_ = new Sprite();
        this.tree1tick2_ = new Sprite();
        this.tree1tick3_ = new Sprite();
        this.tree1tick4_ = new Sprite();
        this.tree1tick5_ = new Sprite();
        this.tree1tick6_ = new Sprite();
        this.tree1tick7_ = new Sprite();
        this.tree1tick8_ = new Sprite();
        this.tree1tick9_ = new Sprite();
        this.tree1tick10_ = new Sprite();
        this.tree1tick11_ = new Sprite();
        this.tree1tick12_ = new Sprite();
        this.tree1tick13_ = new Sprite();
        this.tree1tick14_ = new Sprite();
        this.tree1tick15_ = new Sprite();
        this.tree1tick16_ = new Sprite();
        this.tree1tick17_ = new Sprite();
        this.tree1tick18_ = new Sprite();
        this.tree1tick19_ = new Sprite();
        this.tree1tick20_ = new Sprite();
        this.tree2tick1_ = new Sprite();
        this.tree2tick2_ = new Sprite();
        this.tree2tick3_ = new Sprite();
        this.tree2tick4_ = new Sprite();
        this.tree2tick5_ = new Sprite();
        this.tree2tick6_ = new Sprite();
        this.tree2tick7_ = new Sprite();
        this.tree2tick8_ = new Sprite();
        this.tree2tick9_ = new Sprite();
        this.tree2tick10_ = new Sprite();
        this.tree2tick11_ = new Sprite();
        this.tree2tick12_ = new Sprite();
        this.tree2tick13_ = new Sprite();
        this.tree2tick14_ = new Sprite();
        this.tree2tick15_ = new Sprite();
        this.tree2tick16_ = new Sprite();
        this.tree2tick17_ = new Sprite();
        this.tree2tick18_ = new Sprite();
        this.tree2tick19_ = new Sprite();
        this.tree2tick20_ = new Sprite();

        this.tree3tick1_ = new Sprite();
        this.tree3tick2_ = new Sprite();
        this.tree3tick3_ = new Sprite();
        this.tree3tick4_ = new Sprite();
        this.tree3tick5_ = new Sprite();
        this.tree3tick6_ = new Sprite();
        this.tree3tick7_ = new Sprite();
        this.tree3tick8_ = new Sprite();
        this.tree3tick9_ = new Sprite();
        this.tree3tick10_ = new Sprite();
        this.tree3tick11_ = new Sprite();
        this.tree3tick12_ = new Sprite();
        this.tree3tick13_ = new Sprite();
        this.tree3tick14_ = new Sprite();
        this.tree3tick15_ = new Sprite();
        this.tree3tick16_ = new Sprite();
        this.tree3tick17_ = new Sprite();
        this.tree3tick18_ = new Sprite();
        this.tree3tick19_ = new Sprite();
        this.tree3tick20_ = new Sprite();

        this.tree4tick1_ = new Sprite();
        this.tree4tick2_ = new Sprite();
        this.tree4tick3_ = new Sprite();
        this.tree4tick4_ = new Sprite();
        this.tree4tick5_ = new Sprite();
        this.tree4tick6_ = new Sprite();
        this.tree4tick7_ = new Sprite();
        this.tree4tick8_ = new Sprite();
        this.tree4tick9_ = new Sprite();
        this.tree4tick10_ = new Sprite();
        this.tree4tick11_ = new Sprite();
        this.tree4tick12_ = new Sprite();
        this.tree4tick13_ = new Sprite();
        this.tree4tick14_ = new Sprite();
        this.tree4tick15_ = new Sprite();
        this.tree4tick16_ = new Sprite();
        this.tree4tick17_ = new Sprite();
        this.tree4tick18_ = new Sprite();
        this.tree4tick19_ = new Sprite();
        this.tree4tick20_ = new Sprite();

        this.tree5tick1_ = new Sprite();
        this.tree5tick2_ = new Sprite();
        this.tree5tick3_ = new Sprite();
        this.tree5tick4_ = new Sprite();
        this.tree5tick5_ = new Sprite();
        this.tree5tick6_ = new Sprite();
        this.tree5tick7_ = new Sprite();
        this.tree5tick8_ = new Sprite();
        this.tree5tick9_ = new Sprite();
        this.tree5tick10_ = new Sprite();
        this.tree5tick11_ = new Sprite();
        this.tree5tick12_ = new Sprite();
        this.tree5tick13_ = new Sprite();
        this.tree5tick14_ = new Sprite();
        this.tree5tick15_ = new Sprite();
        this.tree5tick16_ = new Sprite();
        this.tree5tick17_ = new Sprite();
        this.tree5tick18_ = new Sprite();
        this.tree5tick19_ = new Sprite();
        this.tree5tick20_ = new Sprite();
        this.bg_ = new Sprite();
        this.pointsBg = new Sprite();
        super(590, 510, "Skill Tree");
        this.x = 3;
        this.y = 22.5;
        this.player = StaticInjectorContext.getInjector().getInstance(GameModel).player;
        this.quitButton = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI","close_button"));
        this.header.addButton(this.quitButton, PopupHeader.RIGHT_BUTTON);

        this.pointsText = new SimpleText(30, 0xFFFFFF, false, 300);
        this.pointsText.setText(this.player.points + "");
        this.pointsText.x = this.player.points < 10 ? this.width/2 - 20 : this.width/2 - 30;
        this.pointsText.y = 33;
        this.pointsText.addEventListener(Event.ENTER_FRAME, this.onEnterFrame);
        addChild(this.pointsText);

        this.pointsBg.graphics.beginFill(0x202020);
        this.pointsBg.graphics.drawCircle(this.width/2 - 11, 39, 37); //+37 x
        this.pointsBg.graphics.endFill();
        this.pointsBg.filters = [glowFilter];
        addChild(pointsBg); //Medium 1

        this.pointsLabel = new SimpleText(14, 0xFFFFFF, false, 300);
        this.pointsLabel.setText("Available");
        this.pointsLabel.x = this.width/2 - 40;
        this.pointsLabel.y = 13;
        addChild(this.pointsLabel);


        this.saveButtonDecor = SliceScalingBitmap(TextureParser.instance.getSliceScalingBitmap("UI", "main_button_decoration_dark", 155));
        this.saveButtonDecor.x = this.width/2 - 79.5;
        this.saveButtonDecor.y = 467;
        this.saveButtonDecor.scaleX = 0.9;
        this.saveButtonDecor.scaleY = 0.9;
        addChild(this.saveButtonDecor);
        this.saveButton = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "generic_green_button"));
        this.saveButton.width = 101.5;
        this.saveButton.setLabel("Save", DefaultLabelFormat.defaultModalTitle);
        this.saveButton.x = this.width/2 -  60;
        this.saveButton.y = 470;
        addChild(this.saveButton);


        //region tree1
        this.tree1Remove = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "skilltree_box", 100));
        this.tree1Remove.scaleX = 0.25;
        this.tree1Remove.scaleY = 0.25;
        this.tree1Remove.x = 8;
        this.tree1Remove.y = 79;
        this.removeBitmap = new RemovePoint();
        this.removeBitmap.x = this.tree1Remove.x;
        this.removeBitmap.y = this.tree1Remove.y+1;
        addChild(this.tree1Remove);
        addChild(this.removeBitmap);

        this.tree1Add = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "skilltree_box", 100));
        this.tree1Add.scaleX = 0.25;
        this.tree1Add.scaleY = 0.25;
        this.tree1Add.x = 86;
        this.tree1Add.y = 79;

        this.addBitmap = new AddPoint();
        this.addBitmap.x = this.tree1Add.x;
        this.addBitmap.y = this.tree1Add.y+1;
        this.tree1Points = new SimpleText(32, 0xFFFFFF).setBold(true).setText(String((this.player.node1TickMaj)+(this.player.node1Med*5)+(this.player.node1TickMin)+(this.player.node1Big*10)));
        if ((this.player.node1TickMaj)+(this.player.node1Med*5)+(this.player.node1TickMin)+(this.player.node1Big*10) > 9) {
            this.tree1Points.x = this.tree1Add.x - 48;
        } else {
            this.tree1Points.x = this.tree1Add.x - 38;
        }
        this.tree1Points.y = this.tree1Add.y - 8;
        addChild(this.tree1Add);
        addChild(this.addBitmap);
        addChild(this.tree1Points);

        this.tree1Label_ = new SimpleText(24, 0xFFFFFF, false, 300);
        this.tree1Label_.setText("Warrior's Strength");
        this.tree1Label_.x = this.tree1Add.x + 40;
        this.tree1Label_.y = this.tree1Add.y + 3;
        addChild(this.tree1Label_);

        this.tree1tick1_.graphics.beginFill(0x949494);
        this.tree1tick1_.graphics.drawRect(this.tree1Remove.x - 10 + 5, this.tree1Remove.y + 45, 20, 6); //30 x
        this.tree1tick1_.graphics.endFill();
        this.tree1tick1_.filters = [glowFilter];
        addChild(tree1tick1_); //min

        this.tree1tick2_.graphics.beginFill(0x949494);
        this.tree1tick2_.graphics.drawRect(this.tree1Remove.x - 10 + 35, this.tree1Add.y + 45, 20, 6);
        this.tree1tick2_.graphics.endFill();
        this.tree1tick2_.filters = [glowFilter];
        addChild(tree1tick2_); //maj

        this.tree1tick3_.graphics.beginFill(0x949494);
        this.tree1tick3_.graphics.drawRect(this.tree1Remove.x - 10 + 65, this.tree1Add.y + 45, 20, 6);
        this.tree1tick3_.graphics.endFill();
        this.tree1tick3_.filters = [glowFilter];
        addChild(tree1tick3_); //maj

        this.tree1tick4_.graphics.beginFill(0x949494);
        this.tree1tick4_.graphics.drawCircle(this.tree1Remove.x - 10 + 102, this.tree1Add.y + 47, 7.5); //+37 x
        this.tree1tick4_.graphics.endFill();
        this.tree1tick4_.filters = [glowFilter];
        addChild(tree1tick4_); //Medium 1

        this.tree1tick5_.graphics.beginFill(0x949494);
        this.tree1tick5_.graphics.drawRect(this.tree1Remove.x - 10 + 120, this.tree1Remove.y + 45, 20, 6); //30 x
        this.tree1tick5_.graphics.endFill();
        this.tree1tick5_.filters = [glowFilter];
        addChild(tree1tick5_); //min

        this.tree1tick6_.graphics.beginFill(0x949494);
        this.tree1tick6_.graphics.drawRect(this.tree1Remove.x - 10 + 150, this.tree1Add.y + 45, 20, 6);
        this.tree1tick6_.graphics.endFill();
        this.tree1tick6_.filters = [glowFilter];
        addChild(tree1tick6_); //maj

        this.tree1tick7_.graphics.beginFill(0x949494);
        this.tree1tick7_.graphics.drawRect(this.tree1Remove.x - 10 + 180, this.tree1Add.y + 45, 20, 6);
        this.tree1tick7_.graphics.endFill();
        this.tree1tick7_.filters = [glowFilter];
        addChild(tree1tick7_); //maj

        this.tree1tick8_.graphics.beginFill(0x949494);
        this.tree1tick8_.graphics.drawCircle(this.tree1Remove.x - 10 + 217, this.tree1Add.y + 47, 7.5); //+37 x
        this.tree1tick8_.graphics.endFill();
        this.tree1tick8_.filters = [glowFilter];
        addChild(tree1tick8_); //Medium 2

        this.tree1tick9_.graphics.beginFill(0x949494);
        this.tree1tick9_.graphics.drawRect(this.tree1Remove.x - 10 + 235, this.tree1Remove.y + 45, 20, 6); //30 x
        this.tree1tick9_.graphics.endFill();
        this.tree1tick9_.filters = [glowFilter];
        addChild(tree1tick9_); //min

        this.tree1tick10_.graphics.beginFill(0x949494);
        this.tree1tick10_.graphics.drawRect(this.tree1Remove.x - 10 + 265, this.tree1Add.y + 45, 20, 6);
        this.tree1tick10_.graphics.endFill();
        this.tree1tick10_.filters = [glowFilter];
        addChild(tree1tick10_); //maj

        this.tree1tick11_.graphics.beginFill(0x949494);
        this.tree1tick11_.graphics.drawRect(this.tree1Remove.x - 10 + 295, this.tree1Add.y + 45, 20, 6);
        this.tree1tick11_.graphics.endFill();
        this.tree1tick11_.filters = [glowFilter];
        addChild(tree1tick11_); //maj

        this.tree1tick12_.graphics.beginFill(0x949494);
        this.tree1tick12_.graphics.drawCircle(this.tree1Remove.x - 10 + 336, this.tree1Add.y + 47, 12.5); //+37 x
        this.tree1tick12_.graphics.endFill();
        this.tree1tick12_.filters = [glowFilter];
        addChild(tree1tick12_); //Medium 2

        this.tree1tick13_.graphics.beginFill(0x949494);
        this.tree1tick13_.graphics.drawRect(this.tree1Remove.x - 10 + 357, this.tree1Remove.y + 45, 20, 6); //30 x
        this.tree1tick13_.graphics.endFill();
        this.tree1tick13_.filters = [glowFilter];
        addChild(tree1tick13_); //min

        this.tree1tick14_.graphics.beginFill(0x949494);
        this.tree1tick14_.graphics.drawRect(this.tree1Remove.x - 10 + 387, this.tree1Add.y + 45, 20, 6);
        this.tree1tick14_.graphics.endFill();
        this.tree1tick14_.filters = [glowFilter];
        addChild(tree1tick14_); //maj

        this.tree1tick15_.graphics.beginFill(0x949494);
        this.tree1tick15_.graphics.drawRect(this.tree1Remove.x - 10 + 417, this.tree1Add.y + 45, 20, 6);
        this.tree1tick15_.graphics.endFill();
        this.tree1tick15_.filters = [glowFilter];
        addChild(tree1tick15_); //maj

        this.tree1tick16_.graphics.beginFill(0x949494);
        this.tree1tick16_.graphics.drawCircle(this.tree1Remove.x - 10 + 454, this.tree1Add.y + 47, 7.5); //+37 x
        this.tree1tick16_.graphics.endFill();
        this.tree1tick16_.filters = [glowFilter];
        addChild(tree1tick16_); //Medium 1

        this.tree1tick17_.graphics.beginFill(0x949494);
        this.tree1tick17_.graphics.drawRect(this.tree1Remove.x - 10 + 472, this.tree1Remove.y + 45, 20, 6); //30 x
        this.tree1tick17_.graphics.endFill();
        this.tree1tick17_.filters = [glowFilter];
        addChild(tree1tick17_); //min

        this.tree1tick18_.graphics.beginFill(0x949494);
        this.tree1tick18_.graphics.drawRect(this.tree1Remove.x - 10 + 502, this.tree1Add.y + 45, 20, 6);
        this.tree1tick18_.graphics.endFill();
        this.tree1tick18_.filters = [glowFilter];
        addChild(tree1tick18_); //maj

        this.tree1tick19_.graphics.beginFill(0x949494);
        this.tree1tick19_.graphics.drawRect(this.tree1Remove.x - 10 + 532, this.tree1Add.y + 45, 20, 6);
        this.tree1tick19_.graphics.endFill();
        this.tree1tick19_.filters = [glowFilter];
        addChild(tree1tick19_); //maj

        this.tree1tick20_.graphics.beginFill(0x949494);
        this.tree1tick20_.graphics.drawCircle(this.tree1Remove.x - 10 + 573, this.tree1Add.y + 47, 12.5); //+37 x
        this.tree1tick20_.graphics.endFill();
        this.tree1tick20_.filters = [glowFilter];
        addChild(tree1tick20_); //big 2
        //endregion
        //region tree2
        this.tree2Remove = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "skilltree_box", 100));
        this.tree2Remove.scaleX = 0.25;
        this.tree2Remove.scaleY = 0.25;
        this.tree2Remove.x = 8;
        this.tree2Remove.y = 79 + 80;
        this.removeBitmap = new RemovePoint();
        this.removeBitmap.x = this.tree2Remove.x;
        this.removeBitmap.y = this.tree2Remove.y+1;
        addChild(this.tree2Remove);
        addChild(this.removeBitmap);

        this.tree2Add = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "skilltree_box", 100));
        this.tree2Add.scaleX = 0.25;
        this.tree2Add.scaleY = 0.25;
        this.tree2Add.x = 86;
        this.tree2Add.y = 79 + 80;
        //this.tree2Add.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver2);
        //this.tree2Add.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut2);
        this.addBitmap = new AddPoint();
        this.addBitmap.x = this.tree2Add.x;
        this.addBitmap.y = this.tree2Add.y+1;
        this.tree2Points = new SimpleText(32, 0xFFFFFF).setBold(true).setText(String((this.player.node2TickMaj)+(this.player.node2Med*5)+(this.player.node2TickMin)+(this.player.node2Big*10)));
        if ((this.player.node2TickMaj)+(this.player.node2Med*5)+(this.player.node2TickMin)+(this.player.node2Big*10) > 9) {
            this.tree2Points.x = this.tree2Add.x - 48;
        } else {
            this.tree2Points.x = this.tree2Add.x - 38;
        }
        this.tree2Points.y = this.tree2Add.y - 8;
        this.tree2Points.addEventListener(Event.ENTER_FRAME, this.onEnterFrameTree2Points);
        addChild(this.tree2Add);
        addChild(this.addBitmap);
        addChild(this.tree2Points);

        this.tree2Label_ = new SimpleText(24, 0xFFFFFF, false, 300);
        this.tree2Label_.setText("Teachings of the Wise");
        this.tree2Label_.x = this.tree2Add.x + 40;
        this.tree2Label_.y = this.tree2Add.y + 3;
        addChild(this.tree2Label_);

        this.tree2tick1_.graphics.beginFill(0x949494);
        this.tree2tick1_.graphics.drawRect(this.tree2Remove.x - 10 + 5, this.tree2Remove.y + 45, 20, 6); //30 x
        this.tree2tick1_.graphics.endFill();
        this.tree2tick1_.filters = [glowFilter];
        //this.tree2tick1_.addEventListener(Event.ENTER_FRAME, this.onEnterFrameTree2Tick1);
        addChild(tree2tick1_); //min

        this.tree2tick2_.graphics.beginFill(0x949494);
        this.tree2tick2_.graphics.drawRect(this.tree2Remove.x - 10 + 35, this.tree2Add.y + 45, 20, 6);
        this.tree2tick2_.graphics.endFill();
        this.tree2tick2_.filters = [glowFilter];
        addChild(tree2tick2_); //maj

        this.tree2tick3_.graphics.beginFill(0x949494);
        this.tree2tick3_.graphics.drawRect(this.tree2Remove.x - 10 + 65, this.tree2Add.y + 45, 20, 6);
        this.tree2tick3_.graphics.endFill();
        this.tree2tick3_.filters = [glowFilter];
        addChild(tree2tick3_); //maj

        this.tree2tick4_.graphics.beginFill(0x949494);
        this.tree2tick4_.graphics.drawCircle(this.tree2Remove.x - 10 + 102, this.tree2Add.y + 47, 7.5); //+37 x
        this.tree2tick4_.graphics.endFill();
        this.tree2tick4_.filters = [glowFilter];
        addChild(tree2tick4_); //Medium 1

        this.tree2tick5_.graphics.beginFill(0x949494);
        this.tree2tick5_.graphics.drawRect(this.tree2Remove.x - 10 + 120, this.tree2Remove.y + 45, 20, 6); //30 x
        this.tree2tick5_.graphics.endFill();
        this.tree2tick5_.filters = [glowFilter];
        addChild(tree2tick5_); //min

        this.tree2tick6_.graphics.beginFill(0x949494);
        this.tree2tick6_.graphics.drawRect(this.tree2Remove.x - 10 + 150, this.tree2Add.y + 45, 20, 6);
        this.tree2tick6_.graphics.endFill();
        this.tree2tick6_.filters = [glowFilter];
        addChild(tree2tick6_); //maj

        this.tree2tick7_.graphics.beginFill(0x949494);
        this.tree2tick7_.graphics.drawRect(this.tree2Remove.x - 10 + 180, this.tree2Add.y + 45, 20, 6);
        this.tree2tick7_.graphics.endFill();
        this.tree2tick7_.filters = [glowFilter];
        addChild(tree2tick7_); //maj

        this.tree2tick8_.graphics.beginFill(0x949494);
        this.tree2tick8_.graphics.drawCircle(this.tree2Remove.x - 10 + 217, this.tree2Add.y + 47, 7.5); //+37 x
        this.tree2tick8_.graphics.endFill();
        this.tree2tick8_.filters = [glowFilter];
        addChild(tree2tick8_); //Medium 2

        this.tree2tick9_.graphics.beginFill(0x949494);
        this.tree2tick9_.graphics.drawRect(this.tree2Remove.x - 10 + 235, this.tree2Remove.y + 45, 20, 6); //30 x
        this.tree2tick9_.graphics.endFill();
        this.tree2tick9_.filters = [glowFilter];
        addChild(tree2tick9_); //min

        this.tree2tick10_.graphics.beginFill(0x949494);
        this.tree2tick10_.graphics.drawRect(this.tree2Remove.x - 10 + 265, this.tree2Add.y + 45, 20, 6);
        this.tree2tick10_.graphics.endFill();
        this.tree2tick10_.filters = [glowFilter];
        addChild(tree2tick10_); //maj

        this.tree2tick11_.graphics.beginFill(0x949494);
        this.tree2tick11_.graphics.drawRect(this.tree2Remove.x - 10 + 295, this.tree2Add.y + 45, 20, 6);
        this.tree2tick11_.graphics.endFill();
        this.tree2tick11_.filters = [glowFilter];
        addChild(tree2tick11_); //maj

        this.tree2tick12_.graphics.beginFill(0x949494);
        this.tree2tick12_.graphics.drawCircle(this.tree2Remove.x - 10 + 336, this.tree2Add.y + 47, 12.5); //+37 x
        this.tree2tick12_.graphics.endFill();
        this.tree2tick12_.filters = [glowFilter];
        addChild(tree2tick12_); //Medium 2

        this.tree2tick13_.graphics.beginFill(0x949494);
        this.tree2tick13_.graphics.drawRect(this.tree2Remove.x - 10 + 357, this.tree2Remove.y + 45, 20, 6); //30 x
        this.tree2tick13_.graphics.endFill();
        this.tree2tick13_.filters = [glowFilter];
        addChild(tree2tick13_); //min

        this.tree2tick14_.graphics.beginFill(0x949494);
        this.tree2tick14_.graphics.drawRect(this.tree2Remove.x - 10 + 387, this.tree2Add.y + 45, 20, 6);
        this.tree2tick14_.graphics.endFill();
        this.tree2tick14_.filters = [glowFilter];
        addChild(tree2tick14_); //maj

        this.tree2tick15_.graphics.beginFill(0x949494);
        this.tree2tick15_.graphics.drawRect(this.tree2Remove.x - 10 + 417, this.tree2Add.y + 45, 20, 6);
        this.tree2tick15_.graphics.endFill();
        this.tree2tick15_.filters = [glowFilter];
        addChild(tree2tick15_); //maj

        this.tree2tick16_.graphics.beginFill(0x949494);
        this.tree2tick16_.graphics.drawCircle(this.tree2Remove.x - 10 + 454, this.tree2Add.y + 47, 7.5); //+37 x
        this.tree2tick16_.graphics.endFill();
        this.tree2tick16_.filters = [glowFilter];
        addChild(tree2tick16_); //Medium 1

        this.tree2tick17_.graphics.beginFill(0x949494);
        this.tree2tick17_.graphics.drawRect(this.tree2Remove.x - 10 + 472, this.tree2Remove.y + 45, 20, 6); //30 x
        this.tree2tick17_.graphics.endFill();
        this.tree2tick17_.filters = [glowFilter];
        addChild(tree2tick17_); //min

        this.tree2tick18_.graphics.beginFill(0x949494);
        this.tree2tick18_.graphics.drawRect(this.tree2Remove.x - 10 + 502, this.tree2Add.y + 45, 20, 6);
        this.tree2tick18_.graphics.endFill();
        this.tree2tick18_.filters = [glowFilter];
        addChild(tree2tick18_); //maj

        this.tree2tick19_.graphics.beginFill(0x949494);
        this.tree2tick19_.graphics.drawRect(this.tree2Remove.x - 10 + 532, this.tree2Add.y + 45, 20, 6);
        this.tree2tick19_.graphics.endFill();
        this.tree2tick19_.filters = [glowFilter];
        addChild(tree2tick19_); //maj

        this.tree2tick20_.graphics.beginFill(0x949494);
        this.tree2tick20_.graphics.drawCircle(this.tree2Remove.x - 10 + 573, this.tree2Add.y + 47, 12.5); //+37 x
        this.tree2tick20_.graphics.endFill();
        this.tree2tick20_.filters = [glowFilter];
        addChild(tree2tick20_); //big 2
        //endregion
        //region tree3
        this.tree3Remove = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "skilltree_box", 100));
        this.tree3Remove.scaleX = 0.25;
        this.tree3Remove.scaleY = 0.25;
        this.tree3Remove.x = 8;
        this.tree3Remove.y = 79 + 160;
        //this.tree3Add.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver1);
        //this.tree3Add.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut1);
        this.removeBitmap = new RemovePoint();
        this.removeBitmap.x = this.tree3Remove.x;
        this.removeBitmap.y = this.tree3Remove.y+1;
        addChild(this.tree3Remove);
        addChild(this.removeBitmap);

        this.tree3Add = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "skilltree_box", 100));
        this.tree3Add.scaleX = 0.25;
        this.tree3Add.scaleY = 0.25;
        this.tree3Add.x = 86;
        this.tree3Add.y = 79 + 160;
        //this.tree3Add.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver2);
        //this.tree3Add.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut2);
        this.addBitmap = new AddPoint();
        this.addBitmap.x = this.tree3Add.x;
        this.addBitmap.y = this.tree3Add.y+1;
        this.tree3Points = new SimpleText(32, 0xFFFFFF).setBold(true).setText(String((this.player.node3TickMaj)+(this.player.node3Med*5)+(this.player.node3TickMin)+(this.player.node3Big*10)));
        if ((this.player.node3TickMaj)+(this.player.node3Med*5)+(this.player.node3TickMin)+(this.player.node3Big*10) > 9) {
            this.tree3Points.x = this.tree3Add.x - 48;
        } else {
            this.tree3Points.x = this.tree3Add.x - 38;
        }
        this.tree3Points.y = this.tree3Add.y - 8;
        this.tree3Points.addEventListener(Event.ENTER_FRAME, this.onEnterFrameTree3Points);
        addChild(this.tree3Add);
        addChild(this.addBitmap);
        addChild(this.tree3Points);

        this.tree3Label_ = new SimpleText(24, 0xFFFFFF, false, 300);
        this.tree3Label_.setText("Noble Defender");
        this.tree3Label_.x = this.tree3Add.x + 40;
        this.tree3Label_.y = this.tree3Add.y + 3;
        addChild(this.tree3Label_);

        this.tree3tick1_.graphics.beginFill(0x949494);
        this.tree3tick1_.graphics.drawRect(this.tree3Remove.x - 10 + 5, this.tree3Remove.y + 45, 20, 6); //30 x
        this.tree3tick1_.graphics.endFill();
        this.tree3tick1_.filters = [glowFilter];
        //this.tree3tick1_.addEventListener(Event.ENTER_FRAME, this.onEnterFrameTree3Tick1);
        addChild(tree3tick1_); //min

        this.tree3tick2_.graphics.beginFill(0x949494);
        this.tree3tick2_.graphics.drawRect(this.tree3Remove.x - 10 + 35, this.tree3Add.y + 45, 20, 6);
        this.tree3tick2_.graphics.endFill();
        this.tree3tick2_.filters = [glowFilter];
        addChild(tree3tick2_); //maj

        this.tree3tick3_.graphics.beginFill(0x949494);
        this.tree3tick3_.graphics.drawRect(this.tree3Remove.x - 10 + 65, this.tree3Add.y + 45, 20, 6);
        this.tree3tick3_.graphics.endFill();
        this.tree3tick3_.filters = [glowFilter];
        addChild(tree3tick3_); //maj

        this.tree3tick4_.graphics.beginFill(0x949494);
        this.tree3tick4_.graphics.drawCircle(this.tree3Remove.x - 10 + 102, this.tree3Add.y + 47, 7.5); //+37 x
        this.tree3tick4_.graphics.endFill();
        this.tree3tick4_.filters = [glowFilter];
        addChild(tree3tick4_); //Medium 1

        this.tree3tick5_.graphics.beginFill(0x949494);
        this.tree3tick5_.graphics.drawRect(this.tree3Remove.x - 10 + 120, this.tree3Remove.y + 45, 20, 6); //30 x
        this.tree3tick5_.graphics.endFill();
        this.tree3tick5_.filters = [glowFilter];
        addChild(tree3tick5_); //min

        this.tree3tick6_.graphics.beginFill(0x949494);
        this.tree3tick6_.graphics.drawRect(this.tree3Remove.x - 10 + 150, this.tree3Add.y + 45, 20, 6);
        this.tree3tick6_.graphics.endFill();
        this.tree3tick6_.filters = [glowFilter];
        addChild(tree3tick6_); //maj

        this.tree3tick7_.graphics.beginFill(0x949494);
        this.tree3tick7_.graphics.drawRect(this.tree3Remove.x - 10 + 180, this.tree3Add.y + 45, 20, 6);
        this.tree3tick7_.graphics.endFill();
        this.tree3tick7_.filters = [glowFilter];
        addChild(tree3tick7_); //maj

        this.tree3tick8_.graphics.beginFill(0x949494);
        this.tree3tick8_.graphics.drawCircle(this.tree3Remove.x - 10 + 217, this.tree3Add.y + 47, 7.5); //+37 x
        this.tree3tick8_.graphics.endFill();
        this.tree3tick8_.filters = [glowFilter];
        addChild(tree3tick8_); //Medium 2

        this.tree3tick9_.graphics.beginFill(0x949494);
        this.tree3tick9_.graphics.drawRect(this.tree3Remove.x - 10 + 235, this.tree3Remove.y + 45, 20, 6); //30 x
        this.tree3tick9_.graphics.endFill();
        this.tree3tick9_.filters = [glowFilter];
        addChild(tree3tick9_); //min

        this.tree3tick10_.graphics.beginFill(0x949494);
        this.tree3tick10_.graphics.drawRect(this.tree3Remove.x - 10 + 265, this.tree3Add.y + 45, 20, 6);
        this.tree3tick10_.graphics.endFill();
        this.tree3tick10_.filters = [glowFilter];
        addChild(tree3tick10_); //maj

        this.tree3tick11_.graphics.beginFill(0x949494);
        this.tree3tick11_.graphics.drawRect(this.tree3Remove.x - 10 + 295, this.tree3Add.y + 45, 20, 6);
        this.tree3tick11_.graphics.endFill();
        this.tree3tick11_.filters = [glowFilter];
        addChild(tree3tick11_); //maj

        this.tree3tick12_.graphics.beginFill(0x949494);
        this.tree3tick12_.graphics.drawCircle(this.tree3Remove.x - 10 + 336, this.tree3Add.y + 47, 12.5); //+37 x
        this.tree3tick12_.graphics.endFill();
        this.tree3tick12_.filters = [glowFilter];
        addChild(tree3tick12_); //Medium 2

        this.tree3tick13_.graphics.beginFill(0x949494);
        this.tree3tick13_.graphics.drawRect(this.tree3Remove.x - 10 + 357, this.tree3Remove.y + 45, 20, 6); //30 x
        this.tree3tick13_.graphics.endFill();
        this.tree3tick13_.filters = [glowFilter];
        addChild(tree3tick13_); //min

        this.tree3tick14_.graphics.beginFill(0x949494);
        this.tree3tick14_.graphics.drawRect(this.tree3Remove.x - 10 + 387, this.tree3Add.y + 45, 20, 6);
        this.tree3tick14_.graphics.endFill();
        this.tree3tick14_.filters = [glowFilter];
        addChild(tree3tick14_); //maj

        this.tree3tick15_.graphics.beginFill(0x949494);
        this.tree3tick15_.graphics.drawRect(this.tree3Remove.x - 10 + 417, this.tree3Add.y + 45, 20, 6);
        this.tree3tick15_.graphics.endFill();
        this.tree3tick15_.filters = [glowFilter];
        addChild(tree3tick15_); //maj

        this.tree3tick16_.graphics.beginFill(0x949494);
        this.tree3tick16_.graphics.drawCircle(this.tree3Remove.x - 10 + 454, this.tree3Add.y + 47, 7.5); //+37 x
        this.tree3tick16_.graphics.endFill();
        this.tree3tick16_.filters = [glowFilter];
        addChild(tree3tick16_); //Medium 1

        this.tree3tick17_.graphics.beginFill(0x949494);
        this.tree3tick17_.graphics.drawRect(this.tree3Remove.x - 10 + 472, this.tree3Remove.y + 45, 20, 6); //30 x
        this.tree3tick17_.graphics.endFill();
        this.tree3tick17_.filters = [glowFilter];
        addChild(tree3tick17_); //min

        this.tree3tick18_.graphics.beginFill(0x949494);
        this.tree3tick18_.graphics.drawRect(this.tree3Remove.x - 10 + 502, this.tree3Add.y + 45, 20, 6);
        this.tree3tick18_.graphics.endFill();
        this.tree3tick18_.filters = [glowFilter];
        addChild(tree3tick18_); //maj

        this.tree3tick19_.graphics.beginFill(0x949494);
        this.tree3tick19_.graphics.drawRect(this.tree3Remove.x - 10 + 532, this.tree3Add.y + 45, 20, 6);
        this.tree3tick19_.graphics.endFill();
        this.tree3tick19_.filters = [glowFilter];
        addChild(tree3tick19_); //maj

        this.tree3tick20_.graphics.beginFill(0x949494);
        this.tree3tick20_.graphics.drawCircle(this.tree3Remove.x - 10 + 573, this.tree3Add.y + 47, 12.5); //+37 x
        this.tree3tick20_.graphics.endFill();
        this.tree3tick20_.filters = [glowFilter];
        addChild(tree3tick20_); //big 2
        //endregion
        //region tree4
        this.tree4Remove = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "skilltree_box", 100));
        this.tree4Remove.scaleX = 0.25;
        this.tree4Remove.scaleY = 0.25;
        this.tree4Remove.x = 8;
        this.tree4Remove.y = 79 + 240;
        //this.tree4Add.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver1);
        //this.tree4Add.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut1);
        this.removeBitmap = new RemovePoint();
        this.removeBitmap.x = this.tree4Remove.x;
        this.removeBitmap.y = this.tree4Remove.y+1;
        addChild(this.tree4Remove);
        addChild(this.removeBitmap);

        this.tree4Add = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "skilltree_box", 100));
        this.tree4Add.scaleX = 0.25;
        this.tree4Add.scaleY = 0.25;
        this.tree4Add.x = 86;
        this.tree4Add.y = 79 + 240;
        //this.tree4Add.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver2);
        //this.tree4Add.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut2);
        this.addBitmap = new AddPoint();
        this.addBitmap.x = this.tree4Add.x;
        this.addBitmap.y = this.tree4Add.y+1;
        this.tree4Points = new SimpleText(32, 0xFFFFFF).setBold(true).setText(String((this.player.node4TickMaj)+(this.player.node4Med*5)+(this.player.node4TickMin)+(this.player.node4Big*10)));
        if ((this.player.node4TickMaj)+(this.player.node4Med*5)+(this.player.node4TickMin)+(this.player.node4Big*10) > 9) {
            this.tree4Points.x = this.tree4Add.x - 48;
        } else {
            this.tree4Points.x = this.tree4Add.x - 38;
        }
        this.tree4Points.y = this.tree4Add.y - 8;
        this.tree4Points.addEventListener(Event.ENTER_FRAME, this.onEnterFrameTree4Points);
        addChild(this.tree4Add);
        addChild(this.addBitmap);
        addChild(this.tree4Points);

        this.tree4Label_ = new SimpleText(24, 0xFFFFFF, false, 300);
        this.tree4Label_.setText("Swift Assassin");
        this.tree4Label_.x = this.tree4Add.x + 40;
        this.tree4Label_.y = this.tree4Add.y + 3;
        addChild(this.tree4Label_);

        this.tree4tick1_.graphics.beginFill(0x949494);
        this.tree4tick1_.graphics.drawRect(this.tree4Remove.x - 10 + 5, this.tree4Remove.y + 45, 20, 6); //30 x
        this.tree4tick1_.graphics.endFill();
        this.tree4tick1_.filters = [glowFilter];
        //this.tree4tick1_.addEventListener(Event.ENTER_FRAME, this.onEnterFrameTree4Tick1);
        addChild(tree4tick1_); //min

        this.tree4tick2_.graphics.beginFill(0x949494);
        this.tree4tick2_.graphics.drawRect(this.tree4Remove.x - 10 + 35, this.tree4Add.y + 45, 20, 6);
        this.tree4tick2_.graphics.endFill();
        this.tree4tick2_.filters = [glowFilter];
        addChild(tree4tick2_); //maj

        this.tree4tick3_.graphics.beginFill(0x949494);
        this.tree4tick3_.graphics.drawRect(this.tree4Remove.x - 10 + 65, this.tree4Add.y + 45, 20, 6);
        this.tree4tick3_.graphics.endFill();
        this.tree4tick3_.filters = [glowFilter];
        addChild(tree4tick3_); //maj

        this.tree4tick4_.graphics.beginFill(0x949494);
        this.tree4tick4_.graphics.drawCircle(this.tree4Remove.x - 10 + 102, this.tree4Add.y + 47, 7.5); //+37 x
        this.tree4tick4_.graphics.endFill();
        this.tree4tick4_.filters = [glowFilter];
        addChild(tree4tick4_); //Medium 1

        this.tree4tick5_.graphics.beginFill(0x949494);
        this.tree4tick5_.graphics.drawRect(this.tree4Remove.x - 10 + 120, this.tree4Remove.y + 45, 20, 6); //30 x
        this.tree4tick5_.graphics.endFill();
        this.tree4tick5_.filters = [glowFilter];
        addChild(tree4tick5_); //min

        this.tree4tick6_.graphics.beginFill(0x949494);
        this.tree4tick6_.graphics.drawRect(this.tree4Remove.x - 10 + 150, this.tree4Add.y + 45, 20, 6);
        this.tree4tick6_.graphics.endFill();
        this.tree4tick6_.filters = [glowFilter];
        addChild(tree4tick6_); //maj

        this.tree4tick7_.graphics.beginFill(0x949494);
        this.tree4tick7_.graphics.drawRect(this.tree4Remove.x - 10 + 180, this.tree4Add.y + 45, 20, 6);
        this.tree4tick7_.graphics.endFill();
        this.tree4tick7_.filters = [glowFilter];
        addChild(tree4tick7_); //maj

        this.tree4tick8_.graphics.beginFill(0x949494);
        this.tree4tick8_.graphics.drawCircle(this.tree4Remove.x - 10 + 217, this.tree4Add.y + 47, 7.5); //+37 x
        this.tree4tick8_.graphics.endFill();
        this.tree4tick8_.filters = [glowFilter];
        addChild(tree4tick8_); //Medium 2

        this.tree4tick9_.graphics.beginFill(0x949494);
        this.tree4tick9_.graphics.drawRect(this.tree4Remove.x - 10 + 235, this.tree4Remove.y + 45, 20, 6); //30 x
        this.tree4tick9_.graphics.endFill();
        this.tree4tick9_.filters = [glowFilter];
        addChild(tree4tick9_); //min

        this.tree4tick10_.graphics.beginFill(0x949494);
        this.tree4tick10_.graphics.drawRect(this.tree4Remove.x - 10 + 265, this.tree4Add.y + 45, 20, 6);
        this.tree4tick10_.graphics.endFill();
        this.tree4tick10_.filters = [glowFilter];
        addChild(tree4tick10_); //maj

        this.tree4tick11_.graphics.beginFill(0x949494);
        this.tree4tick11_.graphics.drawRect(this.tree4Remove.x - 10 + 295, this.tree4Add.y + 45, 20, 6);
        this.tree4tick11_.graphics.endFill();
        this.tree4tick11_.filters = [glowFilter];
        addChild(tree4tick11_); //maj

        this.tree4tick12_.graphics.beginFill(0x949494);
        this.tree4tick12_.graphics.drawCircle(this.tree4Remove.x - 10 + 336, this.tree4Add.y + 47, 12.5); //+37 x
        this.tree4tick12_.graphics.endFill();
        this.tree4tick12_.filters = [glowFilter];
        addChild(tree4tick12_); //Medium 2

        this.tree4tick13_.graphics.beginFill(0x949494);
        this.tree4tick13_.graphics.drawRect(this.tree4Remove.x - 10 + 357, this.tree4Remove.y + 45, 20, 6); //30 x
        this.tree4tick13_.graphics.endFill();
        this.tree4tick13_.filters = [glowFilter];
        addChild(tree4tick13_); //min

        this.tree4tick14_.graphics.beginFill(0x949494);
        this.tree4tick14_.graphics.drawRect(this.tree4Remove.x - 10 + 387, this.tree4Add.y + 45, 20, 6);
        this.tree4tick14_.graphics.endFill();
        this.tree4tick14_.filters = [glowFilter];
        addChild(tree4tick14_); //maj

        this.tree4tick15_.graphics.beginFill(0x949494);
        this.tree4tick15_.graphics.drawRect(this.tree4Remove.x - 10 + 417, this.tree4Add.y + 45, 20, 6);
        this.tree4tick15_.graphics.endFill();
        this.tree4tick15_.filters = [glowFilter];
        addChild(tree4tick15_); //maj

        this.tree4tick16_.graphics.beginFill(0x949494);
        this.tree4tick16_.graphics.drawCircle(this.tree4Remove.x - 10 + 454, this.tree4Add.y + 47, 7.5); //+37 x
        this.tree4tick16_.graphics.endFill();
        this.tree4tick16_.filters = [glowFilter];
        addChild(tree4tick16_); //Medium 1

        this.tree4tick17_.graphics.beginFill(0x949494);
        this.tree4tick17_.graphics.drawRect(this.tree4Remove.x - 10 + 472, this.tree4Remove.y + 45, 20, 6); //30 x
        this.tree4tick17_.graphics.endFill();
        this.tree4tick17_.filters = [glowFilter];
        addChild(tree4tick17_); //min

        this.tree4tick18_.graphics.beginFill(0x949494);
        this.tree4tick18_.graphics.drawRect(this.tree4Remove.x - 10 + 502, this.tree4Add.y + 45, 20, 6);
        this.tree4tick18_.graphics.endFill();
        this.tree4tick18_.filters = [glowFilter];
        addChild(tree4tick18_); //maj

        this.tree4tick19_.graphics.beginFill(0x949494);
        this.tree4tick19_.graphics.drawRect(this.tree4Remove.x - 10 + 532, this.tree4Add.y + 45, 20, 6);
        this.tree4tick19_.graphics.endFill();
        this.tree4tick19_.filters = [glowFilter];
        addChild(tree4tick19_); //maj

        this.tree4tick20_.graphics.beginFill(0x949494);
        this.tree4tick20_.graphics.drawCircle(this.tree4Remove.x - 10 + 573, this.tree4Add.y + 47, 12.5); //+37 x
        this.tree4tick20_.graphics.endFill();
        this.tree4tick20_.filters = [glowFilter];
        addChild(tree4tick20_); //big 2
        //endregion
        //region tree5
        this.tree5Remove = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "skilltree_box", 100));
        this.tree5Remove.scaleX = 0.25;
        this.tree5Remove.scaleY = 0.25;
        this.tree5Remove.x = 8;
        this.tree5Remove.y = 79 + 320;
        //this.tree5Add.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver1);
        //this.tree5Add.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut1);
        this.removeBitmap = new RemovePoint();
        this.removeBitmap.x = this.tree5Remove.x;
        this.removeBitmap.y = this.tree5Remove.y+1;
        addChild(this.tree5Remove);
        addChild(this.removeBitmap);

        this.tree5Add = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "skilltree_box", 100));
        this.tree5Add.scaleX = 0.25;
        this.tree5Add.scaleY = 0.25;
        this.tree5Add.x = 86;
        this.tree5Add.y = 79 + 320;
        //this.tree5Add.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver2);
        //this.tree5Add.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut2);
        this.addBitmap = new AddPoint();
        this.addBitmap.x = this.tree5Add.x;
        this.addBitmap.y = this.tree5Add.y+1;
        this.tree5Points = new SimpleText(32, 0xFFFFFF).setBold(true).setText(String((this.player.node5TickMaj)+(this.player.node5Med*5)+(this.player.node5TickMin)+(this.player.node5Big*10)));
        if ((this.player.node5TickMaj)+(this.player.node5Med*5)+(this.player.node5TickMin)+(this.player.node5Big*10) > 9) {
            this.tree5Points.x = this.tree5Add.x - 48;
        } else {
            this.tree5Points.x = this.tree5Add.x - 38;
        }
        this.tree5Points.y = this.tree5Add.y - 8;
        this.tree5Points.addEventListener(Event.ENTER_FRAME, this.onEnterFrameTree5Points);
        addChild(this.tree5Add);
        addChild(this.addBitmap);
        addChild(this.tree5Points);

        this.tree5Label_ = new SimpleText(24, 0xFFFFFF, false, 300);
        this.tree5Label_.setText("Luck of the Irish");
        this.tree5Label_.x = this.tree5Add.x + 40;
        this.tree5Label_.y = this.tree5Add.y + 3;
        addChild(this.tree5Label_);

        this.tree5tick1_.graphics.beginFill(0x949494);
        this.tree5tick1_.graphics.drawRect(this.tree5Remove.x - 10 + 5, this.tree5Remove.y + 45, 20, 6); //30 x
        this.tree5tick1_.graphics.endFill();
        this.tree5tick1_.filters = [glowFilter];
        //this.tree5tick1_.addEventListener(Event.ENTER_FRAME, this.onEnterFrameTree5Tick1);
        addChild(tree5tick1_); //min

        this.tree5tick2_.graphics.beginFill(0x949494);
        this.tree5tick2_.graphics.drawRect(this.tree5Remove.x - 10 + 35, this.tree5Add.y + 45, 20, 6);
        this.tree5tick2_.graphics.endFill();
        this.tree5tick2_.filters = [glowFilter];
        addChild(tree5tick2_); //maj

        this.tree5tick3_.graphics.beginFill(0x949494);
        this.tree5tick3_.graphics.drawRect(this.tree5Remove.x - 10 + 65, this.tree5Add.y + 45, 20, 6);
        this.tree5tick3_.graphics.endFill();
        this.tree5tick3_.filters = [glowFilter];
        addChild(tree5tick3_); //maj

        this.tree5tick4_.graphics.beginFill(0x949494);
        this.tree5tick4_.graphics.drawCircle(this.tree5Remove.x - 10 + 102, this.tree5Add.y + 47, 7.5); //+37 x
        this.tree5tick4_.graphics.endFill();
        this.tree5tick4_.filters = [glowFilter];
        addChild(tree5tick4_); //Medium 1

        this.tree5tick5_.graphics.beginFill(0x949494);
        this.tree5tick5_.graphics.drawRect(this.tree5Remove.x - 10 + 120, this.tree5Remove.y + 45, 20, 6); //30 x
        this.tree5tick5_.graphics.endFill();
        this.tree5tick5_.filters = [glowFilter];
        addChild(tree5tick5_); //min

        this.tree5tick6_.graphics.beginFill(0x949494);
        this.tree5tick6_.graphics.drawRect(this.tree5Remove.x - 10 + 150, this.tree5Add.y + 45, 20, 6);
        this.tree5tick6_.graphics.endFill();
        this.tree5tick6_.filters = [glowFilter];
        addChild(tree5tick6_); //maj

        this.tree5tick7_.graphics.beginFill(0x949494);
        this.tree5tick7_.graphics.drawRect(this.tree5Remove.x - 10 + 180, this.tree5Add.y + 45, 20, 6);
        this.tree5tick7_.graphics.endFill();
        this.tree5tick7_.filters = [glowFilter];
        addChild(tree5tick7_); //maj

        this.tree5tick8_.graphics.beginFill(0x949494);
        this.tree5tick8_.graphics.drawCircle(this.tree5Remove.x - 10 + 217, this.tree5Add.y + 47, 7.5); //+37 x
        this.tree5tick8_.graphics.endFill();
        this.tree5tick8_.filters = [glowFilter];
        addChild(tree5tick8_); //Medium 2

        this.tree5tick9_.graphics.beginFill(0x949494);
        this.tree5tick9_.graphics.drawRect(this.tree5Remove.x - 10 + 235, this.tree5Remove.y + 45, 20, 6); //30 x
        this.tree5tick9_.graphics.endFill();
        this.tree5tick9_.filters = [glowFilter];
        addChild(tree5tick9_); //min

        this.tree5tick10_.graphics.beginFill(0x949494);
        this.tree5tick10_.graphics.drawRect(this.tree5Remove.x - 10 + 265, this.tree5Add.y + 45, 20, 6);
        this.tree5tick10_.graphics.endFill();
        this.tree5tick10_.filters = [glowFilter];
        addChild(tree5tick10_); //maj

        this.tree5tick11_.graphics.beginFill(0x949494);
        this.tree5tick11_.graphics.drawRect(this.tree5Remove.x - 10 + 295, this.tree5Add.y + 45, 20, 6);
        this.tree5tick11_.graphics.endFill();
        this.tree5tick11_.filters = [glowFilter];
        addChild(tree5tick11_); //maj

        this.tree5tick12_.graphics.beginFill(0x949494);
        this.tree5tick12_.graphics.drawCircle(this.tree5Remove.x - 10 + 336, this.tree5Add.y + 47, 12.5); //+37 x
        this.tree5tick12_.graphics.endFill();
        this.tree5tick12_.filters = [glowFilter];
        addChild(tree5tick12_); //Medium 2

        this.tree5tick13_.graphics.beginFill(0x949494);
        this.tree5tick13_.graphics.drawRect(this.tree5Remove.x - 10 + 357, this.tree5Remove.y + 45, 20, 6); //30 x
        this.tree5tick13_.graphics.endFill();
        this.tree5tick13_.filters = [glowFilter];
        addChild(tree5tick13_); //min

        this.tree5tick14_.graphics.beginFill(0x949494);
        this.tree5tick14_.graphics.drawRect(this.tree5Remove.x - 10 + 387, this.tree5Add.y + 45, 20, 6);
        this.tree5tick14_.graphics.endFill();
        this.tree5tick14_.filters = [glowFilter];
        addChild(tree5tick14_); //maj

        this.tree5tick15_.graphics.beginFill(0x949494);
        this.tree5tick15_.graphics.drawRect(this.tree5Remove.x - 10 + 417, this.tree5Add.y + 45, 20, 6);
        this.tree5tick15_.graphics.endFill();
        this.tree5tick15_.filters = [glowFilter];
        addChild(tree5tick15_); //maj

        this.tree5tick16_.graphics.beginFill(0x949494);
        this.tree5tick16_.graphics.drawCircle(this.tree5Remove.x - 10 + 454, this.tree5Add.y + 47, 7.5); //+37 x
        this.tree5tick16_.graphics.endFill();
        this.tree5tick16_.filters = [glowFilter];
        addChild(tree5tick16_); //Medium 1

        this.tree5tick17_.graphics.beginFill(0x949494);
        this.tree5tick17_.graphics.drawRect(this.tree5Remove.x - 10 + 472, this.tree5Remove.y + 45, 20, 6); //30 x
        this.tree5tick17_.graphics.endFill();
        this.tree5tick17_.filters = [glowFilter];
        addChild(tree5tick17_); //min

        this.tree5tick18_.graphics.beginFill(0x949494);
        this.tree5tick18_.graphics.drawRect(this.tree5Remove.x - 10 + 502, this.tree5Add.y + 45, 20, 6);
        this.tree5tick18_.graphics.endFill();
        this.tree5tick18_.filters = [glowFilter];
        addChild(tree5tick18_); //maj

        this.tree5tick19_.graphics.beginFill(0x949494);
        this.tree5tick19_.graphics.drawRect(this.tree5Remove.x - 10 + 532, this.tree5Add.y + 45, 20, 6);
        this.tree5tick19_.graphics.endFill();
        this.tree5tick19_.filters = [glowFilter];
        addChild(tree5tick19_); //maj

        this.tree5tick20_.graphics.beginFill(0x949494);
        this.tree5tick20_.graphics.drawCircle(this.tree5Remove.x - 10 + 573, this.tree5Add.y + 47, 12.5); //+37 x
        this.tree5tick20_.graphics.endFill();
        this.tree5tick20_.filters = [glowFilter];
        addChild(tree5tick20_); //big 2
        //endregion

        this.tree1tick1_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver1Tick1);
        this.tree1tick2_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver1Tick2);
        this.tree1tick3_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver1Tick2);
        this.tree1tick4_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver1Tick3);
        this.tree1tick5_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver1Tick1);
        this.tree1tick6_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver1Tick2);
        this.tree1tick7_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver1Tick2);
        this.tree1tick8_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver1Tick4);
        this.tree1tick9_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver1Tick1);
        this.tree1tick10_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver1Tick2);
        this.tree1tick11_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver1Tick2);
        this.tree1tick12_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver1Tick5);
        this.tree1tick13_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver1Tick1);
        this.tree1tick14_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver1Tick2);
        this.tree1tick15_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver1Tick2);
        this.tree1tick16_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver1Tick3);
        this.tree1tick17_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver1Tick1);
        this.tree1tick18_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver1Tick2);
        this.tree1tick19_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver1Tick2);
        this.tree1tick20_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver1Tick6);
        this.tree1tick1_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut1Tick1);
        this.tree1tick2_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut1Tick2);
        this.tree1tick3_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut1Tick2);
        this.tree1tick4_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut1Tick3);
        this.tree1tick5_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut1Tick1);
        this.tree1tick6_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut1Tick2);
        this.tree1tick7_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut1Tick2);
        this.tree1tick8_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut1Tick4);
        this.tree1tick9_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut1Tick1);
        this.tree1tick10_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut1Tick2);
        this.tree1tick11_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut1Tick2);
        this.tree1tick12_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut1Tick5);
        this.tree1tick13_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut1Tick1);
        this.tree1tick14_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut1Tick2);
        this.tree1tick15_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut1Tick2);
        this.tree1tick16_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut1Tick3);
        this.tree1tick17_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut1Tick1);
        this.tree1tick18_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut1Tick2);
        this.tree1tick19_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut1Tick2);
        this.tree1tick20_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut1Tick6);

        this.tree2tick1_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver2Tick1);
        this.tree2tick2_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver2Tick2);
        this.tree2tick3_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver2Tick2);
        this.tree2tick4_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver2Tick3);
        this.tree2tick5_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver2Tick1);
        this.tree2tick6_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver2Tick2);
        this.tree2tick7_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver2Tick2);
        this.tree2tick8_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver2Tick4);
        this.tree2tick9_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver2Tick1);
        this.tree2tick10_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver2Tick2);
        this.tree2tick11_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver2Tick2);
        this.tree2tick12_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver2Tick5);
        this.tree2tick13_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver2Tick1);
        this.tree2tick14_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver2Tick2);
        this.tree2tick15_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver2Tick2);
        this.tree2tick16_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver2Tick3);
        this.tree2tick17_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver2Tick1);
        this.tree2tick18_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver2Tick2);
        this.tree2tick19_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver2Tick2);
        this.tree2tick20_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver2Tick6);
        this.tree2tick1_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut2Tick1);
        this.tree2tick2_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut2Tick2);
        this.tree2tick3_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut2Tick2);
        this.tree2tick4_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut2Tick3);
        this.tree2tick5_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut2Tick1);
        this.tree2tick6_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut2Tick2);
        this.tree2tick7_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut2Tick2);
        this.tree2tick8_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut2Tick4);
        this.tree2tick9_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut2Tick1);
        this.tree2tick10_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut2Tick2);
        this.tree2tick11_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut2Tick2);
        this.tree2tick12_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut2Tick5);
        this.tree2tick13_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut2Tick1);
        this.tree2tick14_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut2Tick2);
        this.tree2tick15_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut2Tick2);
        this.tree2tick16_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut2Tick3);
        this.tree2tick17_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut2Tick1);
        this.tree2tick18_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut2Tick2);
        this.tree2tick19_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut2Tick2);
        this.tree2tick20_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut2Tick6);

        this.tree3tick1_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver3Tick1);
        this.tree3tick2_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver3Tick2);
        this.tree3tick3_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver3Tick2);
        this.tree3tick4_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver3Tick3);
        this.tree3tick5_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver3Tick1);
        this.tree3tick6_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver3Tick2);
        this.tree3tick7_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver3Tick2);
        this.tree3tick8_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver3Tick4);
        this.tree3tick9_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver3Tick1);
        this.tree3tick10_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver3Tick2);
        this.tree3tick11_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver3Tick2);
        this.tree3tick12_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver3Tick5);
        this.tree3tick13_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver3Tick1);
        this.tree3tick14_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver3Tick2);
        this.tree3tick15_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver3Tick2);
        this.tree3tick16_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver3Tick3);
        this.tree3tick17_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver3Tick1);
        this.tree3tick18_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver3Tick2);
        this.tree3tick19_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver3Tick2);
        this.tree3tick20_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver3Tick6);
        this.tree3tick1_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut3Tick1);
        this.tree3tick2_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut3Tick2);
        this.tree3tick3_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut3Tick2);
        this.tree3tick4_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut3Tick3);
        this.tree3tick5_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut3Tick1);
        this.tree3tick6_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut3Tick2);
        this.tree3tick7_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut3Tick2);
        this.tree3tick8_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut3Tick4);
        this.tree3tick9_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut3Tick1);
        this.tree3tick10_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut3Tick2);
        this.tree3tick11_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut3Tick2);
        this.tree3tick12_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut3Tick5);
        this.tree3tick13_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut3Tick1);
        this.tree3tick14_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut3Tick2);
        this.tree3tick15_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut3Tick2);
        this.tree3tick16_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut3Tick3);
        this.tree3tick17_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut3Tick1);
        this.tree3tick18_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut3Tick2);
        this.tree3tick19_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut3Tick2);
        this.tree3tick20_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut3Tick6);

        this.tree4tick1_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver4Tick1);
        this.tree4tick2_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver4Tick2);
        this.tree4tick3_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver4Tick2);
        this.tree4tick4_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver4Tick3);
        this.tree4tick5_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver4Tick1);
        this.tree4tick6_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver4Tick2);
        this.tree4tick7_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver4Tick2);
        this.tree4tick8_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver4Tick4);
        this.tree4tick9_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver4Tick1);
        this.tree4tick10_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver4Tick2);
        this.tree4tick11_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver4Tick2);
        this.tree4tick12_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver4Tick5);
        this.tree4tick13_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver4Tick1);
        this.tree4tick14_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver4Tick2);
        this.tree4tick15_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver4Tick2);
        this.tree4tick16_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver4Tick3);
        this.tree4tick17_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver4Tick1);
        this.tree4tick18_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver4Tick2);
        this.tree4tick19_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver4Tick2);
        this.tree4tick20_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver4Tick6);
        this.tree4tick1_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut4Tick1);
        this.tree4tick2_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut4Tick2);
        this.tree4tick3_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut4Tick2);
        this.tree4tick4_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut4Tick3);
        this.tree4tick5_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut4Tick1);
        this.tree4tick6_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut4Tick2);
        this.tree4tick7_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut4Tick2);
        this.tree4tick8_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut4Tick4);
        this.tree4tick9_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut4Tick1);
        this.tree4tick10_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut4Tick2);
        this.tree4tick11_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut4Tick2);
        this.tree4tick12_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut4Tick5);
        this.tree4tick13_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut4Tick1);
        this.tree4tick14_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut4Tick2);
        this.tree4tick15_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut4Tick2);
        this.tree4tick16_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut4Tick3);
        this.tree4tick17_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut4Tick1);
        this.tree4tick18_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut4Tick2);
        this.tree4tick19_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut4Tick2);
        this.tree4tick20_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut4Tick6);

        this.tree5tick1_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver5Tick1);
        this.tree5tick2_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver5Tick2);
        this.tree5tick3_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver5Tick2);
        this.tree5tick4_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver5Tick3);
        this.tree5tick5_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver5Tick1);
        this.tree5tick6_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver5Tick2);
        this.tree5tick7_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver5Tick2);
        this.tree5tick8_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver5Tick4);
        this.tree5tick9_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver5Tick1);
        this.tree5tick10_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver5Tick2);
        this.tree5tick11_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver5Tick2);
        this.tree5tick12_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver5Tick5);
        this.tree5tick13_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver5Tick1);
        this.tree5tick14_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver5Tick2);
        this.tree5tick15_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver5Tick2);
        this.tree5tick16_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver5Tick3);
        this.tree5tick17_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver5Tick1);
        this.tree5tick18_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver5Tick2);
        this.tree5tick19_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver5Tick2);
        this.tree5tick20_.addEventListener(MouseEvent.ROLL_OVER, this.onRollOver5Tick6);
        this.tree5tick1_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut5Tick1);
        this.tree5tick2_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut5Tick2);
        this.tree5tick3_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut5Tick2);
        this.tree5tick4_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut5Tick3);
        this.tree5tick5_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut5Tick1);
        this.tree5tick6_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut5Tick2);
        this.tree5tick7_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut5Tick2);
        this.tree5tick8_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut5Tick4);
        this.tree5tick9_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut5Tick1);
        this.tree5tick10_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut5Tick2);
        this.tree5tick11_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut5Tick2);
        this.tree5tick12_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut5Tick5);
        this.tree5tick13_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut5Tick1);
        this.tree5tick14_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut5Tick2);
        this.tree5tick15_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut5Tick2);
        this.tree5tick16_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut5Tick3);
        this.tree5tick17_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut5Tick1);
        this.tree5tick18_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut5Tick2);
        this.tree5tick19_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut5Tick2);
        this.tree5tick20_.addEventListener(MouseEvent.ROLL_OUT, this.onRollOut5Tick6);

    }

    private function reDrawRect(sprite:Sprite, x:Number, y:Number, width:Number = 20, height:Number = 6, fillColor:int = 0, circle:Boolean = false):Sprite
    {
        if (!sprite) return null;

        const g:Graphics = sprite.graphics;

        if(!circle) {
            g.clear();
            g.beginFill(fillColor);
            g.drawRect(x, y, width, height);
            g.endFill();
        } else if(circle) {
            g.clear();
            g.beginFill(fillColor);
            g.drawCircle(x, y, width);
            g.endFill();
        }

        return sprite;
    }


    public function onEnterFrameTree2Points(arg1:Event):void
    {
        var local1:int = (this.player.node2TickMaj)+(this.player.node2Med*5)+(this.player.node2TickMin)+(this.player.node2Big*10);
        if(this.tree2Points != null){
            if(this.tree2Points.parent != null){
                this.tree2Points.parent.removeChild(this.tree2Points);
            }
        }
        if ((this.player.node2TickMaj)+(this.player.node2Med*5)+(this.player.node2TickMin)+(this.player.node2Big*10) > 9) {
            this.tree2Points.x = this.tree2Add.x - 48;
        } else {
            this.tree2Points.x = this.tree2Add.x - 38;
        }
        this.tree2Points.setText( local1 + "").setColor(0xFFFFFF).setSize(32).setBold(true);
        addChild(this.tree2Points);
    }

    public function onEnterFrameTree3Points(arg1:Event):void
    {
        var local1:int = (this.player.node3TickMaj)+(this.player.node3Med*5)+(this.player.node3TickMin)+(this.player.node3Big*10);
        if(this.tree3Points != null){
            if(this.tree3Points.parent != null){
                this.tree3Points.parent.removeChild(this.tree3Points);
            }
        }
        if ((this.player.node3TickMaj)+(this.player.node3Med*5)+(this.player.node3TickMin)+(this.player.node3Big*10) > 9) {
            this.tree3Points.x = this.tree3Add.x - 48;
        } else {
            this.tree3Points.x = this.tree3Add.x - 38;
        }
        this.tree3Points.setText( local1 + "").setColor(0xFFFFFF).setSize(32).setBold(true);
        addChild(this.tree3Points);
    }

    public function onEnterFrameTree4Points(arg1:Event):void
    {
        var local1:int = (this.player.node4TickMaj)+(this.player.node4Med*5)+(this.player.node4TickMin)+(this.player.node4Big*10);
        if(this.tree4Points != null){
            if(this.tree4Points.parent != null){
                this.tree4Points.parent.removeChild(this.tree4Points);
            }
        }
        if ((this.player.node4TickMaj)+(this.player.node4Med*5)+(this.player.node4TickMin)+(this.player.node4Big*10) > 9) {
            this.tree4Points.x = this.tree4Add.x - 48;
        } else {
            this.tree4Points.x = this.tree4Add.x - 38;
        }
        this.tree4Points.setText( local1 + "").setColor(0xFFFFFF).setSize(32).setBold(true);
        addChild(this.tree4Points);
    }

    public function onEnterFrameTree5Points(arg1:Event):void
    {
        var local1:int = (this.player.node5TickMaj)+(this.player.node5Med*5)+(this.player.node5TickMin)+(this.player.node5Big*10);
        if(this.tree5Points != null){
            if(this.tree5Points.parent != null){
                this.tree5Points.parent.removeChild(this.tree5Points);
            }
        }
        if ((this.player.node5TickMaj)+(this.player.node5Med*5)+(this.player.node5TickMin)+(this.player.node5Big*10) > 9) {
            this.tree5Points.x = this.tree5Add.x - 48;
        } else {
            this.tree5Points.x = this.tree5Add.x - 38;
        }
        this.tree5Points.setText( local1 + "").setColor(0xFFFFFF).setSize(32).setBold(true);
        addChild(this.tree5Points);
    }

    //region tree1 tooltips
    private function onRollOver1Tick1(arg1:MouseEvent): void
    {
        if(tree1Tick1ToolTip != null)
        {
            if(tree1Tick1ToolTip.parent != null)
            {
                tree1Tick1ToolTip.parent.removeChild(tree1Tick1ToolTip);
            }
        }
        tree1Tick1ToolTip = new SkillTreeToolTip("+1 Dex", "-1 Point", 150);
        StaticInjectorContext.getInjector().getInstance(ShowTooltipSignal).dispatch(tree1Tick1ToolTip);
    }

    private function onRollOut1Tick1(param1:MouseEvent): void
    {
        if(tree1Tick1ToolTip && tree1Tick1ToolTip.parent)
        {
            tree1Tick1ToolTip.parent.removeChild(tree1Tick1ToolTip);
        }
    }

    private function onRollOver1Tick2(arg1:MouseEvent): void
    {
        if(tree1Tick2ToolTip != null)
        {
            if(tree1Tick2ToolTip.parent != null)
            {
                tree1Tick2ToolTip.parent.removeChild(tree1Tick2ToolTip);
            }
        }
        tree1Tick2ToolTip = new SkillTreeToolTip("+1 Att", "-1 Point", 150);
        StaticInjectorContext.getInjector().getInstance(ShowTooltipSignal).dispatch(tree1Tick2ToolTip);
    }

    private function onRollOut1Tick2(param1:MouseEvent): void
    {
        if(tree1Tick2ToolTip && tree1Tick2ToolTip.parent)
        {
            tree1Tick2ToolTip.parent.removeChild(tree1Tick2ToolTip);
        }
    }

    private function onRollOver1Tick3(arg1:MouseEvent): void
    {
        if(tree1Tick3ToolTip != null)
        {
            if(tree1Tick3ToolTip.parent != null)
            {
                tree1Tick3ToolTip.parent.removeChild(tree1Tick3ToolTip);
            }
        }
        tree1Tick3ToolTip = new SkillTreeToolTip("+5% Att", "-5 Point", 150);
        StaticInjectorContext.getInjector().getInstance(ShowTooltipSignal).dispatch(tree1Tick3ToolTip);
    }

    private function onRollOut1Tick3(param1:MouseEvent): void
    {
        if(tree1Tick3ToolTip && tree1Tick3ToolTip.parent)
        {
            tree1Tick3ToolTip.parent.removeChild(tree1Tick3ToolTip);
        }
    }

    private function onRollOver1Tick4(arg1:MouseEvent): void
    {
        if(tree1Tick4ToolTip != null)
        {
            if(tree1Tick4ToolTip.parent != null)
            {
                tree1Tick4ToolTip.parent.removeChild(tree1Tick4ToolTip);
            }
        }
        tree1Tick4ToolTip = new SkillTreeToolTip("+5% Dex", "-5 Point", 150);
        StaticInjectorContext.getInjector().getInstance(ShowTooltipSignal).dispatch(tree1Tick4ToolTip);
    }

    private function onRollOut1Tick4(param1:MouseEvent): void
    {
        if(tree1Tick4ToolTip && tree1Tick4ToolTip.parent)
        {
            tree1Tick4ToolTip.parent.removeChild(tree1Tick4ToolTip);
        }
    }
    

    private function onRollOver1Tick5(arg1:MouseEvent):void
    {
        if(tree1Tick5ToolTip != null)
        {
            if(tree1Tick5ToolTip.parent != null)
            {
                tree1Tick5ToolTip.parent.removeChild(tree1Tick5ToolTip);
            }
        }
        tree1Tick5ToolTip = new BigSkillTreeToolTip(
                "Big Node of Warrior's Strength",
                "Cost: 10 Points",
                20,
                "Positive Benefits: \nImmunity to Paralyze",
                65,
                "Negative Benefits: \n-15% Life",
                110);
        StaticInjectorContext.getInjector().getInstance(ShowTooltipSignal).dispatch(tree1Tick5ToolTip);
    }

    private function onRollOut1Tick5(param1:MouseEvent) : void
    {
        if(tree1Tick5ToolTip && tree1Tick5ToolTip.parent)
        {
            tree1Tick5ToolTip.parent.removeChild(tree1Tick5ToolTip);
        }
    }

    private function onRollOver1Tick6(arg1:MouseEvent):void
    {
        if(tree1Tick6ToolTip != null)
        {
            if(tree1Tick6ToolTip.parent != null)
            {
                tree1Tick6ToolTip.parent.removeChild(tree1Tick6ToolTip);
            }
        }
        tree1Tick6ToolTip = new BigSkillTreeToolTip(
                "Big Node of Warrior's Strength",
                "Cost: 10 Points",
                20,
                "Positive Benefits: \nFlat +0.5 Range",
                65,
                "Negative Benefits: \nImmune to Speedy",
                110);
        StaticInjectorContext.getInjector().getInstance(ShowTooltipSignal).dispatch(tree1Tick6ToolTip);
    }

    private function onRollOut1Tick6(param1:MouseEvent) : void
    {
        if(tree1Tick6ToolTip && tree1Tick6ToolTip.parent)
        {
            tree1Tick6ToolTip.parent.removeChild(tree1Tick6ToolTip);
        }
    }
    //endregion
    //region tree2 tooltips
    private function onRollOver2Tick1(arg1:MouseEvent): void
    {
        if(tree2Tick2ToolTip != null)
        {
            if(tree2Tick2ToolTip.parent != null)
            {
                tree2Tick2ToolTip.parent.removeChild(tree2Tick2ToolTip);
            }
        }
        tree2Tick2ToolTip = new SkillTreeToolTip("+10 Mana", "-1 Point", 150);
        StaticInjectorContext.getInjector().getInstance(ShowTooltipSignal).dispatch(tree2Tick2ToolTip);
    }

    private function onRollOut2Tick1(param1:MouseEvent): void
    {
        if(tree2Tick2ToolTip && tree2Tick2ToolTip.parent)
        {
            tree2Tick2ToolTip.parent.removeChild(tree2Tick2ToolTip);
        }
    }

    private function onRollOver2Tick2(arg1:MouseEvent): void
    {
        if(tree2Tick2ToolTip != null)
        {
            if(tree2Tick2ToolTip.parent != null)
            {
                tree2Tick2ToolTip.parent.removeChild(tree2Tick2ToolTip);
            }
        }
        tree2Tick2ToolTip = new SkillTreeToolTip("+1 Wis", "-1 Point", 150);
        StaticInjectorContext.getInjector().getInstance(ShowTooltipSignal).dispatch(tree2Tick2ToolTip);
    }

    private function onRollOut2Tick2(param1:MouseEvent): void
    {
        if(tree2Tick2ToolTip && tree2Tick2ToolTip.parent)
        {
            tree2Tick2ToolTip.parent.removeChild(tree2Tick2ToolTip);
        }
    }

    private function onRollOver2Tick3(arg1:MouseEvent): void
    {
        if(tree2Tick3ToolTip != null)
        {
            if(tree2Tick3ToolTip.parent != null)
            {
                tree2Tick3ToolTip.parent.removeChild(tree2Tick3ToolTip);
            }
        }
        tree2Tick3ToolTip = new SkillTreeToolTip("+5% Wis", "-5 Point", 150);
        StaticInjectorContext.getInjector().getInstance(ShowTooltipSignal).dispatch(tree2Tick3ToolTip);
    }

    private function onRollOut2Tick3(param1:MouseEvent): void
    {
        if(tree2Tick3ToolTip && tree2Tick3ToolTip.parent)
        {
            tree2Tick3ToolTip.parent.removeChild(tree2Tick3ToolTip);
        }
    }

    private function onRollOver2Tick4(arg1:MouseEvent): void
    {
        if(tree2Tick4ToolTip != null)
        {
            if(tree2Tick4ToolTip.parent != null)
            {
                tree2Tick4ToolTip.parent.removeChild(tree2Tick4ToolTip);
            }
        }
        tree2Tick4ToolTip = new SkillTreeToolTip("+30% Mana", "-5 Point", 150);
        StaticInjectorContext.getInjector().getInstance(ShowTooltipSignal).dispatch(tree2Tick4ToolTip);
    }

    private function onRollOut2Tick4(param1:MouseEvent): void
    {
        if(tree2Tick4ToolTip && tree2Tick4ToolTip.parent)
        {
            tree2Tick4ToolTip.parent.removeChild(tree2Tick4ToolTip);
        }
    }


    private function onRollOver2Tick5(arg1:MouseEvent):void
    {
        if(tree2Tick5ToolTip != null)
        {
            if(tree2Tick5ToolTip.parent != null)
            {
                tree2Tick5ToolTip.parent.removeChild(tree2Tick5ToolTip);
            }
        }
        tree2Tick5ToolTip = new BigSkillTreeToolTip(
                "Intellectual Mind",
                "Cost: 10 Points",
                20,
                "Positive Benefits: \nImmunity to Confuse",
                65,
                "Negative Benefits: \n-15% Attack",
                110);
        StaticInjectorContext.getInjector().getInstance(ShowTooltipSignal).dispatch(tree2Tick5ToolTip);
    }

    private function onRollOut2Tick5(param1:MouseEvent) : void
    {
        if(tree2Tick5ToolTip && tree2Tick5ToolTip.parent)
        {
            tree2Tick5ToolTip.parent.removeChild(tree2Tick5ToolTip);
        }
    }

    private function onRollOver2Tick6(arg1:MouseEvent):void
    {
        if(tree2Tick6ToolTip != null)
        {
            if(tree2Tick6ToolTip.parent != null)
            {
                tree2Tick6ToolTip.parent.removeChild(tree2Tick6ToolTip);
            }
        }
        tree2Tick6ToolTip = new BigSkillTreeToolTip(
                "Patient Wisdom",
                "Cost: 10 Points",
                20,
                "Positive Benefits: \n+2 Mana Leech",
                65,
                "Negative Benefits: \nImmune to Berserk",
                110);
        StaticInjectorContext.getInjector().getInstance(ShowTooltipSignal).dispatch(tree2Tick6ToolTip);
    }

    private function onRollOut2Tick6(param1:MouseEvent) : void
    {
        if(tree2Tick6ToolTip && tree2Tick6ToolTip.parent)
        {
            tree2Tick6ToolTip.parent.removeChild(tree2Tick6ToolTip);
        }
    }
    //endregion
    //region tree3 tooltips
    private function onRollOver3Tick1(arg1:MouseEvent): void
    {
        if(tree3Tick2ToolTip != null)
        {
            if(tree3Tick2ToolTip.parent != null)
            {
                tree3Tick2ToolTip.parent.removeChild(tree3Tick2ToolTip);
            }
        }
        tree3Tick2ToolTip = new SkillTreeToolTip("+4 Vit", "-1 Point", 150);
        StaticInjectorContext.getInjector().getInstance(ShowTooltipSignal).dispatch(tree3Tick2ToolTip);
    }

    private function onRollOut3Tick1(param1:MouseEvent): void
    {
        if(tree3Tick2ToolTip && tree3Tick2ToolTip.parent)
        {
            tree3Tick2ToolTip.parent.removeChild(tree3Tick2ToolTip);
        }
    }

    private function onRollOver3Tick2(arg1:MouseEvent): void
    {
        if(tree3Tick2ToolTip != null)
        {
            if(tree3Tick2ToolTip.parent != null)
            {
                tree3Tick2ToolTip.parent.removeChild(tree3Tick2ToolTip);
            }
        }
        tree3Tick2ToolTip = new SkillTreeToolTip("+2 Def", "-1 Point", 150);
        StaticInjectorContext.getInjector().getInstance(ShowTooltipSignal).dispatch(tree3Tick2ToolTip);
    }

    private function onRollOut3Tick2(param1:MouseEvent): void
    {
        if(tree3Tick2ToolTip && tree3Tick2ToolTip.parent)
        {
            tree3Tick2ToolTip.parent.removeChild(tree3Tick2ToolTip);
        }
    }

    private function onRollOver3Tick3(arg1:MouseEvent): void
    {
        if(tree3Tick3ToolTip != null)
        {
            if(tree3Tick3ToolTip.parent != null)
            {
                tree3Tick3ToolTip.parent.removeChild(tree3Tick3ToolTip);
            }
        }
        tree3Tick3ToolTip = new SkillTreeToolTip("+5% Def", "-5 Point", 150);
        StaticInjectorContext.getInjector().getInstance(ShowTooltipSignal).dispatch(tree3Tick3ToolTip);
    }

    private function onRollOut3Tick3(param1:MouseEvent): void
    {
        if(tree3Tick3ToolTip && tree3Tick3ToolTip.parent)
        {
            tree3Tick3ToolTip.parent.removeChild(tree3Tick3ToolTip);
        }
    }

    private function onRollOver3Tick4(arg1:MouseEvent): void
    {
        if(tree3Tick4ToolTip != null)
        {
            if(tree3Tick4ToolTip.parent != null)
            {
                tree3Tick4ToolTip.parent.removeChild(tree3Tick4ToolTip);
            }
        }
        tree3Tick4ToolTip = new SkillTreeToolTip("+40% Life", "-5 Point", 150);
        StaticInjectorContext.getInjector().getInstance(ShowTooltipSignal).dispatch(tree3Tick4ToolTip);
    }

    private function onRollOut3Tick4(param1:MouseEvent): void
    {
        if(tree3Tick4ToolTip && tree3Tick4ToolTip.parent)
        {
            tree3Tick4ToolTip.parent.removeChild(tree3Tick4ToolTip);
        }
    }


    private function onRollOver3Tick5(arg1:MouseEvent):void
    {
        if(tree3Tick5ToolTip != null)
        {
            if(tree3Tick5ToolTip.parent != null)
            {
                tree3Tick5ToolTip.parent.removeChild(tree3Tick5ToolTip);
            }
        }
        tree3Tick5ToolTip = new BigSkillTreeToolTip(
                "Steel Tower",
                "Cost: 10 Points",
                20,
                "Positive Benefits: \nImmunity to Armor Break",
                65,
                "Negative Benefits: \n-15% Speed",
                110);
        StaticInjectorContext.getInjector().getInstance(ShowTooltipSignal).dispatch(tree3Tick5ToolTip);
    }

    private function onRollOut3Tick5(param1:MouseEvent) : void
    {
        if(tree3Tick5ToolTip && tree3Tick5ToolTip.parent)
        {
            tree3Tick5ToolTip.parent.removeChild(tree3Tick5ToolTip);
        }
    }

    private function onRollOver3Tick6(arg1:MouseEvent):void
    {
        if(tree3Tick6ToolTip != null)
        {
            if(tree3Tick6ToolTip.parent != null)
            {
                tree3Tick6ToolTip.parent.removeChild(tree3Tick6ToolTip);
            }
        }
        tree3Tick6ToolTip = new BigSkillTreeToolTip(
                "Battering Ram",
                "Cost: 10 Points",
                20,
                "Positive Benefits: \n+2 Mana Leech",
                65,
                "Negative Benefits: \n-50% Mana",
                110);
        StaticInjectorContext.getInjector().getInstance(ShowTooltipSignal).dispatch(tree3Tick6ToolTip);
    }

    private function onRollOut3Tick6(param1:MouseEvent) : void
    {
        if(tree3Tick6ToolTip && tree3Tick6ToolTip.parent)
        {
            tree3Tick6ToolTip.parent.removeChild(tree3Tick6ToolTip);
        }
    }
    //endregion
    //region tree4 tooltips
    private function onRollOver4Tick1(arg1:MouseEvent): void
    {
        if(tree4Tick2ToolTip != null)
        {
            if(tree4Tick2ToolTip.parent != null)
            {
                tree4Tick2ToolTip.parent.removeChild(tree4Tick2ToolTip);
            }
        }
        tree4Tick2ToolTip = new SkillTreeToolTip("+1 Dex", "-1 Point", 150);
        StaticInjectorContext.getInjector().getInstance(ShowTooltipSignal).dispatch(tree4Tick2ToolTip);
    }

    private function onRollOut4Tick1(param1:MouseEvent): void
    {
        if(tree4Tick2ToolTip && tree4Tick2ToolTip.parent)
        {
            tree4Tick2ToolTip.parent.removeChild(tree4Tick2ToolTip);
        }
    }

    private function onRollOver4Tick2(arg1:MouseEvent): void
    {
        if(tree4Tick2ToolTip != null)
        {
            if(tree4Tick2ToolTip.parent != null)
            {
                tree4Tick2ToolTip.parent.removeChild(tree4Tick2ToolTip);
            }
        }
        tree4Tick2ToolTip = new SkillTreeToolTip("+1 Spd", "-1 Point", 150);
        StaticInjectorContext.getInjector().getInstance(ShowTooltipSignal).dispatch(tree4Tick2ToolTip);
    }

    private function onRollOut4Tick2(param1:MouseEvent): void
    {
        if(tree4Tick2ToolTip && tree4Tick2ToolTip.parent)
        {
            tree4Tick2ToolTip.parent.removeChild(tree4Tick2ToolTip);
        }
    }

    private function onRollOver4Tick3(arg1:MouseEvent): void
    {
        if(tree4Tick3ToolTip != null)
        {
            if(tree4Tick3ToolTip.parent != null)
            {
                tree4Tick3ToolTip.parent.removeChild(tree4Tick3ToolTip);
            }
        }
        tree4Tick3ToolTip = new SkillTreeToolTip("+5% Spd", "-5 Point", 150);
        StaticInjectorContext.getInjector().getInstance(ShowTooltipSignal).dispatch(tree4Tick3ToolTip);
    }

    private function onRollOut4Tick3(param1:MouseEvent): void
    {
        if(tree4Tick3ToolTip && tree4Tick3ToolTip.parent)
        {
            tree4Tick3ToolTip.parent.removeChild(tree4Tick3ToolTip);
        }
    }

    private function onRollOver4Tick4(arg1:MouseEvent): void
    {
        if(tree4Tick4ToolTip != null)
        {
            if(tree4Tick4ToolTip.parent != null)
            {
                tree4Tick4ToolTip.parent.removeChild(tree4Tick4ToolTip);
            }
        }
        tree4Tick4ToolTip = new SkillTreeToolTip("+5% Dex", "-5 Point", 150);
        StaticInjectorContext.getInjector().getInstance(ShowTooltipSignal).dispatch(tree4Tick4ToolTip);
    }

    private function onRollOut4Tick4(param1:MouseEvent): void
    {
        if(tree4Tick4ToolTip && tree4Tick4ToolTip.parent)
        {
            tree4Tick4ToolTip.parent.removeChild(tree4Tick4ToolTip);
        }
    }


    private function onRollOver4Tick5(arg1:MouseEvent):void
    {
        if(tree4Tick5ToolTip != null)
        {
            if(tree4Tick5ToolTip.parent != null)
            {
                tree4Tick5ToolTip.parent.removeChild(tree4Tick5ToolTip);
            }
        }
        tree4Tick5ToolTip = new BigSkillTreeToolTip(
                "Unstoppable Assassin",
                "Cost: 10 Points",
                20,
                "Positive Benefits: \nImmunity to Slowed",
                65,
                "Negative Benefits: \n-15% Defense",
                110);
        StaticInjectorContext.getInjector().getInstance(ShowTooltipSignal).dispatch(tree4Tick5ToolTip);
    }

    private function onRollOut4Tick5(param1:MouseEvent) : void
    {
        if(tree4Tick5ToolTip && tree4Tick5ToolTip.parent)
        {
            tree4Tick5ToolTip.parent.removeChild(tree4Tick5ToolTip);
        }
    }

    private function onRollOver4Tick6(arg1:MouseEvent):void
    {
        if(tree4Tick6ToolTip != null)
        {
            if(tree4Tick6ToolTip.parent != null)
            {
                tree4Tick6ToolTip.parent.removeChild(tree4Tick6ToolTip);
            }
        }
        tree4Tick6ToolTip = new BigSkillTreeToolTip(
                "Fleet Footed",
                "Cost: 10 Points",
                20,
                "Positive Benefits: \nImmune to tile damage",
                65,
                "Negative Benefits: \nTBD",
                110);
        StaticInjectorContext.getInjector().getInstance(ShowTooltipSignal).dispatch(tree4Tick6ToolTip);
    }

    private function onRollOut4Tick6(param1:MouseEvent) : void
    {
        if(tree4Tick6ToolTip && tree4Tick6ToolTip.parent)
        {
            tree4Tick6ToolTip.parent.removeChild(tree4Tick6ToolTip);
        }
    }
    //endregion
    //region tree5 tooltips
    private function onRollOver5Tick1(arg1:MouseEvent): void
    {
        if(tree5Tick2ToolTip != null)
        {
            if(tree5Tick2ToolTip.parent != null)
            {
                tree5Tick2ToolTip.parent.removeChild(tree5Tick2ToolTip);
            }
        }
        tree5Tick2ToolTip = new SkillTreeToolTip("+2% Loot Boost", "-1 Point", 150);
        StaticInjectorContext.getInjector().getInstance(ShowTooltipSignal).dispatch(tree5Tick2ToolTip);
    }

    private function onRollOut5Tick1(param1:MouseEvent): void
    {
        if(tree5Tick2ToolTip && tree5Tick2ToolTip.parent)
        {
            tree5Tick2ToolTip.parent.removeChild(tree5Tick2ToolTip);
        }
    }

    private function onRollOver5Tick2(arg1:MouseEvent): void
    {
        if(tree5Tick2ToolTip != null)
        {
            if(tree5Tick2ToolTip.parent != null)
            {
                tree5Tick2ToolTip.parent.removeChild(tree5Tick2ToolTip);
            }
        }
        tree5Tick2ToolTip = new SkillTreeToolTip("+2% Loot Boost", "-1 Point", 150);
        StaticInjectorContext.getInjector().getInstance(ShowTooltipSignal).dispatch(tree5Tick2ToolTip);
    }

    private function onRollOut5Tick2(param1:MouseEvent): void
    {
        if(tree5Tick2ToolTip && tree5Tick2ToolTip.parent)
        {
            tree5Tick2ToolTip.parent.removeChild(tree5Tick2ToolTip);
        }
    }

    private function onRollOver5Tick3(arg1:MouseEvent): void
    {
        if(tree5Tick3ToolTip != null)
        {
            if(tree5Tick3ToolTip.parent != null)
            {
                tree5Tick3ToolTip.parent.removeChild(tree5Tick3ToolTip);
            }
        }
        tree5Tick3ToolTip = new SkillTreeToolTip("+5% Loot Boost", "-5 Point", 150);
        StaticInjectorContext.getInjector().getInstance(ShowTooltipSignal).dispatch(tree5Tick3ToolTip);
    }

    private function onRollOut5Tick3(param1:MouseEvent): void
    {
        if(tree5Tick3ToolTip && tree5Tick3ToolTip.parent)
        {
            tree5Tick3ToolTip.parent.removeChild(tree5Tick3ToolTip);
        }
    }

    private function onRollOver5Tick4(arg1:MouseEvent): void
    {
        if(tree5Tick4ToolTip != null)
        {
            if(tree5Tick4ToolTip.parent != null)
            {
                tree5Tick4ToolTip.parent.removeChild(tree5Tick4ToolTip);
            }
        }
        tree5Tick4ToolTip = new SkillTreeToolTip("+5% Loot Boost", "-5 Point", 150);
        StaticInjectorContext.getInjector().getInstance(ShowTooltipSignal).dispatch(tree5Tick4ToolTip);
    }

    private function onRollOut5Tick4(param1:MouseEvent): void
    {
        if(tree5Tick4ToolTip && tree5Tick4ToolTip.parent)
        {
            tree5Tick4ToolTip.parent.removeChild(tree5Tick4ToolTip);
        }
    }


    private function onRollOver5Tick5(arg1:MouseEvent):void
    {
        if(tree5Tick5ToolTip != null)
        {
            if(tree5Tick5ToolTip.parent != null)
            {
                tree5Tick5ToolTip.parent.removeChild(tree5Tick5ToolTip);
            }
        }
        tree5Tick5ToolTip = new BigSkillTreeToolTip(
                "Team Work",
                "Cost: 10 Points",
                20,
                "Positive Benefits: \n+20% Loot Boost",
                65,
                "Negative Benefits: \nNo soul-bound damage loot\nboost",
                110);
        StaticInjectorContext.getInjector().getInstance(ShowTooltipSignal).dispatch(tree5Tick5ToolTip);
    }

    private function onRollOut5Tick5(param1:MouseEvent) : void
    {
        if(tree5Tick5ToolTip && tree5Tick5ToolTip.parent)
        {
            tree5Tick5ToolTip.parent.removeChild(tree5Tick5ToolTip);
        }
    }

    private function onRollOver5Tick6(arg1:MouseEvent):void
    {
        if(tree5Tick6ToolTip != null)
        {
            if(tree5Tick6ToolTip.parent != null)
            {
                tree5Tick6ToolTip.parent.removeChild(tree5Tick6ToolTip);
            }
        }
        tree5Tick6ToolTip = new BigSkillTreeToolTip(
                "Needle in a hay stack",
                "Cost: 10 Points",
                20,
                "Positive Benefits: \n+200% Loot Boost",
                65,
                "Negative Benefits: \nOnly White Bags",
                110);
        StaticInjectorContext.getInjector().getInstance(ShowTooltipSignal).dispatch(tree5Tick6ToolTip);
    }

    private function onRollOut5Tick6(param1:MouseEvent) : void
    {
        if(tree5Tick6ToolTip && tree5Tick6ToolTip.parent)
        {
            tree5Tick6ToolTip.parent.removeChild(tree5Tick6ToolTip);
        }
    }
    //endregion


    public function onEnterFrame(arg1:Event) : void
    {
        trace("enteronframe")
        var local1:int = 0;
        var local2:int = 0;
        if(this.player.maxedLife){
            local1 += 6;
        }
        if(this.player.maxedMana){
            local1 += 5;
        }
        if(this.player.maxedAtt){
            local1 += 3;
        }
        if(this.player.maxedDef){
            local1 += 3;
        }
        if(this.player.maxedSpd){
            local1 += 3;
        }
        if(this.player.maxedDex){
            local1 += 3;
        }
        if(this.player.maxedVit){
            local1 += 3;
        }
        if(this.player.maxedWis){
            local1 += 3;
        }
        if(this.player.superMaxedLife){
            local1 += 5;
            local2 ++;
        }
        if(this.player.superMaxedMana){
            local1 += 4;
            local2 ++;
        }
        if(this.player.superMaxedAtt){
            local1 += 2;
            local2 ++;
        }
        if(this.player.superMaxedDef){
            local1 += 2;
            local2 ++;
        }
        if(this.player.superMaxedSpd){
            local1 += 2;
            local2 ++;
        }
        if(this.player.superMaxedDex){
            local1 += 2;
            local2 ++;
        }
        if(this.player.superMaxedVit){
            local1 += 2;
            local2 ++;
        }
        if(this.player.superMaxedWis){
            local1 += 2;
            local2 ++;
        }

        if(local2 > 7){
            local1 += this.player.currFame_ > 1449 ? 29 : this.player.currFame_ / 50;
        }

        if (this.player.node1TickMin > 5)
            this.player.node1TickMin = 5;


        if (this.player.node1TickMaj > 10)
            this.player.node1TickMaj = 10;


        if (this.player.node1Med > 3)
            this.player.node1Med = 3;


        if (this.player.node1Big > 2)
            this.player.node1Big = 2;

        if (this.player.node2TickMin > 5)
            this.player.node2TickMin = 5;

        if (this.player.node2TickMaj > 10)
            this.player.node2TickMaj = 10;

        if (this.player.node2Med > 3)
            this.player.node2Med = 3;

        if (this.player.node2Big > 2)
            this.player.node2Big = 2;

        if (this.player.node3TickMin > 5)
            this.player.node3TickMin = 5;

        if (this.player.node3TickMaj > 10)
            this.player.node3TickMaj = 10;

        if (this.player.node3Med > 3)
            this.player.node3Med = 3;

        if (this.player.node3Big > 2)
            this.player.node3Big = 2;

        if (this.player.node4TickMin > 5)
            this.player.node4TickMin = 5;

        if (this.player.node4TickMaj > 10)
            this.player.node4TickMaj = 10;

        if (this.player.node4Med > 3)
            this.player.node4Med = 3;

        if (this.player.node4Big > 2)
            this.player.node4Big = 2;

        if (this.player.node5TickMin > 5)
            this.player.node5TickMin = 5;

        if (this.player.node5TickMaj > 10)
            this.player.node5TickMaj = 10;

        if (this.player.node5Med > 3)
            this.player.node5Med = 3;

        if (this.player.node5Big > 2)
            this.player.node5Big = 2;
        
        //

        if (this.player.node1TickMin < 0)
            this.player.node1TickMin = 0;


        if (this.player.node1TickMaj < 0)
            this.player.node1TickMaj = 0;


        if (this.player.node1Med < 0)
            this.player.node1Med = 0;


        if (this.player.node1Big < 0)
            this.player.node1Big = 0;

        if (this.player.node2TickMin < 0)
            this.player.node2TickMin = 0;

        if (this.player.node2TickMaj < 0)
            this.player.node2TickMaj = 0;

        if (this.player.node2Med < 0)
            this.player.node2Med = 0;

        if (this.player.node2Big < 0)
            this.player.node2Big = 0;

        if (this.player.node3TickMin < 0)
            this.player.node3TickMin = 0;

        if (this.player.node3TickMaj < 0)
            this.player.node3TickMaj = 0;

        if (this.player.node3Med < 0)
            this.player.node3Med = 0;

        if (this.player.node3Big < 0)
            this.player.node3Big = 0;

        if (this.player.node4TickMin < 0)
            this.player.node4TickMin = 0;

        if (this.player.node4TickMaj < 0)
            this.player.node4TickMaj = 0;

        if (this.player.node4Med < 0)
            this.player.node4Med = 0;

        if (this.player.node4Big < 0)
            this.player.node4Big = 0;

        if (this.player.node5TickMin < 0)
            this.player.node5TickMin = 0;

        if (this.player.node5TickMaj < 0)
            this.player.node5TickMaj = 0;

        if (this.player.node5Med < 0)
            this.player.node5Med = 0;

        if (this.player.node5Big < 0)
            this.player.node5Big = 0;
        

        if(local1 > this.player.node1TickMin + this.player.node1TickMaj + (this.player.node1Med * 5) + (this.player.node1Big * 10) + this.player.node2TickMin + this.player.node2TickMaj + (this.player.node2Med * 5) + (this.player.node2Big * 10) + this.player.node3TickMin + this.player.node3TickMaj + (this.player.node3Med * 5) + (this.player.node3Big * 10) + this.player.node4TickMin + this.player.node4TickMaj + (this.player.node4Med * 5) + (this.player.node4Big * 10) + this.player.node5TickMin + this.player.node5TickMaj + (this.player.node5Med * 5) + (this.player.node5Big * 10))
        {
            this.player.points = local1 - (this.player.node1TickMin + this.player.node1TickMaj + (this.player.node1Med * 5) + (this.player.node1Big * 10) + this.player.node2TickMin + this.player.node2TickMaj + (this.player.node2Med * 5) + (this.player.node2Big * 10) + this.player.node3TickMin + this.player.node3TickMaj + (this.player.node3Med * 5) + (this.player.node3Big * 10) + this.player.node4TickMin + this.player.node4TickMaj + (this.player.node4Med * 5) + (this.player.node4Big * 10) + this.player.node5TickMin + this.player.node5TickMaj + (this.player.node5Med * 5) + (this.player.node5Big * 10))
        } else {
            this.player.points = 0;
        }
        if(this.pointsText != null){
            if(this.pointsText.parent != null){
                this.pointsText.parent.removeChild(this.pointsText);
            }
        }
        this.pointsText.setText(this.player.points + "").setSize(30).setColor(0xFFFFFF);
        this.pointsText.x = this.player.points < 10 ? this.width/2 - 20 : this.width/2 - 30;
        addChild(this.pointsText);
    }
    private function calcTreeTicks(treeNumber:int):int
    {
        var local1:int = 0;
        switch (treeNumber){
            case 1:
                local1 += this.player.node1TickMin + this.player.node1TickMaj + this.player.node1Med + this.player.node1Big;
                break;
            case 2:
                local1 += this.player.node2TickMin + this.player.node2TickMaj + this.player.node2Med + this.player.node2Big;
                break;
            case 3:
                local1 += this.player.node3TickMin + this.player.node3TickMaj + this.player.node3Med + this.player.node3Big;
                break;
            case 4:
                local1 += this.player.node4TickMin + this.player.node4TickMaj + this.player.node4Med + this.player.node4Big;
                break;
            case 5:
                local1 += this.player.node5TickMin + this.player.node5TickMaj + this.player.node5Med + this.player.node5Big;
                break;
        }
        return local1;
    }

    public function addRedrawRect(treeNumber:int, removePoint:Boolean):void
    {
        var local1:int = calcTreeTicks(treeNumber);
        var local2:Boolean = removePoint;
        //if(removePoint){local1++};
        switch(treeNumber) {
            case 1:
                for(var i:int = 0; i <= local1; i++){
                    reDrawRect(this.tree1tick1_,this.tree1Remove.x - 10 + 5,this.tree1Remove.y + 45,this.tree1tick1_.width,this.tree1tick1_.height,local1 >= 1 ? 0x521b62 : 0x949494);
                    reDrawRect(this.tree1tick2_,this.tree1Remove.x - 10 + 35,this.tree1Remove.y + 45,this.tree1tick2_.width,this.tree1tick2_.height,local1 >= 2 ? 0x521b62 : 0x949494);
                    reDrawRect(this.tree1tick3_,this.tree1Remove.x - 10 + 65,this.tree1Remove.y + 45,this.tree1tick3_.width,this.tree1tick3_.height,local1 >= 3 ? 0x521b62 : 0x949494);
                    reDrawRect(this.tree1tick4_,this.tree1Remove.x - 10 + 102,this.tree1Remove.y + 47,7.5,this.tree1tick4_.height,local1 >= 4 ? 0x521b62 : 0x949494,true);
                    reDrawRect(this.tree1tick5_,this.tree1Remove.x - 10 + 120,this.tree1Remove.y + 45,this.tree1tick5_.width,this.tree1tick5_.height,local1 >= 5 ? 0x521b62 : 0x949494);
                    reDrawRect(this.tree1tick6_,this.tree1Remove.x - 10 + 150,this.tree1Remove.y + 45,this.tree1tick6_.width,this.tree1tick6_.height,local1 >= 6 ? 0x521b62 : 0x949494);
                    reDrawRect(this.tree1tick7_,this.tree1Remove.x - 10 + 180,this.tree1Remove.y + 45,this.tree1tick7_.width,this.tree1tick7_.height,local1 >= 7 ? 0x521b62 : 0x949494);
                    reDrawRect(this.tree1tick8_,this.tree1Remove.x - 10 + 217,this.tree1Remove.y + 47,7.5,this.tree1tick8_.height,local1 >= 8 ? 0x521b62 : 0x949494,true);
                    reDrawRect(this.tree1tick9_,this.tree1Remove.x - 10 + 235,this.tree1Remove.y + 45,this.tree1tick9_.width,this.tree1tick9_.height,local1 >= 9 ? 0x521b62 : 0x949494);
                    reDrawRect(this.tree1tick10_,this.tree1Remove.x - 10 + 265,this.tree1Remove.y + 45,this.tree1tick10_.width,this.tree1tick10_.height,local1 >= 10 ? 0x521b62 : 0x949494);
                    reDrawRect(this.tree1tick11_,this.tree1Remove.x - 10 + 295,this.tree1Remove.y + 45,this.tree1tick11_.width,this.tree1tick11_.height,local1 >= 11 ? 0x521b62 : 0x949494);
                    reDrawRect(this.tree1tick12_,this.tree1Remove.x - 10 + 336,this.tree1Remove.y + 47,12.5,this.tree1tick12_.height,local1 >= 12 ? 0x521b62 : 0x949494,true);
                    reDrawRect(this.tree1tick13_,this.tree1Remove.x - 10 + 357,this.tree1Remove.y + 45,this.tree1tick13_.width,this.tree1tick13_.height,local1 >= 13 ? 0x521b62 : 0x949494);
                    reDrawRect(this.tree1tick14_,this.tree1Remove.x - 10 + 387,this.tree1Remove.y + 45,this.tree1tick14_.width,this.tree1tick14_.height,local1 >= 14 ? 0x521b62 : 0x949494);
                    reDrawRect(this.tree1tick15_,this.tree1Remove.x - 10 + 417,this.tree1Remove.y + 45,this.tree1tick15_.width,this.tree1tick15_.height,local1 >= 15 ? 0x521b62 : 0x949494);
                    reDrawRect(this.tree1tick16_,this.tree1Remove.x - 10 + 454,this.tree1Remove.y + 47,7.5,this.tree1tick16_.height,local1 >= 16 ? 0x521b62 : 0x949494,true);
                    reDrawRect(this.tree1tick17_,this.tree1Remove.x - 10 + 472,this.tree1Remove.y + 45,this.tree1tick17_.width,this.tree1tick17_.height,local1 >= 17 ? 0x521b62 : 0x949494);
                    reDrawRect(this.tree1tick18_,this.tree1Remove.x - 10 + 502,this.tree1Remove.y + 45,this.tree1tick18_.width,this.tree1tick18_.height,local1 >= 18 ? 0x521b62 : 0x949494);
                    reDrawRect(this.tree1tick19_,this.tree1Remove.x - 10 + 532,this.tree1Remove.y + 45,this.tree1tick19_.width,this.tree1tick19_.height,local1 >= 19 ? 0x521b62 : 0x949494);
                    reDrawRect(this.tree1tick20_,this.tree1Remove.x - 10 + 573,this.tree1Remove.y + 47,12.5,this.tree1tick20_.height,local1 >= 20 ? 0x521b62 : 0x949494,true);
                }
                break;
            case 2:
                for(i = 0; i <= local1; i++){
                    reDrawRect(this.tree2tick1_,this.tree2Remove.x - 10 + 5,this.tree2Remove.y + 45,this.tree2tick1_.width,this.tree2tick1_.height,local1 >= 1 ? 0x5099ea : 0x949494);
                    reDrawRect(this.tree2tick2_,this.tree2Remove.x - 10 + 35,this.tree2Remove.y + 45,this.tree2tick2_.width,this.tree2tick2_.height,local1 >= 2 ? 0x5099ea : 0x949494);
                    reDrawRect(this.tree2tick3_,this.tree2Remove.x - 10 + 65,this.tree2Remove.y + 45,this.tree2tick3_.width,this.tree2tick3_.height,local1 >= 3 ? 0x5099ea : 0x949494);
                    reDrawRect(this.tree2tick4_,this.tree2Remove.x - 10 + 102,this.tree2Remove.y + 47,7.5,this.tree2tick4_.height,local1 >= 4 ? 0x5099ea : 0x949494,true);
                    reDrawRect(this.tree2tick5_,this.tree2Remove.x - 10 + 120,this.tree2Remove.y + 45,this.tree2tick5_.width,this.tree2tick5_.height,local1 >= 5 ? 0x5099ea : 0x949494);
                    reDrawRect(this.tree2tick6_,this.tree2Remove.x - 10 + 150,this.tree2Remove.y + 45,this.tree2tick6_.width,this.tree2tick6_.height,local1 >= 6 ? 0x5099ea : 0x949494);
                    reDrawRect(this.tree2tick7_,this.tree2Remove.x - 10 + 180,this.tree2Remove.y + 45,this.tree2tick7_.width,this.tree2tick7_.height,local1 >= 7 ? 0x5099ea : 0x949494);
                    reDrawRect(this.tree2tick8_,this.tree2Remove.x - 10 + 217,this.tree2Remove.y + 47,7.5,this.tree2tick8_.height,local1 >= 8 ? 0x5099ea : 0x949494,true);
                    reDrawRect(this.tree2tick9_,this.tree2Remove.x - 10 + 235,this.tree2Remove.y + 45,this.tree2tick9_.width,this.tree2tick9_.height,local1 >= 9 ? 0x5099ea : 0x949494);
                    reDrawRect(this.tree2tick10_,this.tree2Remove.x - 10 + 265,this.tree2Remove.y + 45,this.tree2tick10_.width,this.tree2tick10_.height,local1 >= 10 ? 0x5099ea : 0x949494);
                    reDrawRect(this.tree2tick11_,this.tree2Remove.x - 10 + 295,this.tree2Remove.y + 45,this.tree2tick11_.width,this.tree2tick11_.height,local1 >= 11 ? 0x5099ea : 0x949494);
                    reDrawRect(this.tree2tick12_,this.tree2Remove.x - 10 + 336,this.tree2Remove.y + 47,12.5,this.tree2tick12_.height,local1 >= 12 ? 0x5099ea : 0x949494,true);
                    reDrawRect(this.tree2tick13_,this.tree2Remove.x - 10 + 357,this.tree2Remove.y + 45,this.tree2tick13_.width,this.tree2tick13_.height,local1 >= 13 ? 0x5099ea : 0x949494);
                    reDrawRect(this.tree2tick14_,this.tree2Remove.x - 10 + 387,this.tree2Remove.y + 45,this.tree2tick14_.width,this.tree2tick14_.height,local1 >= 14 ? 0x5099ea : 0x949494);
                    reDrawRect(this.tree2tick15_,this.tree2Remove.x - 10 + 417,this.tree2Remove.y + 45,this.tree2tick15_.width,this.tree2tick15_.height,local1 >= 15 ? 0x5099ea : 0x949494);
                    reDrawRect(this.tree2tick16_,this.tree2Remove.x - 10 + 454,this.tree2Remove.y + 47,7.5,this.tree2tick16_.height,local1 >= 16 ? 0x5099ea : 0x949494,true);
                    reDrawRect(this.tree2tick17_,this.tree2Remove.x - 10 + 472,this.tree2Remove.y + 45,this.tree2tick17_.width,this.tree2tick17_.height,local1 >= 17 ? 0x5099ea : 0x949494);
                    reDrawRect(this.tree2tick18_,this.tree2Remove.x - 10 + 502,this.tree2Remove.y + 45,this.tree2tick18_.width,this.tree2tick18_.height,local1 >= 18 ? 0x5099ea : 0x949494);
                    reDrawRect(this.tree2tick19_,this.tree2Remove.x - 10 + 532,this.tree2Remove.y + 45,this.tree2tick19_.width,this.tree2tick19_.height,local1 >= 19 ? 0x5099ea : 0x949494);
                    reDrawRect(this.tree2tick20_,this.tree2Remove.x - 10 + 573,this.tree2Remove.y + 47,12.5,this.tree2tick20_.height,local1 >= 20 ? 0x5099ea : 0x949494,true);
                }
                break;
            case 3:
                for(i = 0; i <= local1; i++){
                    reDrawRect(this.tree3tick1_,this.tree3Remove.x - 10 + 5,this.tree3Remove.y + 45,this.tree3tick1_.width,this.tree3tick1_.height,local1 >= 1 ? 0x1a1a1a : 0x949494);
                    reDrawRect(this.tree3tick2_,this.tree3Remove.x - 10 + 35,this.tree3Remove.y + 45,this.tree3tick2_.width,this.tree3tick2_.height,local1 >= 2 ? 0x1a1a1a : 0x949494);
                    reDrawRect(this.tree3tick3_,this.tree3Remove.x - 10 + 65,this.tree3Remove.y + 45,this.tree3tick3_.width,this.tree3tick3_.height,local1 >= 3 ? 0x1a1a1a : 0x949494);
                    reDrawRect(this.tree3tick4_,this.tree3Remove.x - 10 + 102,this.tree3Remove.y + 47,7.5,this.tree3tick4_.height,local1 >= 4 ? 0x1a1a1a : 0x949494,true);
                    reDrawRect(this.tree3tick5_,this.tree3Remove.x - 10 + 120,this.tree3Remove.y + 45,this.tree3tick5_.width,this.tree3tick5_.height,local1 >= 5 ? 0x1a1a1a : 0x949494);
                    reDrawRect(this.tree3tick6_,this.tree3Remove.x - 10 + 150,this.tree3Remove.y + 45,this.tree3tick6_.width,this.tree3tick6_.height,local1 >= 6 ? 0x1a1a1a : 0x949494);
                    reDrawRect(this.tree3tick7_,this.tree3Remove.x - 10 + 180,this.tree3Remove.y + 45,this.tree3tick7_.width,this.tree3tick7_.height,local1 >= 7 ? 0x1a1a1a : 0x949494);
                    reDrawRect(this.tree3tick8_,this.tree3Remove.x - 10 + 217,this.tree3Remove.y + 47,7.5,this.tree3tick8_.height,local1 >= 8 ? 0x1a1a1a : 0x949494,true);
                    reDrawRect(this.tree3tick9_,this.tree3Remove.x - 10 + 235,this.tree3Remove.y + 45,this.tree3tick9_.width,this.tree3tick9_.height,local1 >= 9 ? 0x1a1a1a : 0x949494);
                    reDrawRect(this.tree3tick10_,this.tree3Remove.x - 10 + 265,this.tree3Remove.y + 45,this.tree3tick10_.width,this.tree3tick10_.height,local1 >= 10 ? 0x1a1a1a : 0x949494);
                    reDrawRect(this.tree3tick11_,this.tree3Remove.x - 10 + 295,this.tree3Remove.y + 45,this.tree3tick11_.width,this.tree3tick11_.height,local1 >= 11 ? 0x1a1a1a : 0x949494);
                    reDrawRect(this.tree3tick12_,this.tree3Remove.x - 10 + 336,this.tree3Remove.y + 47,12.5,this.tree3tick12_.height,local1 >= 12 ? 0x1a1a1a : 0x949494,true);
                    reDrawRect(this.tree3tick13_,this.tree3Remove.x - 10 + 357,this.tree3Remove.y + 45,this.tree3tick13_.width,this.tree3tick13_.height,local1 >= 13 ? 0x1a1a1a : 0x949494);
                    reDrawRect(this.tree3tick14_,this.tree3Remove.x - 10 + 387,this.tree3Remove.y + 45,this.tree3tick14_.width,this.tree3tick14_.height,local1 >= 14 ? 0x1a1a1a : 0x949494);
                    reDrawRect(this.tree3tick15_,this.tree3Remove.x - 10 + 417,this.tree3Remove.y + 45,this.tree3tick15_.width,this.tree3tick15_.height,local1 >= 15 ? 0x1a1a1a : 0x949494);
                    reDrawRect(this.tree3tick16_,this.tree3Remove.x - 10 + 454,this.tree3Remove.y + 47,7.5,this.tree3tick16_.height,local1 >= 16 ? 0x1a1a1a : 0x949494,true);
                    reDrawRect(this.tree3tick17_,this.tree3Remove.x - 10 + 472,this.tree3Remove.y + 45,this.tree3tick17_.width,this.tree3tick17_.height,local1 >= 17 ? 0x1a1a1a : 0x949494);
                    reDrawRect(this.tree3tick18_,this.tree3Remove.x - 10 + 502,this.tree3Remove.y + 45,this.tree3tick18_.width,this.tree3tick18_.height,local1 >= 18 ? 0x1a1a1a : 0x949494);
                    reDrawRect(this.tree3tick19_,this.tree3Remove.x - 10 + 532,this.tree3Remove.y + 45,this.tree3tick19_.width,this.tree3tick19_.height,local1 >= 19 ? 0x1a1a1a : 0x949494);
                    reDrawRect(this.tree3tick20_,this.tree3Remove.x - 10 + 573,this.tree3Remove.y + 47,12.5,this.tree3tick20_.height,local1 >= 20 ? 0x1a1a1a : 0x949494,true);
                }
                break;
            case 4:
                for(i = 0; i <= local1; i++){
                    reDrawRect(this.tree4tick1_,this.tree4Remove.x - 10 + 5,this.tree4Remove.y + 45,this.tree4tick1_.width,this.tree4tick1_.height,local1 >= 1 ? 0x9dcf62 : 0x949494);
                    reDrawRect(this.tree4tick2_,this.tree4Remove.x - 10 + 35,this.tree4Remove.y + 45,this.tree4tick2_.width,this.tree4tick2_.height,local1 >= 2 ? 0x9dcf62 : 0x949494);
                    reDrawRect(this.tree4tick3_,this.tree4Remove.x - 10 + 65,this.tree4Remove.y + 45,this.tree4tick3_.width,this.tree4tick3_.height,local1 >= 3 ? 0x9dcf62 : 0x949494);
                    reDrawRect(this.tree4tick4_,this.tree4Remove.x - 10 + 102,this.tree4Remove.y + 47,7.5,this.tree4tick4_.height,local1 >= 4 ? 0x9dcf62 : 0x949494,true);
                    reDrawRect(this.tree4tick5_,this.tree4Remove.x - 10 + 120,this.tree4Remove.y + 45,this.tree4tick5_.width,this.tree4tick5_.height,local1 >= 5 ? 0x9dcf62 : 0x949494);
                    reDrawRect(this.tree4tick6_,this.tree4Remove.x - 10 + 150,this.tree4Remove.y + 45,this.tree4tick6_.width,this.tree4tick6_.height,local1 >= 6 ? 0x9dcf62 : 0x949494);
                    reDrawRect(this.tree4tick7_,this.tree4Remove.x - 10 + 180,this.tree4Remove.y + 45,this.tree4tick7_.width,this.tree4tick7_.height,local1 >= 7 ? 0x9dcf62 : 0x949494);
                    reDrawRect(this.tree4tick8_,this.tree4Remove.x - 10 + 217,this.tree4Remove.y + 47,7.5,this.tree4tick8_.height,local1 >= 8 ? 0x9dcf62 : 0x949494,true);
                    reDrawRect(this.tree4tick9_,this.tree4Remove.x - 10 + 235,this.tree4Remove.y + 45,this.tree4tick9_.width,this.tree4tick9_.height,local1 >= 9 ? 0x9dcf62 : 0x949494);
                    reDrawRect(this.tree4tick10_,this.tree4Remove.x - 10 + 265,this.tree4Remove.y + 45,this.tree4tick10_.width,this.tree4tick10_.height,local1 >= 10 ? 0x9dcf62 : 0x949494);
                    reDrawRect(this.tree4tick11_,this.tree4Remove.x - 10 + 295,this.tree4Remove.y + 45,this.tree4tick11_.width,this.tree4tick11_.height,local1 >= 11 ? 0x9dcf62 : 0x949494);
                    reDrawRect(this.tree4tick12_,this.tree4Remove.x - 10 + 336,this.tree4Remove.y + 47,12.5,this.tree4tick12_.height,local1 >= 12 ? 0x9dcf62 : 0x949494,true);
                    reDrawRect(this.tree4tick13_,this.tree4Remove.x - 10 + 357,this.tree4Remove.y + 45,this.tree4tick13_.width,this.tree4tick13_.height,local1 >= 13 ? 0x9dcf62 : 0x949494);
                    reDrawRect(this.tree4tick14_,this.tree4Remove.x - 10 + 387,this.tree4Remove.y + 45,this.tree4tick14_.width,this.tree4tick14_.height,local1 >= 14 ? 0x9dcf62 : 0x949494);
                    reDrawRect(this.tree4tick15_,this.tree4Remove.x - 10 + 417,this.tree4Remove.y + 45,this.tree4tick15_.width,this.tree4tick15_.height,local1 >= 15 ? 0x9dcf62 : 0x949494);
                    reDrawRect(this.tree4tick16_,this.tree4Remove.x - 10 + 454,this.tree4Remove.y + 47,7.5,this.tree4tick16_.height,local1 >= 16 ? 0x9dcf62 : 0x949494,true);
                    reDrawRect(this.tree4tick17_,this.tree4Remove.x - 10 + 472,this.tree4Remove.y + 45,this.tree4tick17_.width,this.tree4tick17_.height,local1 >= 17 ? 0x9dcf62 : 0x949494);
                    reDrawRect(this.tree4tick18_,this.tree4Remove.x - 10 + 502,this.tree4Remove.y + 45,this.tree4tick18_.width,this.tree4tick18_.height,local1 >= 18 ? 0x9dcf62 : 0x949494);
                    reDrawRect(this.tree4tick19_,this.tree4Remove.x - 10 + 532,this.tree4Remove.y + 45,this.tree4tick19_.width,this.tree4tick19_.height,local1 >= 19 ? 0x9dcf62 : 0x949494);
                    reDrawRect(this.tree4tick20_,this.tree4Remove.x - 10 + 573,this.tree4Remove.y + 47,12.5,this.tree4tick20_.height,local1 >= 20 ? 0x9dcf62 : 0x949494,true);
                }
                break;
            case 5:
                for(i = 0; i <= local1; i++){
                    reDrawRect(this.tree5tick1_,this.tree5Remove.x - 10 + 5,this.tree5Remove.y + 45,this.tree5tick1_.width,this.tree5tick1_.height,local1 >= 1 ? 0x4ca48b : 0x949494);
                    reDrawRect(this.tree5tick2_,this.tree5Remove.x - 10 + 35,this.tree5Remove.y + 45,this.tree5tick2_.width,this.tree5tick2_.height,local1 >= 2 ? 0x4ca48b : 0x949494);
                    reDrawRect(this.tree5tick3_,this.tree5Remove.x - 10 + 65,this.tree5Remove.y + 45,this.tree5tick3_.width,this.tree5tick3_.height,local1 >= 3 ? 0x4ca48b : 0x949494);
                    reDrawRect(this.tree5tick4_,this.tree5Remove.x - 10 + 102,this.tree5Remove.y + 47,7.5,this.tree5tick4_.height,local1 >= 4 ? 0x4ca48b : 0x949494,true);
                    reDrawRect(this.tree5tick5_,this.tree5Remove.x - 10 + 120,this.tree5Remove.y + 45,this.tree5tick5_.width,this.tree5tick5_.height,local1 >= 5 ? 0x4ca48b : 0x949494);
                    reDrawRect(this.tree5tick6_,this.tree5Remove.x - 10 + 150,this.tree5Remove.y + 45,this.tree5tick6_.width,this.tree5tick6_.height,local1 >= 6 ? 0x4ca48b : 0x949494);
                    reDrawRect(this.tree5tick7_,this.tree5Remove.x - 10 + 180,this.tree5Remove.y + 45,this.tree5tick7_.width,this.tree5tick7_.height,local1 >= 7 ? 0x4ca48b : 0x949494);
                    reDrawRect(this.tree5tick8_,this.tree5Remove.x - 10 + 217,this.tree5Remove.y + 47,7.5,this.tree5tick8_.height,local1 >= 8 ? 0x4ca48b : 0x949494,true);
                    reDrawRect(this.tree5tick9_,this.tree5Remove.x - 10 + 235,this.tree5Remove.y + 45,this.tree5tick9_.width,this.tree5tick9_.height,local1 >= 9 ? 0x4ca48b : 0x949494);
                    reDrawRect(this.tree5tick10_,this.tree5Remove.x - 10 + 265,this.tree5Remove.y + 45,this.tree5tick10_.width,this.tree5tick10_.height,local1 >= 10 ? 0x4ca48b : 0x949494);
                    reDrawRect(this.tree5tick11_,this.tree5Remove.x - 10 + 295,this.tree5Remove.y + 45,this.tree5tick11_.width,this.tree5tick11_.height,local1 >= 11 ? 0x4ca48b : 0x949494);
                    reDrawRect(this.tree5tick12_,this.tree5Remove.x - 10 + 336,this.tree5Remove.y + 47,12.5,this.tree5tick12_.height,local1 >= 12 ? 0x4ca48b : 0x949494,true);
                    reDrawRect(this.tree5tick13_,this.tree5Remove.x - 10 + 357,this.tree5Remove.y + 45,this.tree5tick13_.width,this.tree5tick13_.height,local1 >= 13 ? 0x4ca48b : 0x949494);
                    reDrawRect(this.tree5tick14_,this.tree5Remove.x - 10 + 387,this.tree5Remove.y + 45,this.tree5tick14_.width,this.tree5tick14_.height,local1 >= 14 ? 0x4ca48b : 0x949494);
                    reDrawRect(this.tree5tick15_,this.tree5Remove.x - 10 + 417,this.tree5Remove.y + 45,this.tree5tick15_.width,this.tree5tick15_.height,local1 >= 15 ? 0x4ca48b : 0x949494);
                    reDrawRect(this.tree5tick16_,this.tree5Remove.x - 10 + 454,this.tree5Remove.y + 47,7.5,this.tree5tick16_.height,local1 >= 16 ? 0x4ca48b : 0x949494,true);
                    reDrawRect(this.tree5tick17_,this.tree5Remove.x - 10 + 472,this.tree5Remove.y + 45,this.tree5tick17_.width,this.tree5tick17_.height,local1 >= 17 ? 0x4ca48b : 0x949494);
                    reDrawRect(this.tree5tick18_,this.tree5Remove.x - 10 + 502,this.tree5Remove.y + 45,this.tree5tick18_.width,this.tree5tick18_.height,local1 >= 18 ? 0x4ca48b : 0x949494);
                    reDrawRect(this.tree5tick19_,this.tree5Remove.x - 10 + 532,this.tree5Remove.y + 45,this.tree5tick19_.width,this.tree5tick19_.height,local1 >= 19 ? 0x4ca48b : 0x949494);
                    reDrawRect(this.tree5tick20_,this.tree5Remove.x - 10 + 573,this.tree5Remove.y + 47,12.5,this.tree5tick20_.height,local1 >= 20 ? 0x4ca48b : 0x949494,true);
                }
                break;



        }
    }

    internal function calcNextCost(treeNumber:int, removePoint:Boolean = false):int
    {
        var local1:int = calcTreePoints(treeNumber);
        var local2:int;
        switch (local1){
            case 0: removePoint ? local2 = 0 : local2 = 1;
                break;
            case 1: removePoint ? local2 = -1 : local2 = 1;
                break;
            case 2: removePoint ? local2 = -1 : local2 = 1;
                break;
            case 3: removePoint ? local2 = -1 : local2 = 5;
                break;
            case 8: removePoint ? local2 = -5 : local2 = 1;
                break;
            case 9: removePoint ? local2 = -1 : local2 = 1;
                break;
            case 10: removePoint ? local2 = -1 : local2 = 1;
                break;
            case 11: removePoint ? local2 = -1 : local2 = 5;
                break;
            case 16: removePoint ? local2 = -5 : local2 = 1;
                break;
            case 17: removePoint ? local2 = -1 : local2 = 1;
                break;
            case 18: removePoint ? local2 = -1 : local2 = 1;
                break;
            case 19: removePoint ? local2 = -1 : local2 = 10;
                break;
            case 29: removePoint ? local2 = -10 : local2 = 1;
                break;
            case 30: removePoint ? local2 = -1 : local2 = 1;
                break;
            case 31: removePoint ? local2 = -1 : local2 = 1;
                break;
            case 32: removePoint ? local2 = -1 : local2 = 5;
                break;
            case 37: removePoint ? local2 = -5 : local2 = 1;
                break;
            case 38: removePoint ? local2 = -1 : local2 = 1;
                break;
            case 39: removePoint ? local2 = -1 : local2 = 1;
                break;
            case 40: removePoint ? local2 = -1 : local2 = 10;
                break;
            case 50: removePoint ? local2 = -10 : local2 = 99;
                break;
            default: local2 = 99;
                break

        }
        return local2;
    }

    internal function calcTreePoints(treeNumber:int):int{
        var local1:int = 0;
        switch (treeNumber)
        {
            case 1:
                local1 = (this.player.node1TickMaj)+(this.player.node1Med*5)+(this.player.node1TickMin)+(this.player.node1Big*10);
                trace("calc tree point: "+local1);break;
            case 2:
                local1 = (this.player.node2TickMaj)+(this.player.node2Med*5)+(this.player.node2TickMin)+(this.player.node2Big*10);break;
            case 3:
                local1 = (this.player.node3TickMaj)+(this.player.node3Med*5)+(this.player.node3TickMin)+(this.player.node3Big*10);break;
            case 4:
                local1 = (this.player.node4TickMaj)+(this.player.node4Med*5)+(this.player.node4TickMin)+(this.player.node4Big*10);break;
            case 5:
                local1 = (this.player.node5TickMaj)+(this.player.node5Med*5)+(this.player.node5TickMin)+(this.player.node5Big*10);break;
        }
        return local1;
    }


}
}
