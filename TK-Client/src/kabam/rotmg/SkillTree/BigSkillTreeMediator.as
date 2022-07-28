package kabam.rotmg.SkillTree {
import flash.events.MouseEvent;

import kabam.lib.net.api.MessageProvider;
import kabam.lib.net.impl.SocketServer;
import kabam.rotmg.messaging.impl.GameServerConnection;
import kabam.rotmg.messaging.impl.outgoing.BigSkillTree;

import robotlegs.bender.bundles.mvcs.Mediator;

public class BigSkillTreeMediator extends Mediator {


    [Inject]
    public var view:SkillTreeModal;
    [Inject]
    public var socketServer:SocketServer;
    [Inject]
    public var messages:MessageProvider;


    override public function initialize():void{
        this.view.bigSkill1.addEventListener(MouseEvent.CLICK, this.onClicked1);
        this.view.bigSkill2.addEventListener(MouseEvent.CLICK, this.onClicked2);
        this.view.bigSkill3.addEventListener(MouseEvent.CLICK, this.onClicked3);
        this.view.bigSkill4.addEventListener(MouseEvent.CLICK, this.onClicked4);
        this.view.bigSkill5.addEventListener(MouseEvent.CLICK, this.onClicked5);
        this.view.bigSkill6.addEventListener(MouseEvent.CLICK, this.onClicked6);
        this.view.bigSkill7.addEventListener(MouseEvent.CLICK, this.onClicked7);
        this.view.bigSkill8.addEventListener(MouseEvent.CLICK, this.onClicked8);
        this.view.bigSkill9.addEventListener(MouseEvent.CLICK, this.onClicked9);
        this.view.bigSkill10.addEventListener(MouseEvent.CLICK, this.onClicked10);
        this.view.bigSkill11.addEventListener(MouseEvent.CLICK, this.onClicked11);
        this.view.bigSkill12.addEventListener(MouseEvent.CLICK, this.onClicked12);
    }

    private function onClicked1(arg1:MouseEvent):void{
        if(this.view.player.smallSkill1 < 5 || this.view.player.smallSkill9 < 3 || this.view.player.smallSkill7 < 2 || this.view.checkBigSkills()){
            return;
        }
        var _local1:BigSkillTree;
        _local1 = (this.messages.require(GameServerConnection.BIGSKILLTREE) as BigSkillTree);
        _local1.skillNumber = 1;
        this.socketServer.sendMessage(_local1);
    }

    private function onClicked2(arg1:MouseEvent):void{
        if(this.view.player.smallSkill2 < 5 || this.view.player.smallSkill10 < 3 || this.view.player.smallSkill8 < 2 || this.view.checkBigSkills()){
            return;
        }
        var _local1:BigSkillTree;
        _local1 = (this.messages.require(GameServerConnection.BIGSKILLTREE) as BigSkillTree);
        _local1.skillNumber = 2;
        this.socketServer.sendMessage(_local1);
    }

    private function onClicked3(arg1:MouseEvent):void{
        if(this.view.player.smallSkill3 < 5 || this.view.player.smallSkill6 < 3 || this.view.checkBigSkills()){
            return;
        }
        var _local1:BigSkillTree;
        _local1 = (this.messages.require(GameServerConnection.BIGSKILLTREE) as BigSkillTree);
        _local1.skillNumber = 3;
        this.socketServer.sendMessage(_local1);
    }

    private function onClicked4(arg1:MouseEvent):void{
        if(this.view.player.smallSkill4 < 5 || this.view.player.smallSkill1 < 3 || this.view.checkBigSkills()){
            return;
        }
        var _local1:BigSkillTree;
        _local1 = (this.messages.require(GameServerConnection.BIGSKILLTREE) as BigSkillTree);
        _local1.skillNumber = 4;
        this.socketServer.sendMessage(_local1);
    }

    private function onClicked5(arg1:MouseEvent):void{
        if(this.view.player.smallSkill5 < 5 || this.view.player.smallSkill4 < 3 || this.view.player.smallSkill7 < 2 || this.view.checkBigSkills()){
            return;
        }
        var _local1:BigSkillTree;
        _local1 = (this.messages.require(GameServerConnection.BIGSKILLTREE) as BigSkillTree);
        _local1.skillNumber = 5;
        this.socketServer.sendMessage(_local1);
    }

    private function onClicked6(arg1:MouseEvent):void{
        if(this.view.player.smallSkill6 < 5 || this.view.player.smallSkill3 < 2 || this.view.player.smallSkill11 < 1 || this.view.checkBigSkills()){
            return;
        }
        var _local1:BigSkillTree;
        _local1 = (this.messages.require(GameServerConnection.BIGSKILLTREE) as BigSkillTree);
        _local1.skillNumber = 6;
        this.socketServer.sendMessage(_local1);
    }

    private function onClicked7(arg1:MouseEvent):void{
        if(this.view.player.smallSkill7 < 5 || this.view.player.smallSkill9 < 3 || this.view.checkBigSkills()){
            return;
        }
        var _local1:BigSkillTree;
        _local1 = (this.messages.require(GameServerConnection.BIGSKILLTREE) as BigSkillTree);
        _local1.skillNumber = 7;
        this.socketServer.sendMessage(_local1);
    }

    private function onClicked8(arg1:MouseEvent):void{
        if(this.view.player.smallSkill8 < 5 || this.view.player.smallSkill10 < 3 || this.view.checkBigSkills()){
            return;
        }
        var _local1:BigSkillTree;
        _local1 = (this.messages.require(GameServerConnection.BIGSKILLTREE) as BigSkillTree);
        _local1.skillNumber = 8;
        this.socketServer.sendMessage(_local1);
    }

    private function onClicked9(arg1:MouseEvent):void{
        if(this.view.player.smallSkill9 < 5 || this.view.player.smallSkill1 < 3 || this.view.checkBigSkills()){
            return;
        }
        var _local1:BigSkillTree;
        _local1 = (this.messages.require(GameServerConnection.BIGSKILLTREE) as BigSkillTree);
        _local1.skillNumber = 9;
        this.socketServer.sendMessage(_local1);
    }

    private function onClicked10(arg1:MouseEvent):void{
        if(this.view.player.smallSkill10 < 5 || this.view.player.smallSkill2 < 3 || this.view.checkBigSkills()){
            return;
        }
        var _local1:BigSkillTree;
        _local1 = (this.messages.require(GameServerConnection.BIGSKILLTREE) as BigSkillTree);
        _local1.skillNumber = 10;
        this.socketServer.sendMessage(_local1);
    }

    private function onClicked11(arg1:MouseEvent):void{
        if(this.view.player.smallSkill11 < 5 || this.view.player.smallSkill6 < 3 || this.view.checkBigSkills()){
            return;
        }
        var _local1:BigSkillTree;
        _local1 = (this.messages.require(GameServerConnection.BIGSKILLTREE) as BigSkillTree);
        _local1.skillNumber = 11;
        this.socketServer.sendMessage(_local1);
    }

    private function onClicked12(arg1:MouseEvent):void{
        if(this.view.player.smallSkill12 < 5 || this.view.player.smallSkill11 < 3 || this.view.checkBigSkills()){
            return;
        }
        var _local1:BigSkillTree;
        _local1 = (this.messages.require(GameServerConnection.BIGSKILLTREE) as BigSkillTree);
        _local1.skillNumber = 12;
        this.socketServer.sendMessage(_local1);
    }

}
}
