package com.company.assembleegameclient.screens
{
   import com.company.assembleegameclient.appengine.SavedCharactersList;
   import com.company.assembleegameclient.objects.ObjectLibrary;
   import com.company.rotmg.graphics.ScreenGraphic;
   import flash.display.Sprite;
   import flash.events.Event;
   import flash.events.MouseEvent;

import io.decagames.rotmg.ui.buttons.SliceScalingButton;
import io.decagames.rotmg.ui.defaults.DefaultLabelFormat;
import io.decagames.rotmg.ui.sliceScaling.SliceScalingBitmap;
import io.decagames.rotmg.ui.texture.TextureParser;

import kabam.rotmg.core.model.PlayerModel;
   import kabam.rotmg.game.view.CreditDisplay;
   import kabam.rotmg.ui.view.components.ScreenBase;
   import org.osflash.signals.Signal;
   
   public class NewCharacterScreen extends Sprite
   {
      private var backButton_:SliceScalingButton;
      private var buttonsBackground:SliceScalingBitmap;
      private var creditDisplay_:CreditDisplay;
      private var boxes_:Object;
      public var tooltip:Signal;
      public var close:Signal;
      public var selected:Signal;
      public var buy:Signal;
      
      private var isInitialized:Boolean = false;
      
      public function NewCharacterScreen()
      {
         this.boxes_ = {};
         super();
         this.tooltip = new Signal(Sprite);
         this.selected = new Signal(int);
         this.close = new Signal();
         this.buy = new Signal(int);
         addChild(new ScreenBase());
         addChild(new AccountScreen());
         //addChild(new ScreenGraphic());
      }
      
      public function initialize(model:PlayerModel) : void
      {
         var playerXML:XML = null;
         var objectType:int = 0;
         var characterType:String = null;
         var overrideIsAvailable:Boolean = false;
         var charBox:CharacterBox = null;
         if(this.isInitialized)
         {
            return;
         }
         this.isInitialized = true;
         this.buttonsBackground = TextureParser.instance.getSliceScalingBitmap("UI","popup_header_title",800);
         this.buttonsBackground.y = 502.5;
         addChild(this.buttonsBackground);
         this.backButton_ = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI","generic_green_button"));
         this.backButton_.x = 350;
         this.backButton_.y = 520;
         this.backButton_.width = 100;
         this.backButton_.setLabel("done",DefaultLabelFormat.questButtonCompleteLabel);
         this.backButton_.addEventListener(MouseEvent.CLICK,this.onBackClick);
         addChild(this.backButton_);
         this.creditDisplay_ = new CreditDisplay();
         this.creditDisplay_.draw(model.getCredits(),model.getFame());
         addChild(this.creditDisplay_);
         for(var i:int = 0; i < ObjectLibrary.playerChars_.length; i++)
         {
            playerXML = ObjectLibrary.playerChars_[i];
            objectType = int(playerXML.@type);
            characterType = playerXML.@id;
            if(!model.isClassAvailability(characterType,SavedCharactersList.UNAVAILABLE))
            {
               overrideIsAvailable = model.isClassAvailability(characterType,SavedCharactersList.UNRESTRICTED);
               charBox = new CharacterBox(playerXML,model.getCharStats()[objectType],model,overrideIsAvailable);
               charBox.x = 50 + 140 * int(i % 5) + 70 - charBox.width / 2;
               charBox.y = 88 + 140 * int(i / 5);
               this.boxes_[objectType] = charBox;
               charBox.addEventListener(MouseEvent.ROLL_OVER,this.onCharBoxOver);
               charBox.addEventListener(MouseEvent.ROLL_OUT,this.onCharBoxOut);
               charBox.characterSelectClicked_.add(this.onCharBoxClick);
               charBox.buyButtonClicked_.add(this.onBuyClicked);
               addChild(charBox);
            }
         }
         this.backButton_.x = stage.stageWidth / 2 - this.backButton_.width / 2;
         this.backButton_.y = 520;
         this.creditDisplay_.x = stage.stageWidth;
         this.creditDisplay_.y = 20;
      }
      
      private function onBackClick(event:Event) : void
      {
         this.close.dispatch();
      }
      
      private function onCharBoxOver(event:MouseEvent) : void
      {
         var charBox:CharacterBox = event.currentTarget as CharacterBox;
         charBox.setOver(true);
         this.tooltip.dispatch(charBox.getTooltip());
      }
      
      private function onCharBoxOut(event:MouseEvent) : void
      {
         var charBox:CharacterBox = event.currentTarget as CharacterBox;
         charBox.setOver(false);
         this.tooltip.dispatch(null);
      }
      
      private function onCharBoxClick(event:MouseEvent) : void
      {
         this.tooltip.dispatch(null);
         var charBox:CharacterBox = event.currentTarget.parent as CharacterBox;
         if(!charBox.available_)
         {
            return;
         }
         var objectType:int = charBox.objectType();
         var displayId:String = ObjectLibrary.typeToDisplayId_[objectType];
         this.selected.dispatch(objectType);
      }
      
      public function updateCreditsAndFame(credits:int, fame:int) : void
      {
         this.creditDisplay_.draw(credits,fame);
      }
      
      public function update(model:PlayerModel) : void
      {
         var playerXML:XML = null;
         var objectType:int = 0;
         var characterType:String = null;
         var overrideIsAvailable:Boolean = false;
         var charBox:CharacterBox = null;
         for(var i:int = 0; i < ObjectLibrary.playerChars_.length; i++)
         {
            playerXML = ObjectLibrary.playerChars_[i];
            objectType = int(playerXML.@type);
            characterType = String(playerXML.@id);
            if(!model.isClassAvailability(characterType,SavedCharactersList.UNAVAILABLE))
            {
               overrideIsAvailable = model.isClassAvailability(characterType,SavedCharactersList.UNRESTRICTED);
               charBox = this.boxes_[objectType];
               if(charBox)
               {
                  charBox.setIsBuyButtonEnabled(true);
                  if(overrideIsAvailable || model.isLevelRequirementsMet(objectType))
                  {
                     charBox.unlock();
                  }
               }
            }
         }
      }
      
      private function onBuyClicked(e:MouseEvent) : void
      {
         var objectType:int = 0;
         var charBox:CharacterBox = e.target.parent as CharacterBox;
         if(charBox && !charBox.available_)
         {
            objectType = int(charBox.playerXML_.@type);
            charBox.setIsBuyButtonEnabled(false);
            this.buy.dispatch(objectType);
         }
      }
   }
}
