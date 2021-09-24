using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AutoOutlineGenerator.Editor
{
    /// <summary>
    /// Edgeのページ情報から、ひとつのサイズを取得する
    /// </summary>
    public class PageSizeFinder
    {
        public void SplitPage()
        {
            //TODO Propertyを取得するための仕組みを追加
            List<PageProperty> pageProperties = new List<PageProperty>();
            var implicationArea = CalculateImplicationArea(pageProperties.Select(x => x.RectInt));
            //implicationAreaを用いて、最初の分割を行う
            
            //pivotの位置を設定する→座標の変換も含む
            var pivot = GetPivot(implicationArea);
            var textureRectInt = new RectInt();
            
            //分割するためのRectsを取得
            var rects = SplitTexture(textureRectInt.size, implicationArea.size);
            
        }

        public List<RectInt> SplitTexture(Vector2Int textureSize,Vector2Int pageSize)
        {
            var rects = new List<RectInt>();
            for (var y = 0; y < textureSize.y / pageSize.y; y++)
            {
                for (var x = 0; x < textureSize.x / pageSize.x; x++)
                {
                    rects.Add(new RectInt(new Vector2Int(x * pageSize.x, y * pageSize.y), pageSize));
                }
            }
        }
        
        /// <summary>
        /// 複数のRectIntを覆う最小のRectIntを導出する
        /// </summary>
        /// <param name="areas"></param>
        /// <returns></returns>
        public static RectInt CalculateImplicationArea(IEnumerable<RectInt> areas)
        {
            return areas.Aggregate((implication, next) => MinMaxRectInt(Math.Min(implication.xMin, next.xMin),
                Math.Min(implication.yMin, next.yMin), Math.Max(implication.xMax, next.xMax),
                Math.Max(implication.yMax, next.yMax)));
        }

        private static RectInt MinMaxRectInt(int minX, int minY, int maxX, int maxY)
        {
            return new RectInt(minX, minY, maxX - minX, maxY - minY);
        }

        private Vector2 GetPivot(RectInt rectInt)
        {
            //rectの座標系でのpivot
            var rectPivot = new Vector2(-rectInt.xMin / (float) (rectInt.xMax - rectInt.xMin),
                -rectInt.yMin / (float) (rectInt.yMax - rectInt.yMin));
            return new Vector2(rectPivot.x, 1 - rectPivot.y);
        }
        
    }
}