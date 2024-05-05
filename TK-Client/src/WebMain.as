package {
import com.company.assembleegameclient.map.Camera;
import com.company.assembleegameclient.parameters.Parameters;
import com.company.assembleegameclient.util.AssetLoader;
import com.company.assembleegameclient.util.StageProxy;
import flash.display.LoaderInfo;
import flash.display.Sprite;
import flash.display.Stage;
import flash.display.StageScaleMode;
import flash.events.Event;
import kabam.lib.net.NetConfig;
import kabam.rotmg.account.AccountConfig;
import kabam.rotmg.appengine.AppEngineConfig;
import kabam.rotmg.application.ApplicationConfig;
import kabam.rotmg.assets.AssetsConfig;
import kabam.rotmg.characters.CharactersConfig;
import kabam.rotmg.classes.ClassesConfig;
import kabam.rotmg.core.CoreConfig;
import kabam.rotmg.core.StaticInjectorContext;
import kabam.rotmg.death.DeathConfig;
import kabam.rotmg.dialogs.DialogsConfig;
import kabam.rotmg.errors.ErrorConfig;
import kabam.rotmg.Engine.EngineConfig;
import kabam.rotmg.fame.FameConfig;
import kabam.rotmg.game.GameConfig;
import kabam.rotmg.hud.HUDConfig;
import kabam.rotmg.language.LanguageConfig;
import kabam.rotmg.legends.LegendsConfig;
import kabam.rotmg.maploading.MapLoadingConfig;
import kabam.rotmg.minimap.MiniMapConfig;
import kabam.rotmg.news.NewsConfig;
import kabam.rotmg.servers.ServersConfig;
import kabam.rotmg.stage3D.Stage3DConfig;
import kabam.rotmg.startup.StartupConfig;
import kabam.rotmg.startup.control.StartupSignal;
import kabam.rotmg.tooltips.TooltipsConfig;
import kabam.rotmg.ui.UIConfig;
import robotlegs.bender.bundles.mvcs.MVCSBundle;
import robotlegs.bender.extensions.signalCommandMap.SignalCommandMapExtension;
import robotlegs.bender.framework.api.IContext;
import robotlegs.bender.framework.api.LogLevel;

[SWF(frameRate="60", backgroundColor="#000000", width="800", height="600")]
public class WebMain extends Sprite {

    public static var sWidth:Number = 800;
    public static var sHeight:Number = 600;

    public static var STAGE:Stage;

    public function WebMain() {

        // In future we create a launcher & loader thingy with this?
//        var bytes:ByteArray = new ByteArray();
//
//        bytes.objectEncoding = ObjectEncoding.AMF3;
//        bytes.writeUTF("Test");
//        bytes.writeUTF("TestA");
//        bytes.writeUTF("TestB");
//        bytes.writeUTF("TestC");
//        bytes.position = 0;
//
//        var appDataDir:File = File.applicationStorageDirectory;
//
//        var myFile:File = appDataDir.resolvePath("test.txt");
//
//        var stream:FileStream = new FileStream();
//
//        try {
//            stream.open(myFile, FileMode.WRITE);
//            stream.writeBytes(bytes);
//            stream.close();
//        } catch (e:Error) {
//            trace("Error writing to file: " + e.message);
//        }
//
//        try {
//            stream.open(myFile, FileMode.READ);
//
//            var myString:String = stream.readUTFBytes(stream.bytesAvailable);
//
//            trace(myString);
//
//            stream.close();
//
//        } catch (e:Error) {
//            trace("Error reading file: " + e.message);
//        }

        if (stage) {
            stage.addEventListener("resize", onStageResize, false, 0, true);
            setup();
        }
        else {
            addEventListener(Event.ADDED_TO_STAGE, onAddedToStage);
        }
    }

    protected var context:IContext;

    private function setup():void
    {
        STAGE = stage;
        hackParameters();
        createContext();
        new AssetLoader().load();
        stage.scaleMode = StageScaleMode.EXACT_FIT;
        var startup:StartupSignal = context.injector.getInstance(StartupSignal);
        startup.dispatch();
    }

    private function hackParameters():void {
        Parameters.root = stage.root;
    }

    private function createContext():void {
        var stageProxy:StageProxy = new StageProxy(this);
        context = new StaticInjectorContext();
        context.injector.map(LoaderInfo).toValue(root.stage.root.loaderInfo);
        context.injector.map(StageProxy).toValue(stageProxy);
        context
                .extend(MVCSBundle)
                .extend(SignalCommandMapExtension)
                .configure(StartupConfig)
                .configure(NetConfig)
                .configure(AssetsConfig)
                .configure(DialogsConfig)
                .configure(ApplicationConfig)
                .configure(AppEngineConfig)
                .configure(AccountConfig)
                .configure(ErrorConfig)
                .configure(CoreConfig)
                .configure(DeathConfig)
                .configure(CharactersConfig)
                .configure(ServersConfig)
                .configure(GameConfig)
                //.configure(ConsoleConfig)
                .configure(UIConfig)
                .configure(MiniMapConfig)
                .configure(LanguageConfig)
                .configure(LegendsConfig)
                .configure(NewsConfig)
                .configure(FameConfig)
                .configure(TooltipsConfig)
                .configure(MapLoadingConfig)
                .configure(ClassesConfig)
                .configure(Stage3DConfig)
                .configure(HUDConfig)
                .configure(EngineConfig)
                .configure(this);
        context.logLevel = LogLevel.DEBUG;
    }

    public function onStageResize(_arg_1:Event):void {
        if (Parameters.data_.FS) {
            scaleX = (stage.stageWidth / 800);
            scaleY = (stage.stageHeight / 600);
            x = ((800 - stage.stageWidth) / 2);
            y = ((600 - stage.stageHeight) / 2);
        }
        else {
            scaleX = 1;
            scaleY = 1;
            x = 0;
            y = 0;
        }
        sWidth = stage.stageWidth;
        sHeight = stage.stageHeight;
        Camera.adjustDimensions();
        Stage3DConfig.Dimensions();
    }

    private function onAddedToStage(event:Event):void {
        removeEventListener(Event.ADDED_TO_STAGE, onAddedToStage);
        setup();
    }
}
}
