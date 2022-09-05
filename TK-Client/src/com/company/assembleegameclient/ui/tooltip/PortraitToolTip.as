package com.company.assembleegameclient.ui.tooltip
{
   import com.company.assembleegameclient.objects.GameObject;
import com.company.assembleegameclient.ui.QuestHealthBar;
import com.company.util.BitmapUtil;
   import flash.display.Bitmap;
   import flash.display.BitmapData;
   
   public class PortraitToolTip extends ToolTip
   {
      private var portrait_:Bitmap;
      private var hpBar_:QuestHealthBar;

      public function PortraitToolTip(go:GameObject)
      {
         super(6036765,1,16549442,1,false);
         this.portrait_ = new Bitmap();
         this.portrait_.x = 0;
         this.portrait_.y = 0;
         var portraitBD:BitmapData = go.getPortrait();
         portraitBD = BitmapUtil.cropToBitmapData(portraitBD,10,10,portraitBD.width - 20,portraitBD.height - 20);
         this.portrait_.bitmapData = portraitBD;
         addChild(this.portrait_);

         this.hpBar_ = new QuestHealthBar(go, 24, 3);
         this.hpBar_.y = this.portrait_.height + 4;
         addChild(this.hpBar_);

         filters = [];
      }

      public override function draw():void
      {
         this.hpBar_.draw();
         super.draw();
      }
   }
}
