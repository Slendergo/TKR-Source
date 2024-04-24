package kabam.rotmg.application.impl
{
import com.company.assembleegameclient.parameters.Parameters;
import com.company.assembleegameclient.parameters.Parameters;
   import kabam.rotmg.application.api.ApplicationSetup;

   public class ReleaseSetup implements ApplicationSetup
   {
      private const CDN_APPENGINE:String = "http://play.tkr.gg:lo";
      private const CDN_APPENGINE_S:String = "https://tkr.gg";
      private const TESTING_CDN_APPENGINE:String = "http://play.tkr.gg:2003";

      private const BUILD_LABEL:String = "<font color=\"#FF0000\">TKR</font> <font color=\"#FFFF00\">v{VERSION}.{MINOR}.{PATCH}</font>";
      private const TESTING_BUILD_LABEL:String = "<font color=\"#FF0000\">TESTING - TKR</font> <font color=\"#FFFF00\">v{VERSION}.{MINOR}.{PATCH}</font>";

      public function getAppEngineUrl() : String
      {
         return Parameters.TESTING_SERVER ? TESTING_CDN_APPENGINE : CDN_APPENGINE;
      }

      public function getAppEngineUrlEncrypted() : String
      {
         return Parameters.TESTING_SERVER ? TESTING_CDN_APPENGINE : CDN_APPENGINE_S;
      }

      public function getBuildLabel() : String
      {
         return (Parameters.TESTING_SERVER ? TESTING_BUILD_LABEL : BUILD_LABEL).replace("{VERSION}",Parameters.BUILD_VERSION).replace("{MINOR}",Parameters.MINOR_VERSION).replace("{PATCH}",Parameters.PATCH_VERSION);
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
