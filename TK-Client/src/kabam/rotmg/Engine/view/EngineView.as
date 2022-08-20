package kabam.rotmg.Engine.view {
import com.company.assembleegameclient.ui.Scrollbar;

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
    public var player:Player;
    internal var text_:String;

    public var close:Signal = new Signal();

    public var sprite_:Sprite;

    public var upgrade_:SliceScalingButton;


    public function EngineView(gs:GameSprite, go:Player) {
        this.gs_ = gs;
        this.player = go;
        super(425, 425, "Strange Engine");
        
        //stage1 Fuel Guage
        this.fuelGague_ = new FuelGauge();
        this.fuelGague_.x = 16;
        this.fuelGague_.y = 16;
        addChild(this.fuelGague_);

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
        this.quitButton.removeEventListener(MouseEvent.CLICK, this.onClose);
        this.close.dispatch();
    }

    public var test_:int;

    public function draw():void {
        test_ ++
        if(test_ > 850) {
            test_ = 0
        }
        this.fuelGague_.draw(this.test_); // change to fuel
    }
}
}
