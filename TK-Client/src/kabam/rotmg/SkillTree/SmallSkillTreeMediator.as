package kabam.rotmg.SkillTree {
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

    override public function initialize():void{
        this.view.smallSkill1.addEventListener(MouseEvent.CLICK, this.onClicked1);
        this.view.smallSkill2.addEventListener(MouseEvent.CLICK, this.onClicked2);
        this.view.smallSkill3.addEventListener(MouseEvent.CLICK, this.onClicked3);
        this.view.smallSkill4.addEventListener(MouseEvent.CLICK, this.onClicked4);
        this.view.smallSkill5.addEventListener(MouseEvent.CLICK, this.onClicked5);
        this.view.smallSkill6.addEventListener(MouseEvent.CLICK, this.onClicked6);
        this.view.smallSkill7.addEventListener(MouseEvent.CLICK, this.onClicked7);
        this.view.smallSkill8.addEventListener(MouseEvent.CLICK, this.onClicked8);
        this.view.smallSkill9.addEventListener(MouseEvent.CLICK, this.onClicked9);
        this.view.smallSkill10.addEventListener(MouseEvent.CLICK, this.onClicked10);
        this.view.smallSkill11.addEventListener(MouseEvent.CLICK, this.onClicked11);
        this.view.smallSkill12.addEventListener(MouseEvent.CLICK, this.onClicked12);
        this.view.saveButton.addEventListener(MouseEvent.CLICK, this.onSaved);
    }

    private function onSaved(evt:MouseEvent):void{
        var _local1:SmallSkillTree;
        _local1 = (this.messages.require(GameServerConnection.SMALLSKILLTREE) as SmallSkillTree);
        _local1.skillNumber = 20;
        this.socketServer.sendMessage(_local1);
        this.closeDialogs.dispatch();
    }

    private function onClicked1(arg1:MouseEvent):void{
        if (this.view.player.points <= 0) {
            this.view.smallSkill1.disabled = true;
            this.view.player.points = 0;
            return;
        }
        if (this.view.player.smallSkill1 >= 5) {
            this.view.smallSkill1.disabled = true;
            this.view.player.smallSkill1 = 5;
            return;
        }
        this.view.player.smallSkill1 += 1;
        this.view.player.points -= 1;
        var _local1:SmallSkillTree;
        _local1 = (this.messages.require(GameServerConnection.SMALLSKILLTREE) as SmallSkillTree);
        _local1.skillNumber = 1;
        this.socketServer.sendMessage(_local1);
    }

    private function onClicked2(arg1:MouseEvent):void{
        if(this.view.player.points <= 0){
            this.view.smallSkill2.disabled = true;
            this.view.player.points = 0;
            return;
        }
        if(this.view.player.smallSkill2 >= 5){
            this.view.smallSkill2.disabled = true;
            this.view.player.smallSkill2 = 5;
            return;
        }
        this.view.player.smallSkill2 += 1;
        this.view.player.points -= 1;
        var _local1:SmallSkillTree;
        _local1 = (this.messages.require(GameServerConnection.SMALLSKILLTREE) as SmallSkillTree);
        _local1.skillNumber = 2;
        this.socketServer.sendMessage(_local1);
    }

    private function onClicked3(arg1:MouseEvent):void{
        if(this.view.player.points <= 0){
            this.view.smallSkill3.disabled = true;
            this.view.player.points = 0;
            return;
        }
        if(this.view.player.smallSkill3 >= 5){
            this.view.smallSkill3.disabled = true;
            this.view.player.smallSkill3 = 5;
            return;
        }
        this.view.player.smallSkill3 += 1;
        this.view.player.points -= 1;
        var _local1:SmallSkillTree;
        _local1 = (this.messages.require(GameServerConnection.SMALLSKILLTREE) as SmallSkillTree);
        _local1.skillNumber = 3;
        this.socketServer.sendMessage(_local1);
    }

    private function onClicked4(arg1:MouseEvent):void{
        if(this.view.player.points <= 0){
            this.view.smallSkill4.disabled = true;
            this.view.player.points = 0;
            return;
        }
        if(this.view.player.smallSkill4 >= 5){
            this.view.smallSkill4.disabled = true;
            this.view.player.smallSkill4 = 5;
            return;
        }
        this.view.player.smallSkill4 += 1;
        this.view.player.points -= 1;
        var _local1:SmallSkillTree;
        _local1 = (this.messages.require(GameServerConnection.SMALLSKILLTREE) as SmallSkillTree);
        _local1.skillNumber = 4;
        this.socketServer.sendMessage(_local1);
    }

    private function onClicked5(arg1:MouseEvent):void{
        if(this.view.player.points <= 0){
            this.view.smallSkill5.disabled = true;
            this.view.player.points = 0;
            return;
        }
        if(this.view.player.smallSkill5 >= 5){
            this.view.smallSkill5.disabled = true;
            this.view.player.smallSkill5 = 5;
            return;
        }
        this.view.player.smallSkill5 += 1;
        this.view.player.points -= 1;
        var _local1:SmallSkillTree;
        _local1 = (this.messages.require(GameServerConnection.SMALLSKILLTREE) as SmallSkillTree);
        _local1.skillNumber = 5;
        this.socketServer.sendMessage(_local1);
    }

    private function onClicked6(arg1:MouseEvent):void{
        if(this.view.player.points <= 0){
            this.view.smallSkill6.disabled = true;
            this.view.player.points = 0;
            return;
        }
        if(this.view.player.smallSkill6 >= 5){
            this.view.smallSkill6.disabled = true;
            this.view.player.smallSkill6 = 5;
            return;
        }
        this.view.player.smallSkill6 += 1;
        this.view.player.points -= 1;
        var _local1:SmallSkillTree;
        _local1 = (this.messages.require(GameServerConnection.SMALLSKILLTREE) as SmallSkillTree);
        _local1.skillNumber = 6;
        this.socketServer.sendMessage(_local1);
    }

    private function onClicked7(arg1:MouseEvent):void{
        if(this.view.player.points <= 0){
            this.view.smallSkill7.disabled = true;
            this.view.player.points = 0;
            return;
        }
        if(this.view.player.smallSkill7 >= 5){
            this.view.smallSkill7.disabled = true;
            this.view.player.smallSkill7 = 5;
            return;
        }
        this.view.player.smallSkill7 += 1;
        this.view.player.points -= 1;
        var _local1:SmallSkillTree;
        _local1 = (this.messages.require(GameServerConnection.SMALLSKILLTREE) as SmallSkillTree);
        _local1.skillNumber = 7;
        this.socketServer.sendMessage(_local1);
    }

    private function onClicked8(arg1:MouseEvent):void{
        if(this.view.player.points <= 0){
            this.view.smallSkill8.disabled = true;
            this.view.player.points = 0;
            return;
        }
        if(this.view.player.smallSkill8 >= 5){
            this.view.smallSkill8.disabled = true;
            this.view.player.smallSkill8 = 5;
            return;
        }
        this.view.player.smallSkill8 += 1;
        this.view.player.points -= 1;
        var _local1:SmallSkillTree;
        _local1 = (this.messages.require(GameServerConnection.SMALLSKILLTREE) as SmallSkillTree);
        _local1.skillNumber = 8;
        this.socketServer.sendMessage(_local1);
    }

    private function onClicked9(arg1:MouseEvent):void{
        if(this.view.player.points <= 0){
            this.view.smallSkill9.disabled = true;
            this.view.player.points = 0;
            return;
        }
        if(this.view.player.smallSkill9 >= 5){
            this.view.smallSkill9.disabled = true;
            this.view.player.smallSkill9 = 5;
            return;
        }
        this.view.player.smallSkill9 += 1;
        this.view.player.points -= 1;
        var _local1:SmallSkillTree;
        _local1 = (this.messages.require(GameServerConnection.SMALLSKILLTREE) as SmallSkillTree);
        _local1.skillNumber = 9;
        this.socketServer.sendMessage(_local1);
    }

    private function onClicked10(arg1:MouseEvent):void{
        if(this.view.player.points <= 0){
            this.view.smallSkill10.disabled = true;
            this.view.player.points = 0;
            return;
        }
        if(this.view.player.smallSkill10 >= 5){
            this.view.smallSkill10.disabled = true;
            this.view.player.smallSkill10 = 5;
            return;
        }
        this.view.player.smallSkill10 += 1;
        this.view.player.points -= 1;
        var _local1:SmallSkillTree;
        _local1 = (this.messages.require(GameServerConnection.SMALLSKILLTREE) as SmallSkillTree);
        _local1.skillNumber = 10;
        this.socketServer.sendMessage(_local1);
    }

    private function onClicked11(arg1:MouseEvent):void{
        if(this.view.player.points <= 0){
            this.view.smallSkill11.disabled = true;
            this.view.player.points = 0;
            return;
        }
        if(this.view.player.smallSkill11 >= 5){
            this.view.smallSkill11.disabled = true;
            this.view.player.smallSkill11 = 5;
            return;
        }
        this.view.player.smallSkill11 += 1;
        this.view.player.points -= 1;
        var _local1:SmallSkillTree;
        _local1 = (this.messages.require(GameServerConnection.SMALLSKILLTREE) as SmallSkillTree);
        _local1.skillNumber = 11;
        this.socketServer.sendMessage(_local1);
    }

    private function onClicked12(arg1:MouseEvent):void{
        if(this.view.player.points <= 0){
            this.view.smallSkill12.disabled = true;
            this.view.player.points = 0;
            return;
        }
        if(this.view.player.smallSkill12 >= 5){
            this.view.smallSkill12.disabled = true;
            this.view.player.smallSkill12 = 5;
            return;
        }
        this.view.player.smallSkill12 += 1;
        this.view.player.points -= 1;
        var _local1:SmallSkillTree;
        _local1 = (this.messages.require(GameServerConnection.SMALLSKILLTREE) as SmallSkillTree);
        _local1.skillNumber = 12;
        this.socketServer.sendMessage(_local1);
    }


}
}
