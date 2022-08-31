using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AutoEdge2Slice.Editor
{
    [FilePath("ProjectSettings/SpriteSettings.asset", FilePathAttribute.Location.ProjectFolder)]
    internal class SpriteSettings : ScriptableSingleton<SpriteSettings>
    {
        [SerializeField]
        private TextureImporterCompression _textureImporterCompression = TextureImporterCompression.Uncompressed;
        [SerializeField]
        private FilterMode _filterMode = FilterMode.Point;
        [SerializeField]
        private float _pixelPerUnit = 64;
        [SerializeField]
        private int _maxTextureSize = 16384;

        [Space(1), SerializeField] private Object _outlineGeneratorFactory;

        public TextureImporterCompression TextureImporterCompression => _textureImporterCompression;
        public FilterMode FilterMode => _filterMode;
        public float PixelPerUnit => _pixelPerUnit;
        public int MaxTextureSize => _maxTextureSize;
        public IOutlineGeneratorFactory OutlineGeneratorFactory => _outlineGeneratorFactory as IOutlineGeneratorFactory;

        private TypeCache.TypeCollection _typeCollection;
        private void OnEnable()
        {
            hideFlags &= ~HideFlags.NotEditable;
        }

        public void Save()
        {
            Save(true);
        }
    }
}