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
      public static const PlayersCXML:Class = EmbeddedData_PlayersCXML;
       public static const GroundCXML:Class = EmbeddedData_GroundCXML;
       public static const SkinsCXML:Class = EmbeddedData_SkinsCXML;
       public static const PermapetsCXML:Class = EmbeddedData_PermapetsCXML;
       public static const CaveOfAThousandTreasuresCXML:Class = EmbeddedData_CaveOfAThousandTreasuresCXML;
       public static const CustomBosses:Class = EmbeddedData_CustomBossesCXML;
       public static const CustomGround:Class = EmbeddedData_customGroundCXML;
       public static const CustomItems:Class = EmbeddedData_CustomItemsCXML;
       public static const CustomShoots:Class = EmbeddedData_CustomShootsCXML;
       public static const CustomMobs:Class = EmbeddedData_CustomMobsCXML;
       public static const LostHalls:Class = EmbeddedData_LostHallsCXML;
       public static const LostHallsGround:Class = EmbeddedData_LostHallsGroundCXML;
       public static const dat1:Class = EmbeddedData_dat1CXML;
       public static const stackedItems:Class = EmbeddedData_StackedItemsCXML;
       public static const wallsAndEnvironment:Class = EmbeddedData_WallsAndEnvironmentCXML;
       public static const Char16_16:Class = EmbeddedData_Char16_16;
      public static const tieredItems:Class = EmbeddedData_TieredItems;
       public static const weaponsItems:Class = EmbeddedData_WeaponsItemsCXML;
       public static const abilitiesItems:Class = EmbeddedData_AbilitiesItemsCXML;
       public static const ringItems:Class = EmbeddedData_RingItemsCXML;
       public static const armorItems:Class = EmbeddedData_ArmorItemsCXML;
       public static const containers:Class = EmbeddedData_Containers;


      public static const skinsXML:XML = XML(new SkinsCXML());
      public static const groundFiles:Array = [new GroundCXML(), new CustomGround(), new LostHallsGround()];
      public static const objectFiles:Array = [new stackedItems(),
                                                new CustomMobs(),
                                                new containers(),
                                                new abilitiesItems(),
                                                new ringItems(),
                                                new armorItems(),
                                                new weaponsItems(),
                                                new tieredItems(),
                                                new dat1(),
                                                new LostHalls(),
                                                new CustomBosses(),
                                                new wallsAndEnvironment(),
                                                new CaveOfAThousandTreasuresCXML(),
                                                new CustomItems(),
                                                new CustomShoots(),
                                                new Char16_16(),
                                                new PermapetsCXML(),
                                                new PlayersCXML()];/*,new ProjectilesCXML(),new EquipCXML(),new DyesCXML(),new TextilesCXML(),new PermapetsCXML(),new PlayersCXML(),new ObjectsCXML(),new TestingObjectsCXML(),new StaticObjectsCXML(),new TutorialObjectsCXML(),new MonstersCXML(),new PetsCXML(),new TempObjectsCXML(),/*new ShoreCXML(),new LowCXML(),new MidCXML(),new HighCXML(),new MountainsCXML(),new EncountersCXML(),new OryxCastleCXML(),new TombOfTheAncientsCXML(),new SpriteWorldCXML(),new UndeadLairCXML(),new OceanTrenchCXML(),new ForbiddenJungleCXML(),new OryxChamberCXML(),new OryxWineCellarCXML(),new ManorOfTheImmortalsCXML(),new PirateCaveCXML(),new SnakePitCXML(),new AbyssOfDemonsCXML(),new GhostShipCXML(),new MadLabCXML(),new CaveOfAThousandTreasuresCXML(),new CandyLandCXML(),new HauntedCemeteryCXML()];*/
      private static const RegionsCXML:Class = EmbeddedData_RegionsCXML;
      public static const regionFiles:Array = [new RegionsCXML()];
      private static const TutorialScriptCXML:Class = EmbeddedData_TutorialScriptCXML;
      public static const tutorialXML:XML = XML(new TutorialScriptCXML());
       
      
      public function EmbeddedData()
      {
         super();
      }
   }
}
