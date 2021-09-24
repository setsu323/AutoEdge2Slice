using UnityEngine;

namespace AutoOutlineGenerator.Editor
{
    public class PageProperty
    {
        public Vector2Int TextureSize;
        /// <summary>
        /// →が+x方向,↓が+y方向
        /// </summary>
        public Vector2Int Offset;
        public Vector2Int Standard;

        public PageProperty(Vector2Int textureSize, Vector2Int offset, bool isCenterStandard)
        {
            if (isCenterStandard)
            {
                Standard = textureSize / 2;
            }
            else
            {
                Standard = Vector2Int.zero;
            }

            TextureSize = textureSize;
            Offset = offset;
        }
    }
}