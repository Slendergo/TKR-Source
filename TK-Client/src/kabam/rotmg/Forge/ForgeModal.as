package kabam.rotmg.Forge {
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.objects.GameObject;
import com.company.assembleegameclient.objects.Player;
import com.company.assembleegameclient.ui.panels.itemgrids.forgeInventory.ForgeInventory;

import flash.events.MouseEvent;

import io.decagames.rotmg.ui.buttons.SliceScalingButton;
import io.decagames.rotmg.ui.defaults.DefaultLabelFormat;
import io.decagames.rotmg.ui.popups.header.PopupHeader;
import io.decagames.rotmg.ui.popups.modal.ModalPopup;
import io.decagames.rotmg.ui.texture.TextureParser;

import kabam.rotmg.core.StaticInjectorContext;
import kabam.rotmg.game.model.GameModel;

import org.osflash.signals.Signal;
import org.swiftsuspenders.Injector;

public class ForgeModal extends ModalPopup {

    public var gs_:GameSprite;
    public var quitButton:SliceScalingButton;
    public var forgeButton:SliceScalingButton;
    internal var player_:Player;
    public var injector:Injector;
    public const close:Signal = new Signal();
    public var gameObject_:GameObject;
    public var itemForgeTile:ForgeInventory;

    public function ForgeModal(gs:GameSprite, gm:GameObject) {
        super(425, 160, "Forge");
        this.gs_ = gs;
        this.gameObject_ = gm;
        this.player_ = StaticInjectorContext.getInjector().getInstance(GameModel).player;
        this.quitButton = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "close_button"));
        this.header.addButton(this.quitButton, PopupHeader.RIGHT_BUTTON);
        this.quitButton.addEventListener(MouseEvent.CLICK, this.onClose);
        this.forgeButton = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "generic_green_button"));
        this.forgeButton.width = 100;
        this.forgeButton.x = (425 / 2) - (this.forgeButton.width / 2);
        this.forgeButton.y = 160 - this.forgeButton.height - 8;
        this.forgeButton.setLabel("Forge", DefaultLabelFormat.defaultModalTitle);
        addChild(forgeButton);

        this.itemForgeTile = new ForgeInventory(this.gs_, this.player_.equipment_, this.player_.hasBackpack_);
        this.itemForgeTile.x = 32;
        this.itemForgeTile.y = -64;
        addChild(this.itemForgeTile);

        this.x = 160;
        this.y = 30;
    }

    private function onClose(evt:MouseEvent) : void
    {
        this.close.dispatch();
    }
}
}
