using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;

namespace AutoOutlineGenerator.Editor.SpriteExtension
{
    public class SimpleTextureDataProvider : ITextureDataProvider
    {
        public Texture2D texture { get; }
        public Texture2D previewTexture { get; }
        
        //textureの実際のサイズを取得するにはTextureImporterが必要
        private TextureImporter _textureImporter;
        
        
        public void GetTextureActualWidthAndHeight(out int width, out int height)
        {
            
            width = height = 0;
            _textureImporter.GetWidthAndHeight(ref width, ref height);
        }

        private Texture2D _readableTexture;
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