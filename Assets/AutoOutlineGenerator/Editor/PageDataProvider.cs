using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AutoOutlineGenerator.Editor
{
    /// <summary>
    /// Edgeのページ情報から、ひとつのサイズを取得する
    /// </summary>
    public class PageDataProvider
    {
        private readonly List<PageProperty> _pageProperties;
        private RectInt? _cachedImplicationArea = null;

        public PageDataProvider(List<PageProperty> pageProperties)
        {
            _pageProperties = pageProperties;
        }

        public RectInt CalculateImplicationArea()
        {
            _cachedImplicationArea ??= CalculateImplicationArea(_pageProperties.Select(x => x.RectInt));
            return (RectInt) _cachedImplicationArea;
        }

        public Vector2 Pivot => GetPivot(CalculateImplicationArea());
        public int GetSplitCount => _pageProperties.Count;
        
        /// <summary>
        /// 複数のRectIntを覆う最小のRectIntを導出する
        /// </summary>
        /// <param name="areas"></param>
        /// <returns></returns>
        private static RectInt CalculateImplicationArea(IEnumerable<RectInt> areas)
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
            var rectPivot = new Vector2(-rectInt.xMin / (float) (rectInt.xMax - rectInt.xMin),
                -rectInt.yMin / (float) (rectInt.yMax - rectInt.yMin));
            //↓Unityの座標系に直している
            return new Vector2(rectPivot.x, 1 - rectPivot.y);
        }
        
    }
}