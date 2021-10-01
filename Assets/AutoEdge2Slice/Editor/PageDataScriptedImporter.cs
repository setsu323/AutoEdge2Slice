using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEditor.U2D.Sprites;
using UnityEngine;

namespace AutoOutlineGenerator.Editor
{
    [ScriptedImporter(1,new []{"edgpd"})]
    public class PageDataScriptedImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            ImportSpriteAsset(ctx.assetPath);
        }

        public static void ImportSpriteAsset(string path)
        {
            var targetTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(GetTextureTargetPath(path));
            
            //textureから実装する
            var spriteEditorDataProvider = GetSpriteEditorDataProvider(targetTexture);
            
            var autoSpriteDivider = new SpriteDivider(spriteEditorDataProvider);
            var outlineOptimizer = new VerticalSplitOutlineGenerator(spriteEditorDataProvider);

            var pageDataDivider = new PageDataDivider(spriteEditorDataProvider.GetDataProvider<ITextureDataProvider>(),
                File.ReadAllText(path));
            
            pageDataDivider.SetPageData(out var rectInt,out var pivot);
            autoSpriteDivider.DivideSprite(rectInt,pivot);
            outlineOptimizer.GenerateOutline();
            
            spriteEditorDataProvider.Apply();
            AssetDatabase.ImportAsset(GetTextureTargetPath(path));
        }

        private static ISpriteEditorDataProvider GetSpriteEditorDataProvider(Texture2D texture2D)
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