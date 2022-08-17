package kabam.rotmg.assets
{
public class EmbeddedData
{
   public static const skinsXML:XML = XML(new EmbeddedData_SkinsCXML());
   public static const PlayersCXML:Class = EmbeddedData_PlayersCXML;
   public static const GroundCXML:Class = EmbeddedData_GroundsCXML;
   public static const ObjectCXML:Class = EmbeddedData_ObjectsCXML;
   public static const TalismanCXML:Class = EmbeddedData_TalismansCXML;

   public static const RegionsCXML:Class = EmbeddedData_RegionsCXML;
   private static const TutorialScriptCXML:Class = EmbeddedData_TutorialScriptCXML;
   public static const tutorialXML:XML = XML(new TutorialScriptCXML());


   public function EmbeddedData()
   {
      super();
   }
}
}
