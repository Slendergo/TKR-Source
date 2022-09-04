package kabam.rotmg.application.impl
{
   import com.company.assembleegameclient.parameters.Parameters;
   import kabam.rotmg.application.api.ApplicationSetup;

   public class ReleaseSetup implements ApplicationSetup
   {
      private const CDN_APPENGINE:String = "http://play.tkr.gg:2000";
      private const CDN_APPENGINE_S:String = "https://tkr.gg";
      private const CDN_STATICS:String = "http://play.tkr.gg:2000";
      private const BUILD_LABEL:String = "<font color=\"#FF0000\">TKR</font> <font color=\"#FFFF00\">v{VERSION}.{MINOR}.{PATCH}</font>";

      public function getAppEngineUrl(toStatics:Boolean = false) : String
      {
         return toStatics ? this.CDN_STATICS : this.CDN_APPENGINE;
      }

      public function getAppEngineUrlEncrypted() : String
      {
         return CDN_APPENGINE_S;
      }

      public function getBuildLabel() : String
      {
         return this.BUILD_LABEL.replace("{VERSION}",Parameters.BUILD_VERSION).replace("{MINOR}",Parameters.MINOR_VERSION).replace("{PATCH}",Parameters.PATCH_VERSION);
      }

      public function isGameLoopMonitored() : Boolean
      {
         return false;
      }

      public function useProductionDialogs() : Boolean
      {
         return true;
      }

      public function areErrorsReported() : Boolean
      {
         return false;
      }

      public function isDebug():Boolean {
         return false;
      }
   }
}
