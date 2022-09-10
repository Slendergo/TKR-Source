package kabam.rotmg.messaging.impl.incoming
{
   import com.company.assembleegameclient.util.FreeList;
   import flash.utils.IDataInput;
   import kabam.rotmg.messaging.impl.data.ObjectStatusData;
   
   public class NewTick extends IncomingMessage
   {
       
      
      public var tickId_:int;
      
      public var tickTime_:int;

      public var statuses_:Vector.<ObjectStatusData>;
      public var aoes_:Vector.<AoeData>;

      public function NewTick(id:uint, callback:Function)
      {
         this.statuses_ = new Vector.<ObjectStatusData>();
         this.aoes_ = new Vector.<AoeData>();

         super(id,callback);
      }
      
      override public function parseFromInput(data:IDataInput) : void
      {
         var i:int = 0;
         this.tickId_ = data.readInt();
         this.tickTime_ = data.readInt();
         var len:int = data.readShort();
         for(i = len; i < this.statuses_.length; i++)
         {
            FreeList.deleteObject(this.statuses_[i]);
         }
         this.statuses_.length = Math.min(len,this.statuses_.length);
         while(this.statuses_.length < len)
         {
            this.statuses_.push(FreeList.newObject(ObjectStatusData) as ObjectStatusData);
         }
         for(i = 0; i < len; i++)
         {
            this.statuses_[i].parseFromInput(data);
         }
         var len:int = data.readShort();
         for(i = len; i < this.aoes_.length; i++)
         {
            FreeList.deleteObject(this.aoes_[i]);
         }
         this.aoes_.length = Math.min(len,this.aoes_.length);
         while(this.aoes_.length < len)
         {
            this.aoes_.push(FreeList.newObject(AoeData) as AoeData);
         }
         for(i = 0; i < len; i++)
         {
            this.aoes_[i].parseFromInput(data);
         }
      }
      
      override public function toString() : String
      {
         return formatToString("NEW_TICK","tickId_","tickTime_","statuses_");
      }
   }
}
