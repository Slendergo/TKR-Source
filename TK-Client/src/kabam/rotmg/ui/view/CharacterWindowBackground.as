package kabam.rotmg.ui.view
{
import flash.display.Bitmap;
import flash.display.Sprite;

import kabam.rotmg.assets.HudViewBackground;
import kabam.rotmg.assets.MinimapOverlay;

public class CharacterWindowBackground extends Sprite
   {
       
      
      public function CharacterWindowBackground()
      {
//         var bg:Sprite = new Sprite();
//         bg.graphics.beginFill(3552822);
//         bg.graphics.drawRect(0,0,200,600);
//         addChild(bg);
         addChild(new Bitmap(new HudViewBackground().bitmapData));
      }
   }
}
