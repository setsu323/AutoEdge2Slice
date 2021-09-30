using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;

namespace AutoOutlineGenerator.Editor
{
    public class SpriteSettingWriter
    {
        //TODO Spriteの設定を行えるようにする
        private readonly TextureImporter _textureImporter;
        private readonly ISpriteEditorDataProvider _spriteEditorDataProvider;

        public SpriteSettingWriter(TextureImporter textureImporter, ISpriteEditorDataProvider spriteEditorDataProvider)
        {
            _textureImporter = textureImporter;
            _spriteEditorDataProvider = spriteEditorDataProvider;
        }
        public SpriteSettingWriter(ISpriteEditorDataProvider spriteEditorDataProvider)
        {
            _spriteEditorDataProvider = spriteEditorDataProvider;
            _textureImporter = spriteEditorDataProvider.targetObject as TextureImporter;
        }

        public void WriteSpriteSetting()
        {
            _textureImporter.textureType = TextureImporterType.Sprite;
            _textureImporter.spriteImportMode = SpriteImportMode.Multiple;
            _textureImporter.maxTextureSize = 16384;
            _textureImporter.textureCompression = TextureImporterCompression.Uncompressed;
            _textureImporter.filterMode = FilterMode.Point;
            _textureImporter.spritePixelsPerUnit = 64;
            _spriteEditorDataProvider.Apply();
        }
    }
}