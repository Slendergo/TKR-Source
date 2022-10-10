package com.company.assembleegameclient.ui.panels
{
import com.company.assembleegameclient.ui.Slot;
import com.company.assembleegameclient.ui.panels.itemgrids.*;
   import com.company.assembleegameclient.objects.GameObject;
   import com.company.assembleegameclient.objects.Player;
   import com.company.assembleegameclient.ui.panels.itemgrids.itemtiles.EquipmentTile;
import com.company.assembleegameclient.ui.tooltip.TooltipHelper;

import kabam.rotmg.constants.ItemConstants;

public class EquippedTalismanGrid extends ItemGrid
   {
      private const NUM_SLOTS:uint = 8;
      
      private var tiles:Vector.<EquipmentTile>;
      
      public function EquippedTalismanGrid(gridOwner:GameObject, currentPlayer:Player, itemIndexOffset:int = 0)
      {
         var tile:EquipmentTile = null;
         super(gridOwner,currentPlayer,itemIndexOffset);
         this.tiles = new Vector.<EquipmentTile>(this.NUM_SLOTS);
         for(var i:int = 0; i < this.NUM_SLOTS; i++)
         {
            tile = new EquipmentTile(i + itemIndexOffset,this,interactive, i < 4 ? 0x545454 : i == 6 || i == 7 ? 0x44262C : 0x6B5C32);
            addToGrid(tile,2,i);
            tile.setType(ItemConstants.TALISMAN_TYPE, true);
            this.tiles[i] = tile;
         }
      }

      public function toggleTierTags(_arg_1:Boolean) : void
      {
         var _local_2:EquipmentTile = null;
         for each(_local_2 in this.tiles)
         {
            _local_2.toggleTierTag(_arg_1);
         }
      }

      override public function setItems(items:Vector.<int>, datas:Vector.<Object>, itemIndexOffset:int = 0) : void
      {
         var numItems:int = 0;
         var i:int = 0;
         if(items)
         {
            numItems = items.length;
            for(i = 0; i < this.tiles.length; i++)
            {
               if(i + itemIndexOffset < numItems)
               {
                  this.tiles[i].setItem(items[i + itemIndexOffset], datas != null ? datas[i + indexOffset] : null);
               }
               else
               {
                  this.tiles[i].setItem(-1, null);
               }
            }
         }
      }
   }
}
