package kabam.rotmg.Engine.view {
import com.company.assembleegameclient.objects.Engine;
import com.company.assembleegameclient.ui.Scrollbar;
import com.company.assembleegameclient.ui.panels.itemgrids.engineInventory.EngineInventory;

import flash.display.Graphics;

import flash.display.Shape;

import flash.display.Sprite;

import io.decagames.rotmg.ui.sliceScaling.SliceScalingBitmap;

import kabam.rotmg.PotionStorage.*;

import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.objects.GameObject;
import com.company.assembleegameclient.objects.Player;
import com.gskinner.motion.GTween;

import flash.events.Event;
import flash.events.MouseEvent;
import flash.filters.GlowFilter;

import io.decagames.rotmg.ui.buttons.SliceScalingButton;
import io.decagames.rotmg.ui.popups.header.PopupHeader;
import io.decagames.rotmg.ui.popups.modal.ModalPopup;
import io.decagames.rotmg.ui.texture.TextureParser;
import io.decagames.rotmg.ui.defaults.DefaultLabelFormat;

import kabam.rotmg.core.StaticInjectorContext;
import kabam.rotmg.Engine.view.components.FuelGauge;
import kabam.rotmg.game.model.GameModel;
import kabam.rotmg.game.signals.AddTextLineSignal;

import org.osflash.signals.Signal;

public class EngineView extends ModalPopup {

    public var gs_:GameSprite;
    public var fuelGague_:FuelGauge;
    internal var quitButton:SliceScalingButton;
    public var engine_:Engine;
    internal var text_:String;
    public var itemEngineTile:EngineInventory;
    internal var player_:Player;
    public var fuelButton:SliceScalingButton;

    public var close:Signal = new Signal();

    public function EngineView(gs:GameSprite, go:Engine) {
        this.gs_ = gs;
        this.engine_ = go;
        this.player_ = StaticInjectorContext.getInjector().getInstance(GameModel).player;
        super(425, 425, "Strange Engine");
        
        this.fuelGague_ = new FuelGauge();
        this.fuelGague_.x = 16;
        this.fuelGague_.y = 16;
        addChild(this.fuelGague_);

        this.itemEngineTile = new EngineInventory(this.gs_, this.player_.equipment_, this.player_.hasBackpack_);
        this.itemEngineTile.x = 245;
        this.itemEngineTile.y = this.player_.hasBackpack_ ? 100 : 200;
        addChild(this.itemEngineTile);

        this.fuelButton = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "generic_green_button"));
        this.fuelButton.width = 100;
        this.fuelButton.x = 280;
        this.fuelButton.y = 382;
        this.fuelButton.setLabel("Fuel", DefaultLabelFormat.defaultModalTitle);
        addChild(fuelButton);

        this.quitButton = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "close_button"));
        this.header.addButton(this.quitButton, PopupHeader.RIGHT_BUTTON);
        this.quitButton.addEventListener(MouseEvent.CLICK, this.onClose);

        this.x = this.width / 2 - 140;
        this.y = this.height / 2 - 195;

        this.draw();

        this.addEventListener(Event.ENTER_FRAME, this.onEnterFrame);
    }

    public function onEnterFrame(_arg1:Event):void
    {
        this.draw();
    }

    public function onClose(arg1:Event):void {
        this.removeEventListener(Event.ENTER_FRAME, this.onEnterFrame);
        this.quitButton.removeEventListener(MouseEvent.CLICK, this.onClose);
        this.close.dispatch();
    }

    public function draw():void {
        trace("Drawing currentValue_:" + this.engine_.currentValue_)
        this.fuelGague_.draw(this.engine_.currentValue_);
    }
}
}
