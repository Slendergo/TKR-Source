package kabam.rotmg.ui.view.components
{
   import com.company.assembleegameclient.ui.SoundIcon;
   import flash.display.Sprite;

import kabam.rotmg.ui.view.TitleView_TitleScreenBackground;

public class ScreenBase extends Sprite
   {
      //private var map:MapBackground;
      
      private var soundIcon:SoundIcon;
      
      private var darkenFactory:DarkenFactory;
      
      public function ScreenBase()
      {
         this.darkenFactory = new DarkenFactory();
         super();
         //this.map = new MapBackground();
         //addChild(this.map);
         addChild(new TitleView_TitleScreenBackground());
         addChild(this.darkenFactory.create());
      }
   }
}
