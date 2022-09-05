package com.company.assembleegameclient.ui {

import com.company.assembleegameclient.objects.Character;
import com.company.assembleegameclient.objects.GameObject;

import flash.display.Graphics;
import flash.display.Sprite;

public class QuestHealthBar extends Sprite {

    private var go_:GameObject;
    private var w_:int;
    private var h_:int;

    public function QuestHealthBar(go:GameObject, w:int, h:int)
    {
        this.go_ = go;
        this.w_ = w;
        this.h_ = h;
    }

    public function draw():void
    {
        var percent:Number = this.go_.hp_ / this.go_.maxHP_;
        var g:Graphics = this.graphics;
        g.clear();
        g.beginFill(0x111111);
        g.drawRect(0, 0, this.w_, this.h_);
        g.endFill();
        g.beginFill(Character.green2redu(percent * 100.0));
        g.drawRect(0, 0, this.w_ * percent, this.h_);
        g.endFill();
    }
}
}
