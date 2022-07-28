package com.company.assembleegameclient.objects.particles
{
   import com.company.assembleegameclient.objects.TextureData;
   import com.company.assembleegameclient.objects.animation.AnimationsData;
   
   public class ParticleProperties
   {
       
      
      public var id_:String;
      
      public var textureData_:TextureData;
      
      public var size_:int = 100;
      
      public var z_:Number = 0.0;
      
      public var duration_:Number = 0.0;
      
      public var animationsData_:AnimationsData = null;
      
      public function ParticleProperties(particleXML:XML)
      {
         super();
         this.id_ = particleXML.@id;
         this.textureData_ = new TextureData(particleXML);
         if(particleXML.hasOwnProperty("Size"))
         {
            this.size_ = Number(particleXML.Size);
         }
         if(particleXML.hasOwnProperty("Z"))
         {
            this.z_ = Number(particleXML.Z);
         }
         if(particleXML.hasOwnProperty("Duration"))
         {
            this.duration_ = Number(particleXML.Duration);
         }
         if(particleXML.hasOwnProperty("Animation"))
         {
            this.animationsData_ = new AnimationsData(particleXML);
         }
      }
   }
}
