using UnityEditor.U2D.Sprites;
using UnityEngine;

namespace AutoOutlineGenerator.Editor
{
    public class PageDataDivider
    {
        private ITextureDataProvider _textureDataProvider;
        private int _textureWidth;
        private int _textureHeight;

        public PageDataDivider(ITextureDataProvider textureDataProvider)
        {
            _textureDataProvider = textureDataProvider;
            _textureDataProvider.GetTextureActualWidthAndHeight(out _textureWidth, out _textureHeight);
        }

        public void SetPageData(RectInt[] rectInt, Vector2[] pivot)
        {
            
        }
        
        /// <summary>
        /// Edge2の座標系からTexture2Dの座標系に変換する
        /// </summary>
        /// <param name="rectInt"></param>
        /// <returns></returns>
        private RectInt ConvertRectCoordinate(RectInt rectInt)
        {
            return new RectInt(rectInt.xMin, _textureHeight - rectInt.yMax, rectInt.size.x, rectInt.size.y);
        }
    }
}