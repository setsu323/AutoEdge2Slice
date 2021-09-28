using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEditor.UIElements;
using UnityEngine;
using SpriteUtility = UnityEditor.Sprites.SpriteUtility;

namespace AutoOutlineGenerator.SpriteExtension
{
    public class ShapeEditorExtension
    {
        //Testを行いたいなぁ……
        public static List<Vector2[]> GenerateSplitRectOutline(SpriteRect spriteRect, float detail,
            byte alphaTolerance, ITextureDataProvider textureDataProvider)
        {
            var outline = new List<Vector2[]>();
            var texture = textureDataProvider.GetReadableTexture2D();
            if (texture == null) return outline;

            var toCapConversionRate = GetCapConversionRate(texture, textureDataProvider);
            
            SplitRectVertically(spriteRect.rect,spriteRect.pivot.y,out var upperRect,out var lowerRect);

            var upperOutline = GenerateOutline(texture, upperRect, spriteRect.rect, detail, alphaTolerance,
                toCapConversionRate);
            var lowerOutline = GenerateOutline(texture, lowerRect, spriteRect.rect, detail, alphaTolerance,
                toCapConversionRate);

            return upperOutline.Concat(lowerOutline).ToList();
        }

        private static IEnumerable<Vector2[]> GenerateOutline(Texture2D readableTexture,Rect actualSpriteRect,Rect actualWholeSpriteRect,float detail,byte alphaTolerance,Vector2 toCapConversionRate)
        {
            SpriteUtility.GenerateOutline(readableTexture, actualSpriteRect, detail, alphaTolerance, true, out var paths);

            var outline = new List<Vector2[]>();
            
            var outlineRect = new Rect {size = actualSpriteRect.size, center = Vector2.zero};
            var outlineOffset = actualSpriteRect.center - actualWholeSpriteRect.center;
            
            foreach (var path in paths)
            {
                var convertedPath = path.Select(x =>
                    CapPointToRect(ConvertToActualSpriteRect(x, toCapConversionRate), outlineRect) + outlineOffset).ToArray();
                outline.Add(convertedPath);
            }

            return outline;
        }
        internal static void SplitRectVertically(Rect rect, float length,out Rect upperRect,out Rect lowerRect)
        {
            var splitPoint = Mathf.Lerp(rect.yMin, rect.yMax, length);
            upperRect = Rect.MinMaxRect(rect.xMin, rect.yMin, rect.xMax, splitPoint);
            lowerRect = Rect.MinMaxRect(rect.xMin, splitPoint, rect.xMax, rect.yMax);
        }
        internal static Rect ConvertToCappedSpriteRect(Rect actualSpriteRect, Vector2 toCapConversionRate)
        {
            var rect = actualSpriteRect;
            rect.xMin *= toCapConversionRate.x;
            rect.xMax *= toCapConversionRate.x;
            rect.yMin *= toCapConversionRate.y;
            rect.yMax *= toCapConversionRate.y;
            return rect;
        }
        internal static Vector2 ConvertToActualSpriteRect(Vector2 cappedVector, Vector2 toCapConversionRate)
        {
            return new Vector2(cappedVector.x / toCapConversionRate.x, cappedVector.y / toCapConversionRate.y);
        }
        internal static Vector2 GetCapConversionRate(Texture2D texture,ITextureDataProvider textureDataProvider)
        {
            textureDataProvider.GetTextureActualWidthAndHeight(out var actualWidth,out var actualHeight);
            var cappedWidth = texture.width;
            var cappedHeight = texture.height;
            return new Vector2(cappedWidth / (float) actualWidth, cappedHeight / (float) actualHeight);
        }
        private static Vector2 CapPointToRect(Vector2 so, Rect r)
        {
            so.x = Mathf.Min(r.xMax, so.x);
            so.x = Mathf.Max(r.xMin, so.x);
            so.y = Mathf.Min(r.yMax, so.y);
            so.y = Mathf.Max(r.yMin, so.y);
            return so;
        }

        private Texture2D _readableTexture;
        private Texture2D _texture;
    }
}