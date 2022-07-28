package com.company.assembleegameclient.map.mapoverlay
{
   import com.company.assembleegameclient.map.Camera;
   import com.company.assembleegameclient.objects.GameObject;
   import com.company.ui.SimpleText;
   import flash.display.Sprite;
   import flash.filters.GlowFilter;
   import flash.geom.Point;
   
   public class CharacterStatusText extends Sprite implements IMapOverlayElement
   {
       
      
      public const MAX_DRIFT:int = 40;
      
      public var go_:GameObject;
      
      public var offset_:Point;
      
      public var color_:uint;
      
      public var lifetime_:int;
      
      public var offsetTime_:int;
      
      private var startTime_:int = 0;
      
      public function CharacterStatusText(go:GameObject, text:String, color:uint, lifetime:int, offsetTime:int = 0)
      {
         super();
         this.go_ = go;
         this.offset_ = new Point(0,-go.texture_.height * (go.size_ / 100) * 5 - 20);
         this.color_ = color;
         this.lifetime_ = lifetime;
         this.offsetTime_ = offsetTime;
         var t:SimpleText = new SimpleText(24,color,false,0,0);
         t.setBold(true);
         t.text = text;
         t.updateMetrics();
         t.filters = [new GlowFilter(0,1,4,4,2,1)];
         t.x = -t.width / 2;
         t.y = -t.height / 2;
         addChild(t);
         visible = false;
      }
      
      public function draw(camera:Camera, time:int) : Boolean
      {
         var dt:int = 0;
         var drift:Number = NaN;
         if(this.startTime_ == 0)
         {
            this.startTime_ = time + this.offsetTime_;
         }
         if(time < this.startTime_)
         {
            visible = false;
            return true;
         }
         dt = time - this.startTime_;
         if(dt > this.lifetime_ || this.go_ != null && this.go_.map_ == null)
         {
            return false;
         }
         if(this.go_ == null || !this.go_.drawn_)
         {
            visible = false;
            return true;
         }
         visible = true;
         x = (this.go_ != null?this.go_.posS_[0]:0) + (this.offset_ != null?this.offset_.x:0);
         drift = dt / this.lifetime_ * this.MAX_DRIFT;
         y = (this.go_ != null?this.go_.posS_[1]:0) + (this.offset_ != null?this.offset_.y:0) - drift;
         return true;
      }
      
      public function getGameObject() : GameObject
      {
         return this.go_;
      }
      
      public function dispose() : void
      {
         parent.removeChild(this);
      }
   }
}
