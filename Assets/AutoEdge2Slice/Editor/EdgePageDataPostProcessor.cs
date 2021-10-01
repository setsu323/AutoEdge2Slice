using System;
using System.IO;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine;

namespace AutoOutlineGenerator.Editor
{
    public class EdgePageDataPostProcessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            foreach (var importedAssetPath in importedAssets)
            {
                if (Path.GetExtension(importedAssetPath) == ".xml")
                {
                    //Edgeのページデータかを判定する
                    var document = XDocument.Parse(File.ReadAllText(importedAssetPath));
                    if (document.Root.Name == "ExportedPages")
                    {
                        PageDataScriptedImporter.ImportSpriteAsset(importedAssetPath);
                    }
                }
            }
        }

        private void OnPreprocessTexture()
        {
            var textureImporter = assetImporter as TextureImporter;
            var pageDataPath = Path.ChangeExtension(textureImporter.assetPath, "xml");
            var guid = AssetDatabase.AssetPathToGUID(pageDataPath);
            if (guid != "")
            {
                var spriteSettingWriter = new SpriteSettingWriter(textureImporter);
                spriteSettingWriter.WriteSpriteSetting();
            }
        }
    }
}