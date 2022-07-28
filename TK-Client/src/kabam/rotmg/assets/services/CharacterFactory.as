package kabam.rotmg.assets.services
{
   import com.company.assembleegameclient.util.AnimatedChar;
   import com.company.assembleegameclient.util.AnimatedChars;
   import com.company.assembleegameclient.util.MaskedImage;
   import com.company.assembleegameclient.util.TextureRedrawer;
import com.company.assembleegameclient.util.redrawers.GlowRedrawer;
import com.company.util.BitmapUtil;
   import flash.display.BitmapData;
   import kabam.rotmg.assets.model.Animation;
   import kabam.rotmg.assets.model.CharacterTemplate;
   
   public class CharacterFactory
   {
       
      
      private var texture1:int;
      
      private var texture2:int;
      
      private var size:int;
      
      public function CharacterFactory()
      {
         super();
      }
      
      public function makeCharacter(template:CharacterTemplate) : AnimatedChar
      {
         return AnimatedChars.getAnimatedChar(template.file,template.index);
      }
      
      public function makeIcon(template:CharacterTemplate, size:int = 100, texture1:int = 0, texture2:int = 0) : BitmapData
      {
         this.texture1 = texture1;
         this.texture2 = texture2;
         this.size = size;
         var character:AnimatedChar = this.makeCharacter(template);
         var data:BitmapData = this.makeFrame(character,AnimatedChar.STAND,0);
         data = GlowRedrawer.outlineGlow(data,0);
         data = BitmapUtil.cropToBitmapData(data,6,6,data.width - 12,data.height - 6);
         return data;
      }
      
      public function makeWalkingIcon(template:CharacterTemplate, size:int = 100, texture1:int = 0, texture2:int = 0) : Animation
      {
         this.texture1 = texture1;
         this.texture2 = texture2;
         this.size = size;
         var character:AnimatedChar = this.makeCharacter(template);
         var first:BitmapData = this.makeFrame(character,AnimatedChar.WALK,0.5);
         first = GlowRedrawer.outlineGlow(first,0);
         var second:BitmapData = this.makeFrame(character,AnimatedChar.WALK,0);
         second = GlowRedrawer.outlineGlow(second,0);
         var animation:Animation = new Animation();
         animation.setFrames(first,second);
         return animation;
      }
      
      private function makeFrame(character:AnimatedChar, action:int, offset:Number) : BitmapData
      {
         var data:MaskedImage = character.imageFromDir(AnimatedChar.RIGHT,action,offset);
         return TextureRedrawer.resize(data.image_,data.mask_,this.size,false,this.texture1,this.texture2);
      }
   }
}
