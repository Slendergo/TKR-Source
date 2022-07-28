package com.company.assembleegameclient.game
{
import com.company.assembleegameclient.game.events.MoneyChangedEvent;
import com.company.assembleegameclient.map.Camera;
import com.company.assembleegameclient.map.Map;
import com.company.assembleegameclient.objects.GameObject;
import com.company.assembleegameclient.objects.IInteractiveObject;
import com.company.assembleegameclient.objects.ObjectLibrary;
import com.company.assembleegameclient.objects.Player;
import com.company.assembleegameclient.objects.Projectile;
import com.company.assembleegameclient.parameters.Parameters;
import com.company.assembleegameclient.tutorial.Tutorial;
import com.company.assembleegameclient.ui.GuildText;
import com.company.assembleegameclient.ui.RankText;
import com.company.assembleegameclient.ui.TextBox;
import com.company.assembleegameclient.util.TextureRedrawer;
import com.company.ui.SimpleText;
import com.company.util.BitmapUtil;
import com.company.util.CachingColorTransformer;
import com.company.util.MoreColorUtil;
import com.company.util.PointUtil;
import com.gskinner.motion.GTween;

import flash.display.Bitmap;
import flash.display.BitmapData;
import flash.display.Sprite;
import flash.display.StageScaleMode;
import flash.events.Event;
import flash.external.ExternalInterface;
import flash.filters.ColorMatrixFilter;
import flash.filters.DropShadowFilter;
import flash.utils.ByteArray;
import flash.utils.getTimer;
import flash.utils.setTimeout;

import kabam.lib.loopedprocs.LoopedCallback;
import kabam.lib.loopedprocs.LoopedProcess;
import kabam.rotmg.constants.GeneralConstants;
import kabam.rotmg.core.model.MapModel;
import kabam.rotmg.core.model.PlayerModel;
import kabam.rotmg.core.view.Layers;
import kabam.rotmg.game.view.CreditDisplay;
import kabam.rotmg.messaging.impl.GameServerConnection;
import kabam.rotmg.messaging.impl.incoming.MapInfo;
import kabam.rotmg.servers.api.Server;
import kabam.rotmg.stage3D.Renderer;
import kabam.rotmg.ui.UIUtils;
import kabam.rotmg.ui.view.HUDView;

import org.osflash.signals.Signal;

public class GameSprite extends Sprite
{
   protected static const PAUSED_FILTER:ColorMatrixFilter = new ColorMatrixFilter(MoreColorUtil.greyscaleFilterMatrix);

   public const closed:Signal = new Signal();
   public const monitor:Signal = new Signal(String,int);
   public const modelInitialized:Signal = new Signal();
   public const drawCharacterWindow:Signal = new Signal(Player);
   public var map:Map;
   public var camera_:Camera;
   public var gsc_:GameServerConnection;
   public var mui_:MapUserInput;
   public var textBox_:TextBox;
   public var tutorial_:Tutorial;
   public var isNexus_:Boolean = false;
   public var idleWatcher_:IdleWatcher;
   public var hudView:HUDView;
   public var rankText_:RankText;
   public var guildText_:GuildText;
   public var creditDisplay_:CreditDisplay;
   public var isEditor:Boolean;
   public var lastUpdate_:int = 0;
   public var moveRecords_:MoveRecords;
   public var mapModel:MapModel;
   public var model:PlayerModel;
   private var focus:GameObject;
   private var frameTimeSum_:int = 0;
   private var frameTimeCount_:int = 0;
   private var isGameStarted:Boolean;
   private var displaysPosY:uint = 4;
   public var dmgCounter:SimpleText;

   public function GameSprite(server:Server, gameId:int, createCharacter:Boolean, charId:int, keyTime:int, key:ByteArray, model:PlayerModel, mapJSON:String)
   {
      this.camera_ = new Camera();
      this.moveRecords_ = new MoveRecords();
      super();
      this.model = model;
      this.map = new Map(this);
      addChild(this.map);
      this.gsc_ = new GameServerConnection(this,server,gameId,createCharacter,charId,keyTime,key,mapJSON);
      this.mui_ = new MapUserInput(this);
      this.textBox_ = new TextBox(this,600,600);
      addChild(this.textBox_);
      this.idleWatcher_ = new IdleWatcher();
      this.dmgCounter = new SimpleText(14,16777215,false,0,0);
      this.dmgCounter.setBold(true);
      this.dmgCounter.filters = [new DropShadowFilter(0,0,0)];
      this.dmgCounter.x = 0;
      this.dmgCounter.y = 400;
   }

   public function setFocus(focus:GameObject) : void
   {
      focus = focus || this.map.player_;
      this.focus = focus;
   }

   public function applyMapInfo(mapInfo:MapInfo) : void
   {
      this.map.setProps(mapInfo.width_,mapInfo.height_,mapInfo.name_,mapInfo.background_,mapInfo.allowPlayerTeleport_,mapInfo.showDisplays_);
      this.showPreloader(mapInfo);
   }

   public function showPreloader(mapInfo:MapInfo) : void
   {
      //var showMapLoading:ShowMapLoadingSignal = StaticInjectorContext.getInjector().getInstance(ShowMapLoadingSignal);
      //showMapLoading && showMapLoading.dispatch(mapInfo);
   }

   private function hidePreloader() : void
   {
      //var hideMapLoading:HideMapLoadingSignal = StaticInjectorContext.getInjector().getInstance(HideMapLoadingSignal);
      //hideMapLoading && hideMapLoading.dispatch();
   }

   public function hudModelInitialized() : void
   {
      this.hudView = new HUDView();
      this.hudView.x = 600;
      addChild(this.hudView);
   }

   public function initialize() : void
   {
      this.map.initialize();
      this.creditDisplay_ = new CreditDisplay(this);
      this.creditDisplay_.x = 594;
      this.creditDisplay_.y = 0;
      addChild(this.creditDisplay_);
      this.modelInitialized.dispatch();

      if(this.map.showDisplays_)
      {
         this.showSafeAreaDisplays();
      }

      if(this.map.name_ == "Tutorial")
      {
         this.startTutorial();
      }

      if (this.map.name_ == "Nexus")
      {
         isNexus_ = true;
      }

      //Parameters.save();
      this.hidePreloader();
      stage.dispatchEvent(new Event(Event.RESIZE));
      this.parent.parent.setChildIndex((this.parent.parent as Layers).top, 2);
   }

   private function showSafeAreaDisplays() : void
   {
      this.showRankText();
      this.showGuildText();
   }

   private function showGuildText() : void
   {
      this.guildText_ = new GuildText("",-1);
      this.guildText_.x = 64;
      this.guildText_.y = 6;
      addChild(this.guildText_);
   }

   private function showRankText() : void
   {
      this.rankText_ = new RankText(-1,true,false);
      this.rankText_.x = 8;
      this.rankText_.y = this.displaysPosY;
      this.displaysPosY = this.displaysPosY + UIUtils.NOTIFICATION_SPACE;
      addChild(this.rankText_);
   }

   private function callTracking(functionName:String) : void
   {
      if(ExternalInterface.available == false)
      {
         return;
      }
      try
      {
         ExternalInterface.call(functionName);
      }
      catch(err:Error)
      {
      }
   }

   private function startTutorial() : void
   {
      this.tutorial_ = new Tutorial(this);
      addChild(this.tutorial_);
   }

   private function updateNearestInteractive() : void
   {
      var dist:Number = NaN;
      var go:GameObject = null;
      var iObj:IInteractiveObject = null;
      if(!this.map || !this.map.player_)
      {
         return;
      }
      var player:Player = this.map.player_;
      var minDist:Number = GeneralConstants.MAXIMUM_INTERACTION_DISTANCE;
      var closestInteractive:IInteractiveObject = null;
      var playerX:Number = player.x_;
      var playerY:Number = player.y_;
      for each(go in this.map.goDict_)
      {
         iObj = go as IInteractiveObject;
         if(iObj)
         {
            if(Math.abs(playerX - go.x_) < GeneralConstants.MAXIMUM_INTERACTION_DISTANCE || Math.abs(playerY - go.y_) < GeneralConstants.MAXIMUM_INTERACTION_DISTANCE)
            {
               dist = PointUtil.distanceXY(go.x_,go.y_,playerX,playerY);
               if(dist < GeneralConstants.MAXIMUM_INTERACTION_DISTANCE && dist < minDist)
               {
                  minDist = dist;
                  closestInteractive = iObj;
               }
            }
         }
      }
      this.mapModel.currentInteractiveTarget = closestInteractive;
   }

   public function onScreenResize(_arg_1:Event):void
   {
      var uiscale:Boolean = Parameters.data_.uiscale;
      var sWidth:Number = (800 / stage.stageWidth);
      var sHeight:Number = (600 / stage.stageHeight);
      var result:Number = (sWidth / sHeight);
      if (this.map != null)
      {
         this.map.scaleX = (sWidth * Parameters.data_.mscale);
         this.map.scaleY = (sHeight * Parameters.data_.mscale);
      }
      if (this.hudView != null)
      {
         if (uiscale)
         {
            this.hudView.scaleX = result;
            this.hudView.scaleY = 1;
            this.hudView.y = 0;
         }
         else
         {
            this.hudView.scaleX = sWidth;
            this.hudView.scaleY = sHeight;
            this.hudView.y = (300 * (1 - sHeight));
         }
         this.hudView.x = (800 - (200 * this.hudView.scaleX));
         if (this.creditDisplay_ != null)
         {
            this.creditDisplay_.x = (this.hudView.x - (6 * this.creditDisplay_.scaleX));
         }
      }
      if (this.rankText_ != null)
      {
         if (uiscale)
         {
            this.rankText_.scaleX = result;
            this.rankText_.scaleY = 1;
         }
         else
         {
            this.rankText_.scaleX = sWidth;
            this.rankText_.scaleY = sHeight;
         }
         this.rankText_.x = (8 * this.rankText_.scaleX);
         this.rankText_.y = (2 * this.rankText_.scaleY);
      }
      if (this.guildText_ != null)
      {
         if (uiscale)
         {
            this.guildText_.scaleX = result;
            this.guildText_.scaleY = 1;
         }
         else
         {
            this.guildText_.scaleX = sWidth;
            this.guildText_.scaleY = sHeight;
         }
         this.guildText_.x = (64 * this.guildText_.scaleX);
         this.guildText_.y = (2 * this.guildText_.scaleY);
      }
      if (this.creditDisplay_ != null)
      {
         if (uiscale)
         {
            this.creditDisplay_.scaleX = result;
            this.creditDisplay_.scaleY = 1;
         }
         else
         {
            this.creditDisplay_.scaleX = sWidth;
            this.creditDisplay_.scaleY = sHeight;
         }
      }
   }

   public function connect() : void
   {
      if(!this.isGameStarted)
      {
         this.isGameStarted = true;
         Renderer.inGame = true;
         this.gsc_.connect();
         this.idleWatcher_.start(this);
         this.lastUpdate_ = getTimer();
         //stage.addEventListener(MoneyChangedEvent.MONEY_CHANGED,this.onMoneyChanged);
         stage.addEventListener(Event.ENTER_FRAME,this.onEnterFrame);
         if(Parameters.data_.FS){
            if (Parameters.data_.mscale == undefined)
            {
               Parameters.data_.mscale = "1.0";
            }
            if (Parameters.data_.stageScale == undefined)
            {
               Parameters.data_.stageScale = StageScaleMode.NO_SCALE;
            }
            if (Parameters.data_.uiscale == undefined)
            {
               Parameters.data_.uiscale = true;
            }
            Parameters.save();
            stage.scaleMode = Parameters.data_.stageScale;
            stage.addEventListener(Event.RESIZE, this.onScreenResize);
            stage.dispatchEvent(new Event(Event.RESIZE));
         }
         Parameters.DamageCounter = [];
         if(!contains(this.dmgCounter)){
            addChild(this.dmgCounter);
         }
         LoopedProcess.addProcess(new LoopedCallback(100,this.updateNearestInteractive));
      }
   }

   public function disconnect() : void
   {
      if(this.isGameStarted)
      {
         this.isGameStarted = false;
         Renderer.inGame = false;
         this.idleWatcher_.stop();
         this.gsc_.serverConnection.disconnect();
         //stage.removeEventListener(MoneyChangedEvent.MONEY_CHANGED,this.onMoneyChanged);
         stage.removeEventListener(Event.ENTER_FRAME,this.onEnterFrame);
         stage.removeEventListener(Event.RESIZE, this.onScreenResize);
         stage.scaleMode = StageScaleMode.EXACT_FIT;
         LoopedProcess.destroyAll();
         stage.dispatchEvent(new Event(Event.RESIZE));
         contains(this.map) && removeChild(this.map);
         this.map.dispose();
         CachingColorTransformer.clear();
         TextureRedrawer.clearCache();
         Projectile.dispose();
         this.gsc_.disconnect();
         Parameters.DamageCounter = [];
         if(contains(this.dmgCounter)){
            removeChild(this.dmgCounter);
         }
      }
   }

   /*private function onMoneyChanged(event:Event) : void
   {
      this.gsc_.checkCredits();
   }*/

   public function evalIsNotInCombatMapArea() : Boolean
   {
      return this.map.name_ == "Nexus" || this.map.name_ == "Vault" || this.map.name_ == "Guild Hall";
   }

   public function DamageCounter() : void
   {
      var gameObject:GameObject = null;
      var objectId:int = 0;

      for each(gameObject in map.goDict_)
      {
         if((gameObject.props_.isQuest_ || gameObject.props_.isChest_) && Parameters.DamageCounter[gameObject.objectId_] > 0 && objectId == 0)
         {
            objectId = gameObject.objectId_;
         }

         if(objectId == 0 && this.dmgCounter.text != ""){
             dmgCounter.text = "";
             dmgCounter.updateMetrics();
         }

         if((gameObject.props_.isQuest_ || gameObject.props_.isChest_) && Parameters.DamageCounter[gameObject.objectId_] > 0)
         {
            if(gameObject != null)
            {
               var name:* = ObjectLibrary.typeToDisplayId_[gameObject.objectType_];
               if(Parameters.DamageCounter[gameObject.objectId_] > gameObject.maxHP_)
                  Parameters.DamageCounter[gameObject.objectId_] = gameObject.maxHP_;
               var dmgInflicted:* = int(Parameters.DamageCounter[gameObject.objectId_] / gameObject.maxHP_ * 100);
               this.dmgCounter.text = "Enemy: " + name + "\nDamage Inflicted: (" + dmgInflicted + "%)";
               this.dmgCounter.updateMetrics();
            }
         }
      }
   }

   private function onEnterFrame(event:Event) : void
   {
      var avgFrameRate:Number = NaN;
      var time:int = getTimer();
      var dt:int = time - this.lastUpdate_;
      if(this.idleWatcher_.update(dt))
      {
         this.closed.dispatch();
         return;
      }
      LoopedProcess.runProcesses(time);
      this.frameTimeSum_ = this.frameTimeSum_ + dt;
      this.frameTimeCount_ = this.frameTimeCount_ + 1;
      if(this.frameTimeSum_ > 300000)
      {
         avgFrameRate = int(Math.round(1000 * this.frameTimeCount_ / this.frameTimeSum_));
         this.frameTimeCount_ = 0;
         this.frameTimeSum_ = 0;
      }
      var mapTime:int = getTimer();
      this.map.update(time,dt);
      this.monitor.dispatch("Map.update",getTimer() - mapTime);
      this.camera_.update(dt);
      var player:Player = this.map.player_;
      if(this.focus)
      {
         this.camera_.configureCamera(this.focus,Boolean(player)?Boolean(player.isHallucinating()):Boolean(false));
         this.map.draw(this.camera_,time);
      }
      if(player != null)
      {
         this.creditDisplay_.draw(player.credits_,player.fame_);
         this.drawCharacterWindow.dispatch(player);
         if(this.map.showDisplays_)
         {
            this.rankText_.draw(player.numStars_);
            this.guildText_.draw(player.guildName_,player.guildRank_);
         }
         if(player.isPaused())
         {
            this.map.filters = [PAUSED_FILTER];
            this.hudView.filters = [PAUSED_FILTER];
            this.map.mouseEnabled = false;
            this.map.mouseChildren = false;
            this.hudView.mouseEnabled = false;
            this.hudView.mouseChildren = false;
         }
         else if(this.map.filters.length > 0)
         {
            this.map.filters = [];
            this.hudView.filters = [];
            this.map.mouseEnabled = true;
            this.map.mouseChildren = true;
            this.hudView.mouseEnabled = true;
            this.hudView.mouseChildren = true;
         }
         this.moveRecords_.addRecord(time,player.x_,player.y_);
      }
      this.lastUpdate_ = time;
      var delta:int = getTimer() - time;
      this.monitor.dispatch("GameSprite.loop",delta);
      this.DamageCounter();
   }
}
}
