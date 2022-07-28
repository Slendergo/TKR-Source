package com.company.assembleegameclient.ui
{
import com.company.assembleegameclient.parameters.Parameters;
import com.company.assembleegameclient.util.FameUtil;
import flash.display.Sprite;
import flash.events.MouseEvent;
import flash.text.engine.ContentElement;
import flash.text.engine.ElementFormat;
import flash.text.engine.GraphicElement;
import flash.text.engine.GroupElement;
import flash.text.engine.TextBlock;
import flash.text.engine.TextElement;

public class TextBoxLine
{
   private static var ELEMENT_FORMATS:ElementFormats = new ElementFormats();

   public var time_:int;
   public var name_:String;
   public var rankIcon_:Sprite = null;
   public var recipient_:String;
   public var toMe_:Boolean;
   public var text_:String;
   public var nameColor_:int;
   public var textColor_:int;

   public function TextBoxLine(time:int, name:String, objectId:int, numStars:int, recipient:String, toMe:Boolean, text:String, nameColor:int, textColor:int)
   {
      super();
      this.time_ = time;
      this.name_ = name;
      if(numStars >= 0)
      {
         this.rankIcon_ = FameUtil.numStarsToIcon(numStars);
      }
      this.recipient_ = recipient;
      this.toMe_ = toMe;
      this.text_ = text;
      this.nameColor_ = nameColor;
      this.textColor_ = textColor;
   }

   public function getTextBlock() : TextBlock
   {
      var vec:Vector.<ContentElement> = new Vector.<ContentElement>(0);
      var nameFormat:ElementFormat = ELEMENT_FORMATS.playerFormat_;
      var sepFormat:ElementFormat = ELEMENT_FORMATS.sepFormat_;
      var textFormat:ElementFormat = ELEMENT_FORMATS.normalFormat_;
      var name:String = this.name_;
      if(this.nameColor_ != 0){
         var loc1:ElementFormats = new ElementFormats(this.nameColor_);
         nameFormat = loc1.exportFormat_;
      }
      if(this.nameColor_ == 0x123456){
         var loc3:ElementFormats = new ElementFormats(0x00FE00);
         nameFormat = loc3.exportFormat_;
      }
      if(this.textColor_ != 0){
         var loc2:ElementFormats = new ElementFormats(this.textColor_);
         textFormat = loc2.exportFormat_;
      }
      switch(this.name_)
      {
         case Parameters.SERVER_CHAT_NAME:
            name = "";
            textFormat = ELEMENT_FORMATS.serverFormat_;
            break;
         case Parameters.CLIENT_CHAT_NAME:
            name = "";
            textFormat = ELEMENT_FORMATS.clientFormat_;
            break;
         case Parameters.HELP_CHAT_NAME:
            name = "";
            textFormat = ELEMENT_FORMATS.helpFormat_;
            break;
         case Parameters.ERROR_CHAT_NAME:
            textFormat = ELEMENT_FORMATS.errorFormat_;
            name = "";
      }
      if(this.name_.charAt(0) == "#")
      {
         name = this.name_.substr(1);
         nameFormat = ELEMENT_FORMATS.enemyFormat_;
      }
      if(this.name_.charAt(0) == "@")
      {
         name = this.name_.substr(1);
         nameFormat = ELEMENT_FORMATS.adminFormat_;
         textFormat = ELEMENT_FORMATS.adminFormat_;
      }
      if(this.recipient_ == Parameters.GUILD_CHAT_NAME)
      {
         nameFormat = ELEMENT_FORMATS.guildFormat_;
         textFormat = ELEMENT_FORMATS.guildFormat_;
      }
      else if(this.recipient_ == Parameters.PARTY_CHAT_NAME)
      {
         nameFormat = ELEMENT_FORMATS.partyFormat_;
         textFormat = ELEMENT_FORMATS.partyFormat_;
      }
      else if(this.recipient_ != "")
      {
         textFormat = ELEMENT_FORMATS.tellFormat_;
         nameFormat = ELEMENT_FORMATS.tellFormat_;
         if(!this.toMe_)
         {
            vec.push(new TextElement("To: ",ELEMENT_FORMATS.tellFormat_));
            nameFormat = ELEMENT_FORMATS.tellFormat_;
            name = this.recipient_;
            this.rankIcon_ = null;
         }
      }
      if(this.rankIcon_ != null)
      {
         this.rankIcon_.y = 3;
         vec.push(new GraphicElement(this.rankIcon_,this.rankIcon_.width + 2,this.rankIcon_.height,ELEMENT_FORMATS.normalFormat_));
      }
      if(name != "")
      {
         vec.push(new TextElement("<" + name + ">",nameFormat),new TextElement(" ",sepFormat));
      }
      vec.push(new TextElement(this.text_,textFormat));
      var groupElement:GroupElement = new GroupElement(vec);
      return new TextBlock(groupElement);
   }
}
}
