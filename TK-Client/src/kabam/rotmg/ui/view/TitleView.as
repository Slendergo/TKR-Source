package kabam.rotmg.ui.view
{
   import com.company.assembleegameclient.constants.ScreenTypes;
   import com.company.assembleegameclient.screens.AccountScreen;
   import com.company.assembleegameclient.screens.TitleMenuOption;
   import com.company.assembleegameclient.ui.SoundIcon;
   import com.company.rotmg.graphics.ScreenGraphic;
   import com.company.ui.SimpleText;
   import flash.display.Sprite;
   import flash.filters.DropShadowFilter;

import io.decagames.rotmg.ui.buttons.SliceScalingButton;

import io.decagames.rotmg.ui.sliceScaling.SliceScalingBitmap;
import io.decagames.rotmg.ui.texture.TextureParser;

import kabam.rotmg.ui.model.EnvironmentData;
   import kabam.rotmg.ui.view.components.DarkenFactory;
   import kabam.rotmg.ui.view.components.MapBackground;
import kabam.rotmg.ui.view.components.MenuOptionsBar;

import org.osflash.signals.Signal;
   
   public class TitleView extends Sprite
   {
      private static var TitleScreenGraphic:Class = TitleView_TitleScreenGraphic;

      private static const COPYRIGHT:String = "© 2010, 2011 by Wild Shadow Studios, Inc.";
       
      
      public var playClicked:Signal;
      public var serversClicked:Signal;
      public var creditsClicked:Signal;
      public var accountClicked:Signal;
      public var legendsClicked:Signal;
      public var editorClicked:Signal;

      private var container:Sprite;

      private var playButton:SliceScalingButton;
      private var serversButton:SliceScalingButton;
      private var creditsButton:SliceScalingButton;
      private var accountButton:SliceScalingButton;
      private var legendsButton:SliceScalingButton;
      private var editorButton:SliceScalingButton;

      private var versionText:SimpleText;
      private var copyrightText:SimpleText;
      private var darkenFactory:DarkenFactory;
      private var data:EnvironmentData;
      private var buttonsBackground:SliceScalingBitmap;
      private var menuOptionsBar:MenuOptionsBar;
      
      public function TitleView()
      {
         this.menuOptionsBar = this.makeMenuOptionsBar();
         this.darkenFactory = new DarkenFactory();
         this.buttonsBackground = TextureParser.instance.getSliceScalingBitmap("UI","popup_header_title",800);
         this.buttonsBackground.y = 502.5;
         super();
         addChild(this.darkenFactory.create());
         addChild(new TitleScreenGraphic());
         addChild(this.buttonsBackground);
         addChild(this.menuOptionsBar);
         addChild(new AccountScreen());
         this.makeChildren();
         addChild(new SoundIcon());
      }

      private function makeMenuOptionsBar() : MenuOptionsBar
      {
         var _loc1_:SliceScalingButton = ButtonFactory.getPlayButton();
         var _loc2_:SliceScalingButton = ButtonFactory.getServersButton();
         var _loc3_:SliceScalingButton = ButtonFactory.getAccountButton();
         var _loc4_:SliceScalingButton = ButtonFactory.getLegendsButton();
         var _loc5_:SliceScalingButton = ButtonFactory.getSupportButton();
         var _loc6_:SliceScalingButton = ButtonFactory.getEditorButton();
         this.playClicked = _loc1_.clicked;
         this.serversClicked = _loc2_.clicked;
         this.accountClicked = _loc3_.clicked;
         this.legendsClicked = _loc4_.clicked;
         this.creditsClicked = _loc5_.clicked;
         this.editorClicked = _loc6_.clicked;
         var _loc7_:MenuOptionsBar = new MenuOptionsBar();
         _loc7_.addButton(_loc1_,MenuOptionsBar.CENTER);
         _loc7_.addButton(_loc2_,MenuOptionsBar.LEFT);
         _loc7_.addButton(_loc6_,MenuOptionsBar.LEFT);
         _loc7_.addButton(_loc3_,MenuOptionsBar.RIGHT);
         _loc7_.addButton(_loc4_,MenuOptionsBar.RIGHT);
         return _loc7_;
      }
      
      private function makeChildren() : void
      {
         this.container = new Sprite();
         this.versionText = new SimpleText(12,8355711,false,0,0);
         this.versionText.filters = [new DropShadowFilter(0,0,0)];
         this.container.addChild(this.versionText);
         this.copyrightText = new SimpleText(12,8355711,false,0,0);
         this.copyrightText.text = COPYRIGHT;
         this.copyrightText.updateMetrics();
         this.copyrightText.filters = [new DropShadowFilter(0,0,0)];
         this.container.addChild(this.copyrightText);
      }
      
      public function initialize(data:EnvironmentData) : void
      {
         this.data = data;
         this.updateVersionText();
         this.positionButtons();
         this.addChildren();
      }
      
      private function updateVersionText() : void
      {
         this.versionText.htmlText = this.data.buildLabel;
         this.versionText.updateMetrics();
      }
      
      private function addChildren() : void
      {
         //addChild(this.container);
         //this.data.isAdmin && this.container.addChild(this.editorButton);
      }
      
      private function positionButtons() : void
      {
         /*this.playButton.x = stage.stageWidth / 2 - this.playButton.width / 2;
         this.playButton.y = 520;
         this.serversButton.x = stage.stageWidth / 2 - this.serversButton.width / 2 - 94;
         this.serversButton.y = 532;
         this.creditsButton.x = 180;
         this.creditsButton.y = 532;
         this.accountButton.x = stage.stageWidth / 2 - this.accountButton.width / 2 + 96;
         this.accountButton.y = 532;
         this.legendsButton.x = 550;
         this.legendsButton.y = 532;
         this.editorButton.x = 100;
         this.editorButton.y = 532;*/
         this.versionText.y = stage.stageHeight - this.versionText.height;
         this.copyrightText.x = stage.stageWidth - this.copyrightText.width;
         this.copyrightText.y = stage.stageHeight - this.copyrightText.height;
      }
   }
}
