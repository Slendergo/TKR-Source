package com.company.assembleegameclient.ui.tooltip
{
   public class TooltipHelper
   {
      
      public static const BETTER_COLOR:String = "#00ff00";
      
      public static const WORSE_COLOR:String = "#ff0000";
      
      public static const NO_DIFF_COLOR:String = "#FFFF8F";

      public static const S:uint = 0x03cafc;

      public static const SPlus:uint = 0xfcc203;

      public static const WIS_BONUS_COLOR:uint = 4219875;

      public static const UNTIERED_COLOR:uint = 0x8A2BE2;

      public static const SET_COLOR:uint = 0x0032c9;

      public static const LEGENDARY_COLOR:uint = 0xffcc66 ;

      public static const HOLDABLE_COLOR:uint = 0x51FF22;

      public static const HEROIC_COLOR:uint = 0x00B2FF;

      public static const TIER_COLOR:uint = 0xffffff;

      public static const REVENGE_COLOR:uint = 0x9b111e;
      public static const ETERNAL_COLOR:uint = 0x98ff98;

      
      public function TooltipHelper()
      {
         super();
      }
      
      public static function wrapInFontTag(text:String, color:String) : String
      {
         var tagStr:String = "<font color=\"" + color + "\">" + text + "</font>";
         return tagStr;
      }
      
      public static function getFormattedRangeString(range:Number) : String
      {
         var decimalPart:Number = range - int(range);
         return int(decimalPart * 10) == 0?int(range).toString():range.toFixed(1);
      }
      
      public static function getTextColor(difference:Number) : String
      {
         if(difference < 0)
         {
            return WORSE_COLOR;
         }
         if(difference > 0)
         {
            return BETTER_COLOR;
         }
         return NO_DIFF_COLOR;
      }
   }
}
