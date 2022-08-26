package kabam.rotmg.essences {
import flash.utils.Dictionary;

public class TalismanLibrary {

    private static var talismanProperties_:Dictionary = new Dictionary();
    private static var types_:Vector.<int> = new Vector.<int>();

    public static function parseFromXML(xml:XML) {
        for each(var talismanXml:XML in xml.Talisman) {
            var talisman:TalismanProperties = new TalismanProperties(talismanXml);
            talismanProperties_[talisman.type_] = talisman;
            types_.push(talisman.type_);
        }
    }

    public static function getTalismanTypes():Vector.<int>{
        return types_;
    }

    public static function getTalisman(type:int):TalismanProperties{
        return talismanProperties_[type];
    }
}
}
