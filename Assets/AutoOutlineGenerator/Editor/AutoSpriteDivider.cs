using System;
using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;

namespace AutoOutlineGenerator.SpriteExtension
{
    public class AutoSpriteDivider
    {
        private ISpriteEditorDataProvider _spriteEditorDataProvider;

        public void DivideSprite(Vector2Int splitSize, int splitCount,Vector2 pivot)
        {
            var cached = _spriteEditorDataProvider.GetSpriteRects();
            var textureDataProvider =
                _spriteEditorDataProvider.GetDataProvider<ITextureDataProvider>();
            
            textureDataProvider.GetTextureActualWidthAndHeight(out var width, out var height);
            SpriteRect[] spriteRects;
            if (cached.Length > splitCount)
            {
                spriteRects = cached;
            }
            else
            {
                spriteRects = new SpriteRect[splitCount];
                cached.CopyTo(spriteRects, 0);
            }

            Split(splitSize, spriteRects, pivot, splitCount, textureDataProvider);
            
            _spriteEditorDataProvider.SetSpriteRects(spriteRects);
            _spriteEditorDataProvider.Apply();
        }

        private static void Split(Vector2Int splitSize,SpriteRect[] spriteRects,Vector2 pivot,int splitCount,ITextureDataProvider textureDataProvider)
        {
            textureDataProvider.GetTextureActualWidthAndHeight(out var width, out var height);
            var spriteNumber = 0;
            for (var y = height - splitSize.y; y >= 0; y -= splitSize.y)
            {
                for (var x = 0; x + splitSize.x <= width; x += splitSize.x)
                {
                    var spriteRect = spriteRects[spriteNumber] ?? new SpriteRect();
                    spriteRect.rect = ShrinkSpriteRect(textureDataProvider,new Rect(new Vector2(x, y), splitSize));

                    spriteRect.pivot = ConvertPivot(new Rect(new Vector2(x, y), splitSize), pivot, spriteRect.rect);
                    
                    spriteRect.alignment = SpriteAlignment.Custom;
                    spriteRect.spriteID = new GUID(System.Guid.NewGuid().ToString());
                    spriteRect.name = spriteNumber.ToString();
                    spriteRects[spriteNumber] = spriteRect;

                    spriteNumber++;
                    if (spriteNumber >= splitCount) return;
                }
            }

            throw new ArgumentException();
        }

        private static Vector2 ConvertPivot(Rect originalRect, Vector2 pivot,Rect targetRect)
        {
            var pos = new Vector2(Mathf.Lerp(originalRect.xMin, originalRect.xMax, pivot.x),
                Mathf.Lerp(originalRect.yMin, originalRect.yMax, pivot.y));
            var x = (pos.x - targetRect.xMin) / targetRect.width;
            var y = (pos.y - targetRect.yMin) / targetRect.height;
            return new Vector2(x, y);
        }

        private static Rect ShrinkSpriteRect(ITextureDataProvider textureDataProvider,Rect rect)
        {
            var pixels = textureDataProvider.GetReadableTexture2D().GetPixels((int) rect.x, (int) rect.y, (int) rect.width, (int) rect.height);
            
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

        public void SetSpriteEditorDataProvider(ISpriteEditorDataProvider spriteEditorDataProvider)
        {
            _spriteEditorDataProvider = spriteEditorDataProvider;
        }
    }
}