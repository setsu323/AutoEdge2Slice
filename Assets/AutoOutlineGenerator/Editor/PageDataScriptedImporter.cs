using System;
using System.IO;
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
            var document = XDocument.Parse(File.ReadAllText(ctx.assetPath));
            //documentから情報を取得する。
            
            var targetTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(GetTextureTargetPath(ctx.assetPath));
            
            //textureから実装する
            var spriteEditorDataProvider = GetSpriteEditorDataProvider(targetTexture);
            
            var autoSpriteDivider = new AutoSpriteDivider(spriteEditorDataProvider);
            var outlineOptimizer = new OutlineGenerator(spriteEditorDataProvider);

            var width = int.Parse(document.Element("width").Value);
            var height = int.Parse(document.Element("height").Value);
            var splitCount = int.Parse(document.Element("splitCount").Value);
            var offsetX = int.Parse(document.Element("offsetX").Value);
            var offsetY = int.Parse(document.Element("offsetY").Value);
            var pivot = new Vector2(offsetX / (float) width, offsetY / (float) height);

            width = 100;
            height = 100;
            splitCount = 3;
            offsetX = 35;
            offsetY = 18;
            pivot = new Vector2(offsetX / (float) width, offsetY / (float) height);
            
            autoSpriteDivider.DivideSprite(new Vector2Int(width, height), splitCount, pivot);
            outlineOptimizer.GenerateOutline();
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