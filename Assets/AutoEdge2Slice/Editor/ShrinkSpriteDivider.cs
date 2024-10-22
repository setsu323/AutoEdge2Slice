using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;

namespace AutoEdge2Slice.Editor
{
    /// <summary>
    /// SpriteのAlphaが最小になるようにSpriteRectを決定する。
    /// </summary>
    internal class ShrinkSpriteDivider
    {
        private readonly ISpriteEditorDataProvider _spriteEditorDataProvider;
        private readonly ITextureDataProvider _textureDataProvider;
        private readonly string _parentName;

        public void DivideSprite(Rect[] rects,Vector2[] pivots)
        {
            var cached = _spriteEditorDataProvider.GetSpriteRects();

            SpriteRect[] spriteRects;
            if (cached.Length > rects.Length)
            {
                spriteRects = cached;
            }
            else
            {
                spriteRects = new SpriteRect[rects.Length];
                cached.CopyTo(spriteRects, 0);
            }
            
            Split(spriteRects,rects,pivots);

            _spriteEditorDataProvider.SetSpriteRects(spriteRects);
            _spriteEditorDataProvider.Apply();
        }
        
        private void Split(SpriteRect[] spriteRects, Rect[] rects, Vector2[] pivots)
        {
            for (var i = 0; i < rects.Length; i++)
            {
                var spriteRect = spriteRects[i] ?? new SpriteRect();
                spriteRect.rect = ShrinkSpriteRect(rects[i]);
                spriteRect.pivot = ConvertPivot(rects[i], pivots[i], spriteRect.rect);
            
                spriteRect.alignment = SpriteAlignment.Custom;
                spriteRect.spriteID = new GUID(System.Guid.NewGuid().ToString());

                spriteRect.name = _parentName + "_" + i;
                spriteRects[i] = spriteRect;
            }
        }

        private static Vector2 ConvertPivot(Rect originalRect, Vector2 pivot,Rect targetRect)
        {
            var pos = new Vector2(Mathf.Lerp(originalRect.xMin, originalRect.xMax, pivot.x),
                Mathf.Lerp(originalRect.yMin, originalRect.yMax, pivot.y));
            var x = (pos.x - targetRect.xMin) / targetRect.width;
            var y = (pos.y - targetRect.yMin) / targetRect.height;
            return new Vector2(x, y);
        }
        
        private Rect ShrinkSpriteRect(Rect rect)
        {
            //TODO UnityのPOTやmaxSizeに対応する
            var pixels = _textureDataProvider.GetReadableTexture2D().GetPixels((int) rect.x, (int) rect.y, (int) rect.width, (int) rect.height);
            
            var xMin = (int)rect.width;
            var yMin = (int)rect.height;
            var xMax = 0;
            var yMax = 0;
            for (var x = 0; x < rect.width; x++)
            {
                for (var y = 0; y < rect.height; y++)
                {
                    if (!(pixels[x + y * (int) rect.width].a > 0)) continue;
                    
                    if (x < xMin)
                    {
                        xMin = x;
                    }

                    if (y < yMin)
                    {
                        yMin = y;
                    }

                    if (x > xMax)
                    {
                        xMax = x;
                    }

                    if (y > yMax)
                    {
                        yMax = y;
                    }
                }
            }
            return Rect.MinMaxRect(rect.xMin + xMin, rect.yMin + yMin, rect.xMin + xMax + 1, rect.yMin + yMax + 1);

        }
        
        public ShrinkSpriteDivider(ISpriteEditorDataProvider spriteEditorDataProvider, string parentName)
        {
            _spriteEditorDataProvider = spriteEditorDataProvider;
            _parentName = parentName;
            _textureDataProvider = _spriteEditorDataProvider.GetDataProvider<ITextureDataProvider>();
        }
    }
}