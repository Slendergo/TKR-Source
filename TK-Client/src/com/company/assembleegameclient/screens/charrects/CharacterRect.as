package com.company.assembleegameclient.screens.charrects
{
import com.company.rotmg.graphics.StarGraphic;
import com.company.ui.SimpleText;

import flash.display.Graphics;
   import flash.display.Shape;
   import flash.display.Sprite;
   import flash.events.MouseEvent;
import flash.filters.DropShadowFilter;
import flash.geom.ColorTransform;
import flash.text.TextField;

import io.decagames.rotmg.ui.sliceScaling.SliceScalingBitmap;
import io.decagames.rotmg.ui.texture.TextureParser;
import io.decagames.rotmg.utils.colors.Tint;

public class CharacterRect extends Sprite
   {
      public static const WIDTH:int = 760;
      public static const HEIGHT:int = 70;

      private var color_:uint;
      private var overColor_:uint;
      private var box_:SliceScalingBitmap;
      public var selectContainer:Sprite;
      protected var taglineIcon:Sprite;
      protected var taglineText:SimpleText;
      protected var classNameText:SimpleText;
      
      public function CharacterRect(color:uint, overColor:uint)
      {
         super();
         this.color_ = color;
         this.overColor_ = overColor;
         this.makeBox();
         addChild(this.box_);
         addEventListener(MouseEvent.MOUSE_OVER,this.onMouseOver);
         addEventListener(MouseEvent.ROLL_OUT,this.onRollOut);
      }

      protected static function makeDropShadowFilter():Array {
         return ([new DropShadowFilter(0, 0, 0, 1, 8, 8)]);
      }

      public function makeBox() : void
      {
         this.box_ = TextureParser.instance.getSliceScalingBitmap("UI","popup_header_title",WIDTH);
         this.box_.height = HEIGHT;
         this.box_.x = 0;
         this.box_.y = 0;
         addChild(this.box_);
      }

      protected function onMouseOver(_arg1:MouseEvent):void {
         this.drawBox(true);
      }

      protected function onRollOut(_arg1:MouseEvent):void {
         this.drawBox(false);
      }

      private function drawBox(param1:Boolean) : void
      {
         if(param1)
         {
            Tint.add(this.box_,13224393,0.2);
         }
         else
         {
            this.box_.transform.colorTransform = new ColorTransform();
         }
         this.box_.scaleX = 1;
         this.box_.scaleY = 1;
         this.box_.x = 0;
         this.box_.y = 0;
      }

      public function makeContainer():void {
         this.selectContainer = new Sprite();
         this.selectContainer.mouseChildren = false;
         this.selectContainer.buttonMode = true;
         this.selectContainer.graphics.beginFill(0xFF00FF, 0);
         this.selectContainer.graphics.drawRect(0, 0, WIDTH, HEIGHT);
         addChild(this.selectContainer);
      }



   }
}
