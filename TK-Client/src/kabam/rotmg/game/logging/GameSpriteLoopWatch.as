package kabam.rotmg.game.logging
{

public class GameSpriteLoopWatch
   {
      private static const COUNT:int = 10;
       
      
      private var times:Vector.<int>;
      
      private var index:int;
      
      private var count:int;
      
      public var rollingTotal:int;
      
      public var mean:int;
      
      public var max:int;
      
      public var min:int;
      
      public function GameSpriteLoopWatch()
      {
         this.times = new Vector.<int>(COUNT,true);
         this.index = 0;
         this.rollingTotal = 0;
         this.count = 0;
         this.min = int.MAX_VALUE;
         this.max = -1;
      }
      
      public function logTime(time:int) : void
      {
         if(this.count < COUNT)
         {
            this.rollingTotal = this.rollingTotal + time;
            this.count++;
            this.times[this.index] = time;
         }
         else
         {
            this.rollingTotal = this.rollingTotal - this.times[this.index];
            this.rollingTotal = this.rollingTotal + time;
            this.times[this.index] = time;
         }
         if(++this.index == COUNT)
         {
            this.index = 0;
         }
         this.mean = this.rollingTotal / this.count;
         if(time > this.max)
         {
            this.max = time;
         }
         if(time < this.min)
         {
            this.min = time;
         }
      }
   }
}
