package com.company.assembleegameclient.screens
{
   import com.company.assembleegameclient.ui.ClickableText;
   import com.company.assembleegameclient.ui.Scrollbar;
import com.company.assembleegameclient.ui.SoundIcon;
import com.company.rotmg.graphics.ScreenGraphic;
   import com.company.ui.SimpleText;
   import flash.display.DisplayObject;
   import flash.display.Shape;
   import flash.display.Sprite;
   import flash.events.Event;
   import flash.events.MouseEvent;
   import flash.filters.DropShadowFilter;
   import flash.geom.Rectangle;

import io.decagames.rotmg.ui.buttons.SliceScalingButton;
import io.decagames.rotmg.ui.scroll.UIScrollbar;
import io.decagames.rotmg.ui.sliceScaling.SliceScalingBitmap;
import io.decagames.rotmg.ui.texture.TextureParser;

import kabam.rotmg.core.StaticInjectorContext;

import kabam.rotmg.core.model.PlayerModel;
import kabam.rotmg.dialogs.control.OpenDialogSignal;
import kabam.rotmg.game.view.CreditDisplay;
   import kabam.rotmg.news.view.NewsView;
import kabam.rotmg.servers.api.ServerModel;
import kabam.rotmg.ui.noservers.NoServersDialogFactory;
import kabam.rotmg.ui.view.ButtonFactory;
import kabam.rotmg.ui.view.components.MenuOptionsBar;
import kabam.rotmg.ui.view.components.ScreenBase;
   import org.osflash.signals.Signal;
   import org.osflash.signals.natives.NativeMappedSignal;
   
   public class CharacterSelectionAndNewsScreen extends Sprite
   {

       private static const TAB_UNSELECTED:uint = 0xB3B3B3;
       private static const TAB_SELECTED:uint = 0xFFFFFF;
       private const CHARACTER_LIST_Y_POS:int = 70;
       private const CHARACTER_LIST_X_POS:int = 200;
       private var characterListType:int = 1;

      private const SCROLLBAR_REQUIREMENT_HEIGHT:Number = 400;
      private const DROP_SHADOW:DropShadowFilter = new DropShadowFilter(0,0,0,1,8,8);
      private var model:PlayerModel;
      private var isInitialized:Boolean;
      private var nameText:SimpleText;
      private var nameChooseLink_:ClickableText;
      private var creditDisplay:CreditDisplay;
      private var selectACharacterText:SimpleText;
      private var characterList:CharacterList;
      private var characterListHeight:Number;
      private var playButton:SliceScalingButton;
      private var backButton:SliceScalingButton;
      private var classesButton:SliceScalingButton;
      private var menuOptionsBar:MenuOptionsBar;
      private var buttonsBackground:SliceScalingBitmap;
      private var lines:Shape;
      private var scrollBar:UIScrollbar;
      public var close:Signal;
      public var showClasses:Signal;
      public var newCharacter:Signal;
      public var chooseName:Signal;
      public var playGame:Signal;

       private var openCharactersText:SimpleText;
       private var openGraveyardText:SimpleText;

      public function CharacterSelectionAndNewsScreen()
      {
         this.playButton = ButtonFactory.getPlayButton();
         this.backButton = ButtonFactory.getMainButton();
         this.classesButton = ButtonFactory.getClassesButton();
         this.newCharacter = new Signal();
         this.chooseName = new Signal();
         this.playGame = new Signal();
         super();
         addChild(new ScreenBase());
         addChild(new AccountScreen(true));
         addChild(new SoundIcon());
         this.close = new NativeMappedSignal(this.backButton,MouseEvent.CLICK);
         this.showClasses = new NativeMappedSignal(this.classesButton,MouseEvent.CLICK);
      }
      
      public function initialize(model:PlayerModel) : void
      {
         if(this.isInitialized)
         {
            return;
         }
         this.isInitialized = true;
         this.model = model;
         this.createDisplayAssets(model);
      }
      
      private function createDisplayAssets(model:PlayerModel) : void
      {
         this.createNameText();
         this.createCreditDisplay();
         this.createSelectCharacterText();
         this.createBoundaryLines();
         this.createCharacterList();
         this.positionButtons();
          this.createOpenCharactersText();
          var _local2:Graveyard = new Graveyard(model);
          if (_local2.hasCharacters()) {
              this.openCharactersText.setColor(TAB_SELECTED);
              this.createOpenGraveyardText();
          }
         if(this.characterListHeight > this.SCROLLBAR_REQUIREMENT_HEIGHT)
         {
            this.createScrollbar();
         }
         if(!model.isNameChosen())
         {
            this.createChooseNameLink();
         }
         this.buttonsBackground = TextureParser.instance.getSliceScalingBitmap("UI","popup_header_title",800);
         this.buttonsBackground.y = 502.5;
         addChild(this.buttonsBackground);
         this.makeMenuOptionsBar();
      }
       private function removeCharacterList():void {
           if (this.characterList != null) {
               removeChild(this.characterList);
               this.characterList = null;
           }
           if (this.scrollBar != null) {
               removeChild(this.scrollBar);
               this.scrollBar = null;
           }
       }
       private function createOpenCharactersText():void {
           this.openCharactersText = new SimpleText(18,TAB_UNSELECTED);
           this.openCharactersText.setBold(true);
           this.openCharactersText.filters = [this.DROP_SHADOW];
           this.openCharactersText.x = this.CHARACTER_LIST_X_POS;
           this.openCharactersText.y = 79;
           this.openCharactersText.addEventListener(MouseEvent.CLICK, this.onOpenCharacters);
           addChild(this.openCharactersText);
       }

       private function onOpenCharacters(_arg1:MouseEvent):void {
           if (this.characterListType != CharacterList.TYPE_CHAR_SELECT) {
               this.removeCharacterList();
               this.openCharactersText.setColor(TAB_SELECTED);
               this.openGraveyardText.setColor(TAB_UNSELECTED);
               this.createCharacterList();
           }
       }

       private function createOpenGraveyardText():void {
           this.openGraveyardText = new SimpleText(18, TAB_UNSELECTED);
           this.openGraveyardText.setBold(true);
           this.openGraveyardText.filters = [this.DROP_SHADOW];
           this.openGraveyardText.x = (this.CHARACTER_LIST_X_POS + 150);
           this.openGraveyardText.y = 79;
           this.openGraveyardText.addEventListener(MouseEvent.CLICK, this.onOpenGraveyard);
           addChild(this.openGraveyardText);
       }

       private function onOpenGraveyard(_arg1:MouseEvent):void {
           if (this.characterListType != CharacterList.TYPE_GRAVE_SELECT) {
               this.removeCharacterList();
               this.openCharactersText.setColor(TAB_UNSELECTED);
               this.openGraveyardText.setColor(TAB_SELECTED);
               this.createCharacterListGrave();
           }
       }
       private function createCharacterListGrave():void {
           this.characterListType = CharacterList.TYPE_GRAVE_SELECT;
           this.characterList = new CharacterList(this.model, CharacterList.TYPE_GRAVE_SELECT);
           this.characterList.x = this.CHARACTER_LIST_X_POS;
           this.characterList.y = this.CHARACTER_LIST_Y_POS;
           this.characterListHeight = this.characterList.height;
           if (this.characterListHeight > this.SCROLLBAR_REQUIREMENT_HEIGHT) {
               this.createScrollbar();
           }
           addChild(this.characterList);
       }
      private function makeMenuOptionsBar():void {
         this.menuOptionsBar = new MenuOptionsBar();
         this.menuOptionsBar.addButton(this.playButton, MenuOptionsBar.CENTER);
         this.menuOptionsBar.addButton(this.backButton, MenuOptionsBar.LEFT);
         this.menuOptionsBar.addButton(this.classesButton, MenuOptionsBar.RIGHT);
         this.playButton.clicked2.addOnce(this.onPlayClick);
         addChild(this.menuOptionsBar);
      }

      private function positionButtons() : void
      {
         this.playButton.x = (this.getReferenceRectangle().width - this.playButton.width) / 2;
         this.playButton.y = 520;
         this.backButton.x = (this.getReferenceRectangle().width - this.backButton.width) / 2 - 94;
         this.backButton.y = 532;
         this.classesButton.x = (this.getReferenceRectangle().width - this.classesButton.width) / 2 + 96;
         this.classesButton.y = 532;
      }

      private function createScrollbar():void {
         this.scrollBar = new UIScrollbar(400);
         this.scrollBar.x = 780;
         this.scrollBar.y = 113;
         this.scrollBar.scrollObject = this.characterList;
         this.scrollBar.mouseRollSpeedFactor = 4;
         this.scrollBar.content = this.characterList.charRectList_;
         addChild(this.scrollBar);
      }
      
      private function createCharacterList() : void
      {
         this.characterList = new CharacterList(this.model, CharacterList.TYPE_CHAR_SELECT);
         this.characterList.x = 18;
         this.characterList.y = 112;
         this.characterListHeight = this.characterList.height;
         addChild(this.characterList);
      }

      private function createSelectCharacterText() : void
      {
         this.selectACharacterText = new SimpleText(18,11776947,false,0,0);
         this.selectACharacterText.setBold(true);
         this.selectACharacterText.text = "Characters";
         this.selectACharacterText.updateMetrics();
         this.selectACharacterText.filters = [this.DROP_SHADOW];
         this.selectACharacterText.x = 34;
         this.selectACharacterText.y = 79;
         addChild(this.selectACharacterText);
      }
      
      private function createCreditDisplay() : void
      {
         this.creditDisplay = new CreditDisplay();
         this.creditDisplay.draw(this.model.getCredits(),this.model.getFame());
         this.creditDisplay.x = this.getReferenceRectangle().width;
         this.creditDisplay.y = 20;
         addChild(this.creditDisplay);
      }
      
      private function createChooseNameLink() : void
      {
         this.nameChooseLink_ = new ClickableText(16,false,"choose name");
         this.nameChooseLink_.y = 50;
         this.nameChooseLink_.x = this.getReferenceRectangle().width / 2 - this.nameChooseLink_.width / 2;
         this.nameChooseLink_.addEventListener(MouseEvent.CLICK,this.onChooseName);
         addChild(this.nameChooseLink_);
      }
      
      private function createNameText() : void
      {
         this.nameText = new SimpleText(22,11776947,false,0,0);
         this.nameText.setBold(true);
         this.nameText.text = this.model.getName();
         this.nameText.updateMetrics();
         this.nameText.filters = [this.DROP_SHADOW];
         this.nameText.y = 20;
         this.nameText.x = (this.getReferenceRectangle().width - this.nameText.width) / 2;
         addChild(this.nameText);
      }
      
      internal function getReferenceRectangle() : Rectangle
      {
         var rectangle:Rectangle = new Rectangle();
         if(stage)
         {
            rectangle = new Rectangle(0,0,stage.stageWidth,stage.stageHeight);
         }
         return rectangle;
      }

      private function createBoundaryLines():void {
         this.lines = new Shape();
         this.lines.graphics.clear();
         this.lines.graphics.lineStyle(2, 0x545454);
         this.lines.graphics.moveTo(0, 106);
         this.lines.graphics.lineTo(this.getReferenceRectangle().width, 106);
         this.lines.graphics.moveTo(810, 107);
         this.lines.graphics.lineTo(810, 526);
         this.lines.graphics.lineStyle();
         addChild(this.lines);
      }
      
      private function onChooseName(event:MouseEvent) : void
      {
         this.chooseName.dispatch();
      }
      
      private function removeIfAble(object:DisplayObject) : void
      {
         if(object && contains(object))
         {
            removeChild(object);
         }
      }
      
      private function onPlayClick(event:Event) : void
      {
         var servers:ServerModel = StaticInjectorContext.getInjector().getInstance(ServerModel);
         if(!servers.isServerAvailable())
         {
            StaticInjectorContext.getInjector().getInstance(OpenDialogSignal).dispatch(StaticInjectorContext.getInjector().getInstance(NoServersDialogFactory).makeDialog());
         }
         else if(this.model.getCharacterCount() == 0)
         {
            this.newCharacter.dispatch();
         }
         else
         {
            this.playGame.dispatch();
         }
      }
      
      public function setName(name:String) : void
      {
         this.nameText.text = name;
         this.nameText.updateMetrics();
         this.nameText.x = (this.getReferenceRectangle().width - this.nameText.width) * 0.5;
         if(this.nameChooseLink_)
         {
            removeChild(this.nameChooseLink_);
            this.nameChooseLink_ = null;
         }
      }
   }
}
