package com.company.assembleegameclient.objects
{
import flash.display.BitmapData;
import flash.geom.ColorTransform;
import kabam.rotmg.stage3D.GraphicsFillExtra;

public class FlashDescription
{


   public var startTime_:int;

   public var color_:uint;

   public var periodMS_:int;

   public var repeats_:int;

   public var targetR:int;

   public var targetG:int;

   public var targetB:int;

   public function FlashDescription(startTime:int, color:uint, period_:Number, repeats:int)
   {
      super();
      this.startTime_ = startTime;
      this.color_ = color;
      this.periodMS_ = period_ * 1000;
      this.repeats_ = repeats;
      this.targetR = color >> 16 & 255;
      this.targetG = color >> 8 & 255;
      this.targetB = color & 255;
   }

   public function apply(texture:BitmapData, time:int) : BitmapData
   {
      var t:int = (time - this.startTime_) % this.periodMS_;
      var v:Number = Math.sin(t / this.periodMS_ * Math.PI);
      var mv:Number = v * 0.5;
      var ct:ColorTransform = new ColorTransform(1 - mv,1 - mv,1 - mv,1,mv * this.targetR,mv * this.targetG,mv * this.targetB,0);
      var newTexture:BitmapData = texture.clone();
      newTexture.colorTransform(newTexture.rect,ct);
      return newTexture;
   }

   public function applyGPUTextureColorTransform(texture:BitmapData, time:int) : void
   {
      var t:int = (time - this.startTime_) % this.periodMS_;
      var v:Number = Math.sin(t / this.periodMS_ * Math.PI);
      var mv:Number = v * 0.5;
      var ct:ColorTransform = new ColorTransform(1 - mv,1 - mv,1 - mv,1,mv * this.targetR,mv * this.targetG,mv * this.targetB,0);
      GraphicsFillExtra.setColorTransform(texture,ct);
   }

   public function doneAt(time:int) : Boolean
   {
      return time > this.startTime_ + this.periodMS_ * this.repeats_;
   }
}
}
