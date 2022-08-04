package kabam.rotmg.ui.commands {
import flash.display.DisplayObjectContainer;
import com.gskinner.motion.GTween;
import flash.utils.setTimeout;

import kabam.rotmg.ui.view.EternalNotifViewPng;
import kabam.rotmg.ui.view.RevengeNotifViewPng;

import mx.core.BitmapAsset;

public class  ShowEternalPopUICommand {

    private static var EternalNotifPng:Class = EternalNotifViewPng;
    private var view:BitmapAsset;

    [Inject]
    public var contextView:DisplayObjectContainer;


    public function execute():void {
        view = new EternalNotifPng();
        view.x = 160;
        view.y = 200;
        this.contextView.addChild(view);
        view.alpha = 0;
        new GTween(view,0.5,{"alpha":1});
        setTimeout(function():void
        {
            new GTween(view,0.5,{"alpha":0});
        },4500);
        setTimeout(this.remove,4500);
    }

    private function remove() : void
    {
        this.contextView.removeChild(view);
        view = null;
    }


}
}
