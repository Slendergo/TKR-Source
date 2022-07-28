package com.company.assembleegameclient.screens
{
   import com.company.rotmg.graphics.ScreenGraphic;
   import com.company.ui.SimpleText;
   import flash.display.Sprite;
   import flash.events.Event;
   import flash.filters.DropShadowFilter;
   import flash.text.TextFieldAutoSize;
   
   public class AccountLoadingScreen extends Sprite
   {
       
      
      private var loadingText_:SimpleText;
      
      public function AccountLoadingScreen()
      {
         super();
         addChild(new ScreenGraphic());
         this.loadingText_ = new SimpleText(30,16777215,false,0,0);
         this.loadingText_.setBold(true);
         this.loadingText_.htmlText = "<p align=\"center\">Loading...</p>";
         this.loadingText_.autoSize = TextFieldAutoSize.CENTER;
         this.loadingText_.updateMetrics();
         this.loadingText_.filters = [new DropShadowFilter(0,0,0,1,4,4)];
         addChild(this.loadingText_);
         addEventListener(Event.ADDED_TO_STAGE,this.onAddedToStage);
      }
      
      protected function onAddedToStage(event:Event) : void
      {
         removeEventListener(Event.ADDED_TO_STAGE,this.onAddedToStage);
         this.loadingText_.x = stage.stageWidth / 2 - this.loadingText_.width / 2;
         this.loadingText_.y = 526;
      }
   }
}
