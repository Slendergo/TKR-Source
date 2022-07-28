package kabam.rotmg.assets.services
{
   import com.company.assembleegameclient.util.TextureRedrawer;
import com.company.assembleegameclient.util.redrawers.GlowRedrawer;
import com.company.util.AssetLibrary;
   import com.company.util.BitmapUtil;
   import flash.display.Bitmap;
   import flash.display.BitmapData;

   public class IconFactory
   {
       
      
      public function IconFactory()
      {
         super();
      }
      
      public static function makeCoin() : BitmapData
      {
         var data:BitmapData = TextureRedrawer.resize(AssetLibrary.getImageFromSet("lofiObj3",225),null,40,true,0,0);
         return cropAndGlowIcon(data);
      }
      
      public static function makeFame() : BitmapData
      {
         var data:BitmapData = TextureRedrawer.resize(AssetLibrary.getImageFromSet("lofiObj3",224),null,40,true,0,0);
         return cropAndGlowIcon(data);
      }
      
      public static function makeGuildFame() : BitmapData
      {
         var data:BitmapData = TextureRedrawer.resize(AssetLibrary.getImageFromSet("lofiObj3",226),null,40,true,0,0);
         return cropAndGlowIcon(data);
      }
      
      private static function cropAndGlowIcon(data:BitmapData) : BitmapData
      {
         data = GlowRedrawer.outlineGlow(data,4294967295);
         data = BitmapUtil.cropToBitmapData(data,10,10,data.width - 20,data.height - 20);
         return data;
      }
      
      public function makeIconBitmap(id:int) : Bitmap
      {
         var iconBD:BitmapData = AssetLibrary.getImageFromSet("lofiInterfaceBig",id);
         iconBD = TextureRedrawer.redraw(iconBD,320 / iconBD.width,true,0);
         return new Bitmap(iconBD);
      }
   }
}
