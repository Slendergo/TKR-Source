package kabam.rotmg.NewSkillTree {
import com.company.assembleegameclient.sound.SoundEffectLibrary;

import flash.events.Event;
import flash.events.TimerEvent;
import flash.utils.Timer;

import kabam.rotmg.NewSkillTree.*;

import flash.events.MouseEvent;

import kabam.lib.net.api.MessageProvider;
import kabam.lib.net.impl.SocketServer;
import kabam.rotmg.dialogs.control.CloseDialogsSignal;
import kabam.rotmg.messaging.impl.GameServerConnection;
import kabam.rotmg.messaging.impl.outgoing.SmallSkillTree;

import robotlegs.bender.bundles.mvcs.Mediator;

public class SmallSkillTreeMediator extends Mediator {

    [Inject]
    public var view:SkillTreeModal;
    [Inject]
    public var socketServer:SocketServer;
    [Inject]
    public var messages:MessageProvider;
    [Inject]
    public var closeDialogs:CloseDialogsSignal;

    public static const CLICK_PAUSE:uint = 500;

    public var clickTimer:Timer;

    public var pendingClick:Boolean;

    override public function initialize():void{
        this.clickTimer = new Timer(CLICK_PAUSE,1);

        this.view.quitButton.addEventListener(MouseEvent.CLICK,this.onClose);

        this.view.tree1Add.addEventListener(MouseEvent.CLICK, this.onClickedTree1Add);
        this.view.tree1Remove.addEventListener(MouseEvent.CLICK, this.onClickedTree1Remove);

        this.view.tree2Add.addEventListener(MouseEvent.CLICK, this.onClickedTree2Add);
        this.view.tree2Remove.addEventListener(MouseEvent.CLICK, this.onClickedTree2Remove);

        this.view.tree3Add.addEventListener(MouseEvent.CLICK, this.onClickedTree3Add);
        this.view.tree3Remove.addEventListener(MouseEvent.CLICK, this.onClickedTree3Remove);

        this.view.tree4Add.addEventListener(MouseEvent.CLICK, this.onClickedTree4Add);
        this.view.tree4Remove.addEventListener(MouseEvent.CLICK, this.onClickedTree4Remove);

        this.view.tree5Add.addEventListener(MouseEvent.CLICK, this.onClickedTree5Add);
        this.view.tree5Remove.addEventListener(MouseEvent.CLICK, this.onClickedTree5Remove);

        this.view.saveButton.addEventListener(MouseEvent.CLICK, this.onSaved);

        this.view.tree1Points.addEventListener(Event.ENTER_FRAME, this.onEnterFrameTree1Points);

        this.clickTimer.addEventListener(TimerEvent.TIMER_COMPLETE,this.onClickTimerComplete);
        onEnterFrameTreeTick();
    }

    private function onClose(evt:MouseEvent):void{
        this.view.tree1Add.removeEventListener(MouseEvent.CLICK, this.onClickedTree1Add);
        this.view.tree1Remove.removeEventListener(MouseEvent.CLICK, this.onClickedTree1Remove);
        this.view.tree2Add.removeEventListener(MouseEvent.CLICK, this.onClickedTree2Add);
        this.view.tree2Remove.removeEventListener(MouseEvent.CLICK, this.onClickedTree2Remove);
        this.view.tree3Add.removeEventListener(MouseEvent.CLICK, this.onClickedTree3Add);
        this.view.tree3Remove.removeEventListener(MouseEvent.CLICK, this.onClickedTree3Remove);
        this.view.tree4Add.removeEventListener(MouseEvent.CLICK, this.onClickedTree4Add);
        this.view.tree4Remove.removeEventListener(MouseEvent.CLICK, this.onClickedTree4Remove);
        this.view.tree5Add.removeEventListener(MouseEvent.CLICK, this.onClickedTree5Add);
        this.view.tree5Remove.removeEventListener(MouseEvent.CLICK, this.onClickedTree5Remove);
        this.view.tree1Points.removeEventListener(Event.ENTER_FRAME, this.onEnterFrameTree1Points);
        this.view.pointsText.removeEventListener(Event.ENTER_FRAME, this.view.onEnterFrame);
        this.view.tree2Points.removeEventListener(Event.ENTER_FRAME, this.view.onEnterFrameTree2Points);
        this.view.tree3Points.removeEventListener(Event.ENTER_FRAME, this.view.onEnterFrameTree3Points);
        this.view.tree4Points.removeEventListener(Event.ENTER_FRAME, this.view.onEnterFrameTree5Points);
        this.view.tree5Points.removeEventListener(Event.ENTER_FRAME, this.view.onEnterFrameTree5Points);
        SoundEffectLibrary.play("button_click");
        this.closeDialogs.dispatch();
    }

    private function setPendingClick(isPending:Boolean) : void
    {
        this.pendingClick = isPending;
        if(this.pendingClick)
        {
            this.clickTimer.reset();
            this.clickTimer.start();
        }
        else
        {
            this.clickTimer.stop();
        }
    }

    private function onEnterFrameTree1Points(arg1:Event):void
    {
        trace("points1")
        var local1:int = (this.view.player.node1TickMaj)+(this.view.player.node1Med*5)+(this.view.player.node1TickMin)+(this.view.player.node1Big*10);
        if ((this.view.player.node1TickMaj)+(this.view.player.node1Med*5)+(this.view.player.node1TickMin)+(this.view.player.node1Big*10) > 9) {
            this.view.tree1Points.x = this.view.tree1Add.x - 48;
        } else {
            this.view.tree1Points.x = this.view.tree1Add.x - 38;
        }
        this.view.tree1Points.setText( local1 + "").setColor(0xFFFFFF).setSize(32).setBold(true);
    }

    private function onClickTimerComplete(e:TimerEvent) : void
    {
        this.setPendingClick(false);
    }

    private function onSaved(evt:MouseEvent):void{
        trace("onSaved Clicked")
        var _local1:SmallSkillTree;
        _local1 = (this.messages.require(GameServerConnection.SMALLSKILLTREE) as SmallSkillTree);
        _local1.skillNumber = 40;
        this.socketServer.sendMessage(_local1);
        this.view.tree1Add.removeEventListener(MouseEvent.CLICK, this.onClickedTree1Add);
        this.view.tree1Remove.removeEventListener(MouseEvent.CLICK, this.onClickedTree1Remove);
        this.view.tree2Add.removeEventListener(MouseEvent.CLICK, this.onClickedTree2Add);
        this.view.tree2Remove.removeEventListener(MouseEvent.CLICK, this.onClickedTree2Remove);
        this.view.tree3Add.removeEventListener(MouseEvent.CLICK, this.onClickedTree3Add);
        this.view.tree3Remove.removeEventListener(MouseEvent.CLICK, this.onClickedTree3Remove);
        this.view.tree4Add.removeEventListener(MouseEvent.CLICK, this.onClickedTree4Add);
        this.view.tree4Remove.removeEventListener(MouseEvent.CLICK, this.onClickedTree4Remove);
        this.view.tree5Add.removeEventListener(MouseEvent.CLICK, this.onClickedTree5Add);
        this.view.tree5Remove.removeEventListener(MouseEvent.CLICK, this.onClickedTree5Remove);
        this.view.tree1Points.removeEventListener(Event.ENTER_FRAME, this.onEnterFrameTree1Points);
        this.view.pointsText.removeEventListener(Event.ENTER_FRAME, this.view.onEnterFrame);
        this.view.tree2Points.removeEventListener(Event.ENTER_FRAME, this.view.onEnterFrameTree2Points);
        this.view.tree3Points.removeEventListener(Event.ENTER_FRAME, this.view.onEnterFrameTree3Points);
        this.view.tree4Points.removeEventListener(Event.ENTER_FRAME, this.view.onEnterFrameTree5Points);
        this.view.tree5Points.removeEventListener(Event.ENTER_FRAME, this.view.onEnterFrameTree5Points);
        this.closeDialogs.dispatch();
    }

    private function onEnterFrameTreeTick():void
    {
        this.view.addRedrawRect(1, false);
        this.view.addRedrawRect(2,false);
        this.view.addRedrawRect(3,false);
        this.view.addRedrawRect(4,false);
        this.view.addRedrawRect(5,false);
    }


    private function onClickedTree1Add(arg1:MouseEvent):void{
        trace("add Clicked")
        if(pendingClick){
            SoundEffectLibrary.play("error");
            return;
        }
        this.setPendingClick(true);
        var _local1:int = calcNextCost(1);
        var _local2:int = calcNextNode(1);
        if(_local1 == 99)
                return;
        if (this.view.player.points < _local1) {
            this.view.tree1Add.disabled = true;
            if(this.view.player.points <= 0){
                this.view.player.points = 0;
            }
            return;
        }
        if (this.view.player.node1TickMin > 5) {
            this.view.player.node1TickMin = 5;
            return;
        }
        if (this.view.player.node1TickMaj > 10) {
            this.view.player.node1TickMaj = 10;
            return;
        }
        if (this.view.player.node1Med > 3) {
            this.view.player.node1TickMin = 3;
            return;
        }
        if (this.view.player.node1Big > 2) {
            this.view.player.node1TickMin = 2;
            return;
        }

        this.view.player.points -= _local1;
        var _local3:SmallSkillTree;
        _local3 = (this.messages.require(GameServerConnection.SMALLSKILLTREE) as SmallSkillTree);
        switch (_local2){
            case 0:
                break;
            case 1:
                trace("add min")
                this.view.player.node1TickMin += 1;
                _local3.skillNumber = 13;
                break;
            case 2:
                trace("add maj")
                this.view.player.node1TickMaj += 1;
                _local3.skillNumber = 14;
                break;
            case 3:
                trace("add MED")
                this.view.player.node1Med += 1;
                _local3.skillNumber = 15;
                break;
            case 4:
                trace("add BIG")
                this.view.player.node1Big += 1;
                _local3.skillNumber = 16;
                break;
        }
        _local3.removePoint = 0;
        this.socketServer.sendMessage(_local3);
        this.view.addRedrawRect(1, false);
    }

    private function onClickedTree1Remove(arg1:MouseEvent):void{
        trace("add Clicked")
        if(pendingClick){
            SoundEffectLibrary.play("error");
            return;
        }
        this.setPendingClick(true);
        var _local1:int = calcNextCost(1, true);
        var _local2:int = calcNextNode(1, true);
        if(_local1 == 99)
            return;
        if (this.view.player.node1TickMin < 0) {
            this.view.player.node1TickMin = 0;
            return;
        }
        if (this.view.player.node1TickMaj < 0) {
            this.view.player.node1TickMaj = 0;
            return;
        }
        if (this.view.player.node1Med < 0) {
            this.view.player.node1TickMin = 0;
            return;
        }
        if (this.view.player.node1Big < 0) {
            this.view.player.node1TickMin = 0;
            return;
        }

        this.view.player.points += _local1;
        var _local3:SmallSkillTree;
        _local3 = (this.messages.require(GameServerConnection.SMALLSKILLTREE) as SmallSkillTree);
        switch (_local2){
            case 0:
                break;
            case 1:
                this.view.player.node1TickMin -= 1;
                _local3.skillNumber = 13;
                break;
            case 2:
                this.view.player.node1TickMaj -= 1;
                _local3.skillNumber = 14;
                break;
            case 3:
                this.view.player.node1Med -= 1;
                _local3.skillNumber = 15;
                break;
            case 4:
                this.view.player.node1Big -= 1;
                _local3.skillNumber = 16;
                break;
        }
        _local3.removePoint = 1;
        this.socketServer.sendMessage(_local3);
        this.view.addRedrawRect(1, true);
    }

    private function onClickedTree2Add(arg1:MouseEvent):void{
        trace("add Clicked")
        if(pendingClick){
            SoundEffectLibrary.play("error");
            return;
        }
        this.setPendingClick(true);
        var _local1:int = calcNextCost(2);
        var _local2:int = calcNextNode(2);
        if(_local1 == 99)
            return;
        if (this.view.player.points < _local1) {
            this.view.tree2Add.disabled = true;
            if(this.view.player.points <= 0){
                this.view.player.points = 0;
            }
            return;
        }
        if (this.view.player.node2TickMin > 5) {
            this.view.player.node2TickMin = 5;
            return;
        }
        if (this.view.player.node2TickMaj > 10) {
            this.view.player.node2TickMaj = 10;
            return;
        }
        if (this.view.player.node2Med > 3) {
            this.view.player.node2TickMin = 3;
            return;
        }
        if (this.view.player.node2Big > 2) {
            this.view.player.node2TickMin = 2;
            return;
        }
        //this.view.player.points -= _local1;
        var _local3:SmallSkillTree;
        _local3 = (this.messages.require(GameServerConnection.SMALLSKILLTREE) as SmallSkillTree);
        switch (_local2){
            case 0:
                break;
            case 1:
                trace("add min")
                this.view.player.node2TickMin += 1;
                _local3.skillNumber = 17;
                break;
            case 2:
                trace("add maj")
                this.view.player.node2TickMaj += 1;
                _local3.skillNumber = 18;
                break;
            case 3:
                trace("add MED")
                this.view.player.node2Med += 1;
                _local3.skillNumber = 19;
                break;
            case 4:
                trace("add BIG")
                this.view.player.node2Big += 1;
                _local3.skillNumber = 20;
                break;

        }
        _local3.removePoint = 0;
        this.socketServer.sendMessage(_local3);
        this.view.addRedrawRect(2, false);
    }

    private function onClickedTree2Remove(arg1:MouseEvent):void{
        trace("add Clicked")
        if(pendingClick){
            SoundEffectLibrary.play("error");
            return;
        }
        this.setPendingClick(true);
        var _local1:int = calcNextCost(2, true);
        var _local2:int = calcNextNode(2, true);
        if(_local1 == 99)
            return;
        if (this.view.player.node2TickMin < 0) {
            this.view.player.node2TickMin = 0;
            return;
        }
        if (this.view.player.node2TickMaj < 0) {
            this.view.player.node2TickMaj = 0;
            return;
        }
        if (this.view.player.node2Med < 0) {
            this.view.player.node2TickMin = 0;
            return;
        }
        if (this.view.player.node2Big < 0) {
            this.view.player.node2TickMin = 0;
            return;
        }

        //this.view.player.points += _local1;
        var _local3:SmallSkillTree;
        _local3 = (this.messages.require(GameServerConnection.SMALLSKILLTREE) as SmallSkillTree);
        switch (_local2){
            case 0:
                break;
            case 1:
                this.view.player.node2TickMin -= 1;
                _local3.skillNumber = 17;
                break;
            case 2:
                this.view.player.node2TickMaj -= 1;
                _local3.skillNumber = 18;
                break;
            case 3:
                this.view.player.node2Med -= 1;
                _local3.skillNumber = 19;
                break;
            case 4:
                this.view.player.node2Big -= 1;
                _local3.skillNumber = 20;
                break;

        }
        _local3.removePoint = 1;
        this.socketServer.sendMessage(_local3);
        this.view.addRedrawRect(2, true);
    }

    private function onClickedTree3Add(arg1:MouseEvent):void{
        trace("add Clicked")
        if(pendingClick){
            SoundEffectLibrary.play("error");
            return;
        }
        this.setPendingClick(true);
        var _local1:int = calcNextCost(3);
        var _local2:int = calcNextNode(3);
        if(_local1 == 99)
            return;
        if (this.view.player.points < _local1) {
            this.view.tree3Add.disabled = true;
            if(this.view.player.points <= 0){
                this.view.player.points = 0;
            }
            return;
        }
        if (this.view.player.node3TickMin > 5) {
            this.view.player.node3TickMin = 5;
            return;
        }
        if (this.view.player.node3TickMaj > 10) {
            this.view.player.node3TickMaj = 10;
            return;
        }
        if (this.view.player.node3Med > 3) {
            this.view.player.node3TickMin = 3;
            return;
        }
        if (this.view.player.node3Big > 2) {
            this.view.player.node3TickMin = 2;
            return;
        }
        //this.view.player.points -= _local1;
        var _local3:SmallSkillTree;
        _local3 = (this.messages.require(GameServerConnection.SMALLSKILLTREE) as SmallSkillTree);
        switch (_local2){
            case 0:
                break;
            case 1:
                trace("add min")
                this.view.player.node3TickMin += 1;
                _local3.skillNumber = 21;
                break;
            case 2:
                trace("add maj")
                this.view.player.node3TickMaj += 1;
                _local3.skillNumber = 22;
                break;
            case 3:
                trace("add MED")
                this.view.player.node3Med += 1;
                _local3.skillNumber = 23;
                break;
            case 4:
                trace("add BIG")
                this.view.player.node3Big += 1;
                _local3.skillNumber = 24;
                break;

        }
        _local3.removePoint = 0;
        this.socketServer.sendMessage(_local3);
        this.view.addRedrawRect(3, false);
    }

    private function onClickedTree3Remove(arg1:MouseEvent):void{
        trace("add Clicked")
        if(pendingClick){
            SoundEffectLibrary.play("error");
            return;
        }
        this.setPendingClick(true);
        var _local1:int = calcNextCost(3, true);
        var _local2:int = calcNextNode(3, true);
        if(_local1 == 99)
            return;
        if (this.view.player.node3TickMin < 0) {
            this.view.player.node3TickMin = 0;
            return;
        }
        if (this.view.player.node3TickMaj < 0) {
            this.view.player.node3TickMaj = 0;
            return;
        }
        if (this.view.player.node3Med < 0) {
            this.view.player.node3TickMin = 0;
            return;
        }
        if (this.view.player.node3Big < 0) {
            this.view.player.node3TickMin = 0;
            return;
        }

        //this.view.player.points += _local1;
        var _local3:SmallSkillTree;
        _local3 = (this.messages.require(GameServerConnection.SMALLSKILLTREE) as SmallSkillTree);
        switch (_local2){
            case 0:
                break;
            case 1:
                this.view.player.node3TickMin -= 1;
                _local3.skillNumber = 21;
                break;
            case 2:
                this.view.player.node3TickMaj -= 1;
                _local3.skillNumber = 22;
                break;
            case 3:
                this.view.player.node3Med -= 1;
                _local3.skillNumber = 23;
                break;
            case 4:
                this.view.player.node3Big -= 1;
                _local3.skillNumber = 24;
                break;

        }
        _local3.removePoint = 1;
        this.socketServer.sendMessage(_local3);
        this.view.addRedrawRect(3, true);
    }

    private function onClickedTree4Add(arg1:MouseEvent):void{
        trace("add Clicked")
        if(pendingClick){
            SoundEffectLibrary.play("error");
            return;
        }
        this.setPendingClick(true);
        var _local1:int = calcNextCost(4);
        var _local2:int = calcNextNode(4);
        if(_local1 == 99)
            return;
        if (this.view.player.points < _local1) {
            this.view.tree4Add.disabled = true;
            if(this.view.player.points <= 0){
                this.view.player.points = 0;
            }
            return;
        }
        if (this.view.player.node4TickMin > 5) {
            this.view.player.node4TickMin = 5;
            return;
        }
        if (this.view.player.node4TickMaj > 10) {
            this.view.player.node4TickMaj = 10;
            return;
        }
        if (this.view.player.node4Med > 3) {
            this.view.player.node4TickMin = 3;
            return;
        }
        if (this.view.player.node4Big > 2) {
            this.view.player.node4TickMin = 2;
            return;
        }
        //this.view.player.points -= _local1;
        var _local3:SmallSkillTree;
        _local3 = (this.messages.require(GameServerConnection.SMALLSKILLTREE) as SmallSkillTree);
        switch (_local2){
            case 0:
                break;
            case 1:
                trace("add min")
                this.view.player.node4TickMin += 1;
                _local3.skillNumber = 25;
                break;
            case 2:
                trace("add maj")
                this.view.player.node4TickMaj += 1;
                _local3.skillNumber = 26;
                break;
            case 3:
                trace("add MED")
                this.view.player.node4Med += 1;
                _local3.skillNumber = 27;
                break;
            case 4:
                trace("add BIG")
                this.view.player.node4Big += 1;
                _local3.skillNumber = 28;
                break;

        }
        _local3.removePoint = 0;
        this.socketServer.sendMessage(_local3);
        this.view.addRedrawRect(4, false);
    }

    private function onClickedTree4Remove(arg1:MouseEvent):void{
        trace("add Clicked")
        if(pendingClick){
            SoundEffectLibrary.play("error");
            return;
        }
        this.setPendingClick(true);
        var _local1:int = calcNextCost(4, true);
        var _local2:int = calcNextNode(4,true);
        if(_local1 == 99)
            return;
        if (this.view.player.node4TickMin < 0) {
            this.view.player.node4TickMin = 0;
            return;
        }
        if (this.view.player.node4TickMaj < 0) {
            this.view.player.node4TickMaj = 0;
            return;
        }
        if (this.view.player.node4Med < 0) {
            this.view.player.node4TickMin = 0;
            return;
        }
        if (this.view.player.node4Big < 0) {
            this.view.player.node4TickMin = 0;
            return;
        }

        //this.view.player.points += _local1;
        var _local3:SmallSkillTree;
        _local3 = (this.messages.require(GameServerConnection.SMALLSKILLTREE) as SmallSkillTree);
        switch (_local2){
            case 0:
                break;
            case 1:
                this.view.player.node4TickMin -= 1;
                _local3.skillNumber = 25;
                break;
            case 2:
                this.view.player.node4TickMaj -= 1;
                _local3.skillNumber = 26;
                break;
            case 3:
                this.view.player.node4Med -= 1;
                _local3.skillNumber = 27;
                break;
            case 4:
                this.view.player.node4Big -= 1;
                _local3.skillNumber = 28;
                break;

        }
        _local3.removePoint = 1;
        this.socketServer.sendMessage(_local3);
        this.view.addRedrawRect(4, true);
    }

    private function onClickedTree5Add(arg1:MouseEvent):void{
        trace("add Clicked")
        if(pendingClick){
            SoundEffectLibrary.play("error");
            return;
        }
        this.setPendingClick(true);
        var _local1:int = calcNextCost(5);
        var _local2:int = calcNextNode(5);
        if(_local1 == 99)
            return;
        if (this.view.player.points < _local1) {
            this.view.tree5Add.disabled = true;
            if(this.view.player.points <= 0){
                this.view.player.points = 0;
            }
            return;
        }
        if (this.view.player.node5TickMin > 5) {
            this.view.player.node5TickMin = 5;
            return;
        }
        if (this.view.player.node5TickMaj > 10) {
            this.view.player.node5TickMaj = 10;
            return;
        }
        if (this.view.player.node5Med > 3) {
            this.view.player.node5TickMin = 3;
            return;
        }
        if (this.view.player.node5Big > 2) {
            this.view.player.node5TickMin = 2;
            return;
        }
        //this.view.player.points -= _local1;
        var _local3:SmallSkillTree;
        _local3 = (this.messages.require(GameServerConnection.SMALLSKILLTREE) as SmallSkillTree);
        switch (_local2){
            case 0:
                break;
            case 1:
                trace("add min")
                this.view.player.node5TickMin += 1;
                _local3.skillNumber = 29;
                break;
            case 2:
                trace("add maj")
                this.view.player.node5TickMaj += 1;
                _local3.skillNumber = 30;
                break;
            case 3:
                trace("add MED")
                this.view.player.node5Med += 1;
                _local3.skillNumber = 31;
                break;
            case 4:
                trace("add BIG")
                this.view.player.node5Big += 1;
                _local3.skillNumber = 32;
                break;

        }
        _local3.removePoint = 0;
        this.socketServer.sendMessage(_local3);
        this.view.addRedrawRect(5, false);
    }

    private function onClickedTree5Remove(arg1:MouseEvent):void{
        trace("add Clicked")
        if(pendingClick){
            SoundEffectLibrary.play("error");
            return;
        }
        this.setPendingClick(true);
        var _local1:int = calcNextCost(5, true);
        var _local2:int = calcNextNode(5, true);
        if(_local1 == 99)
            return;
        if (this.view.player.node5TickMin < 0) {
            this.view.player.node5TickMin = 0;
            return;
        }
        if (this.view.player.node5TickMaj < 0) {
            this.view.player.node5TickMaj = 0;
            return;
        }
        if (this.view.player.node5Med < 0) {
            this.view.player.node5TickMin = 0;
            return;
        }
        if (this.view.player.node5Big < 0) {
            this.view.player.node5TickMin = 0;
            return;
        }

        //this.view.player.points += _local1;
        var _local3:SmallSkillTree;
        _local3 = (this.messages.require(GameServerConnection.SMALLSKILLTREE) as SmallSkillTree);
        switch (_local2){
            case 0:
                break;
            case 1:
                this.view.player.node5TickMin -= 1;
                _local3.skillNumber = 29;
                break;
            case 2:
                this.view.player.node5TickMaj -= 1;
                _local3.skillNumber = 30;
                break;
            case 3:
                this.view.player.node5Med -= 1;
                _local3.skillNumber = 31;
                break;
            case 4:
                this.view.player.node5Big -= 1;
                _local3.skillNumber = 32;
                break;

        }
        _local3.removePoint = 1;
        this.socketServer.sendMessage(_local3);
        this.view.addRedrawRect(5, true);
    }

    private function calcNextCost(treeNumber:int, removePoint:Boolean = false):int
    {
        var local1:int = calcTreePoints(treeNumber);
        var local2:int;
        switch (local1){
            case 0: removePoint ? local2 = 0 : local2 = 1;
                break;
            case 1: removePoint ? local2 = -1 : local2 = 1;
                break;
            case 2: removePoint ? local2 = -1 : local2 = 1;
                break;
            case 3: removePoint ? local2 = -1 : local2 = 5;
                break;
            case 8: removePoint ? local2 = -5 : local2 = 1;
                break;
            case 9: removePoint ? local2 = -1 : local2 = 1;
                break;
            case 10: removePoint ? local2 = -1 : local2 = 1;
                break;
            case 11: removePoint ? local2 = -1 : local2 = 5;
                break;
            case 16: removePoint ? local2 = -5 : local2 = 1;
                break;
            case 17: removePoint ? local2 = -1 : local2 = 1;
                break;
            case 18: removePoint ? local2 = -1 : local2 = 1;
                break;
            case 19: removePoint ? local2 = -1 : local2 = 10;
                break;
            case 29: removePoint ? local2 = -10 : local2 = 1;
                break;
            case 30: removePoint ? local2 = -1 : local2 = 1;
                break;
            case 31: removePoint ? local2 = -1 : local2 = 1;
                break;
            case 32: removePoint ? local2 = -1 : local2 = 5;
                break;
            case 37: removePoint ? local2 = -5 : local2 = 1;
                break;
            case 38: removePoint ? local2 = -1 : local2 = 1;
                break;
            case 39: removePoint ? local2 = -1 : local2 = 1;
                break;
            case 40: removePoint ? local2 = -1 : local2 = 10;
                break;
            case 50: removePoint ? local2 = -10 : local2 = 99;
                break;
        }
        return local2;
    }

    private function calcTreePoints(treeNumber:int):int{
        var local1:int = 0;
        switch (treeNumber)
        {
            case 1:
                local1 = (this.view.player.node1TickMaj)+(this.view.player.node1Med*5)+(this.view.player.node1TickMin)+(this.view.player.node1Big*10);
                trace("calc tree point: "+local1);break;
            case 2:
                local1 = (this.view.player.node2TickMaj)+(this.view.player.node2Med*5)+(this.view.player.node2TickMin)+(this.view.player.node2Big*10);break;
            case 3:
                local1 = (this.view.player.node3TickMaj)+(this.view.player.node3Med*5)+(this.view.player.node3TickMin)+(this.view.player.node3Big*10);break;
            case 4:
                local1 = (this.view.player.node4TickMaj)+(this.view.player.node4Med*5)+(this.view.player.node4TickMin)+(this.view.player.node4Big*10);break;
            case 5:
                local1 = (this.view.player.node5TickMaj)+(this.view.player.node5Med*5)+(this.view.player.node5TickMin)+(this.view.player.node5Big*10);break;
        }
        return local1;
    }

    private function calcNextNode(treeNumber:int, removePoint:Boolean = false):int
    {
        // 1 = min
        // 2 = maj
        // 3 - med 1
        // 4 - big 1
        var local1:int = calcTreePoints(treeNumber);
        var local2:int;
        switch (local1){
            case 0: local2 = 1;
                break;
            case 1: !removePoint ? local2 = 2 : local2 = 1;
                break;
            case 2: local2 = 2;
                break;
            case 3: !removePoint ? local2 = 3 : local2 = 2;
                break;
            case 8: !removePoint ? local2 = 1 : local2 = 3;
                break;
            case 9: !removePoint ? local2 = 2 : local2 = 1;
                break;
            case 10: !removePoint ? local2 = 2 : local2 = 2;
                break;
            case 11: !removePoint ? local2 = 3 : local2 = 2;
                break;
            case 16: !removePoint ? local2 = 1 : local2 = 3;
                break;
            case 17: !removePoint ? local2 = 2 : local2 = 1;
                break;
            case 18: local2 = 2;
                break;
            case 19: !removePoint ? local2 = 4 : local2 = 2;
                break;
            case 29: !removePoint ? local2 = 1 : local2 = 4;
                break;
            case 30: !removePoint ? local2 = 2 : local2 = 1;
                break;
            case 31: local2 = 2;
                break;
            case 32: !removePoint ? local2 = 3 : local2 = 2;
                break;
            case 37: !removePoint ? local2 = 1 : local2 = 3;
                break;
            case 38: !removePoint ? local2 = 2 : local2 = 1;
                break;
            case 39: local2 = 2;
                break;
            case 40: !removePoint ? local2 = 4 : local2 = 2;
                break;
            case 50: !removePoint ? local2 = 99 : local2 = 4;
                break;
        }
        return local2;
    }

}
}
