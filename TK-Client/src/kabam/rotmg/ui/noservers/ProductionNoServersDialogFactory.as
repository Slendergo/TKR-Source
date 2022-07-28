package kabam.rotmg.ui.noservers
{
   import com.company.assembleegameclient.ui.dialogs.Dialog;
   
   public class ProductionNoServersDialogFactory implements NoServersDialogFactory
   {
      
      private static const BODY:String = "Realm of the Mad God is currently offline.\n\nGo here for more information:\n<font color=\"#7777EE\"><a href=\"http://forums.wildshadow.com/\">forums.wildshadow.com</a></font>.";
      
      private static const TITLE:String = "Oryx Sleeping";
       
      
      public function ProductionNoServersDialogFactory()
      {
         super();
      }
      
      public function makeDialog() : Dialog
      {
         return new Dialog(BODY,TITLE,null,null);
      }
   }
}
