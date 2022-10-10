package com.company.assembleegameclient.ui.tooltip
{
   public class TooltipHelper
   {
      public static const BETTER_COLOR:String = "#00ff00";
      public static const WORSE_COLOR:String = "#ff0000";
      public static const NO_DIFF_COLOR:String = "#FFFF8F";
      public static const BETTER_COLOR_INT:uint = 0xFF00;
      public static const WORSE_COLOR_INT:uint = 0xFF0000;
      public static const NO_DIFF_COLOR_INT:uint = 0xFFFF8F;
      public static const S:uint = 0x03cafc;
      public static const SPlus:uint = 0xfcc203;
      public static const UNTIERED_COLOR:uint = 0x8A2BE2;
      public static const SET_COLOR:uint = 0x0032c9;
      public static const LEGENDARY_COLOR:uint = 0xebb011;
      public static const MYTHICAL_COLOR:uint = 0xcf1433;

      public static function wrapInFontTag(text:String, color:String):String
      {
         return "<font color=\"" + color + "\">" + text + "</font>";
      }

      public static function getOpenTag(_arg1:uint):String
      {
         return ((('<font color="#' + _arg1.toString(16)) + '">'));
      }

      public static function getCloseTag():String
      {
         return ("</font>");
      }

      public static function getFormattedRangeString(range:Number) : String
      {
         var decimalPart:Number = range - int(range);
         return int(decimalPart * 10) == 0?int(range).toString():range.toFixed(1);
      }

      public static function compareAndGetPlural(_arg1:Number, _arg2:Number, _arg3:String, _arg4:Boolean=true, _arg5:Boolean=true):String
      {
         var plural:String = getPlural(_arg1, _arg3);
         return (wrapInFontTag(plural, ("#" + getTextColorInt((((_arg4) ? (_arg1 - _arg2) : (_arg2 - _arg1)) * int(_arg5))).toString(16))));
      }

      public static function compare(_arg1:Number, _arg2:Number, _arg3:Boolean=true, _arg4:String="", _arg5:Boolean=false, _arg6:Boolean=true):String
      {
         return (wrapInFontTag((((_arg5) ? Math.abs(_arg1) : _arg1) + _arg4), ("#" + getTextColorInt((((_arg3) ? (_arg1 - _arg2) : (_arg2 - _arg1)) * int(_arg6))).toString(16))));
      }

      public static function getPlural(_arg1:Number, _arg2:String):String
      {
         var _local3:String = ((_arg1 + " ") + _arg2);
         if (_arg1 != 1){
            return ((_local3 + "s"));
         };
         return (_local3);
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
      public static function getTextColorInt(difference:Number) : uint
      {
         if(difference < 0)
         {
            return WORSE_COLOR_INT;
         }
         if(difference > 0)
         {
            return BETTER_COLOR_INT;
         }
         return NO_DIFF_COLOR_INT;
      }
   }
}
