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
    [ScriptedImporter(1,"edgpd")]
    public class PageDataScriptedImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            var targetTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(GetTextureTargetPath(ctx.assetPath));
            
            //textureから実装する
            var spriteEditorDataProvider = GetSpriteEditorDataProvider(targetTexture);
            
            var autoSpriteDivider = new AutoSpriteDivider(spriteEditorDataProvider);
            var outlineOptimizer = new OutlineGenerator(spriteEditorDataProvider);

            var pageDataDivider = new PageDataDivider(spriteEditorDataProvider.GetDataProvider<ITextureDataProvider>(),
                File.ReadAllText(ctx.assetPath));
            
            pageDataDivider.SetPageData(out var rectInt,out var pivot);
            autoSpriteDivider.DivideSprite(rectInt,pivot);
            outlineOptimizer.GenerateOutline();
            
            spriteEditorDataProvider.Apply();
            AssetDatabase.ImportAsset(GetTextureTargetPath(ctx.assetPath));
        }

        private static ISpriteEditorDataProvider GetSpriteEditorDataProvider(Texture2D texture2D)
        {
            var factories = new SpriteDataProviderFactories();                
            factories.Init();
            var spriteEditorDataProvider = factories.GetSpriteEditorDataProviderFromObject(texture2D);
            spriteEditorDataProvider.InitSpriteEditorDataProvider();
            return spriteEditorDataProvider;
        }

        private string GetTextureTargetPath(string path)
        {
            return Path.ChangeExtension(path, "png");
        }
    }
}