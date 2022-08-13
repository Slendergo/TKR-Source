package kabam.rotmg.essences {
import flash.utils.Dictionary;

public class TalismanLibrary {

    private static var talismanProperties_:Dictionary = new Dictionary();
    private static var talismans_:Vector.<TalismanProperties> = new Vector.<TalismanProperties>();

    public static function parseFromXML(xml:XML) {
        for each(var talismanXml:XML in xml.Talisman) {
            var talisman:TalismanProperties = new TalismanProperties(talismanXml);
            talismanProperties_[talisman.type_] = talisman;
            talismans_.push(talisman);
        }
    }

    public static function getTalismans():Vector.<TalismanProperties>{
        return talismans_;
    }
}
}
