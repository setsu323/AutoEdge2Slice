using UnityEditor;
using UnityEditor.U2D.Sprites;

namespace AutoOutlineGenerator.SpriteExtension
{
    public class SpriteLayout
    {
        void Save()
        {
            //textureImporterからSpriteSheetを取得して、そこから
            //spriteDataExtを取得して、変更できるようにする
            //m_Spritesから取得する
            SpriteDataExt spriteDataExt = new SpriteDataExt(new SerializedProperty());
            TextureImporter m_TextureImporter = null;
            var so = new SerializedObject(m_TextureImporter);
            var spriteSheetSO = so.FindProperty("m_SpriteSheet.m_Sprites");
            
            //すべて削除……はしない方がいいか……
            spriteSheetSO.ClearArray();
            

        }
    }
}