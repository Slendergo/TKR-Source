package kabam.rotmg.messaging.impl.outgoing.talisman
{
import kabam.rotmg.messaging.impl.outgoing.*;
   import flash.utils.IDataOutput;
   
   public class TalismanEssenceAction extends OutgoingMessage
   {
      public static const ADD_ESSENCE:int = 0;
      public static const TIER_UP:int = 1;
      public static const ENABLE:int = 2;
      public static const DISABLE:int = 3;

      public var actionType_:int;
      public var type_:int;
      public var amount_:int;

      public function TalismanEssenceAction(id:uint, callback:Function)
      {
         super(id,callback);
      }
      
      override public function writeToOutput(data:IDataOutput) : void
      {
         data.writeByte(this.actionType_);
         data.writeInt(this.type_);
         data.writeInt(this.amount_);
      }
      
      override public function toString() : String
      {
         return formatToString("TALISMAN_ESSENCE_ACTION","actionType_", "expectedCost_");
      }
   }
}
