package kabam.rotmg.assets
{
import kabam.rotmg.assets.custom.EmbeddedData_CustomBossesCXML;
import kabam.rotmg.assets.custom.items.EmbeddedData_AbilitiesItemsCXML;
import kabam.rotmg.assets.custom.items.EmbeddedData_ArmorItemsCXML;
import kabam.rotmg.assets.custom.items.EmbeddedData_CustomItemsCXML;
import kabam.rotmg.assets.custom.EmbeddedData_CustomMobsCXML;
import kabam.rotmg.assets.custom.EmbeddedData_CustomShootsCXML;
import kabam.rotmg.assets.custom.items.EmbeddedData_RingItemsCXML;
import kabam.rotmg.assets.custom.items.EmbeddedData_WeaponsItemsCXML;
import kabam.rotmg.assets.custom.EmbeddedData_customGroundCXML;

public class EmbeddedData
   {
      public static const skinsXML:XML = XML(new EmbeddedData_SkinsCXML());
      public static const PlayersCXML:Class = EmbeddedData_PlayersCXML;
      public static const GroundCXML:Class = EmbeddedData_GroundsCXML;
      public static const ObjectCXML:Class = EmbeddedData_ObjectsCXML;

      public static const RegionsCXML:Class = EmbeddedData_RegionsCXML;
      private static const TutorialScriptCXML:Class = EmbeddedData_TutorialScriptCXML;
      public static const tutorialXML:XML = XML(new TutorialScriptCXML());
       
      
      public function EmbeddedData()
      {
         super();
      }
   }
}
