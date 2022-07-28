package kabam.rotmg.classes.view
{
   import com.company.assembleegameclient.util.Currency;
   import com.company.util.AssetLibrary;
   import flash.display.Bitmap;
   import flash.display.BitmapData;
   import flash.display.DisplayObject;
   import kabam.rotmg.assets.services.CharacterFactory;
   import kabam.rotmg.classes.model.CharacterSkin;
   import kabam.rotmg.classes.model.CharacterSkins;
   import kabam.rotmg.util.components.LegacyBuyButton;
   
   public class CharacterSkinListItemFactory
   {
       
      
      [Inject]
      public var characters:CharacterFactory;
      
      public function CharacterSkinListItemFactory()
      {
         super();
      }
      
      public function make(skins:CharacterSkins) : Vector.<DisplayObject>
      {
         var count:int = 0;
         count = skins.getCount();
         var items:Vector.<DisplayObject> = new Vector.<DisplayObject>(count,true);
         for(var i:int = 0; i < count; i++)
         {
            items[i] = this.makeCharacterSkinTile(skins.getSkinAt(i));
         }
         return items;
      }
      
      private function makeCharacterSkinTile(model:CharacterSkin) : CharacterSkinListItem
      {
         var view:CharacterSkinListItem = new CharacterSkinListItem();
         view.setSkin(this.makeIcon(model));
         view.setModel(model);
         view.setLockIcon(AssetLibrary.getImageFromSet("lofiInterface2",5));
         view.setBuyButton(this.makeBuyButton());
         return view;
      }
      
      private function makeBuyButton() : LegacyBuyButton
      {
         var button:LegacyBuyButton = new LegacyBuyButton("Buy for ",16,0,Currency.GOLD);
         button.setWidth(120);
         return button;
      }
      
      private function makeIcon(model:CharacterSkin) : Bitmap
      {
         var data:BitmapData = this.characters.makeIcon(model.template);
         return new Bitmap(data);
      }
   }
}
