package kabam.rotmg.assets.model
{
   import flash.display.Bitmap;
   import flash.display.BitmapData;
   import flash.display.Sprite;
   import flash.events.TimerEvent;
   import flash.utils.Timer;
   
   public class Animation extends Sprite
   {
       
      
      private const DEFAULT_SPEED:int = 200;
      
      private const bitmap:Bitmap = makeBitmap();
      
      private const frames:Vector.<BitmapData> = new Vector.<BitmapData>(0);
      
      private const timer:Timer = makeTimer();
      
      private var isStarted:Boolean;
      
      private var index:int;
      
      private var count:uint;
      
      public function Animation()
      {
         super();
      }
      
      private function makeBitmap() : Bitmap
      {
         var bitmap:Bitmap = new Bitmap();
         addChild(bitmap);
         return bitmap;
      }
      
      private function makeTimer() : Timer
      {
         var timer:Timer = new Timer(this.DEFAULT_SPEED);
         timer.addEventListener(TimerEvent.TIMER,this.iterate);
         return timer;
      }
      
      public function getSpeed() : int
      {
         return this.timer.delay;
      }
      
      public function setSpeed(speed:int) : void
      {
         this.timer.delay = speed;
      }
      
      public function setFrames(... newFrames) : void
      {
         var frame:BitmapData = null;
         this.frames.length = 0;
         this.index = 0;
         for each(frame in newFrames)
         {
            this.count = this.frames.push(frame);
         }
         if(this.isStarted)
         {
            this.start();
         }
         else
         {
            this.iterate();
         }
      }
      
      public function addFrame(frame:BitmapData) : void
      {
         this.count = this.frames.push(frame);
         this.isStarted && this.start();
      }
      
      public function start() : void
      {
         if(!this.isStarted && this.count > 0)
         {
            this.timer.start();
            this.iterate();
         }
         this.isStarted = true;
      }
      
      public function stop() : void
      {
         this.isStarted && this.timer.stop();
         this.isStarted = false;
      }
      
      private function iterate(event:TimerEvent = null) : void
      {
         this.index = ++this.index % this.count;
         this.bitmap.bitmapData = this.frames[this.index];
      }
      
      public function dispose() : void
      {
         var frame:BitmapData = null;
         this.stop();
         this.index = 0;
         this.count = 0;
         this.frames.length = 0;
         for each(frame in this.frames)
         {
            frame.dispose();
         }
      }
   }
}
