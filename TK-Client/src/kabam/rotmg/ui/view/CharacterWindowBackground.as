package kabam.rotmg.ui.view
{
   import flash.display.Sprite;
   
   public class CharacterWindowBackground extends Sprite
   {
       
      
      public function CharacterWindowBackground()
      {
         super();
         var bg:Sprite = new Sprite();
         bg.graphics.beginFill(3552822);
         bg.graphics.drawRect(0,0,200,600);
         addChild(bg);
      }
   }
}
