package kabam.rotmg.ui.view
{
   import com.company.assembleegameclient.objects.Player;
import com.company.assembleegameclient.parameters.Parameters;
import com.company.assembleegameclient.ui.ExperienceBoostTimerPopup;
import com.company.assembleegameclient.ui.StatusBar;
   import flash.display.Sprite;
   import flash.events.Event;
   
   public class StatMetersView extends Sprite
   {
       
      
      private var expBar_:StatusBar;
      
      private var fameBar_:StatusBar;
      
      private var hpBar_:StatusBar;
      
      private var mpBar_:StatusBar;

      private var expTimer:ExperienceBoostTimerPopup;

      private var areTempXpListenersAdded:Boolean;

      private var curXPBoost:int;
      
      public function StatMetersView()
      {
         super();
         if(Parameters.data_.HPBarcolors)
         {
            this.expBar_ = new StatusBar(176,16,5931045,3163155, "Lvl X");
            this.fameBar_ = new StatusBar(176,16,14835456,7023616,"Fame");
            this.hpBar_ = new StatusBar(176,16,14693428,6297367,"HP");
            this.mpBar_ = new StatusBar(176,16,6325472,3558271,"MP");
         }
         else
         {
            this.expBar_ = new StatusBar(176,16,5931045,5526612,"Lvl X");
            this.fameBar_ = new StatusBar(176,16,14835456,5526612,"Fame");
            this.hpBar_ = new StatusBar(176,16,14693428,5526612,"HP");
            this.mpBar_ = new StatusBar(176,16,6325472,5526612,"MP");
         }
         /*this.expBar_ = new StatusBar(176,16,5931045,5526612,"Lvl X");
         this.fameBar_ = new StatusBar(176,16,14835456,5526612,"Fame");
         this.hpBar_ = new StatusBar(176,16,14693428,5526612,"HP");
         this.mpBar_ = new StatusBar(176,16,6325472,5526612,"MP");*/
         this.hpBar_.y = 24;
         this.mpBar_.y = 48;
         this.expBar_.visible = true;
         this.fameBar_.visible = false;
         addChild(this.expBar_);
         addChild(this.fameBar_);
         addChild(this.hpBar_);
         addChild(this.mpBar_);
      }
      
      public function update(player:Player) : void
      {
         var lvlText:String = "Lvl " + player.level_;
         if(lvlText != this.expBar_.labelText_.text)
         {
            if(this.expTimer)
            {
               this.expTimer.update(player.xpTimer);
            }
            this.expBar_.labelText_.text = lvlText;
            this.expBar_.labelText_.updateMetrics();
         }
         if(player.level_ != 20)
         {
            if(!this.expBar_.visible)
            {
               this.expBar_.visible = true;
               this.fameBar_.visible = false;
            }
            this.expBar_.draw(player.exp_,player.nextLevelExp_,0);
            if(this.curXPBoost != player.xpBoost_)
            {
               this.curXPBoost = player.xpBoost_;
               if(this.curXPBoost)
               {
                  this.expBar_.showMultiplierText();
               }
               else
               {
                  this.expBar_.hideMultiplierText();
               }
            }
            if(player.xpTimer)
            {
               if(!this.areTempXpListenersAdded)
               {
                  this.expBar_.addEventListener("MULTIPLIER_OVER",this.onExpBarOver);
                  this.expBar_.addEventListener("MULTIPLIER_OUT",this.onExpBarOut);
                  this.areTempXpListenersAdded = true;
               }
            }
            else
            {
               if(this.areTempXpListenersAdded)
               {
                  this.expBar_.removeEventListener("MULTIPLIER_OVER",this.onExpBarOver);
                  this.expBar_.removeEventListener("MULTIPLIER_OUT",this.onExpBarOut);
                  this.areTempXpListenersAdded = false;
               }
               if(this.expTimer && this.expTimer.parent)
               {
                  removeChild(this.expTimer);
                  this.expTimer = null;
               }
            }
         }
         else
         {
            if(!this.fameBar_.visible)
            {
               this.fameBar_.visible = true;
               this.expBar_.visible = false;
            }
            this.fameBar_.draw(player.currFame_,player.nextClassQuestFame_,0);
         }
         this.hpBar_.draw(player.hp_,player.maxHP_,player.maxHPBoost_,player.maxHPMax_,Math.max(player.maxHP_ - player.maxHPBoost_ - player.maxHPMax_,0), player.level_);
         this.mpBar_.draw(player.mp_,player.maxMP_,player.maxMPBoost_,player.maxMPMax_,Math.max(player.maxMP_ - player.maxMPBoost_ - player.maxMPMax_,0), player.level_);
      }

      private function onExpBarOver(e:Event) : void
      {
         addChild(this.expTimer = new ExperienceBoostTimerPopup());
      }

      private function onExpBarOut(e:Event) : void
      {
         if(this.expTimer && this.expTimer.parent)
         {
            removeChild(this.expTimer);
            this.expTimer = null;
         }
      }
   }
}
