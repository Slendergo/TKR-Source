package kabam.rotmg.ui.view
{
   import com.company.assembleegameclient.screens.ServersScreen;

import kabam.lib.tasks.DispatchSignalTask;

import kabam.lib.tasks.TaskMonitor;
import kabam.lib.tasks.TaskSequence;
import kabam.rotmg.core.signals.SetScreenSignal;
   import kabam.rotmg.servers.api.ServerModel;
import kabam.rotmg.servers.services.GetServerListTask;

import robotlegs.bender.bundles.mvcs.Mediator;

   public class ServersMediator extends Mediator
   {


      [Inject]
      public var view:ServersScreen;

      [Inject]
      public var servers:ServerModel;

      [Inject]
      public var setScreen:SetScreenSignal;

      [Inject]
      public var task:GetServerListTask;

      [Inject]
      public var monitor:TaskMonitor;

      public function ServersMediator()
      {
         super();
      }

      override public function initialize() : void
      {
         this.view.gotoTitle.add(this.onGotoTitle);
         this.view.initialize(this.servers.getServers());
         this.view.refresh.add(this.onRefresh);
      }

      public function onRefresh():void{
         var sequence:TaskSequence = new TaskSequence();
         sequence.add(this.task);
//         sequence.add(new DispatchSignalTask(this.setScreen, new ServersScreen()));
         this.monitor.add(sequence);
         sequence.start();
      }

      override public function destroy() : void
      {
         this.view.gotoTitle.remove(this.onGotoTitle);
         this.view.refresh.remove(this.onRefresh);
      }

      private function onGotoTitle() : void
      {
         this.setScreen.dispatch(new TitleView());
      }
   }
}
