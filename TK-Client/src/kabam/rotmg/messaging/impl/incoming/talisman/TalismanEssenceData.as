package kabam.rotmg.messaging.impl.incoming.talisman
{
import com.company.assembleegameclient.util.FreeList;

import kabam.rotmg.messaging.impl.data.TalismanData;
import kabam.rotmg.messaging.impl.incoming.*;
   import flash.utils.IDataInput;
   
   public class TalismanEssenceData extends IncomingMessage
   {
      public var talismans_:Vector.<TalismanData>;
      
      public function TalismanEssenceData(id:uint, callback:Function)
      {
         this.talismans_ = new Vector.<TalismanData>();
         super(id,callback);
      }
      
      override public function parseFromInput(data:IDataInput) : void
      {
         var i:int = 0;
         var len:int = data.readShort();
         for(i = len; i < this.talismans_.length; i++)
         {
            FreeList.deleteObject(this.talismans_[i]);
         }
         this.talismans_.length = Math.min(len, this.talismans_.length);
         while(this.talismans_.length < len)
         {
            this.talismans_.push(FreeList.newObject(TalismanData) as TalismanData);
         }
         for(i = 0; i < len; i++)
         {
            this.talismans_[i].parseFromInput(data);
         }
      }
      
      override public function toString() : String
      {
         return formatToString("TALISMAN_ESSENCE_DATA","talismans_");
      }
   }
}
