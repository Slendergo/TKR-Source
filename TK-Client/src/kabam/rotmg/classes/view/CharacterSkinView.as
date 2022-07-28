package kabam.rotmg.classes.view
{
   import com.company.assembleegameclient.screens.AccountScreen;
   import com.company.assembleegameclient.screens.TitleMenuOption;
   import com.company.rotmg.graphics.ScreenGraphic;
   import flash.display.Shape;
   import flash.display.Sprite;
   import flash.events.MouseEvent;

import io.decagames.rotmg.ui.buttons.SliceScalingButton;
import io.decagames.rotmg.ui.defaults.DefaultLabelFormat;

import io.decagames.rotmg.ui.sliceScaling.SliceScalingBitmap;

import io.decagames.rotmg.ui.texture.TextureParser;
import io.decagames.rotmg.utils.colors.GreyScale;

import kabam.rotmg.game.view.CreditDisplay;
import kabam.rotmg.ui.view.components.MenuOptionsBar;
import kabam.rotmg.ui.view.components.ScreenBase;
   import org.osflash.signals.Signal;
   import org.osflash.signals.natives.NativeMappedSignal;
   
   public class CharacterSkinView extends Sprite
   {
      private const base:ScreenBase = makeScreenBase();
      private const account:AccountScreen = makeAccountScreen();
      private const lines:Shape = makeLines();
      private const creditsDisplay:CreditDisplay = makeCreditDisplay();
      private const menuOptions:MenuOptionsBar = makeMenuOptionsBar();
      private const playBtn:SliceScalingButton = makePlayButton();
      private const backBtn:SliceScalingButton = makeBackButton();
      private const list:CharacterSkinListView = makeListView();
      private const detail:ClassDetailView = makeClassDetailView();
      public const play:Signal = new NativeMappedSignal(playBtn,MouseEvent.CLICK);
      public const back:Signal = new NativeMappedSignal(backBtn,MouseEvent.CLICK);
      private var buttonsBackground:SliceScalingBitmap;
      
      public function CharacterSkinView()
      {
      }

      private function makeMenuOptionsBar():MenuOptionsBar {
         var local1:MenuOptionsBar = new MenuOptionsBar();
         this.buttonsBackground = TextureParser.instance.getSliceScalingBitmap("UI","popup_header_title",800);
         this.buttonsBackground.y = 502.5;
         addChild(this.buttonsBackground);
         addChild(local1);
         return(local1);
      }
      
      private function makeScreenBase() : ScreenBase
      {
         var base:ScreenBase = new ScreenBase();
         addChild(base);
         return base;
      }
      
      private function makeAccountScreen() : AccountScreen
      {
         var screen:AccountScreen = new AccountScreen();
         addChild(screen);
         return screen;
      }
      
      private function makeCreditDisplay() : CreditDisplay
      {
         var display:CreditDisplay = new CreditDisplay();
         display.x = 800;
         display.y = 20;
         addChild(display);
         return display;
      }
      
      private function makeLines() : Shape
      {
         var shape:Shape = new Shape();
         shape.graphics.clear();
         shape.graphics.lineStyle(2,5526612);
         shape.graphics.moveTo(0,105);
         shape.graphics.lineTo(800,105);
         shape.graphics.moveTo(346,105);
         shape.graphics.lineTo(346,526);
         addChild(shape);
         return shape;
      }

      private function makePlayButton():SliceScalingButton {
         var _local1:SliceScalingButton;
         _local1 = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI","generic_green_button"));
         setDefault(_local1,"play",100,false);
         _local1.x = (400 - (_local1.width / 2));
         _local1.y = this.buttonsBackground.y + 17;
         addChild(_local1);
         return (_local1);
      }

      private function makeBackButton():SliceScalingButton {
         var _local1:SliceScalingButton;
         _local1 = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI","generic_green_button"));
         setDefault(_local1,"back",100,true);
         _local1.x = 90;
         _local1.y = this.buttonsBackground.y + 17;
         addChild(_local1);
         return (_local1);
      }

      private static function setDefault(param1:SliceScalingButton, param2:String, param3:int = 100, param4:Boolean = true) : void
      {
         param1.setLabel(param2,DefaultLabelFormat.questButtonCompleteLabel);
         param1.x = 0;
         param1.y = 0;
         param1.width = param3;
         if(param4)
         {
            GreyScale.greyScaleToDisplayObject(param1,true);
         }
      }
      
      private function makeListView() : CharacterSkinListView
      {
         var view:CharacterSkinListView = new CharacterSkinListView();
         view.x = 351;
         view.y = 110;
         addChild(view);
         return view;
      }
      
      private function makeClassDetailView() : ClassDetailView
      {
         var view:ClassDetailView = new ClassDetailView();
         view.x = 5;
         view.y = 110;
         addChild(view);
         return view;
      }

      public function setPlayButtonEnabled(activate:Boolean):void {
         if (!activate) {
            this.playBtn.disabled = true;
         }
      }
   }
}
