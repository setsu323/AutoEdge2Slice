using UnityEditor;
using UnityEngine;

namespace AutoEdge2Slice.Editor
{
    internal class SpriteSettings : ScriptableObject
    {
        [SerializeField]
        private TextureImporterCompression _textureImporterCompression = TextureImporterCompression.Uncompressed;
        [SerializeField]
        private FilterMode _filterMode = FilterMode.Point;
        [SerializeField]
        private float _pixelPerUnit = 64;
        [SerializeField]
        private int _maxTextureSize = 16384;

        [Space(1), SerializeField] private BaseOutlineGeneratorFactory _outlineGeneratorFactory;

        public TextureImporterCompression TextureImporterCompression => _textureImporterCompression;
        public FilterMode FilterMode => _filterMode;
        public float PixelPerUnit => _pixelPerUnit;
        public int MaxTextureSize => _maxTextureSize;
        public BaseOutlineGeneratorFactory OutlineGeneratorFactory => _outlineGeneratorFactory;

        internal static SpriteSettings GetOrCreate()
        {
            var path = "Assets/AutoEdge2Slice/Editor/SpriteSettings.asset";
            var settings = AssetDatabase.LoadAssetAtPath<SpriteSettings>(path);

            if (settings != null) return settings;

            settings = CreateInstance<SpriteSettings>();
            AssetDatabase.CreateAsset(settings,path);
            AssetDatabase.SaveAssets();
            return settings;
        } 
    }
}