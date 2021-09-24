using System.Collections.Generic;
using System.Linq;
using UnityEditor.Sprites;
using UnityEditor.U2D.Sprites;
using UnityEngine;

namespace AutoOutlineGenerator.SpriteExtension
{
    internal class ShapeEditorExtension
    {
        internal static List<SpriteOutline> GenerateSplitRectOutline(Rect actualSpriteRect,Vector2 actualPivot, float detail,
            byte alphaTolerance, ITextureDataProvider textureDataProvider)
        {
            var outline = new List<SpriteOutline>();
            var texture = textureDataProvider.GetReadableTexture2D();
            if (texture == null) return outline;

            var toCapConversionRate = GetCapConversionRate(texture, textureDataProvider);
            
            SplitRectVertically(actualSpriteRect,actualPivot.y,out var upperRect,out var lowerRect);

            var upperOutline = GenerateOutline(texture, upperRect, actualSpriteRect, detail, alphaTolerance,
                toCapConversionRate);
            var lowerOutline = GenerateOutline(texture, lowerRect, actualSpriteRect, detail, alphaTolerance,
                toCapConversionRate);

            return (List<SpriteOutline>) upperOutline.Concat(lowerOutline);
        }

        internal static List<SpriteOutline> GenerateOutline(Texture2D readableTexture,Rect actualSpriteRect,Rect actualWholeSpriteRect,float detail,byte alphaTolerance,Vector2 toCapConversionRate)
        {
            SpriteUtility.GenerateOutline(readableTexture, actualSpriteRect, detail, alphaTolerance, true, out var paths);

            var outline = new List<SpriteOutline>();
            
            var outlineRect = new Rect {size = actualSpriteRect.size, center = Vector2.zero};
            var outlineOffset = actualSpriteRect.center - actualWholeSpriteRect.center;
            
            foreach (var path in paths)
            {
                var points = new SpriteOutline();
                foreach (var cappedVector in path)
                    //Generateしたアウトラインを実際のに変換し、その後Rectにあわせている
                    points.Add(
                        CapPointToRect(ConvertToActualSpriteRect(cappedVector, toCapConversionRate), outlineRect) +
                        outlineOffset);
                outline.Add(points);
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