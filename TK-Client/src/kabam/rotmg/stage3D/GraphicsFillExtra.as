package kabam.rotmg.stage3D
{
   import com.company.assembleegameclient.parameters.Parameters;
   import flash.display.BitmapData;
   import flash.display.GraphicsBitmapFill;
   import flash.display.GraphicsSolidFill;
   import flash.display3D.Context3DVertexBufferFormat;
   import flash.display3D.VertexBuffer3D;
   import flash.geom.ColorTransform;
   import flash.utils.Dictionary;
   import kabam.rotmg.core.StaticInjectorContext;
   import kabam.rotmg.stage3D.proxies.Context3DProxy;

   public class GraphicsFillExtra
   {
      private static var textureOffsets:Dictionary = new Dictionary();
      private static var textureOffsetsSize:uint = 0;
      private static var waterSinks:Dictionary = new Dictionary();
      private static var waterSinksSize:uint = 0;
      private static var colorTransforms:Dictionary = new Dictionary();
      private static var colorTransformsSize:uint = 0;
      private static var vertexBuffers:Dictionary = new Dictionary();
      private static var vertexBuffersSize:uint = 0;
      private static var softwareDraw:Dictionary = new Dictionary();
      private static var softwareDrawSize:uint = 0;
      private static var softwareDrawSolid:Dictionary = new Dictionary();
      private static var softwareDrawSolidSize:uint = 0;
      private static var lastChecked:uint = 0;
      private static const DEFAULT_OFFSET:Vector.<Number> = Vector.<Number>([0,0,0,0]);

      public function GraphicsFillExtra()
      {
         super();
      }

      public static function setColorTransform(bitmap:BitmapData, value:ColorTransform) : void
      {
         if(!Parameters.isGpuRender())
         {
            return;
         }
         if(colorTransforms[bitmap] == null)
         {
            colorTransformsSize++;
         }
         colorTransforms[bitmap] = value;
      }

      public static function getColorTransform(bitmap:BitmapData) : ColorTransform
      {
         var colorTransform:ColorTransform = null;
         if(bitmap in colorTransforms)
         {
            colorTransform = colorTransforms[bitmap];
            colorTransforms[bitmap] = new ColorTransform();
         }
         else
         {
            colorTransform = new ColorTransform();
            colorTransforms[bitmap] = colorTransform;
            colorTransformsSize++;
         }
         return colorTransform;
      }

      public static function setOffsetUV(bitmapFill:GraphicsBitmapFill, u:Number, v:Number) : void
      {
         if(!Parameters.isGpuRender())
         {
            return;
         }
         testOffsetUV(bitmapFill);
         textureOffsets[bitmapFill][0] = u;
         textureOffsets[bitmapFill][1] = v;
      }

      public static function getOffsetUV(bitmapFill:GraphicsBitmapFill) : Vector.<Number>
      {
         if(textureOffsets[bitmapFill] != null)
         {
            return textureOffsets[bitmapFill];
         }
         return DEFAULT_OFFSET;
      }

      private static function testOffsetUV(bitmapFill:GraphicsBitmapFill) : void
      {
         if(!Parameters.isGpuRender())
         {
            return;
         }
         if(textureOffsets[bitmapFill] == null)
         {
            textureOffsetsSize++;
            textureOffsets[bitmapFill] = Vector.<Number>([0,0,0,0]);
         }
      }

      public static function setSinkLevel(bitmapFill:GraphicsBitmapFill, value:Number) : void
      {
         if(!Parameters.isGpuRender())
         {
            return;
         }
         if(waterSinks[bitmapFill] == null)
         {
            waterSinksSize++;
         }
         waterSinks[bitmapFill] = value;
      }

      public static function getSinkLevel(bitmapFill:GraphicsBitmapFill) : Number
      {
         if(waterSinks[bitmapFill] != null && waterSinks[bitmapFill] is Number)
         {
            return waterSinks[bitmapFill];
         }
         return 0;
      }

      public static function setVertexBuffer(bitmapFill:GraphicsBitmapFill, verts:Vector.<Number>) : void
      {
         if(!Parameters.isGpuRender())
         {
            return;
         }
         var context3D:Context3DProxy = StaticInjectorContext.getInjector().getInstance(Context3DProxy);
         var vertexBufferCustom:VertexBuffer3D = context3D.GetContext3D().createVertexBuffer(4,5);
         vertexBufferCustom.uploadFromVector(verts,0,4);
         context3D.GetContext3D().setVertexBufferAt(0,vertexBufferCustom,0,Context3DVertexBufferFormat.FLOAT_3);
         context3D.GetContext3D().setVertexBufferAt(1,vertexBufferCustom,3,Context3DVertexBufferFormat.FLOAT_2);
         if(vertexBuffers[bitmapFill] == null)
         {
            vertexBuffersSize++;
         }
         vertexBuffers[bitmapFill] = vertexBufferCustom;
      }

      public static function getVertexBuffer(bitmapFill:GraphicsBitmapFill) : VertexBuffer3D
      {
         if(vertexBuffers[bitmapFill] != null && vertexBuffers[bitmapFill] is VertexBuffer3D)
         {
            return vertexBuffers[bitmapFill];
         }
         return null;
      }

      public static function clearSink(bitmapFill:GraphicsBitmapFill) : void
      {
         if(!Parameters.isGpuRender())
         {
            return;
         }
         if(waterSinks[bitmapFill] != null)
         {
            waterSinksSize--;
            delete waterSinks[bitmapFill];
         }
      }

      public static function setSoftwareDraw(bitmapFill:GraphicsBitmapFill, value:Boolean) : void
      {
         if(!Parameters.isGpuRender())
         {
            return;
         }
         if(softwareDraw[bitmapFill] == null)
         {
            softwareDrawSize++;
         }
         softwareDraw[bitmapFill] = value;
      }

      public static function isSoftwareDraw(bitmapFill:GraphicsBitmapFill) : Boolean
      {
         if(softwareDraw[bitmapFill] != null && softwareDraw[bitmapFill] is Boolean)
         {
            return softwareDraw[bitmapFill];
         }
         return false;
      }

      public static function setSoftwareDrawSolid(solidFill:GraphicsSolidFill, value:Boolean) : void
      {
         if(!Parameters.isGpuRender())
         {
            return;
         }
         if(softwareDrawSolid[solidFill] == null)
         {
            softwareDrawSolidSize++;
         }
         softwareDrawSolid[solidFill] = value;
      }

      public static function isSoftwareDrawSolid(solidFill:GraphicsSolidFill) : Boolean
      {
         if(softwareDrawSolid[solidFill] != null && softwareDrawSolid[solidFill] is Boolean)
         {
            return softwareDrawSolid[solidFill];
         }
         return false;
      }

      public static function dispose() : void
      {
         textureOffsets = new Dictionary();
         waterSinks = new Dictionary();
         colorTransforms = new Dictionary();
         disposeVertexBuffers();
         softwareDraw = new Dictionary();
         softwareDrawSolid = new Dictionary();
         textureOffsetsSize = 0;
         waterSinksSize = 0;
         colorTransformsSize = 0;
         vertexBuffersSize = 0;
         softwareDrawSize = 0;
         softwareDrawSolidSize = 0;
      }

      public static function disposeVertexBuffers() : void
      {
         var buffer3d:VertexBuffer3D = null;
         for each(buffer3d in vertexBuffers)
         {
            buffer3d.dispose();
         }
         vertexBuffers = new Dictionary();
      }

      public static function manageSize() : void
      {
         if(colorTransformsSize > 2000)
         {
            colorTransforms = new Dictionary();
            colorTransformsSize = 0;
         }
         if(textureOffsetsSize > 2000)
         {
            textureOffsets = new Dictionary();
            textureOffsetsSize = 0;
         }
         if(waterSinksSize > 2000)
         {
            waterSinks = new Dictionary();
            waterSinksSize = 0;
         }
         if(vertexBuffersSize > 1000)
         {
            disposeVertexBuffers();
            vertexBuffersSize = 0;
         }
         if(softwareDrawSize > 2000)
         {
            softwareDraw = new Dictionary();
            softwareDrawSize = 0;
         }
         if(softwareDrawSolidSize > 2000)
         {
            softwareDrawSolid = new Dictionary();
            softwareDrawSolidSize = 0;
         }
      }
   }
}
