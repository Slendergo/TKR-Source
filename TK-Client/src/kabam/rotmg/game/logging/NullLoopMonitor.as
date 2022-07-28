package kabam.rotmg.game.logging
{
   public class NullLoopMonitor implements LoopMonitor
   {
       
      
      public function NullLoopMonitor()
      {
         super();
      }
      
      public function recordTime(name:String, deltaTime:int) : void
      {
      }
   }
}
