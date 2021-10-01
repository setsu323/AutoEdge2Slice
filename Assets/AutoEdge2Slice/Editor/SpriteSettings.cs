using UnityEditor;
using UnityEngine;

namespace AutoOutlineGenerator.Editor
{
    [CreateAssetMenu(fileName = "FILENAME", menuName = "SpriteSettings", order = 0)]
    public class SpriteSettings : ScriptableObject
    {
        public TextureImporterCompression _textureImporterCompression = TextureImporterCompression.Uncompressed;
        public FilterMode _filterMode;
        public float _pixelPerUnit;
        public int _maxTextureSize;
    }
}