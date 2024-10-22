using System.IO;
using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;

namespace AutoEdge2Slice.Editor
{
    internal static class SpriteAssetImporter
    {
        public static void ImportSpriteAsset(string path,bool useShrink)
        {
            var targetTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(GetTextureTargetPath(path));
            var spriteEditorDataProvider = GetSpriteEditorDataProvider(targetTexture);
            
            new PageShapeProvider(spriteEditorDataProvider.GetDataProvider<ITextureDataProvider>(),
                File.ReadAllText(path)).GetPageData(out var rectInt, out var pivot);
            
            //現在のモードによって変えたい
            if (useShrink)
            {
                new ShrinkSpriteDivider(spriteEditorDataProvider, targetTexture.name).DivideSprite(rectInt, pivot);
            }
            else
            {
                new SimpleSpriteDivider(spriteEditorDataProvider, targetTexture.name).DivideSprite(rectInt, pivot);
            }
            
            GetOutlineGeneratorFactory().Create(spriteEditorDataProvider).GenerateOutline();
            
            spriteEditorDataProvider.Apply();
            AssetDatabase.ImportAsset(GetTextureTargetPath(path));
        }

        internal static IOutlineGeneratorFactory GetOutlineGeneratorFactory()
        {
            return SpriteSettings.instance.OutlineGeneratorFactory;
        }
        internal static ISpriteEditorDataProvider GetSpriteEditorDataProvider(Texture2D texture2D)
        {
            var factories = new SpriteDataProviderFactories();                
            factories.Init();
            var spriteEditorDataProvider = factories.GetSpriteEditorDataProviderFromObject(texture2D);
            spriteEditorDataProvider.InitSpriteEditorDataProvider();
            return spriteEditorDataProvider;
        }

        private static string GetTextureTargetPath(string path)
        {
            return Path.ChangeExtension(path, "png");
        }
    }
}