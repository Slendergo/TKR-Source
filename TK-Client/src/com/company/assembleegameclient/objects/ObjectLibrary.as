package com.company.assembleegameclient.objects
{
   import com.company.assembleegameclient.objects.animation.AnimationsData;
import com.company.assembleegameclient.ui.tooltip.TooltipHelper;
import com.company.assembleegameclient.util.TextureRedrawer;
import com.company.assembleegameclient.util.redrawers.GlowRedrawer;
import com.company.util.AssetLibrary;
   import com.company.util.ConversionUtil;
   import flash.display.BitmapData;
   import flash.utils.Dictionary;
   import flash.utils.getDefinitionByName;
   import kabam.rotmg.constants.GeneralConstants;
   import kabam.rotmg.constants.ItemConstants;
   import kabam.rotmg.messaging.impl.data.StatData;

public class ObjectLibrary
   {
      public static var playerChars_:Vector.<XML> = new Vector.<XML>();
      public static var hexTransforms_:Vector.<XML> = new Vector.<XML>();
      public static var playerClassAbbr_:Dictionary = new Dictionary();
      public static const propsLibrary_:Dictionary = new Dictionary();
      public static const xmlLibrary_:Dictionary = new Dictionary();
      public static const idToType_:Dictionary = new Dictionary();
      public static const typeToDisplayId_:Dictionary = new Dictionary();
      public static const typeToTextureData_:Dictionary = new Dictionary();
      public static const typeToTopTextureData_:Dictionary = new Dictionary();
      public static const typeToAnimationsData_:Dictionary = new Dictionary();
      public static const typeToIdItems_:Dictionary = new Dictionary();
      public static const idToTypeItems_:Dictionary = new Dictionary();
      public static const defaultProps_:ObjectProperties = new ObjectProperties(null);

      public static const TYPE_MAP:Object = {
         "SpecialClosedVaultChest":SpecialClosedVaultChest,
         "CaveWall":CaveWall,
         "Character":Character,
         "CharacterChanger":CharacterChanger,
         "ClosedVaultChest":ClosedVaultChest,
         "ConnectedWall":ConnectedWall,
         "Container":Container,
         "DoubleWall": DoubleWall,
         "GameObject":GameObject,
         "GuildBoard":GuildBoard,
         "ClosedGiftChest": ClosedGiftChest,
         "GuildChronicle":GuildChronicle,
         "GuildHallPortal":GuildHallPortal,
         "GuildMerchant":GuildMerchant,
         "GuildRegister":GuildRegister,
         "Merchant":Merchant,
         "MoneyChanger":MoneyChanger,
         "NameChanger":NameChanger,
         "ReskinVendor":ReskinVendor,
         "OneWayContainer":OneWayContainer,
         "Player":Player,
         "Portal":Portal,
         "Projectile":Projectile,
         "Sign":Sign,
         "SpiderWeb":SpiderWeb,
         "Stalagmite":Stalagmite,
         "Wall":Wall,
         "Forge":Forge,
         "StatNPC":StatNPC,
         "MarketNPC":MarketNPC,
         "BountyBoard": BountyBoard,
         "PotionStorage": PotionStorage,
         "Engine":Engine
      };


      public function ObjectLibrary()
      {
         super();
      }

      public static function parseFromXML(xml:XML) : void {
          var objectXML:XML = null;
          var id:String = null;
          var displayId:String = null;
          var objectType:int = 0;
          var found:Boolean = false;
          var i:int = 0;
          for each(objectXML in xml.Object) {
              try {
                  id = String(objectXML.@id);
                  displayId = id;
                  if (objectXML.hasOwnProperty("DisplayId")) {
                      displayId = objectXML.DisplayId;
                  }
                  if (objectXML.hasOwnProperty("Group")) {
                      if (objectXML.Group == "Hexable") {
                          hexTransforms_.push(objectXML);
                      }
                  }
                  objectType = int(objectXML.@type);
                  propsLibrary_[objectType] = new ObjectProperties(objectXML);
                  xmlLibrary_[objectType] = objectXML;
                  idToType_[id] = objectType;
                  typeToDisplayId_[objectType] = displayId;

                  if (String(objectXML.Class) == "Equipment") {
                      typeToIdItems_[objectType] = id.toLowerCase(); /* Saves us the power to do this later */
                      idToTypeItems_[id.toLowerCase()] = objectType;
                  }

                  if (String(objectXML.Class) == "Player") {
                      playerClassAbbr_[objectType] = String(objectXML.@id).substr(0, 2);
                      found = false;
                      for (i = 0; i < playerChars_.length; i++) {
                          if (int(playerChars_[i].@type) == objectType) {
                              playerChars_[i] = objectXML;
                              found = true;
                          }
                      }
                      if (!found) {
                          playerChars_.push(objectXML);
                      }
                  }

                  typeToTextureData_[objectType] = new TextureData(objectXML);
                  if (objectXML.hasOwnProperty("Top")) {
                      typeToTopTextureData_[objectType] = new TextureData(XML(objectXML.Top));
                  }
                  if (objectXML.hasOwnProperty("Animation")) {
                      typeToAnimationsData_[objectType] = new AnimationsData(objectXML);
                  }
              } catch (e:Error) {
                  trace("Failure to add XML Object: " + id + " " + e.getStackTrace())
              }
          }
      }

        public static function sortDictionaryByValue(d:Dictionary):Array
        {
           var a:Array = new Array();
           for (var dictionaryKey:Object in d)
           {
               a.push({key:dictionaryKey,value:d[dictionaryKey]});
           }
           a.sortOn("value",[Array.NUMERIC|Array.DESCENDING]);
           return a;
        }
       public static function getIdFromType(type:int) : String
      {
         var objectXML:XML = xmlLibrary_[type];
         if(objectXML == null)
         {
            return null;
         }
         return String(objectXML.@id);
      }

      public static function getPropsFromId(id:String) : ObjectProperties
      {
         var objectType:int = idToType_[id];
         return propsLibrary_[objectType];
      }

      public static function getXMLfromId(id:String) : XML
      {
         var objectType:int = idToType_[id];
         return xmlLibrary_[objectType];
      }

      public static function getObjectFromType(objectType:int) : GameObject
      {
         var objectXML:XML = xmlLibrary_[objectType];
         var typeReference:String = objectXML.Class;
         var typeClass:Class = TYPE_MAP[typeReference] || makeClass(typeReference);
         return new typeClass(objectXML);
      }

      private static function makeClass(typeReference:String) : Class
      {
         var typeName:String = "com.company.assembleegameclient.objects." + typeReference;
         return getDefinitionByName(typeName) as Class;
      }

      public static function getTextureFromType(objectType:int) : BitmapData
      {
         var textureData:TextureData = typeToTextureData_[objectType];
         if(textureData == null)
         {
            return null;
         }
         return textureData.getTexture();
      }

       public static function getRedrawnTextureFromType(objectType:int, size:int, includeBottom:Boolean, useCaching:Boolean = true, scaleValue:int = 5) : BitmapData
       {
           var textureData:TextureData = typeToTextureData_[objectType];
           var texture:BitmapData = Boolean(textureData)?textureData.getTexture():null;
           if(texture == null)
           {
               texture = AssetLibrary.getImageFromSet("lofiObj3",255);
           }
           var mask:BitmapData = Boolean(textureData)?textureData.mask_:null;
           if(mask == null)
           {
               return TextureRedrawer.redraw(texture,size,includeBottom,0,useCaching,scaleValue);
           }
           var objectXML:XML = xmlLibrary_[objectType];
           var tex1:int = Boolean(objectXML.hasOwnProperty("Tex1"))?int(int(objectXML.Tex1)):int(0);
           var tex2:int = Boolean(objectXML.hasOwnProperty("Tex2"))?int(int(objectXML.Tex2)):int(0);
           texture = TextureRedrawer.resize(texture,mask,size,includeBottom,tex1,tex2);
           texture = GlowRedrawer.outlineGlow(texture,0);
           return texture;
       }

      public static function getSizeFromType(objectType:int) : int
      {
         var objectXML:XML = xmlLibrary_[objectType];
         if(!objectXML.hasOwnProperty("Size"))
         {
            return 100;
         }
         return int(objectXML.Size);
      }

      public static function getSlotTypeFromType(objectType:int) : int
      {
         var objectXML:XML = xmlLibrary_[objectType];
         if(!objectXML.hasOwnProperty("SlotType"))
         {
            return -1;
         }
         return int(objectXML.SlotType);
      }

      public static function isEquippableByPlayer(objectType:int, player:Player) : Boolean
      {
         if(objectType == ItemConstants.NO_ITEM)
         {
            return false;
         }
         var objectXML:XML = xmlLibrary_[objectType];
         var slotType:int = int(objectXML.SlotType.toString());
         for(var i:uint = 0; i < GeneralConstants.NUM_EQUIPMENT_SLOTS; i++)
         {
            if(player.slotTypes_[i] == slotType)
            {
               return true;
            }
         }
         return false;
      }

      public static function getMatchingSlotIndex(objectType:int, player:Player) : int
      {
         if(objectType != ItemConstants.NO_ITEM)
         {
            var objectXML:XML = xmlLibrary_[objectType];
            if(objectXML == null) {
               return -1;
            }

             var i:uint = 0;
             var slotType:int = int(objectXML.SlotType);
             for(i = 0; i < GeneralConstants.NUM_EQUIPMENT_SLOTS; i++)
             {
                 if(player.slotTypes_[i] == slotType)
                 {
                     return i;
                 }
             }

             var props:ObjectProperties = ObjectLibrary.propsLibrary_[objectType];

             var offset:int = GeneralConstants.NUM_EQUIPMENT_SLOTS + GeneralConstants.NUM_INVENTORY_SLOTS + GeneralConstants.NUM_BACKPACK_SLOTS;
             if (props.onlyOneTalisman_) {
                 for (i = offset; i < offset + GeneralConstants.NUM_TALISMAN_SLOTS; i++) {
                     if (player.equipment_[i] == objectType) {
                         return -1;
                     }
                 }
             }

             if(props.isCommonTalisman_)
             {
                 for(i = offset; i < offset + 4; i++)
                 {
                     if(player.equipment_[i] == -1 && player.slotTypes_[i] == slotType) {
                         return i;
                     }
                 }
             }
             else if(props.isLegendaryTalisman_)
             {
                 for(i = offset + 4; i < offset + 6; i++)
                 {
                     if(player.equipment_[i] == -1 && player.slotTypes_[i] == slotType) {
                         return i;
                     }
                 }
             }
             else if(props.isMythicTalisman_)
             {
                 for(i = offset + 6; i < offset + 8; i++)
                 {
                     if(player.equipment_[i] == -1 && player.slotTypes_[i] == slotType) {
                         return i;
                     }
                 }
             }
         }
         return -1;
      }

      public static function isUsableByPlayer(objectType:int, player:Player) : Boolean
      {
         if(player == null)
         {
            return true;
         }
         var objectXML:XML = xmlLibrary_[objectType];
         if(objectXML == null || !objectXML.hasOwnProperty("SlotType"))
         {
            return false;
         }
         var slotType:int = objectXML.SlotType;
         if(slotType == ItemConstants.POTION_TYPE)
         {
            return true;
         }
         for(var i:int = 0; i < player.slotTypes_.length; i++)
         {
            if(player.slotTypes_[i] == slotType)
            {
               return true;
            }
         }
         return false;
      }

      public static function isSoulbound(objectType:int) : Boolean
      {
         var objectXML:XML = xmlLibrary_[objectType];
         if(objectXML == null) {
            return false;
         }
         return objectXML.hasOwnProperty("Soulbound");
      }

      public static function usableBy(objectType:int) : Vector.<String>
      {
         var playerXML:XML = null;
         var slotTypes:Vector.<int> = null;
         var i:int = 0;
         var objectXML:XML = xmlLibrary_[objectType];
         if(objectXML == null || !objectXML.hasOwnProperty("SlotType"))
         {
            return null;
         }
         var slotType:int = objectXML.SlotType;
         if(slotType == ItemConstants.POTION_TYPE || slotType == ItemConstants.RING_TYPE || slotType == ItemConstants.TALISMAN_TYPE)
         {
            return null;
         }
         var usable:Vector.<String> = new Vector.<String>();
         for each(playerXML in playerChars_)
         {
            slotTypes = ConversionUtil.toIntVector(playerXML.SlotTypes);
            for(i = 0; i < slotTypes.length; i++)
            {
               if(slotTypes[i] == slotType)
               {
                  usable.push(typeToDisplayId_[int(playerXML.@type)]);
                  break;
               }
            }
         }
         return usable;
      }

      public static function playerMeetsRequirements(objectType:int, player:Player) : Boolean
      {
         var reqXML:XML = null;
         if(player == null)
         {
            return true;
         }
         var objectXML:XML = xmlLibrary_[objectType];
         for each(reqXML in objectXML.EquipRequirement)
         {
            if(!playerMeetsRequirement(reqXML,player))
            {
               return false;
            }
         }
         return true;
      }

      public static function playerMeetsRequirement(reqXML:XML, p:Player) : Boolean
      {
         var val:int = 0;
         if(reqXML.toString() == "Stat")
         {
            val = int(reqXML.@value);
            switch(int(reqXML.@stat))
            {
               case StatData.MAX_HP_STAT:
                  return p.maxHP_ >= val;
               case StatData.MAX_MP_STAT:
                  return p.maxMP_ >= val;
               case StatData.LEVEL_STAT:
                  return p.level_ >= val;
               case StatData.ATTACK_STAT:
                  return p.attack_ >= val;
               case StatData.DEFENSE_STAT:
                  return p.defense_ >= val;
               case StatData.SPEED_STAT:
                  return p.speed_ >= val;
               case StatData.VITALITY_STAT:
                  return p.vitality_ >= val;
               case StatData.WISDOM_STAT:
                  return p.wisdom_ >= val;
               case StatData.DEXTERITY_STAT:
                  return p.dexterity_ >= val;
            }
         }
         return false;
      }
   }
}
