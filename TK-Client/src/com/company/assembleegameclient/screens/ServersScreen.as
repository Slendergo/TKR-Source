package com.company.assembleegameclient.screens
{
   import com.company.assembleegameclient.ui.Scrollbar;
   import com.company.rotmg.graphics.ScreenGraphic;
   import com.company.ui.SimpleText;
   import flash.display.Graphics;
   import flash.display.Shape;
   import flash.display.Sprite;
   import flash.events.Event;
   import flash.events.MouseEvent;
   import flash.filters.DropShadowFilter;
   import kabam.rotmg.servers.api.Server;
   import kabam.rotmg.ui.view.components.ScreenBase;
   import org.osflash.signals.Signal;
   
   public class ServersScreen extends Sprite
   {
       
      
      private var doneButton_:TitleMenuOption;
      
      private var selectServerText_:SimpleText;
      
      private var lines_:Shape;
      
      private var content_:Sprite;
      
      private var serverBoxes_:ServerBoxes;
      
      private var scrollBar_:Scrollbar;
      
      public var gotoTitle:Signal;
      
      public function ServersScreen()
      {
         super();
         addChild(new ScreenBase());
         this.gotoTitle = new Signal();
         addChild(new ScreenBase());
         addChild(new AccountScreen());
      }
      
      private function onScrollBarChange(event:Event) : void
      {
         this.serverBoxes_.y = 8 - this.scrollBar_.pos() * (this.serverBoxes_.height - 400);
      }
      
      public function initialize(servers:Vector.<Server>) : void
      {
         this.selectServerText_ = new SimpleText(18,11776947,false,0,0);
         this.selectServerText_.setBold(true);
         this.selectServerText_.text = "Select Server";
         this.selectServerText_.updateMetrics();
         this.selectServerText_.filters = [new DropShadowFilter(0,0,0,1,8,8)];
         this.selectServerText_.x = 18;
         this.selectServerText_.y = 72;
         addChild(this.selectServerText_);
         this.lines_ = new Shape();
         addChild(this.lines_);
         this.content_ = new Sprite();
         this.content_.x = 4;
         this.content_.y = 100;
         var maskShape:Shape = new Shape();
         maskShape.graphics.beginFill(16777215);
         maskShape.graphics.drawRect(0,0,776,430);
         maskShape.graphics.endFill();
         this.content_.addChild(maskShape);
         this.content_.mask = maskShape;
         addChild(this.content_);
         this.serverBoxes_ = new ServerBoxes(servers);
         this.serverBoxes_.y = 8;
         this.serverBoxes_.addEventListener(Event.COMPLETE,this.onDone);
         this.content_.addChild(this.serverBoxes_);
         if(this.serverBoxes_.height > 400)
         {
            this.scrollBar_ = new Scrollbar(16,400);
            this.scrollBar_.x = 800 - this.scrollBar_.width - 4;
            this.scrollBar_.y = 104;
            this.scrollBar_.setIndicatorSize(400,this.serverBoxes_.height);
            this.scrollBar_.addEventListener(Event.CHANGE,this.onScrollBarChange);
            addChild(this.scrollBar_);
         }
         addChild(new ScreenGraphic());
         this.doneButton_ = new TitleMenuOption("done",36,false);
         this.doneButton_.addEventListener(MouseEvent.CLICK,this.onDone);
         addChild(this.doneButton_);
         var g:Graphics = this.lines_.graphics;
         g.clear();
         g.lineStyle(2,5526612);
         g.moveTo(0,100);
         g.lineTo(stage.stageWidth,100);
         g.lineStyle();
         this.doneButton_.x = stage.stageWidth / 2 - this.doneButton_.width / 2;
         this.doneButton_.y = 524;
      }
      
      private function onDone(event:Event) : void
      {
         this.gotoTitle.dispatch();
      }
   }
}
