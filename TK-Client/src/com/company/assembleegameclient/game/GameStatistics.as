package com.company.assembleegameclient.game {
import com.company.assembleegameclient.util.FilterUtil;
import com.company.ui.SimpleText;

import flash.display.Sprite;
import flash.system.System;

public class GameStatistics extends Sprite {

   private var gs:GameSprite;
   private var fpsText:SimpleText;
   private var pingText:SimpleText;
   private var memText:SimpleText;
   private var lastPing:int;

   public function GameStatistics(gs:GameSprite) {
      this.gs = gs;
      this.fpsText = new SimpleText(17, 0xFFFFFF);
      this.fpsText.text = "FPS";
      this.fpsText.autoSize = "center";
      this.fpsText.setBold(true);
      this.fpsText.useTextDimensions();
      this.fpsText.filters = FilterUtil.getTextOutlineFilter();
      this.fpsText.x = -this.fpsText.width;
      addChild(this.fpsText);
      this.pingText = new SimpleText(17, 0xFFFFFF);
      this.pingText.text = "PING";
      this.pingText.autoSize = "center";
      this.pingText.setBold(true);
      this.pingText.useTextDimensions();
      this.pingText.filters = FilterUtil.getTextOutlineFilter();
      this.pingText.x = -this.pingText.width;
      this.pingText.y = this.fpsText.height;
      addChild(this.pingText);
      this.memText = new SimpleText(17, 0xFFFFFF);
      this.memText.text = "MEM";
      this.memText.autoSize = "center";
      this.memText.setBold(true);
      this.memText.useTextDimensions();
      this.memText.filters = FilterUtil.getTextOutlineFilter();
      this.memText.x = -this.memText.width;
      this.memText.y = this.pingText.y + this.pingText.height;
      addChild(this.memText);
   }

   public function update(avgFPS:Number):void {
      this.fpsText.setText("FPS: " + avgFPS);
      this.fpsText.x = -this.fpsText.width;

      this.pingText.setText("PING: " + lastPing);
      this.pingText.x = -this.pingText.width;

      this.memText.setText("MEM: " + int(System.privateMemory / 1000000));
      this.memText.x = -this.memText.width;
   }
}
}