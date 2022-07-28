package kabam.rotmg.BountyBoard.SubscriptionUI {
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.objects.GameObject;
import com.company.assembleegameclient.objects.Player;
import com.company.assembleegameclient.ui.options.Options;
import com.company.assembleegameclient.util.GuildUtil;
import com.company.ui.SimpleText;
import com.gskinner.motion.GTween;

import flash.display.DisplayObject;

import flash.display.DisplayObjectContainer;
import flash.display.Graphics;
import flash.display.Shape;
import flash.display.Sprite;
import flash.events.Event;
import flash.events.MouseEvent;
import flash.filters.DropShadowFilter;
import flash.utils.getTimer;

import io.decagames.rotmg.ui.buttons.SliceScalingButton;
import io.decagames.rotmg.ui.defaults.DefaultLabelFormat;
import io.decagames.rotmg.ui.popups.header.PopupHeader;
import io.decagames.rotmg.ui.popups.modal.ModalPopup;
import io.decagames.rotmg.ui.scroll.UIScrollbar;
import io.decagames.rotmg.ui.sliceScaling.SliceScalingBitmap;
import io.decagames.rotmg.ui.texture.TextureParser;
import io.decagames.rotmg.utils.colors.GreyScale;
import io.decagames.rotmg.utils.colors.Tint;

import kabam.lib.net.api.MessageProvider;
import kabam.lib.net.impl.SocketServer;
import kabam.rotmg.BountyBoard.SubscriptionUI.playerList.BountyPlayerListLine;
import kabam.rotmg.BountyBoard.SubscriptionUI.signals.BountyMemberListSendSignal;
import kabam.rotmg.core.StaticInjectorContext;
import kabam.rotmg.game.model.GameModel;
import kabam.rotmg.messaging.impl.GameServerConnection;
import kabam.rotmg.messaging.impl.incoming.bounty.BountyMemberListSend;
import kabam.rotmg.messaging.impl.outgoing.bounty.BountyMemberListRequest;
import kabam.rotmg.messaging.impl.outgoing.bounty.BountyRequest;

import org.osflash.signals.Signal;

public class SubscriptionUI extends ModalPopup {
    /* UI */
    internal var quitButton:SliceScalingButton;
    internal var reloadButton:SliceScalingButton;
    internal var startButton:SliceScalingButton;
    internal var player:Player;
    internal var text_:String;
    public var gameSprite:GameSprite;

    /* Misc */
    internal var close:Signal = new Signal();
    internal var clicked:Signal = new Signal();
    internal var playerIds:Vector.<int> = new Vector.<int>();
    internal var players:Vector.<Player> = new Vector.<Player>();

    /* Player Ranks */
    internal var initiatePlayers:Vector.<Player> = new Vector.<Player>();
    internal var memberPlayers:Vector.<Player> = new Vector.<Player>();
    internal var officerPlayers:Vector.<Player> = new Vector.<Player>();
    internal var leaderPlayers:Vector.<Player> = new Vector.<Player>();
    internal var founderPlayers:Vector.<Player> = new Vector.<Player>();

    internal var mainSprite_:Sprite;
    internal var listSprite_:Sprite;

    internal var coolDownTime:int = 3000;
    internal var nextCooldown:int = 0;

    internal var scrollBar_:UIScrollbar;

    internal var bountyId:int; /* 1 -> Easy, 2 -> Medium, 3 -> Hard, 4 -> Hell */
    internal var bountyCount:int;
    internal var playersCount:int;
    internal var playersToSend:Vector.<int> = new Vector.<int>();

    internal var playersLength:int;

    internal var playersText_:SimpleText; /* Where All Players and Listed Players will Appear (in Number) */

    public const getPlayersSignal:BountyMemberListSendSignal = new BountyMemberListSendSignal();

    public var gameObject:GameObject;

    public function SubscriptionUI(arg1:String, gs:GameSprite, bountyid:int, gm:GameObject):void {
        super(475,500,arg1);
        this.gameSprite = gs;
        this.bountyId = bountyid;
        this.gameObject = gm;

        /* Effect */
        this.alpha = 0;
        new GTween(this, 0.2, {"alpha": 1});

        /* Start */
        init();

        /* Position */
        this.x = 150;
        this.y = 22.5;

        /* Get Player */
        this.player = StaticInjectorContext.getInjector().getInstance(GameModel).player;

        /* UI */
        this.quitButton = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI","close_button"));
        this.header.addButton(this.quitButton, PopupHeader.RIGHT_BUTTON);
        this.quitButton.addEventListener(MouseEvent.CLICK,this.onClose);
        this.reloadButton = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "reload_button"));
        this.header.addButton(this.reloadButton, PopupHeader.LEFT_BUTTON);
        this.reloadButton.addEventListener(MouseEvent.CLICK, this.onRefresh);
        this.reloadButton.addEventListener(Event.ENTER_FRAME, this.onEnterFrame);

        this.startButton = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "generic_green_button"));
        this.startButton.setLabel("Start", DefaultLabelFormat.defaultModalTitle);
        this.startButton.scaleX = 0.6;
        this.startButton.scaleY = 0.6;
        this.startButton.width = 150;
        this.startButton.x = 300;
        addChild(this.startButton);

    }

    public function init() : void
    {
        /* Get the Player's ID's from the Server */
        var messages:MessageProvider = StaticInjectorContext.getInjector().getInstance(MessageProvider);
        var socketServer:SocketServer = StaticInjectorContext.getInjector().getInstance(SocketServer);
        var _local_1:BountyMemberListRequest;
        _local_1 = (messages.require(GameServerConnection.BOUNTYMEMBERLISTREQUEST) as BountyMemberListRequest);
        socketServer.sendMessage(_local_1);

        /* Obtained! */
        this.getPlayersSignal.add(this.onGetIds);

        /* Cooldown */
        coolDownStart();
    }

    public function coolDownStart() : void
    {
        this.nextCooldown = getTimer() + coolDownTime;
    }

    public function coolDownMS() : int
    {
        var time:int = getTimer();
        return Math.max(0,this.nextCooldown - time);
    }

    public function onGetIds(ids:BountyMemberListSend) : void
    {
        for(var i:int = 0; i < ids.playerIds.length; i++)
        {
            this.playerIds[i] = ids.playerIds[i]; /* Get Players ID's */
            this.players[i] = this.gameSprite.map.goDict_[this.playerIds[i]]; /* With Players ID's, we can search the players in the map :D */
        }

        /* Let's divide the players into different groups according to their rank */
        for each(var player:Player in players)
        {
            if(player == null || player.guildRank_ == -1)
            {
                continue;
            }
            switch(player.guildRank_)
            {
                case GuildUtil.INITIATE: /* initiate */
                    this.initiatePlayers.push(player);
                    continue;
                case GuildUtil.MEMBER: /* Member */
                    this.memberPlayers.push(player);
                    continue;
                case GuildUtil.OFFICER: /* Officer */
                    this.officerPlayers.push(player);
                    continue;
                case GuildUtil.LEADER: /* Leader */
                    this.leaderPlayers.push(player);
                    continue;
                case GuildUtil.FOUNDER: /* Founder */
                    this.founderPlayers.push(player);
            }
        }

        playersLength = this.initiatePlayers.length + this.memberPlayers.length + this.officerPlayers.length + this.leaderPlayers.length + this.founderPlayers.length;

        makeList(); /* Let's do this! */

    }

    public function onEnterFrame(evt:Event) : void
    {
        this.reloadButton.disabledAlt(this.coolDownMS() > 0, true);
    }

    public function makeList() : void
    {

        /* Background */
        var maskShape:Shape = new Shape();
        maskShape.graphics.beginFill(0x282828);
        maskShape.graphics.drawRect(-35,20,500, 500);
        maskShape.graphics.endFill();
        addChild(maskShape);

        /* Background of Players Text */
        var maskShape2:Shape = new Shape();
        maskShape2.graphics.beginFill(0x282828);
        maskShape2.graphics.drawRect(0,0,175, 100);
        maskShape2.graphics.endFill();
        addChild(maskShape2);

        /* Players Text */
        var textColor:uint = 11776947;
        this.playersText_ = new SimpleText(18,textColor,false,0,0).setBold(true);
        this.playersText_.setText("Players: " + "(" + this.playersLength + ")");
        this.playersText_.useTextDimensions();
        this.playersText_.filters = [new DropShadowFilter(0,0,0,1,8,8)];
        this.playersText_.x = 5;
        this.playersText_.y = 0;
        addChild(this.playersText_);

        /* Setup all positions for the List */
        var g:Graphics = null;
        this.mainSprite_ = new Sprite();
        var shape:Shape = new Shape();
        g = shape.graphics;
        g.beginFill(0);
        g.drawRect(0,0,475,500);
        g.endFill();
        this.mainSprite_.y = 25;
        this.mainSprite_.addChild(shape);
        this.mainSprite_.mask = shape;
        addChild(this.mainSprite_);
        addChild(this.playersText_);


        this.listSprite_ = new Sprite();
        var id:int = 0; /* ID and Position of the Player in the list, 1 => First */
        for each(var founderPlayer:Player in founderPlayers) /* Founder Player/s */
        {
            var memberListFounder:BountyPlayerListLine = new BountyPlayerListLine(id, founderPlayer, this, founderPlayer == this.player);
            memberListFounder.alpha = 0;
            memberListFounder.scaleX = 2;
            memberListFounder.scaleY = 2;
            memberListFounder.y = id * BountyPlayerListLine.HEIGHT;
            this.listSprite_.addChild(memberListFounder);
            new GTween(memberListFounder, 0.1, {"alpha": 1, "scaleX": 1, "scaleY": 1}); /* Effect */
            id++;
        }

        for each(var leaderMember:Player in leaderPlayers) /* Leader Player/s */
        {
            var memberListLeader:BountyPlayerListLine = new BountyPlayerListLine(id, leaderMember, this, leaderMember == this.player);
            memberListLeader.alpha = 0;
            memberListLeader.scaleX = 2;
            memberListLeader.scaleY = 2;
            memberListLeader.y = id * BountyPlayerListLine.HEIGHT;
            this.listSprite_.addChild(memberListLeader);
            new GTween(memberListLeader, 0.15, {"alpha": 1, "scaleX": 1, "scaleY": 1}); /* Effect */
            id++;
        }

        /* Line after Leader */
        var shape1:Shape = new Shape();
        var line:Graphics = shape1.graphics;
        line.lineStyle(2,5526612);
        line.moveTo(0,id * BountyPlayerListLine.HEIGHT);
        line.lineTo(stage.stageWidth,id * BountyPlayerListLine.HEIGHT);
        line.lineStyle();
        this.mainSprite_.addChild(shape1);

        for each(var officerPlayer:Player in officerPlayers) /* Officer Player/s */
        {
            var memberListOfficer:BountyPlayerListLine = new BountyPlayerListLine(id, officerPlayer, this, officerPlayer == this.player);
            memberListOfficer.alpha = 0;
            memberListOfficer.scaleX = 2;
            memberListOfficer.scaleY = 2;
            memberListOfficer.y = id * BountyPlayerListLine.HEIGHT;
            this.listSprite_.addChild(memberListOfficer);
            new GTween(memberListOfficer, 0.2, {"alpha": 1, "scaleX": 1, "scaleY": 1}); /* Effect */
            id++;
        }

        for each(var memberPlayer:Player in memberPlayers) /* Member Player/s */
        {
            var memberListMember:BountyPlayerListLine = new BountyPlayerListLine(id, memberPlayer, this, memberPlayer == this.player);
            memberListMember.alpha = 0;
            memberListMember.scaleX = 2;
            memberListMember.scaleY = 2;
            memberListMember.y = id * BountyPlayerListLine.HEIGHT;
            this.listSprite_.addChild(memberListMember);
            new GTween(memberListMember, 0.25, {"alpha": 1, "scaleX": 1, "scaleY": 1}); /* Effect */
            id++;
        }

        for each(var initiatePlayer:Player in initiatePlayers) /* Initiate Player/s */
        {
            var memberListInitiate:BountyPlayerListLine = new BountyPlayerListLine(id, initiatePlayer, this, initiatePlayer == this.player);
            memberListInitiate.alpha = 0;
            memberListInitiate.scaleX = 2;
            memberListInitiate.scaleY = 2;
            memberListInitiate.y = id * BountyPlayerListLine.HEIGHT;
            this.listSprite_.addChild(memberListInitiate);
            new GTween(memberListInitiate, 0.3, {"alpha": 1, "scaleX": 1, "scaleY": 1}); /* Effect */
            id++;
        }
        this.mainSprite_.addChild(this.listSprite_); /* Add it! */

        if(this.scrollBar_ == null) /* Scroll Bar :O */
        {
            this.scrollBar_ = new UIScrollbar(480);
            this.scrollBar_.x = 458;
            this.scrollBar_.y = 20;
            this.scrollBar_.content = this.listSprite_;
            addChild(this.scrollBar_);
        }

        checkPlayersAccepted();
    }

    public function checkPlayersAccepted() : void
    {
        this.playersCount = 0;
        this.checkNumbers();
        /* Check Bounty */
        switch (bountyId) {
            case 1: /* Easy */
                bountyCount = 15;
                break;
            case 2: /* Medium */
                bountyCount = 55;
                break;
            case 3: /* Hard */
                bountyCount = 75;
                break;
            case 4: /* Hell */
                bountyCount = 128;
                break;
        }

        for(var x:int = 0; x < this.listSprite_.numChildren; x++)
        {
            var z:* = this.listSprite_.getChildAt(x);
            if (z.playerAccepted && this.playersCount < bountyCount)
            {
                this.playersCount += 1;
                this.checkNumbers();
            }
            else
            {
                z.disable();
                this.checkNumbers();
            }
        }
        this.startButton.addEventListener(MouseEvent.CLICK, this.onStart);
    }

    public function onStart(evt:Event) : void
    {
        playersToSend.length = this.listSprite_.numChildren;
        for(var x:int = 0; x < this.listSprite_.numChildren; x++)
        {
            var z:* = this.listSprite_.getChildAt(x);
            if(z == null) continue;
            if (z.playerAccepted)
            {
                playersToSend[x] = z.playerId;
            }
        }

        var messages:MessageProvider = StaticInjectorContext.getInjector().getInstance(MessageProvider);
        var socketServer:SocketServer = StaticInjectorContext.getInjector().getInstance(SocketServer);
        var bounty:BountyRequest;
        bounty = (messages.require(GameServerConnection.BOUNTYREQUEST) as BountyRequest);
        bounty.BountyId = bountyId;
        bounty.playersAllowed = playersToSend;
        socketServer.sendMessage(bounty);
        this.onCloseAtStart();
    }

    public function checkNumbers() : void
    {
        removeChild(this.playersText_);
        this.playersText_ = null;
        this.playersText_ = new SimpleText(18,11776947,false,0,0).setBold(true);
        this.playersText_.setText("Players: " + "(" + this.playersLength + ") " + this.playersCount + "/" + this.bountyCount);
        this.playersText_.useTextDimensions();
        this.playersText_.filters = [new DropShadowFilter(0,0,0,1,8,8)];
        addChild(this.playersText_);
    }

    public function checkDisablePlayer(place:int) : void
    {
        this.playersCount -= 1;
    }

    public function checkEnablePlayer(place:int) : Boolean
    {
        var player:* = this.listSprite_.getChildAt(place);
        if(this.playersCount < bountyCount)
        {
            this.playersCount += 1;
            return true;
        }
        else
        {
            player.disable();
            return false;
        }
    }

    public function onRefresh(evt:Event) : void
    {
        /* Check Cooldown */
        if(this.coolDownMS() > 0)
        {
            return;
        }
        /* Remove everything from before */
        if(this.mainSprite_ != null)
        {
            removeChild(this.mainSprite_);
        }
        if(this.scrollBar_ != null)
        {
            removeChild(this.scrollBar_);
        }
        this.mainSprite_ = null;
        this.scrollBar_ = null;
        this.players.length = 0;
        this.playerIds.length = 0;
        this.initiatePlayers.length = 0;
        this.memberPlayers.length = 0;
        this.officerPlayers.length = 0;
        this.leaderPlayers.length = 0;
        this.founderPlayers.length = 0;
        /* Again :) */
        this.init();
    }

    public function onCloseAtStart() : void
    {
        if(this.mainSprite_ != null)
        {
            removeChild(this.mainSprite_);
        }
        if(this.scrollBar_ != null)
        {
            removeChild(this.scrollBar_);
        }
        this.mainSprite_ = null;
        this.scrollBar_ = null;
        this.players.length = 0;
        this.playerIds.length = 0;
        this.initiatePlayers.length = 0;
        this.memberPlayers.length = 0;
        this.officerPlayers.length = 0;
        this.leaderPlayers.length = 0;
        this.founderPlayers.length = 0;
        this.close.dispatch();
        this.reloadButton.removeEventListener(Event.ENTER_FRAME, this.onEnterFrame);
        this.reloadButton.removeEventListener(MouseEvent.CLICK, this.onRefresh);
        this.quitButton.removeEventListener(MouseEvent.CLICK, this.onClose);
        this.reloadButton = null;
        this.quitButton = null;
    }

    public function onClose(arg1:Event) : void
    {
        if(this.mainSprite_ != null)
        {
            removeChild(this.mainSprite_);
        }
        if(this.scrollBar_ != null)
        {
            removeChild(this.scrollBar_);
        }
        this.mainSprite_ = null;
        this.scrollBar_ = null;
        this.players.length = 0;
        this.playerIds.length = 0;
        this.initiatePlayers.length = 0;
        this.memberPlayers.length = 0;
        this.officerPlayers.length = 0;
        this.leaderPlayers.length = 0;
        this.founderPlayers.length = 0;
        this.close.dispatch();
        this.reloadButton.removeEventListener(Event.ENTER_FRAME, this.onEnterFrame);
        this.reloadButton.removeEventListener(MouseEvent.CLICK, this.onRefresh);
        this.quitButton.removeEventListener(MouseEvent.CLICK, this.onClose);
        this.reloadButton = null;
        this.quitButton = null;
    }

}

}
