package kabam.rotmg.assets.task {
import avm2.intrinsics.memory.casi32;

import com.company.assembleegameclient.map.GroundLibrary;
import com.company.assembleegameclient.objects.ObjectLibrary;

import flash.net.URLLoaderDataFormat;

import flash.utils.IDataInput;

import kabam.lib.tasks.BaseTask;
import kabam.rotmg.appengine.api.AppEngineClient;

import robotlegs.bender.framework.api.ILogger;

public class GetServerXmlsTask extends BaseTask {

    [Inject]
    public var client:AppEngineClient;

    [Inject]
    public var logger:ILogger;

    public function GetServerXmlsTask()
    {
        super();
    }

    override protected function startTask() : void
    {
        this.logger.info("GetServerXMLData start");
        this.client.setDataFormat(URLLoaderDataFormat.BINARY);
        this.client.complete.addOnce(this.onComplete);
        this.client.sendRequest("/char/getServerXmls", null);
    }

    private function onComplete(isOK:Boolean, data:*) : void
    {
        if(isOK)
        {
            this.onDataComplete(data);
        }
        else
        {
            this.onTextError(data);
        }
        completeTask(isOK, data);
    }

    //process the data
    private function onDataComplete(data:IDataInput):void {
        var count:int = data.readInt();
        for (var i:int = 0; i < count; i++) {
            //get the data, convert byte to xml and insert it in client
            this.insertXml(data.readUTFBytes(data.readInt()));
        }
    }

    private function insertXml(rawXml:String):void {
        var xml:XML = XML(rawXml);
        try
        {
            GroundLibrary.parseFromXML(xml);
            ObjectLibrary.parseFromXML(xml, false);
        }
        catch (e:*) {
            logger.error(e);
        }
    }

    private function onTextError(data:String):void {
        logger.error("Error trying to get XMLS from Server.");
    }

}
}
