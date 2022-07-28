package com.company.assembleegameclient.ui.tooltip.slotcomparisons
{
   public class PoisonComparison extends SlotComparison
   {
       
      
      public function PoisonComparison()
      {
         super();
      }
      
      override protected function compareSlots(itemXML:XML, curItemXML:XML) : void
      {
         var activate:XMLList = null;
         var otherActivate:XMLList = null;
         var damage:int = 0;
         var otherDamage:int = 0;
         var radius:Number = NaN;
         var otherRadius:Number = NaN;
         var duration:Number = NaN;
         var otherDuration:Number = NaN;
         var avg:Number = NaN;
         var otherAvg:Number = NaN;
         var text:String = null;
         activate = itemXML.Activate.(text() == "PoisonGrenade");
         otherActivate = curItemXML.Activate.(text() == "PoisonGrenade");
         comparisonText = "";
         if(activate.length() == 1 && otherActivate.length() == 1)
         {
            damage = int(activate[0].@totalDamage);
            otherDamage = int(otherActivate[0].@totalDamage);
            radius = Number(activate[0].@radius);
            otherRadius = Number(otherActivate[0].@radius);
            duration = Number(activate[0].@duration);
            otherDuration = Number(otherActivate[0].@duration);
            avg = 0.33 * damage + 0.33 * radius + 0.33 * duration;
            otherAvg = 0.33 * otherDamage + 0.33 * otherRadius + 0.33 * otherDuration;
            text = damage + " HP over " + duration + " secs within " + radius + " sqrs\n";
            comparisonText = comparisonText + ("Poison Grenade: " + wrapInColoredFont(text,getTextColor(avg - otherAvg)));
            processedTags[activate[0].toXMLString()] = true;
         }
      }
   }
}
