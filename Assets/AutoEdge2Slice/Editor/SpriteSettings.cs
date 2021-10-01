using UnityEditor;
using UnityEngine;

namespace AutoOutlineGenerator.Editor
{
    internal class SpriteSettings : ScriptableObject
    {
        public TextureImporterCompression _textureImporterCompression = TextureImporterCompression.Uncompressed;
        public FilterMode _filterMode;
        public float _pixelPerUnit;
        public int _maxTextureSize;

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