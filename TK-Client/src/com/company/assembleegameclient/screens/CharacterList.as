package com.company.assembleegameclient.screens
{
   import com.company.assembleegameclient.screens.charrects.CharacterRectList;
   import flash.display.Graphics;
   import flash.display.Shape;
   import flash.display.Sprite;
   import kabam.rotmg.core.model.PlayerModel;
   
   public class CharacterList extends Sprite
   {
      
      public static const WIDTH:int = 760;
      
      public static const HEIGHT:int = 430;

       public static const TYPE_CHAR_SELECT:int = 1;
       public static const TYPE_GRAVE_SELECT:int = 2;
      
      public var charRectList_:Sprite;
      
      public function CharacterList(model:PlayerModel, tab: int)
      {
         var shape:Shape = null;
         var g:Graphics = null;
         super();
          switch (tab) {
              case TYPE_CHAR_SELECT:
                  this.charRectList_ = new CharacterRectList();
                  break;
              case TYPE_GRAVE_SELECT:
                  this.charRectList_ = new Graveyard(model);
                  break;
              default:
                  this.charRectList_ = new Sprite();
          }
         addChild(this.charRectList_);
         if(height > 400)
         {
            shape = new Shape();
            g = shape.graphics;
            g.beginFill(0);
            g.drawRect(0,0,WIDTH,HEIGHT);
            g.endFill();
            addChild(shape);
            mask = shape;
         }
      }
      
      public function setPos(pos:Number) : void
      {
         this.charRectList_.y = pos;
      }
   }
}
