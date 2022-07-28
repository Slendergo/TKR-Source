package kabam.rotmg.application.impl
{
   import com.company.assembleegameclient.parameters.Parameters;
   import kabam.rotmg.application.api.ApplicationSetup;

   public class ReleaseSetup implements ApplicationSetup
   {
      private const CDN_APPENGINE:String = "http://127.0.0.1:2000";
      private const CDN_STATICS:String = "https://127.0.0.1/static";
      private const BUILD_LABEL:String = "TK1 - build: {VERSION}{MINOR}";

      public function ReleaseSetup()
      {
         super();
      }

      public function getAppEngineUrl(toStatics:Boolean = false) : String
      {
         return toStatics ? this.CDN_STATICS : this.CDN_APPENGINE;
      }

      public function getBuildLabel() : String
      {
         return this.BUILD_LABEL.replace("{VERSION}",Parameters.BUILD_VERSION).replace("{MINOR}",Parameters.MINOR_VERSION);
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
