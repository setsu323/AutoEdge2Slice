using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;

namespace AutoOutlineGenerator.Editor
{
    public class SpriteSettingWriter
    {
        //TODO Spriteの設定を行えるようにする
        private readonly TextureImporter _textureImporter;

        public SpriteSettingWriter(TextureImporter textureImporter)
        {
            _textureImporter = textureImporter;
        }

        public void WriteSpriteSetting()
        {
            _textureImporter.textureType = TextureImporterType.Sprite;
            _textureImporter.spriteImportMode = SpriteImportMode.Multiple;
            _textureImporter.maxTextureSize = 16384;
            _textureImporter.textureCompression = TextureImporterCompression.Uncompressed;
            _textureImporter.filterMode = FilterMode.Point;
            _textureImporter.spritePixelsPerUnit = 64;
        }
    }
}