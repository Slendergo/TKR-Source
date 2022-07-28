package com.company.assembleegameclient.ui.tooltip
{
import com.company.assembleegameclient.map.partyoverlay.PlayerArrow;
import com.company.assembleegameclient.objects.SellableObject;
import com.company.assembleegameclient.parameters.Parameters;
import com.company.assembleegameclient.ui.options.Options;
import com.company.util.GraphicsUtil;
import flash.display.CapsStyle;
import flash.display.DisplayObject;
import flash.display.GraphicsPath;
import flash.display.GraphicsSolidFill;
import flash.display.GraphicsStroke;
import flash.display.IGraphicsData;
import flash.display.JointStyle;
import flash.display.LineScaleMode;
import flash.display.Sprite;
import flash.events.Event;
import flash.events.MouseEvent;
import flash.filters.DropShadowFilter;

import kabam.rotmg.game.view.SellableObjectPanel;

import kabam.rotmg.tooltips.view.TooltipsView;

public class ToolTip extends Sprite
{


   private var background_:uint;

   private var backgroundAlpha_:Number;

   public var outline_:uint;

   public var outlineAlpha_:Number;

   private var followMouse_:Boolean;

   public var contentWidth_:int;

   public var contentHeight_:int;

   private var targetObj:DisplayObject;

   public var backgroundFill_:GraphicsSolidFill= new GraphicsSolidFill(0,1);

   public var outlineFill_:GraphicsSolidFill= new GraphicsSolidFill(16777215,1);

   private var lineStyle_:GraphicsStroke = new GraphicsStroke(1,false,LineScaleMode.NORMAL,CapsStyle.NONE,JointStyle.ROUND,3,outlineFill_);

   private var path_:GraphicsPath = new GraphicsPath(new Vector.<int>(),new Vector.<Number>());

   private const graphicsData_:Vector.<IGraphicsData> = new <IGraphicsData>[lineStyle_,backgroundFill_,path_,GraphicsUtil.END_FILL,GraphicsUtil.END_STROKE];

   public function ToolTip(background:uint, backgroundAlpha:Number, outline:uint, outlineAlpha:Number, followMouse:Boolean = true)
   {
      super();
      this.background_ = background;
      this.backgroundAlpha_ = backgroundAlpha;
      this.outline_ = outline;
      this.outlineAlpha_ = outlineAlpha;
      this.followMouse_ = followMouse;
      mouseEnabled = false;
      mouseChildren = false;
      filters = [new DropShadowFilter(0,0,0,1,16,16)];
      addEventListener(Event.ADDED_TO_STAGE,this.onAddedToStage);
      addEventListener(Event.REMOVED_FROM_STAGE,this.onRemovedFromStage);
   }

   protected function alignUI():void {
   }


   public function attachToTarget(dObj:DisplayObject) : void
   {
      if(dObj)
      {
         this.targetObj = dObj;
         this.targetObj.addEventListener(MouseEvent.ROLL_OUT,this.onLeaveTarget);
      }
   }

   public function detachFromTarget() : void
   {
      if(this.targetObj)
      {
         this.targetObj.removeEventListener(MouseEvent.ROLL_OUT,this.onLeaveTarget);
         if(parent)
         {
            parent.removeChild(this);
         }
         this.targetObj = null;
      }
   }

   private function onLeaveTarget(e:MouseEvent) : void
   {
      this.detachFromTarget();
   }

   private function onAddedToStage(event:Event) : void
   {
      this.draw();
      if(this.followMouse_)
      {
         this.alignUI();
         this.draw();
         this.position();
         addEventListener(Event.ENTER_FRAME,this.onEnterFrame);
      }
   }

   private function onRemovedFromStage(event:Event) : void
   {
      if(this.followMouse_)
      {
         removeEventListener(Event.ENTER_FRAME,this.onEnterFrame);
      }
   }

   private function onEnterFrame(event:Event) : void
   {
      this.position();
   }


   /*protected function position():void
   {
      var _local_1:Number;
      var _local_2:Number;
      var _local_3:Number = (800 / stage.stageWidth);
      var _local_4:Number = (600 / stage.stageHeight);
      if ((this.parent is Options))
      {
         _local_1 = ((((stage.mouseX + (stage.stageWidth / 2)) - 400) / stage.stageWidth) * 800);
         _local_2 = ((((stage.mouseY + (stage.stageHeight / 2)) - 300) / stage.stageHeight) * 600);
      }
      else
      {
         _local_1 = (((stage.stageWidth - 800) / 2) + stage.mouseX);
         _local_2 = (((stage.stageHeight - 600) / 2) + stage.mouseY);
         if (((this.parent is TooltipsView)))
         {
            if (Parameters.data_.FS)
            {
               this.parent.scaleX = (_local_3 / _local_4);
               this.parent.scaleY = 1;
               _local_1 = (_local_1 * _local_4);
               _local_2 = (_local_2 * _local_4);
            }
            else
            {
               this.parent.scaleX = _local_3;
               this.parent.scaleY = _local_4;
            }
         }
      }
      if (stage == null)
      {
         return;
      }
      if (((stage.mouseX + (0.5 * stage.stageWidth)) - 400) < (stage.stageWidth / 2))
      {
         x = (_local_1 + 12);
      }
      else
      {
         x = ((_local_1 - width) - 1);
      }
      if (x < 12)
      {
         x = 12;
      }
      if (((stage.mouseY + (0.5 * stage.stageHeight)) - 300) < (stage.stageHeight / 3))
      {
         y = (_local_2 + 12);
      }
      else
      {
         y = ((_local_2 - height) - 1);
      }
      if (y < 12)
      {
         y = 12;
      }
   }*/

   protected function position() : void
   {
      if(stage == null)
      {
         return;
      }
      if(stage.mouseX < stage.stageWidth / 2)
      {
         x = stage.mouseX + 12;
      }
      else
      {
         x = stage.mouseX - width - 1;
      }
      if(x < 12)
      {
         x = 12;
      }
      if(stage.mouseY < stage.stageHeight / 3)
      {
         y = stage.mouseY + 12;
      }
      else
      {
         y = stage.mouseY - height - 1;
      }
      if(y < 12)
      {
         y = 12;
      }
   }

   public function draw() : void
   {
      this.backgroundFill_.color = this.background_;
      this.backgroundFill_.alpha = this.backgroundAlpha_;
      this.outlineFill_.color = this.outline_;
      this.outlineFill_.alpha = this.outlineAlpha_;
      graphics.clear();
      this.contentWidth_ = width;
      this.contentHeight_ = height;
      GraphicsUtil.clearPath(this.path_);
      GraphicsUtil.drawCutEdgeRect(-6, -6, (this.contentWidth_ + 12), (this.contentHeight_ + 12), 4, [1, 1, 1, 1], this.path_);
      graphics.drawGraphicsData(this.graphicsData_);
   }
}
}
