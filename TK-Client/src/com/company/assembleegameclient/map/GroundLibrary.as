package com.company.assembleegameclient.map
{
   import com.company.assembleegameclient.objects.TextureData;
   import com.company.util.BitmapUtil;
   import flash.display.BitmapData;
   import flash.utils.Dictionary;
   
   public class GroundLibrary
   {
      
      public static const propsLibrary_:Dictionary = new Dictionary();
      
      public static const xmlLibrary_:Dictionary = new Dictionary();
      
      private static var tileTypeColorDict_:Dictionary = new Dictionary();
      
      public static const typeToTextureData_:Dictionary = new Dictionary();
      
      public static var idToType_:Dictionary = new Dictionary();
      
      public static var defaultProps_:GroundProperties;
       
      
      public function GroundLibrary()
      {
         super();
      }
      
      public static function parseFromXML(xml:XML) : void
      {
         var groundXML:XML = null;
         var groundType:int = 0;
         for each(groundXML in xml.Ground)
         {
            groundType = int(groundXML.@type);
            propsLibrary_[groundType] = new GroundProperties(groundXML);
            xmlLibrary_[groundType] = groundXML;
            typeToTextureData_[groundType] = new TextureData(groundXML);
            idToType_[String(groundXML.@id)] = groundType;
         }
         defaultProps_ = propsLibrary_[255];
      }
      
      public static function getIdFromType(type:int) : String
      {
         var props:GroundProperties = propsLibrary_[type];
         if(props == null)
         {
            return null;
         }
         return props.id_;
      }
      
      public static function getBitmapData(type:int, id:int = 0) : BitmapData
      {
         return typeToTextureData_[type].getTexture(id);
      }
      
      public static function getColor(groundType:int) : uint
      {
         var groundXML:XML = null;
         var color:uint = 0;
         var bd:BitmapData = null;
         if(!tileTypeColorDict_.hasOwnProperty(groundType))
         {
            groundXML = xmlLibrary_[groundType];
            if(groundXML.hasOwnProperty("Color"))
            {
               color = uint(groundXML.Color);
            }
            else
            {
               bd = getBitmapData(groundType);
               color = BitmapUtil.mostCommonColor(bd);
            }
            tileTypeColorDict_[groundType] = color;
         }
         return tileTypeColorDict_[groundType];
      }
   }
}
