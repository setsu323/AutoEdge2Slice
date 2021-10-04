using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEditor.U2D.Sprites;
using UnityEngine;

namespace AutoOutlineGenerator.Editor
{
    /// <summary>
    /// Edgeのページデータ書き出し情報を読み込んで、
    /// Unityの座標系に対応したRectとPivotを出力する。
    /// </summary>
    public class PageDataLoader
    {
        private readonly int _textureHeight;
        private readonly string _data;

        public PageDataLoader(ITextureDataProvider textureDataProvider,string data)
        {
            _data = data;
            textureDataProvider.GetTextureActualWidthAndHeight(out _, out _textureHeight);
        }

        public PageDataLoader(int textureHeight, string data)
        {
            _data = data;
            _textureHeight = textureHeight;
        }

        public void GetPageData(out Rect[] rect, out Vector2[] pivot)
        {
            var document = XDocument.Parse(_data);
            var root = document.Root;
            var useCenter = int.Parse(root.Element("Coordinate").Value) == 1;
            
            var exportRects = root.Elements("Page").Select(x => GetExportRect(x.Element("ExportRect").Value));
            var destPos = root.Elements("Page").Select(x => GetDestPos(x.Element("DestPos").Value));

            var rects = exportRects.ToArray();
            rect = rects;
            
            pivot = rects.Zip(destPos, (rect, pos) => ConvertDestPosToPivot(rect, pos, useCenter)).ToArray();
        }

        private static Vector2 ConvertDestPosToPivot(Rect rect,Vector2 destPos,bool useCenter)
        {
            var pivotPos = -destPos;
            if (useCenter)
            {
                pivotPos += new Vector2(Mathf.Floor(rect.size.x / 2), Mathf.Floor(rect.size.y / 2));
            }
            var pivot = new Vector2(pivotPos.x / rect.width, pivotPos.y / rect.height);
            
            //Unity用に座標を変換
            return new Vector2(pivot.x, 1 - pivot.y);
        }

        private Rect GetExportRect(string value)
        {
            var rectMembers = value.Split(',').Select(int.Parse).ToArray();
            var rect =  new Rect(rectMembers[0], rectMembers[1], rectMembers[2], rectMembers[3]);
            return ConvertRectCoordinate(rect);
        }

        /// <summary>
        /// Edge2の座標系からTexture2Dの座標系に変換する
        /// </summary>
        /// <param name="rectInt"></param>
        /// <returns></returns>
        private Rect ConvertRectCoordinate(Rect rectInt)
        {
            return new Rect(rectInt.xMin, _textureHeight - rectInt.yMax, rectInt.size.x, rectInt.size.y);
        }
        
        private static Vector2 GetDestPos(string value)
        {
            var pivot = value.Split(',').Select(int.Parse).ToArray();
            return new Vector2(pivot[0], pivot[1]);
        }
        

    }
}