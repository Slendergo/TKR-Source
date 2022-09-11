package kabam.rotmg.messaging.impl.outgoing
{
import avmplus.constructorXml;

import flash.utils.IDataOutput;
   import kabam.rotmg.messaging.impl.data.WorldPosData;
   
   public class PlayerShoot
   {
      public var time_:int;
      public var bulletId_:uint;
      public var containerType_:int;
      public var startingPosX_:Number;
      public var startingPosY_:Number;
      public var angle_:Number;

      public function writeToOutput(data:IDataOutput) : void
      {
         data.writeInt(this.time_);
         data.writeByte(this.bulletId_);
         data.writeInt(this.containerType_);
         data.writeFloat(this.startingPosX_);
         data.writeFloat(this.startingPosY_);
         data.writeFloat(this.angle_);
      }
   }
}
