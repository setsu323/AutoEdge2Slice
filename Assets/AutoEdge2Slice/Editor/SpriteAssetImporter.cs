using System.IO;
using Editor.Interface;
using Editor.SpriteEditor;
using UnityEditor;
using UnityEngine;

namespace AutoEdge2Slice.Editor
{
    internal static class SpriteAssetImporter
    {
        public static void ImportSpriteAsset(string path)
        {
            var targetTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(GetTextureTargetPath(path));
            var spriteEditorDataProvider = GetSpriteEditorDataProvider(targetTexture);

            var spriteDivider = new SpriteDivider(spriteEditorDataProvider, targetTexture.name);

            var outlineGenerator = GetOutlineGeneratorFactory().Create(spriteEditorDataProvider);
            
            var pageDataDivider = new PageShapeProvider(spriteEditorDataProvider.GetDataProvider<ITextureDataProvider>(),
                File.ReadAllText(path));
            
            pageDataDivider.GetPageData(out var rectInt,out var pivot);
            spriteDivider.DivideSprite(rectInt,pivot);
            outlineGenerator.GenerateOutline();
            
            spriteEditorDataProvider.Apply();
            AssetDatabase.ImportAsset(GetTextureTargetPath(path));
        }

        internal static BaseOutlineGeneratorFactory GetOutlineGeneratorFactory()
        {
            const string path = "Assets/AutoEdge2Slice/Editor/SpriteSettings.asset";
            var settings = AssetDatabase.LoadAssetAtPath<SpriteSettings>(path);
            return settings.OutlineGeneratorFactory;
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