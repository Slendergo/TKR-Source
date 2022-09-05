package com.company.assembleegameclient.game {
import com.company.assembleegameclient.util.FilterUtil;
import com.company.ui.SimpleText;

import flash.display.Sprite;
import flash.system.System;

public class GameStatistics extends Sprite {

   private var fps_:SimpleText;
   private var ping_:SimpleText;
   private var mem_:SimpleText;
   private var lastPing_:int;

   public function GameStatistics() {
      this.fps_ = new SimpleText(17, 0xFFFFFF);
      this.fps_.text = "FPS";
      this.fps_.autoSize = "center";
      this.fps_.setBold(true);
      this.fps_.setAlignment("left");
      this.fps_.useTextDimensions();
      this.fps_.filters = FilterUtil.getTextOutlineFilter();
      addChild(this.fps_);
      this.ping_ = new SimpleText(17, 0xFFFFFF);
      this.ping_.text = "PING";
      this.ping_.autoSize = "center";
      this.ping_.setBold(true);
      this.ping_.setAlignment("left");
      this.ping_.useTextDimensions();
      this.ping_.filters = FilterUtil.getTextOutlineFilter();
      this.ping_.y = this.fps_.height;
      addChild(this.ping_);
      this.mem_ = new SimpleText(17, 0xFFFFFF);
      this.mem_.text = "MEM";
      this.mem_.autoSize = "center";
      this.mem_.setBold(true);
      this.mem_.setAlignment("left");
      this.mem_.useTextDimensions();
      this.mem_.filters = FilterUtil.getTextOutlineFilter();
      this.mem_.y = this.ping_.y + this.ping_.height;
      addChild(this.mem_);
   }

   public function setPing(ping:int):void{
      this.lastPing_ = ping;
   }

   public function update(avgFPS:Number):void {
      this.fps_.setText("FPS: " + avgFPS);
      this.ping_.setText("PING: " + lastPing_);
      this.mem_.setText("MEM: " + int(System.privateMemory / 1000000));
   }
}
}