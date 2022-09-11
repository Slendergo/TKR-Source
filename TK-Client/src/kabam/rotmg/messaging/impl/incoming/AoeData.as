package kabam.rotmg.messaging.impl.incoming
{
   import flash.utils.IDataInput;
   import kabam.rotmg.messaging.impl.data.WorldPosData;
   
   public class AoeData
   {
      public var pos_:WorldPosData;
      public var radius_:Number;
      public var damage_:int;
      public var effect_:int;
      public var duration_:Number;
      public var origType_:int;
      public var color_:uint;

      public function AoeData(){
         this.pos_ = new WorldPosData();
      }

      public function parseFromInput(data:IDataInput) : void
      {
         this.pos_.parseFromInput(data);
         this.radius_ = data.readFloat();
         this.damage_ = data.readUnsignedShort();
         this.effect_ = data.readUnsignedByte();
         this.duration_ = data.readFloat();
         this.origType_ = data.readUnsignedShort();
         this.color_ = data.readInt();
      }
   }
}
