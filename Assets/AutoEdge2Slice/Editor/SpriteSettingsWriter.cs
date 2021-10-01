using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;

namespace AutoOutlineGenerator.Editor
{
    public class SpriteSettingsWriter
    {
        private readonly TextureImporter _textureImporter;
        private readonly TextureImporterCompression _textureImporterCompression;
        private readonly FilterMode _filterMode;
        private readonly float _pixelsPerUnit;
        private readonly int _maxTextureSize;

        public SpriteSettingsWriter(TextureImporter textureImporter, TextureImporterCompression textureImporterCompression, FilterMode filterMode, float pixelsPerUnit, int maxTextureSize)
        {
            _textureImporter = textureImporter;
            _textureImporterCompression = textureImporterCompression;
            _filterMode = filterMode;
            _pixelsPerUnit = pixelsPerUnit;
            _maxTextureSize = maxTextureSize;
        }

        public void WriteSpriteSetting()
        {
            _textureImporter.textureType = TextureImporterType.Sprite;
            _textureImporter.spriteImportMode = SpriteImportMode.Multiple;
            _textureImporter.maxTextureSize = _maxTextureSize;
            _textureImporter.textureCompression = _textureImporterCompression;
            _textureImporter.filterMode = _filterMode;
            _textureImporter.spritePixelsPerUnit = _pixelsPerUnit;
        }
    }
}