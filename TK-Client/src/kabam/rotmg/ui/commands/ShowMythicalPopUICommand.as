package kabam.rotmg.ui.commands {
import flash.display.DisplayObjectContainer;
import com.gskinner.motion.GTween;
import flash.utils.setTimeout;
import kabam.rotmg.ui.view.MythicalNotifViewPng;

import mx.core.BitmapAsset;

public class  ShowMythicalPopUICommand {

    private static var MythicalNotifPng:Class = MythicalNotifViewPng;
    private var view:BitmapAsset;

    [Inject]
    public var contextView:DisplayObjectContainer;


    public function execute():void {
        view = new MythicalNotifPng();
        view.x = 0;
        view.y = 0;
        this.contextView.addChild(view);
        view.alpha = 0;
        new GTween(view,0.5,{"alpha":1});
        setTimeout(function():void
        {
            new GTween(view,0.5,{"alpha":0});
        },3500);
        setTimeout(this.remove,4500);
    }

    private function remove() : void
    {
        this.contextView.removeChild(view);
        view = null;
    }


}
}
