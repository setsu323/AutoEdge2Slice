using UnityEditor;
using UnityEditor.U2D.Sprites;

namespace AutoOutlineGenerator.SpriteExtension
{
    public class AutoSpriteDivider
    {
        //textureの情報等
        //

        private TextureImporterDataProvider _textureImporterDataProvider;
        public void DivideSprite()
        {
            //spriteの情報のすべてを扱えるようにしたい。
            TextureImporter textureImporter = null;
            _textureImporterDataProvider = new TextureImporterDataProvider(textureImporter);
        }
    }
}