package kabam.rotmg.game.logging
{
   public class RollingMeanLoopMonitor implements LoopMonitor
   {
      private var watchMap:Object;
      
      public function RollingMeanLoopMonitor()
      {
         super();
         this.watchMap = {};
      }
      
      public function recordTime(name:String, deltaTime:int) : void
      {
         var data:GameSpriteLoopWatch = this.watchMap[name] = this.watchMap[name] || new GameSpriteLoopWatch();
         data.logTime(deltaTime);
      }
   }
}
