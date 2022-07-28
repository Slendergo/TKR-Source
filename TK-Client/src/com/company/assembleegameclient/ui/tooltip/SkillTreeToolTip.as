package com.company.assembleegameclient.ui.tooltip {
import com.company.assembleegameclient.parameters.Parameters;
import com.company.ui.SimpleText;

import flash.filters.DropShadowFilter;
import flash.text.TextField;

import kabam.rotmg.tooltips.view.TooltipsView;

public class SkillTreeToolTip extends ToolTip{


    private var text_:SimpleText;
    private var title_:SimpleText;

    public function SkillTreeToolTip(title:String, description:String, width:int = 150, height:int = 25) {
        super(0x363636, 1, 0xFFFFFF, 1, true);
        this.title_ = new SimpleText(16, 0xFFFFFF, false, width, height).setBold(true).setText(title);
        this.title_.filters = [new DropShadowFilter(0, 0, 0)];
        this.title_.x = 0;
        this.title_.y = 0;
        addChild(this.title_);
        this.text_ = new SimpleText(14, 0xFFFFFF, false, width, height).setBold(true).setText(description);
        this.text_.filters = [new DropShadowFilter(0, 0, 0)];
        this.text_.x = 0;
        this.text_.y = 15;
        addChild(this.text_);
    }

    override protected function position():void
    {
        var _local_1:Number;
        var _local_2:Number;
        var _local_3:Number = (800 / stage.stageWidth);
        var _local_4:Number = (600 / stage.stageHeight);
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
    }
}
}
