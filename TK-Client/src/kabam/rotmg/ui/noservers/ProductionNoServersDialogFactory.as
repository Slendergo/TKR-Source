package kabam.rotmg.ui.noservers
{
   import com.company.assembleegameclient.ui.dialogs.Dialog;
   
   public class ProductionNoServersDialogFactory implements NoServersDialogFactory
   {
      private static const BODY:String = "There are currently no servers online.";
      static const TITLE:String = "No Servers";

      public function ProductionNoServersDialogFactory()
      {
         super();
      }
      
      public function makeDialog() : Dialog
      {
         return new Dialog(BODY,TITLE,"Close",null);
      }
   }
}
