using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;

namespace AutoOutlineGenerator.Editor.SpriteExtension
{
    /// <summary>
    /// 最小限の機能を持ったITextureDataProvider
    /// Internalなメソッドにアクセスするため、2DSpriteの拡張としてしか実装できない
    /// </summary>
    public class SimpleTextureDataProvider : ITextureDataProvider
    {
        public Texture2D texture { get; private set; }

        public Texture2D previewTexture => texture;
        
        //textureの実際のサイズを取得するにはTextureImporterが必要
        private TextureImporter _textureImporter;

        public void GetTextureActualWidthAndHeight(out int width, out int height)
        {
            width = height = 0;
            _textureImporter.GetWidthAndHeight(ref width, ref height);
        }

        private Texture2D _readableTexture;

        public SimpleTextureDataProvider(TextureImporter textureImporter,Texture2D texture2D)
        {
            _textureImporter = textureImporter;
            texture = texture2D;
        }

        public void UpdateTextureImporter(TextureImporter textureImporter,Texture2D texture2D)
        {
            _textureImporter = textureImporter;
            texture = texture2D;
            _readableTexture = null;
        }
        
        public Texture2D GetReadableTexture2D()
        {
            if (_readableTexture == null)
            {
                _readableTexture = SpriteUtility.CreateTemporaryDuplicate(texture, texture.width, texture.height);
                if (_readableTexture != null)
                    _readableTexture.filterMode = texture.filterMode;
            }
            return _readableTexture;
        }
    }
}