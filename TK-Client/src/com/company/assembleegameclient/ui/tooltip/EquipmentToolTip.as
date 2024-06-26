package com.company.assembleegameclient.ui.tooltip
{
import com.company.assembleegameclient.constants.InventoryOwnerTypes;
import com.company.assembleegameclient.misc.UILabel;
import com.company.assembleegameclient.objects.GameObject;
import com.company.assembleegameclient.objects.ObjectLibrary;
import com.company.assembleegameclient.objects.ObjectProperties;
import com.company.assembleegameclient.objects.Player;
import com.company.assembleegameclient.parameters.Parameters;
import com.company.assembleegameclient.ui.LineBreakDesign;
import com.company.assembleegameclient.util.FilterUtil;
import com.company.assembleegameclient.util.TextureRedrawer;
import com.company.assembleegameclient.util.TierUtil;
import com.company.ui.SimpleText;
import com.company.util.AssetLibrary;
import com.company.util.BitmapUtil;
import com.company.util.KeyCodes;

import flash.display.Bitmap;
import flash.display.BitmapData;
import flash.events.TimerEvent;
import flash.filters.DropShadowFilter;
import flash.text.StyleSheet;
import flash.utils.Timer;

import kabam.rotmg.constants.ActivationType;
import kabam.rotmg.market.content.MemMarketItem;
import kabam.rotmg.messaging.impl.data.StatData;

public class EquipmentToolTip extends ToolTip
   {
      private static const MAX_WIDTH:int = 230;
      private static const CSS_TEXT:String = ".in { margin-left:10px; text-indent: -10px; }";
       private var iconSize:Number = 60;
      private var icon_:Bitmap;
      private var titleText_:SimpleText;
      private var tierText_:UILabel;
      private var itemEffectText_:SimpleText;
      private var descText_:SimpleText;
      private var passiveDescText_:SimpleText;
      private var line1_:LineBreakDesign;
      private var effectsText_:SimpleText;
      private var line2_:LineBreakDesign;
      private var restrictionsText_:SimpleText;
      private var player_:Player;
      private var isEquippable_:Boolean = false;
      private var objectType_:int;
      private var curItemXML_:XML = null;
      private var objectXML_:XML = null;
      private var slotTypeToTextBuilder:SlotComparisonFactory;
      private var playerCanUse:Boolean;
      private var restrictions:Vector.<Restriction>;
      private var effects:Vector.<Effect>;
      private var invType:int;
      private var inventoryOwnerType:String;
      private var inventorySlotID:uint;
      private var isInventoryFull:Boolean;
      private var yOffset:int;
      private var comparisonResults:SlotComparisonResult;
      private var isMarketItem:Boolean;
       private var backgroundColor:uint;
       private var outlineColor:uint;
       private var time:Timer;
       private var flashtext:SimpleText;
       private var repeat:int = 0;
      private var itemData_:Object = null;
      private var props_:ObjectProperties = null;
      private var isMythicalItem:Boolean;
      private var isLegendaryItem:Boolean;

      public function EquipmentToolTip(objectType:int, player:Player, invType:int, inventoryOwnerType:String, inventorySlotID:uint = 1.0, isMarketItem:Boolean = false, itemData:Object = null)
      {
         this.itemData_ = itemData;
         this.isMarketItem = isMarketItem;
         this.inventoryOwnerType = inventoryOwnerType;
         this.inventorySlotID = inventorySlotID;
         this.isInventoryFull = Boolean(player)?Boolean(player.isInventoryFull()):Boolean(false);
         this.playerCanUse = player != null?Boolean(ObjectLibrary.isUsableByPlayer(objectType,player)):Boolean(false);
         this.slotTypeToTextBuilder = new SlotComparisonFactory();
         this.objectType_ = objectType;
         this.objectXML_ = ObjectLibrary.xmlLibrary_[objectType];
         var equipSlotIndex:int = Boolean(this.player_)?int(ObjectLibrary.getMatchingSlotIndex(this.objectType_,this.player_)):int(-1);
         this.isEquippable_ = equipSlotIndex != -1;
         this.effects = new Vector.<Effect>();
         this.invType = invType;

         this.props_ = ObjectLibrary.propsLibrary_[objectType];

         this.isMythicalItem = props_.isMythicTalisman_ || this.objectXML_.hasOwnProperty("Mythical")
         this.isLegendaryItem = props_.isLegendaryTalisman_ || this.objectXML_.hasOwnProperty("Legendary");

          if(isLegendaryItem){
              this.backgroundColor = this.playerCanUse || this.player_ == null ? 0x2c2d3d : 6036765;
              this.outlineColor = this.playerCanUse || player == null? TooltipHelper.LEGENDARY_COLOR : 10965039;
          }else if(isMythicalItem){
              this.backgroundColor = this.playerCanUse || this.player_ == null ? 0x222226 : 6036765;
              this.outlineColor = this.playerCanUse || player == null? TooltipHelper.MYTHICAL_COLOR : 10965039;
          }
          else {
              this.backgroundColor = this.playerCanUse || this.player_ == null ? 0x363636 : 6036765;
              this.outlineColor = this.playerCanUse || player == null ? 0x9B9B9B : 10965039;
          }
          super(backgroundColor, 1, outlineColor, 1, true);

         if(this.player_ == null)
         {
            this.curItemXML_ = this.objectXML_;
         }
         else if(this.isEquippable_)
         {
            if(this.player_.equipment_[equipSlotIndex] != -1)
            {
               this.curItemXML_ = ObjectLibrary.xmlLibrary_[this.player_.equipment_[equipSlotIndex]];
            }
         }
         this.addIcon();
         this.addTitle();
         this.addTierText();
         this.handleWisMod();
         this.addDescriptionText();
         this.buildCategorySpecificText();
         this.addNumProjectilesTagsToEffectsList();
         this.addProjectileTagsToEffectsList();
         this.addRateOfFire();
         this.makeHoldItemText();
         this.addActivateTagsToEffectsList();
         this.addActivateOnEquipTagsToEffectsList();
         this.addDoseTagsToEffectsList();
         this.addQuantityLimitTagsToEffectsList();
         this.addQuantityTagsToEffectsList();
         this.addMpCostTagToEffectsList();
         this.addFameBonusTagToEffectsList();
         this.makeEffectsList();
         this.makeLineTwo();
         this.makeRestrictionList();
         this.makeRestrictionText();
         this.makeItemEffectsText();
      }

      private function addRateOfFire() : void {

         if(this.player_ != null)
         {
            var rateOfFire:Number = Number(this.objectXML_.RateOfFire);
            rateOfFire *= 100;
         }

         if (this.objectXML_.hasOwnProperty("RateOfFire") && (this.objectXML_.RateOfFire != 1) || this.objectXML_.hasOwnProperty("RateOfFire")) {
            if(this.player_ != null && (rateOfFire != this.objectXML_.RateOfFire * 100)){
               this.effects.push(new Effect("Rate of Fire ", Math.round(this.objectXML_.RateOfFire * 100) + "%" ? " (" + int(rateOfFire) + "%)" : ""));
            }
            else{
               this.effects.push(new Effect("Rate of Fire ", Math.round(this.objectXML_.RateOfFire * 100) + "%"));
            }
         }
      }

      private static function BuildRestrictionsHTML(restrictions:Vector.<Restriction>) : String
      {
         var restriction:Restriction = null;
         var line:String = null;
         var html:String = "";
         var first:Boolean = true;
         for each(restriction in restrictions)
         {
            if(!first)
            {
               html = html + "\n";
            }
            else
            {
               first = false;
            }
            line = "<font color=\"#" + restriction.color_.toString(16) + "\">" + restriction.text_ + "</font>";
            if(restriction.bold_)
            {
               line = "<b>" + line + "</b>";
            }
            html = html + line;
         }
         return html;
      }

      private function isEmptyEquipSlot() : Boolean
      {
         return this.isEquippable_ && this.curItemXML_ == null;
      }

      private function addIcon() : void
      {
         var eqXML:XML = ObjectLibrary.xmlLibrary_[this.objectType_];
         var scaleValue:int = 5;
         if(eqXML.hasOwnProperty("ScaleValue"))
         {
            scaleValue = eqXML.ScaleValue;
         }
         var texture:BitmapData = ObjectLibrary.getRedrawnTextureFromType(this.objectType_,60,true,true,scaleValue);
         texture = BitmapUtil.cropToBitmapData(texture,4,4,texture.width - 8,texture.height - 8);
         this.icon_ = new Bitmap(texture);
         addChild(this.icon_);
      }

      private function makeItemEffectsText():void {

         if(this.props_.talismanEffect_ != null)
         {
            this.itemEffectText_ = new SimpleText(13, 0x7550C4, false, MAX_WIDTH);
            this.itemEffectText_.setBold(true);
            this.itemEffectText_.wordWrap = true;
            switch(this.props_.talismanEffect_)
            {
              //legendary
               case "Weak Immunity":
                  this.itemEffectText_.text = "Weak Immunity -> Immunity to being Weakened";
                  break;
               case "Call To Arms":
                  this.itemEffectText_.text = "Call to Arms -> Regeneration is doubled";
                  break;
               case "Party Of One":
                  this.itemEffectText_.text = "Party of One -> 50% more chance to get loot, but -1% for every player (Wont go below 0%)";
                  break;
               case "Pocket Change":
                  this.itemEffectText_.text = "Pocket Change -> You have a 30% increased loot chance";
                  break;
               // mythical
               case "Stun Immunity":
                  this.itemEffectText_.text = "Stun Immunity -> Immunity to being Stunned";
                  break;
               case "Luck Of The Irish":
                  this.itemEffectText_.text = "Luck of the Irish -> You have a 30% increased loot chance and an 2% chance to double your drops from bosses";
                  break;
               case "Known After Death":
                  this.itemEffectText_.text = "Known After Death -> Experience from killing mobs is doubled";
                  break;
            }
         }
         else if(isLegendaryItem)
         {
            this.itemEffectText_ = new SimpleText(13, 0xebb011, false, MAX_WIDTH);
            this.itemEffectText_.setBold(true);
            this.itemEffectText_.wordWrap = true;
            if(this.objectXML_.hasOwnProperty("OutOfOneMind")){
               this.itemEffectText_.text = "Out of One’s Mind -> Has a 2% to get Berserk for 3 seconds. 4 seconds Cooldown.";
            }
            else if(this.objectXML_.hasOwnProperty("SteamRoller")) {
               this.itemEffectText_.text = "Steamroller -> Has a 5% chance to get Armored for 5 seconds. 10 seconds Cooldown.";
            }
            else if(this.objectXML_.hasOwnProperty("Mutilate")){
               this.itemEffectText_.text = "Mutilate -> Has a 8% chance to get Damaging for 3 seconds. 4 seconds Cooldown. ";
            }
            else if(this.objectXML_.hasOwnProperty("Demonized")){
               this.itemEffectText_.text = "Demonized -> Upon hitting an enemy you have a 3% chance to Curse it for 5 Seconds. After applying the effect, 4 seconds Cooldown.";
            }
            else if(this.objectXML_.hasOwnProperty("Clarification")){
               this.itemEffectText_.text = "Clarification -> Upon getting hit you have a 10% chance that your MP will be restored 30%. 15 seconds Cooldown.";
            }
            else if(this.objectXML_.hasOwnProperty("SonicBlaster")){
               this.itemEffectText_.text = "Sonic Blaster -> Upon drinking a MP potion, you get Speedy and Invisible for 6 seconds. 30 seconds Cooldown.";
            }
            else if(this.objectXML_.hasOwnProperty("Lucky")){
               this.itemEffectText_.text = "Lucky 15's -> 10% of increasing each stat by 15 (100 HP and MP) for 5 seconds. 20 seconds Cooldown.";
            }
            else if(this.objectXML_.hasOwnProperty("Insanity")){
               this.itemEffectText_.text = "Insanity -> Has a 5% to get Berserk and Damaging for 3 seconds. 7 seconds Cooldown.";
            }
            else if(this.objectXML_.hasOwnProperty("HolyProtection")){
               this.itemEffectText_.text = "Holy Protection -> 10% chance of being Purified (remove all Negative Status). 15 seconds Cooldown.";
            }
            else if(this.objectXML_.hasOwnProperty("GodBless")){
               this.itemEffectText_.text = "God Bless -> Upon getting hit you have a 3% chance to get Invulnerable for 3 seconds. 5 seconds Cooldown.";
            }
            else if(this.objectXML_.hasOwnProperty("GodTouch")){
               this.itemEffectText_.text = "God Touch -> Upon getting hit you have a 2% chance to get healed 25% of your health. 30 seconds Cooldown.";
            }
            else if(this.objectXML_.hasOwnProperty("Electrify")){
               this.itemEffectText_.text = "Electrify -> Upon hitting an enemy, you have a 3% chance to “cast” a scepter-like ability that deals 1000 damage and inflicts Slowed for 3 seconds to up to 5 targets.";
            }
            else if(this.objectXML_.hasOwnProperty("Vampiric")){
               var numProj:int = this.objectXML_.NumProjectiles;
               var multi:Boolean = this.objectXML_.Projectile.hasOwnProperty("MultiHit")
               var chance:Number = 3 - ((numProj / 3))
               chance = multi ? chance / 1.5 : chance;
               this.itemEffectText_.text = "Vampriric -> Upon hitting an enemy, you have a "+ round2(chance,1) +"% chance to “cast” a skull-like ability that deals 300 damage AOE damage and heals for 50hp.";
            }
         }
         else if(isMythicalItem)
         {
            this.itemEffectText_ = new SimpleText(13, 0xcf1433, false, MAX_WIDTH);
            this.itemEffectText_.setBold(true);
            this.itemEffectText_.wordWrap = true;
            if(this.objectXML_.hasOwnProperty("Lucky")){
               this.itemEffectText_.text = "Lucky 15's -> 10% of increasing each stat by 15 (100 HP and MP) for 5 seconds. 20 seconds Cooldown.";
            }
            else if(this.objectXML_.hasOwnProperty("Insanity")){
               this.itemEffectText_.text = "Insanity -> Has a 5% to get Berserk and Damaging for 3 seconds. 7 seconds Cooldown.";
            }
            else if(this.objectXML_.hasOwnProperty("HolyProtection")){
               this.itemEffectText_.text = "Holy Protection -> 10% chance of being Purified (remove all Negative Status). 15 seconds Cooldown.";
            }
            else if(this.objectXML_.hasOwnProperty("GodBless")){
               this.itemEffectText_.text = "God Bless -> Upon getting hit you have a 3% chance to get Invulnerable for 3 seconds. 5 seconds Cooldown.";
            }
            else if(this.objectXML_.hasOwnProperty("GodTouch")){
               this.itemEffectText_.text = "God Touch -> Upon getting hit you have a 2% chance to get healed 25% of your health. 30 seconds Cooldown.";
            }
            else if(this.objectXML_.hasOwnProperty("Electrify")){
               this.itemEffectText_.text = "Electrify -> Upon hitting an enemy, you have a 3% chance to “cast” a scepter-like ability that deals 1000 damage and inflicts Slowed for 3 seconds to up to 5 targets.";
            }
            else if(this.objectXML_.hasOwnProperty("Vampiric")){
               var numProj:int = this.objectXML_.NumProjectiles;
               var multi:Boolean = this.objectXML_.Projectile.hasOwnProperty("MultiHit")
               var chance:Number = 3 - ((numProj / 3))
               chance = multi ? chance / 1.5 : chance;
               this.itemEffectText_.text = "Vampriric -> Upon hitting an enemy, you have a "+ round2(chance,1) +"% chance to “cast” a skull-like ability that deals 300 damage AOE damage and heals for 50hp.";
            }
         }

         if(isLegendaryItem || isMythicalItem || props_.talismanEffect_){
            switch(Parameters.data_.itemDataOutlines)
            {
               case 0:
                  this.itemEffectText_.filters = FilterUtil.getTextOutlineFilter();
                  break;
               case 1:
                  this.itemEffectText_.filters = [new DropShadowFilter(0,0,0,0.5,12,12)];
            }
            addChild(this.itemEffectText_);
         }
      }

      private function addTierText() : void
      {
         this.tierText_ = TierUtil.getTierTag(props_,16);
         if(this.tierText_)
         {
            this.tierText_.y = this.icon_.height / 2 - this.titleText_.actualHeight_ / 2;
            this.tierText_.x = MAX_WIDTH - 30;
            addChild(this.tierText_);
         }
      }
       private function SetFlash(_arg1:SimpleText):void {
           this.repeat = 0;
           this.flashtext = _arg1;
           this.time = new Timer(100);
           this.time.start();
           this.time.addEventListener(TimerEvent.TIMER, OnFlash);
       }

       private function OnFlash(_arg1:TimerEvent):void {
           if (this.repeat % 2 == 0){
               this.flashtext.setColor(0xD9FFD9); //you could remake it so that the colors are set through parameters or something like that
           } else {
               this.flashtext.setColor(0x98ff98);
           }
           this.repeat++;
       }
      /*private function addTierText() : void
      {
         this.tierText_ = new SimpleText(16,16777215,false,30,0);
         this.titleText_.setBold(true);
         this.tierText_.y = this.icon_.height / 2 - this.titleText_.actualHeight_ / 2;
         this.tierText_.x = MAX_WIDTH - 30;
         if(!this.objectXML_.hasOwnProperty("Consumable") && !this.isPet())
         {
            if(this.objectXML_.hasOwnProperty("Tier"))
            {
               this.tierText_.text = "T" + this.objectXML_.Tier;
            }
            else
            {
               this.tierText_.setColor(9055202);
               this.tierText_.text = "UT";
            }
            this.tierText_.updateMetrics();
            addChild(this.tierText_);
         }
      }*/

      private function isPet() : Boolean
      {
         var activateTags:XMLList = null;
         activateTags = this.objectXML_.Activate.(text() == "PermaPet");
         return activateTags.length() >= 1;
      }

      private function addTitle() : void {
          var color:int = this.playerCanUse || this.player_ == null ? int(16777215) : int(16549442);
          this.titleText_ = new SimpleText(16, color, false, MAX_WIDTH - this.icon_.width - 4 - 30, 0);
          this.titleText_.setBold(true);
          this.titleText_.wordWrap = true;
          this.titleText_.text = ObjectLibrary.typeToDisplayId_[this.objectType_];
         if(this.itemData_ != null)
         {
            if(this.itemData_.ObjectId != null)
               this.titleText_.text = this.itemData_.ObjectId;
            if(this.itemData_.Stack > 0)
               this.titleText_.text += " x" + this.itemData_.Stack;
         }
          this.titleText_.filters = [new DropShadowFilter(0, 0, 0, 0.5, 12, 12)];
          this.titleText_.x = this.icon_.width + 4;
          this.titleText_.y = this.icon_.height / 2 - this.titleText_.actualHeight_ / 2;
          this.titleText_.updateMetrics();
          switch (Parameters.data_.itemDataOutlines) {
              case 0:
                  this.titleText_.filters = FilterUtil.getTextOutlineFilter();
                  break;
              case 1:
                  this.titleText_.filters = [new DropShadowFilter(0, 0, 0, 0.5, 12, 12)];
          }
          if(isLegendaryItem){
              this.titleText_.setColor(TooltipHelper.LEGENDARY_COLOR);
          }else if(isMythicalItem){
              this.titleText_.setColor(TooltipHelper.MYTHICAL_COLOR);
          }

         addChild(this.titleText_);
      }

      private function buildUniqueTooltipData() : String
      {
         var effectDataList:XMLList = null;
         var uniqueEffectList:Vector.<Effect> = null;
         var effectDataXML:XML = null;
         if(this.objectXML_.hasOwnProperty("ExtraTooltipData"))
         {
            effectDataList = this.objectXML_.ExtraTooltipData.EffectInfo;
            uniqueEffectList = new Vector.<Effect>();
            for each(effectDataXML in effectDataList)
            {
               uniqueEffectList.push(new Effect(effectDataXML.attribute("name"),effectDataXML.attribute("description")));
            }
            return this.BuildEffectsHTML(uniqueEffectList) + "\n";
         }
         return "";
      }

      private function makeEffectsList() : void
      {
         this.yOffset = this.descText_.y + this.descText_.height;
         if(this.effects.length != 0 || this.comparisonResults.text != "" || this.objectXML_.hasOwnProperty("ExtraTooltipData"))
         {
            this.line1_ = new LineBreakDesign(MAX_WIDTH - 12,0);
             if(isLegendaryItem){
                 this.line1_.setWidthColor(MAX_WIDTH-12,0x967bb6);
             }else if(isMythicalItem){
                 this.line1_.setWidthColor(MAX_WIDTH-12,0xFFFFFF);
             }
            addChild(this.line1_);
            this.effectsText_ = new SimpleText(14,11776947,false,MAX_WIDTH - this.icon_.width - 4,0);
            this.effectsText_.wordWrap = true;
            this.effectsText_.htmlText = this.buildUniqueTooltipData() + this.comparisonResults.text + this.BuildEffectsHTML(this.effects);
            this.effectsText_.useTextDimensions();
            this.effectsText_.filters = [new DropShadowFilter(0,0,0,0.5,12,12)];
            this.effectsText_.x = 4;
            this.effectsText_.y = this.line1_.y + 8;
            switch(Parameters.data_.itemDataOutlines)
            {
               case 0:
                  this.effectsText_.filters = FilterUtil.getTextOutlineFilter();
                  break;
               case 1:
                  this.effectsText_.filters = [new DropShadowFilter(0,0,0,0.5,12,12)];
            }
            addChild(this.effectsText_);
            this.yOffset = this.effectsText_.y + this.effectsText_.height + 8;
         }
      }

      private function addNumProjectilesTagsToEffectsList() : void
      {
         if(this.objectXML_.hasOwnProperty("NumProjectiles") && !this.comparisonResults.processedTags.hasOwnProperty(this.objectXML_.NumProjectiles.toXMLString()))
            if (this.objectXML_.NumProjectiles > 1) // allows only projectiles with greater than 1 projectile amount, like: Demon Blade (2 projectiles)
               this.effects.push(new Effect("Shots",this.objectXML_.NumProjectiles));

         if(this.objectXML_.hasOwnProperty("SpellProjectiles") && !this.comparisonResults.processedTags.hasOwnProperty(this.objectXML_.SpellProjectiles.toXMLString()))
            this.effects.push(new Effect("Shots",this.objectXML_.SpellProjectiles));
      }

      private function addFameBonusTagToEffectsList() : void
      {
         var fameBonus:int = 0;
         var text:String = null;
         var textColor:String = null;
         var curFameBonus:int = 0;
         if(this.objectXML_.hasOwnProperty("FameBonus"))
         {
            fameBonus = int(this.objectXML_.FameBonus);
            text = fameBonus + "%";
            textColor = this.playerCanUse?TooltipHelper.BETTER_COLOR:TooltipHelper.NO_DIFF_COLOR;
            if(this.curItemXML_ != null && this.curItemXML_.hasOwnProperty("FameBonus"))
            {
               curFameBonus = int(this.curItemXML_.FameBonus.text());
               textColor = TooltipHelper.getTextColor(fameBonus - curFameBonus);
            }
            this.effects.push(new Effect("Fame Bonus",TooltipHelper.wrapInFontTag(text,textColor)));
         }
      }

      private function addMpCostTagToEffectsList() : void
      {
         if(this.objectXML_.hasOwnProperty("MpEndCost"))
         {
            if(!this.comparisonResults.processedTags[this.objectXML_.MpEndCost[0].toXMLString()]) {
               this.effects.push(new Effect("MP Cost", this.objectXML_.MpEndCost));
            }
         }
         else if(this.objectXML_.hasOwnProperty("MpCost") && !this.comparisonResults.processedTags[this.objectXML_.MpCost[0].toXMLString()])
         {
            if(!this.comparisonResults.processedTags[this.objectXML_.MpCost[0].toXMLString()]) {
               this.effects.push(new Effect("MP Cost", this.objectXML_.MpCost));
            }
         }
      }

      private function addDoseTagsToEffectsList() : void
      {
         if(this.objectXML_.hasOwnProperty("Doses"))
         {
            this.effects.push(new Effect("Doses",this.objectXML_.Doses));
         }
      }

      private function addQuantityLimitTagsToEffectsList() : void
      {
         if(this.itemData_ != null && this.itemData_.MaxStack > 0)
         {
            this.effects.push(new Effect("Stack Limit", this.itemData_.MaxStack));
         }
         else if(this.objectXML_.hasOwnProperty("QuantityLimit"))
         {
            this.effects.push(new Effect("Stack Limit",this.objectXML_.QuantityLimit));
         }
      }

      private function addQuantityTagsToEffectsList() : void
      {
         if(this.itemData_ != null && this.itemData_.Stack > 0)
         {
            this.effects.push(new Effect("Stack", this.itemData_.Stack));
         }
         else if(this.objectXML_.hasOwnProperty("Quantity"))
         {
            this.effects.push(new Effect("Stack",this.objectXML_.Quantity));
         }
      }

      /*override protected function position() : void
      {
         var _loc1_:Number = NaN;
         var _loc2_:Number = NaN;
         var _loc3_:Number = 800 / stage.stageWidth;
         var _loc4_:Number = 600 / stage.stageHeight;
         if(this.parent is MemMarketItem)
         {
            _loc1_ = (stage.mouseX + stage.stageWidth / 2 - 400) / stage.stageWidth * 800;
            _loc2_ = (stage.mouseY + stage.stageHeight / 2 - 300) / stage.stageHeight * 600;
         }
         //else
         //{
            //_loc1_ = (stage.stageWidth - 800) / 2 + stage.mouseX;
            //_loc2_ = (stage.stageHeight - 600) / 2 + stage.mouseY;
            //this.parent.scaleX = _loc3_;
            //this.parent.scaleY = _loc4_;
         //}
         if(stage == null)
         {
            return;
         }
         if(stage.mouseX + 0.5 * stage.stageWidth - 400 < stage.stageWidth / 2)
         {
            x = _loc1_ + 12;
         }
         else
         {
            x = _loc1_ - width - 1;
         }
         if(x < 12)
         {
            x = 12;
         }
         if(stage.mouseY + 0.5 * stage.stageHeight - 300 < stage.stageHeight / 3)
         {
            y = _loc2_ + 12;
         }
         else
         {
            y = _loc2_ - height - 1;
         }
         if(y < 12)
         {
            y = 12;
         }
      }*/

      override protected function position() : void
      {
         if(isMarketItem){

         }
         else
         {
            if(stage == null)
            {
               return;
            }
            if(stage.mouseX < stage.stageWidth / 2)
            {
               x = stage.mouseX + 12;
            }
            else
            {
               x = stage.mouseX - width - 1;
            }
            if(stage.mouseY < stage.stageHeight / 3)
            {
               y = stage.mouseY + 12;
            }
            else
            {
               y = stage.mouseY - height - 1;
            }
         }
      }

      private function makeHoldItemText():void{
         if (this.objectXML_.hasOwnProperty("HoldableItem")) {
            this.effects.push(new Effect("Holdable Item",  "As long as you hold this item in your inventory, it will give you an effect according to its power!").setColor("#51FF22"));
         }
      }

      private function addProjectileTagsToEffectsList() : void
      {
         var projXML:XML = null;
         var minD:int = 0;
         var maxD:int = 0;
         var range:Number = NaN;
         var condEffectXML:XML = null;
         if(this.objectXML_.hasOwnProperty("Projectile") && !this.comparisonResults.processedTags.hasOwnProperty(this.objectXML_.Projectile.toXMLString()))
         {
            projXML = XML(this.objectXML_.Projectile);
            minD = int(projXML.MinDamage);
            maxD = int(projXML.MaxDamage);
            this.effects.push(new Effect("Damage",(minD == maxD?minD:minD + " - " + maxD).toString()));
            range = Number(projXML.Speed) * Number(projXML.LifetimeMS) / 10000;
            this.effects.push(new Effect("Range",TooltipHelper.getFormattedRangeString(range)));
            if(this.objectXML_.Projectile.hasOwnProperty("MultiHit"))
            {
               this.effects.push(new Effect("","Shots hit multiple targets"));
            }
            if(this.objectXML_.Projectile.hasOwnProperty("PassesCover"))
            {
               this.effects.push(new Effect("","Shots pass through obstacles"));
            }
            if(this.objectXML_.Projectile.hasOwnProperty("ArmorPiercing"))
            {
               this.effects.push(new Effect("","Ignores defense of the target"));
            }
            for each(condEffectXML in projXML.ConditionEffect)
            {
               if(this.comparisonResults.processedTags[condEffectXML.toXMLString()] == null)
               {
                  this.effects.push(new Effect("Shot Effect",this.objectXML_.Projectile.ConditionEffect + " for " + this.objectXML_.Projectile.ConditionEffect.@duration + " secs"));
               }
            }
         }
      }

      private function addActivateTagsToEffectsList() : void
      {
         var activateXML:XML = null;
         var val:String = null;
         var stat:int = 0;
         var amt:int = 0;
         var activationType:String = null;
         for each(activateXML in this.objectXML_.Activate)
         {
            if(this.comparisonResults.processedTags[activateXML.toXMLString()])
            {
               continue;
            }
            activationType = activateXML.toString();
            switch(activationType)
            {
               case ActivationType.COND_EFFECT_AURA:
                  this.effects.push(new Effect("Party Effect","Within " + activateXML.@range + " sqrs"));
                  this.effects.push(new Effect("","  " + activateXML.@effect + " for " + activateXML.@duration + " secs"));
                  continue;
               case ActivationType.COND_EFFECT_SELF:
                  this.effects.push(new Effect("Effect on Self",""));
                  this.effects.push(new Effect("","  " + activateXML.@effect + " for " + activateXML.@duration + " secs"));
                  continue;
               case ActivationType.HEAL:
                  this.effects.push(new Effect("","+" + activateXML.@amount + " HP"));
                  continue;
               case ActivationType.HEAL_NOVA:
                  this.effects.push(new Effect("Party Heal",activateXML.@amount + " HP at " + activateXML.@range + " sqrs"));
                  continue;
               case ActivationType.MAGIC:
                  this.effects.push(new Effect("","+" + activateXML.@amount + " MP"));
                  continue;
               case ActivationType.MAGIC_NOVA:
                  this.effects.push(new Effect("Fill Party Magic",activateXML.@amount + " MP at " + activateXML.@range + " sqrs"));
                  continue;
               case ActivationType.TELEPORT:
                  this.effects.push(new Effect("","Teleport to Target"));
                  continue;
               case ActivationType.VAMPIRE_BLAST:
                  this.effects.push(new Effect("Steal",activateXML.@totalDamage + " HP within " + activateXML.@radius + " sqrs"));
                  continue;
               case ActivationType.TRAP:
                  this.effects.push(new Effect("Trap",activateXML.@totalDamage + " HP within " + activateXML.@radius + " sqrs"));
                  this.effects.push(new Effect("","  " + (Boolean(activateXML.hasOwnProperty("@condEffect"))?activateXML.@condEffect:"Slowed") + " for " + (Boolean(activateXML.hasOwnProperty("@condDuration"))?activateXML.@condDuration:"5") + " secs"));
                  continue;
               case ActivationType.STASIS_BLAST:
                  this.effects.push(new Effect("Stasis on Group",activateXML.@duration + " secs"));
                  continue;
               case ActivationType.DECOY:
                  this.effects.push(new Effect("Decoy",activateXML.@duration + " secs"));
                  continue;
               case ActivationType.LIGHTNING:
                  this.effects.push(new Effect("Lightning",""));
                  this.effects.push(new Effect(""," " + activateXML.@totalDamage + " to " + activateXML.@maxTargets + " targets"));
                  continue;
               case ActivationType.POISON_GRENADE:
                  this.effects.push(new Effect("Poison Grenade",""));
                  this.effects.push(new Effect("",activateXML.@totalDamage + " Damage (" + activateXML.@impactDamage + " on impact) over "+ activateXML.@duration +" secs within "+ activateXML.@radius +" squares"));
                  continue;
               case ActivationType.REMOVE_NEG_COND:
                  this.effects.push(new Effect("","Removes negative conditions"));
                  continue;
               case ActivationType.REMOVE_NEG_COND_SELF:
                  this.effects.push(new Effect("","Removes negative conditions"));
                  continue;
               case ActivationType.INCREMENT_STAT:
                  stat = int(activateXML.@stat);
                  amt = int(activateXML.@amount);
                  if(stat != StatData.HP_STAT && stat != StatData.MP_STAT)
                  {
                     val = "Permanently increases " + StatData.statToName(stat);
                  }
                  else
                  {
                     val = "+" + amt + " " + StatData.statToName(stat);
                  }
                  this.effects.push(new Effect("",val));
                  continue;
               case ActivationType.GENERIC_ACTIVATE:
                       this.effects.push(new Effect("", "Within " + activateXML.@range + " sqrs " + activateXML.@effect + " for " + activateXML.@duration + " seconds"));
                  continue;
               default:

            }
         }
      }
      private function getPoison(_arg1:XML, _arg2:XML=null):void
      {
         var _local3:ComPair = new ComPair(_arg1, _arg2, "totalDamage");
         var _local4:ComPair = new ComPair(_arg1, _arg2, "radius");
         var _local5:ComPair = new ComPair(_arg1, _arg2, "duration");
         var _local7:ComPair = new ComPair(_arg1, _arg2, "impactDamage", 0);
         var _local10:String = this.colorUntiered("Poison: ");








         _local10 = (_local10 + TooltipHelper.compare(_local3.a, _local3.b, true, "", false, !(this.sameActivateEffect)) + " damage");
         if (_local7.a){
            _local10 = (_local10 + " (" + TooltipHelper.compare(_local7.a, _local7.b, true, "", false, !(this.sameActivateEffect)) + " on impact)");
         }
         _local10 = (_local10 + " within " + TooltipHelper.compareAndGetPlural(_local4.a, _local4.b, "square", true, !(this.sameActivateEffect)));
         _local10 = (_local10 + " over "+ TooltipHelper.compareAndGetPlural(_local5.a, _local5.b, "second", false, !(this.sameActivateEffect)));
         this.effects.push(new Effect(_local10, ""));
         this.AddConditionToEffects(_arg1, _arg2, "Nothing", 5);
         this.sameActivateEffect = true;
      }
      private var sameActivateEffect:Boolean;

      private function AddConditionToEffects(_arg1:XML, _arg2:XML, _arg3:String="Nothing", _arg4:Number=5):void
      {
         var _local6:ComPair;
         var _local7:String;
         var _local5:String = ((_arg1.hasOwnProperty("@condEffect")) ? _arg1.@condEffect : _arg3);
         if (_local5 != "Nothing"){
            _local6 = new ComPair(_arg1, _arg2, "condDuration", _arg4);
            if (_arg2){
               _local7 = ((_arg2.hasOwnProperty("@condEffect")) ? _arg2.@condEffect : _arg3);
               if (_local7 == "Nothing"){
                  _local6.b = 0;
               }
            }

            var condition:String = TooltipHelper.compareAndGetPlural(_local6.a, _local6.b, "second");
            var duration:String = _local5;
            this.effects.push(new Effect("Inflicts " + condition + " for " + duration, "Something"));
         }
      }

      private function colorUntiered(_arg1:String):String
      {
         var _local2:Boolean = this.objectXML_.hasOwnProperty("Tier");
         var _local3:Boolean = this.objectXML_.hasOwnProperty("@setType");
         if (_local3){
            return (TooltipHelper.wrapInFontTag(_arg1, ("#" + TooltipHelper.SET_COLOR.toString(16))));
         }
         if (!_local2){
            return (TooltipHelper.wrapInFontTag(_arg1, ("#" + TooltipHelper.UNTIERED_COLOR.toString(16))));
         }
         return (_arg1);
      }

      override protected function alignUI():void {
         this.titleText_.x = (this.icon_.width + 4);
         this.titleText_.y = ((this.icon_.height / 2) - (this.titleText_.height / 2));
         if (this.tierText_) {
            this.tierText_.y = ((this.icon_.height / 2) - (this.tierText_.height / 2));
            this.tierText_.x = (MAX_WIDTH - 30);
         }
         this.descText_.x = 4;
         this.descText_.y = (this.icon_.height + 2);
         if (this.line1_ != null && this.effectsText_ != null) {
            this.line1_.x = 8;
            this.line1_.y = ((this.descText_.y + this.descText_.height));
            this.effectsText_.x = 4;
            this.effectsText_.y = (this.line1_.y + 8);
         }
         this.line2_.x = 8;
         this.line2_.y = this.effectsText_ != null ? ((this.effectsText_.y + this.effectsText_.height) + 8) : ((this.descText_.y + this.descText_.height));
         var _local1:uint = (this.line2_.y + 8);
         if (this.restrictionsText_) {
            this.restrictionsText_.x = 4;
            this.restrictionsText_.y = _local1;
            _local1 = (_local1 + this.restrictionsText_.height);
         }
         /*if (this.powerText) {
            if (contains(this.powerText)) {
               this.powerText.x = 4;
               this.powerText.y = _local1;
            }
         }*/
         if (this.itemEffectText_) {
            if (contains(this.itemEffectText_)) {
               this.itemEffectText_.x = 4;
               this.itemEffectText_.y = _local1;
               this.itemEffectText_.useTextDimensions();
            }
         }
      }

      private static function formatStringForPluralValue(amount:uint, string:String) : String
      {
         if(amount > 1)
         {
            string = string + "s";
         }
         return string;
      }

      private function addActivateOnEquipTagsToEffectsList() : void
      {
         var activateXML:XML = null;
         var customText:String = null;
         var first:Boolean = true;
         for each(activateXML in this.objectXML_.ActivateOnEquip)
         {
            if(first)
            {
               this.effects.push(new Effect("On Equip",""));
               first = false;
            }
            customText = this.comparisonResults.processedActivateOnEquipTags[activateXML.toXMLString()];
            if(customText != null)
            {
               this.effects.push(new Effect("","" + customText));
            }
            else if(activateXML.toString() == "IncrementStat")
            {
               this.effects.push(new Effect("",this.compareIncrementStat(activateXML)));
            }
         }

         if(this.props_.talismanEffect_ != null) {
            this.effects.push(new Effect("Passive", ""));
            this.effects.push(new Effect("", " - " + this.props_.talismanEffect_));
         }
      }

      private function compareIncrementStat(activateXML:XML) : String
      {
         var amountString:String = null;
         var match:XML = null;
         var otherAmount:int = 0;
         var stat:int = int(activateXML.@stat);
         var amount:int = int(activateXML.@amount);
         var textColor:String = this.playerCanUse?TooltipHelper.BETTER_COLOR:TooltipHelper.NO_DIFF_COLOR;
         var otherMatches:XMLList = null;
         if(this.curItemXML_ != null)
         {
            otherMatches = this.curItemXML_.ActivateOnEquip.(@stat == stat);
         }
         if(otherMatches != null && otherMatches.length() == 1)
         {
            match = XML(otherMatches[0]);
            otherAmount = int(match.@amount);
            textColor = TooltipHelper.getTextColor(amount - otherAmount);
         }
         if(amount > -1)
         {
            amountString = String("+" + amount);
         }
         else
         {
            amountString = String(amount);
            textColor = "#ff0000";
         }
         return TooltipHelper.wrapInFontTag(amountString + " " + StatData.statToName(stat),textColor);
      }

      private function addEquipmentItemRestrictions() : void
      {
         if(this.props_.onlyOneTalisman_) {
            this.restrictions.push(new Restriction("Only one can be equip", 0x7fce6d,true));
         }

         this.restrictions.push(new Restriction("Must be equipped to use",11776947,false));
         if(this.isInventoryFull || this.inventoryOwnerType == InventoryOwnerTypes.CURRENT_PLAYER)
         {
            this.restrictions.push(new Restriction("Double-Click to equip",11776947,false));
         }
         else
         {
            this.restrictions.push(new Restriction("Double-Click to take",11776947,false));
         }
      }

      private function addAbilityItemRestrictions() : void
      {
         this.restrictions.push(new Restriction("Press [" + KeyCodes.CharCodeStrings[Parameters.data_.useSpecial] + "] in world to use",16777215,false));
      }

      private function addConsumableItemRestrictions() : void
      {
         this.restrictions.push(new Restriction("Consumed with use",11776947,false));
         if(this.isInventoryFull || this.inventoryOwnerType == InventoryOwnerTypes.CURRENT_PLAYER)
         {
            this.restrictions.push(new Restriction("Double-Click or Shift-Click on item to use",16777215,false));
         }
         else
         {
            this.restrictions.push(new Restriction("Double-Click to take & Shift-Click to use",16777215,false));
         }
      }

      private function addReusableItemRestrictions() : void
      {
         this.restrictions.push(new Restriction("Can be used multiple times",11776947,false));
         this.restrictions.push(new Restriction("Double-Click or Shift-Click on item to use",16777215,false));
      }
       private static function parse(str:String):Number {
           for (var i:Number = 0; i < str.length; i++) {
               var c:String = str.charAt(i);
               if (c != "0") break;
           }

           return Number(str.substr(i));
       }
       private var spriteFile:String = null;
       private var first:Number = -1;
       private var last:Number = -1;
       private var next:Number = -1;
       private var animatedTimer:Timer;

       private function makeAnimation(event:TimerEvent = null):void {
           if (this.spriteFile == null)
               return;

           var size:int = this.iconSize;
           var bitmapData:BitmapData = AssetLibrary.getImageFromSet(this.spriteFile, this.next);

         //  if (Parameters.itemTypes16.indexOf(this.objectType_) != -1 || bitmapData.height == 16)
         //      size = (size * 0.5);

           bitmapData = TextureRedrawer.redraw(bitmapData, size, true, 0, true, 5);

           this.icon_.bitmapData = bitmapData;
           this.icon_.x = this.icon_.y = - 4;

           this.next++;

           if (this.next > this.last)
               this.next = this.first;
       }

       private function makeRestrictionList() : void
      {
         var reqXML:XML = null;
         var reqMet:Boolean = false;
         var stat:int = 0;
         var value:int = 0;

          var spritePeriod:int = -1;
          var spriteFile:String = null;
          var spriteArray:Array = null;
          var first:Number = -1;
          var last:Number = -1;
          var next:Number = -1;
          var makeAnimation:Function;
          var hasPeriod:Boolean = this.objectXML_.hasOwnProperty("@spritePeriod");
          var hasFile:Boolean = this.objectXML_.hasOwnProperty("@spriteFile");
          var hasArray:Boolean = this.objectXML_.hasOwnProperty("@spriteArray");
          var hasAnimatedSprites:Boolean = hasPeriod && hasFile && hasArray;

          if (hasPeriod)
              spritePeriod = 1000 / this.objectXML_.attribute("spritePeriod");

          if (hasFile)
              spriteFile = this.objectXML_.attribute("spriteFile");

          if (hasArray) {
              spriteArray = String(this.objectXML_.attribute("spriteArray")).split('-');
              first = parse(spriteArray[0]);
              last = parse(spriteArray[1]);
          }

          if (hasAnimatedSprites && spritePeriod != -1 && spriteFile != null && spriteArray != null && first != -1 && last != -1) {
              this.spriteFile = spriteFile;
              this.first = first;
              this.last = last;
              this.next = this.first;
              this.animatedTimer = new Timer(spritePeriod);
              this.animatedTimer.addEventListener(TimerEvent.TIMER, this.makeAnimation);
              this.animatedTimer.start();
          }

         this.restrictions = new Vector.<Restriction>();
         if(this.objectXML_.hasOwnProperty("VaultItem") && this.invType != -1 && this.invType != ObjectLibrary.idToType_["Vault Chest"])
         {
            this.restrictions.push(new Restriction("Store this item in your Vault to avoid losing it!",16549442,true));
         }
         if(this.objectXML_.hasOwnProperty("Soulbound"))
         {
            this.restrictions.push(new Restriction("Soulbound",11776947,false));
         }
         if(this.playerCanUse)
         {
            if(this.objectXML_.hasOwnProperty("Usable"))
            {
               this.addAbilityItemRestrictions();
               this.addEquipmentItemRestrictions();
            }
            else if(this.objectXML_.hasOwnProperty("Consumable"))
            {
               this.addConsumableItemRestrictions();
            }
            else if(this.objectXML_.hasOwnProperty("InvUse"))
            {
               this.addReusableItemRestrictions();
            }
            else
            {
               this.addEquipmentItemRestrictions();
            }
         }
         else if(this.player_ != null)
         {
            this.restrictions.push(new Restriction("Not usable by " + ObjectLibrary.typeToDisplayId_[this.player_.objectType_],16549442,true));
         }
         var usable:Vector.<String> = ObjectLibrary.usableBy(this.objectType_);
         if(usable != null)
         {
            this.restrictions.push(new Restriction("Usable by: " + usable.join(", "),11776947,false));
         }
         for each(reqXML in this.objectXML_.EquipRequirement)
         {
            reqMet = ObjectLibrary.playerMeetsRequirement(reqXML,this.player_);
            if(reqXML.toString() == "Stat")
            {
               stat = int(reqXML.@stat);
               value = int(reqXML.@value);
               this.restrictions.push(new Restriction("Requires " + StatData.statToName(stat) + " of " + value,reqMet?11776947:16549442,reqMet?Boolean(false):Boolean(true)));
            }
         }
      }

      private function makeLineTwo():void{
         this.line2_ = new LineBreakDesign((MAX_WIDTH - 12), 0);
          if(isLegendaryItem){
              this.line2_.setWidthColor(MAX_WIDTH-12,0x967bb6);
          }else if(isMythicalItem){
              this.line2_.setWidthColor(MAX_WIDTH-12,0xFFFFFF);
          }
         addChild(this.line2_);
      }

      private function makeRestrictionText() : void
      {
         var sheet:StyleSheet = null;
         if(this.restrictions.length != 0)
         {
            //sheet = new StyleSheet();
            //sheet.parseCSS(CSS_TEXT);
            this.restrictionsText_ = new SimpleText(14,11776947,false,MAX_WIDTH - 4,0);
            //this.restrictionsText_.styleSheet = sheet;
            this.restrictionsText_.wordWrap = true;
            this.restrictionsText_.htmlText = "<span class=\'in\'>" + BuildRestrictionsHTML(this.restrictions) + "</span>";
            this.restrictionsText_.useTextDimensions();
            this.restrictionsText_.x = 4;
            this.restrictionsText_.y = this.line2_.y + 8;
            switch(Parameters.data_.itemDataOutlines)
            {
               case 0:
                  this.restrictionsText_.filters = FilterUtil.getTextOutlineFilter();
                  break;
               case 1:
                  this.restrictionsText_.filters = [new DropShadowFilter(0,0,0,0.5,12,12)];
            }
            addChild(this.restrictionsText_);
         }
      }

      private function addDescriptionText() : void
      {
         var _local_1:int;
          if(this.objectXML_.hasOwnProperty("@setType") || this.objectXML_.hasOwnProperty("SetTier")) {
            _local_1 = TooltipHelper.SET_COLOR;
         }
         else if(this.objectXML_.hasOwnProperty("SNormal")) {
            _local_1 = TooltipHelper.S;
         }
         else if(this.objectXML_.hasOwnProperty("SPlus")) {
            _local_1 = TooltipHelper.SPlus;
         }
         else{
            _local_1 = 0xB3B3B3;
         }
         this.descText_ = new SimpleText(14, _local_1, false, MAX_WIDTH, 0);
         this.descText_.wordWrap = true;
         this.descText_.text = String(this.objectXML_.Description);
         this.descText_.useTextDimensions();
         switch(Parameters.data_.itemDataOutlines)
         {
            case 0:
               this.descText_.filters = FilterUtil.getTextOutlineFilter();
               break;
            case 1:
               this.descText_.filters = [new DropShadowFilter(0,0,0,0.5,12,12)];
         }
         this.descText_.updateMetrics();
         addChild(this.descText_);
      }

      /*private function addDescriptionText() : void
      {
         this.descText_ = new SimpleText(14,11776947,false,MAX_WIDTH,0);
         this.descText_.wordWrap = true;
         this.descText_.text = String(this.objectXML_.Description);
         this.descText_.updateMetrics();
         this.descText_.filters = [new DropShadowFilter(0,0,0,0.5,12,12)];
         this.descText_.x = 4;
         this.descText_.y = this.icon_.height + 2;
         addChild(this.descText_);
      }*/

      private function round2(num:Number, decimals:int):Number
      {
         var m:int = Math.pow(10, decimals);
         return Math.round(num * m) / m;
      }

      private function buildCategorySpecificText() : void
      {
         if(this.curItemXML_ != null)
         {
            this.comparisonResults = this.slotTypeToTextBuilder.getComparisonResults(this.objectXML_,this.curItemXML_);
         }
         else
         {
            this.comparisonResults = new SlotComparisonResult();
         }
      }

      private function handleWisMod():void {
         var _local3:XML;
         var _local4:XML;
         var _local5:String;
         var _local6:String;
         if (this.player_ == null) {
            return;
         }
         var _local1:Number = (this.player_.wisdom_ + this.player_.wisdomBoost_);
         if (_local1 < 30) {
            return;
         }
         var _local2:Vector.<XML> = new Vector.<XML>();
         if (this.curItemXML_ != null) {
            this.curItemXML_ = this.curItemXML_.copy();
            _local2.push(this.curItemXML_);
         }
         if (this.objectXML_ != null) {
            this.objectXML_ = this.objectXML_.copy();
            _local2.push(this.objectXML_);
         }
         for each (_local4 in _local2) {
            for each (_local3 in _local4.Activate) {
               _local5 = _local3.toString();
               if (_local3.@effect != "Stasis") {
                  _local6 = _local3.@useWisMod;
                  if (!(((((((_local6 == "")) || ((_local6 == "false")))) || ((_local6 == "0")))) || ((_local3.@effect == "Stasis")))) {
                     switch (_local5) {
                        case ActivationType.HEAL_NOVA:
                           _local3.@amount = this.modifyWisModStat(_local3.@amount, 0);
                           _local3.@range = this.modifyWisModStat(_local3.@range);
                           break;
                        case ActivationType.COND_EFFECT_AURA:
                           _local3.@duration = this.modifyWisModStat(_local3.@duration);
                           _local3.@range = this.modifyWisModStat(_local3.@range);
                           break;
                        case ActivationType.COND_EFFECT_SELF:
                           _local3.@duration = this.modifyWisModStat(_local3.@duration);
                           break;
                        case ActivationType.STAT_BOOST_AURA:
                           _local3.@amount = this.modifyWisModStat(_local3.@amount, 0);
                           _local3.@duration = this.modifyWisModStat(_local3.@duration);
                           _local3.@range = this.modifyWisModStat(_local3.@range);
                           break;
                        case ActivationType.GENERIC_ACTIVATE:
                           _local3.@duration = this.modifyWisModStat(_local3.@duration);
                           _local3.@range = this.modifyWisModStat(_local3.@range);
                           break;
                        case ActivationType.POISON_GRENADE:
                           _local3.@impactDamage = this.modifyWisModStat(_local3.@impactDamage, 0);
                           _local3.@totalDamage = this.modifyWisModStat(_local3.@totalDamage, 0);
                           break;
                        case ActivationType.LIGHTNING:
                           _local3.@totalDamage = this.modifyWisModStat(_local3.@totalDamage, 0);
                           break;
                        case ActivationType.VAMPIRE_BLAST:
                           _local3.@totalDamage = this.modifyWisModStat(_local3.@totalDamage, 0);
                           break;
                     }
                  }
               }
            }
         }
      }

      public function modifyWisModStat(_arg1:String, _arg2:Number = 1):String {
         var _local5:Number;
         var _local6:int;
         var _local7:Number;
         var _local3:String = "-1";
         var _local4:Number = (this.player_.wisdom_ + this.player_.wisdomBoost_);
         if (_local4 < 50) {
            _local3 = _arg1;
         }
         else {
            _local5 = Number(_arg1);
            _local6 = (((_local5) < 0) ? -1 : 1);
            _local7 = (((_local5 * _local4) / 150) + (_local5 * _local6));
            _local7 = (Math.floor((_local7 * Math.pow(10, _arg2))) / Math.pow(10, _arg2));
            if ((_local7 - (int(_local7) * _local6)) >= ((1 / Math.pow(10, _arg2)) * _local6)) {
               _local3 = _local7.toFixed(1);
            }
            else {
               _local3 = _local7.toFixed(0);
            }
         }
         return (_local3);
      }

      private function BuildEffectsHTML(effects:Vector.<Effect>) : String
      {
         var effect:Effect = null;
         var textColor:String = null;
         var nameColor:String = null;
         var html:String = "";
         var first:Boolean = true;
         for each(effect in effects)
         {
            textColor = "#FFFF8F";
            if(!first)
            {
               html = html + "\n";
            }
            else
            {
               first = false;
            }
            nameColor = effect.color_ != "" ? effect.color_ : null;
            if(effect.name_ != "")
            {
               html = html + ("<font color=\"" + nameColor + "\">" + effect.name_ + "</font>" + ": ");
            }
            if(this.isEmptyEquipSlot())
            {
               textColor = "#00ff00";
            }
            html = html + ("<font color=\"" + textColor + "\">" + effect.value_ + "</font>");
         }
         return html;
      }

   }
}

class ComPair
{
   public var a:Number;
   public var b:Number;

   public function ComPair(_arg1:XML, _arg2:XML, _arg3:String, _arg4:Number=0)
   {
      this.a = (this.b = ((_arg1.hasOwnProperty(("@" + _arg3))) ? _arg1.@[_arg3] : _arg4));
      if (_arg2){
         this.b = ((_arg2.hasOwnProperty(("@" + _arg3))) ? _arg2.@[_arg3] : _arg4);
      };
   }
   public function add(_arg1:Number):void
   {
      this.a = (this.a + _arg1);
      this.b = (this.b + _arg1);
   }

}

class Effect
{


   public var name_:String;

   public var value_:String;

   public var color_:String = "#B3B3B3";

   function Effect(name:String, value:String)
   {
      super();
      this.name_ = name;
      this.value_ = value;
   }

   public function setColor(_arg1:String):Effect {
      this.color_ = _arg1;
      return (this);
   }

}

class Restriction
{


   public var text_:String;

   public var color_:uint;

   public var bold_:Boolean;

   function Restriction(text:String, color:uint, bold:Boolean)
   {
      super();
      this.text_ = text;
      this.color_ = color;
      this.bold_ = bold;
   }
}
