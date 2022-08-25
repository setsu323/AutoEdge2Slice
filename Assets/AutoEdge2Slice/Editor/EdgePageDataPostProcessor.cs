using System.IO;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine;

namespace AutoEdge2Slice.Editor
{
    internal class EdgePageDataPostProcessor : AssetPostprocessor
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
                        SpriteAssetImporter.ImportSpriteAsset(importedAssetPath);
                        //上で分割を行うため、Postprocessの段階ではまだSpriteアセットの設定が為されていない。
                        //そのためDelayCallを使ってAnimationを生成するタイミングをずらしている。
                        EditorApplication.delayCall += () => CreateAnimation(importedAssetPath);
                    }
                }
            }
        }

        private static void CreateAnimation(string path)
        {
            var dataProvider = new AnimationClipDataProvider();
            if (!dataProvider.TryGetPageDocument(path, out var document)) return;
            if (!dataProvider.TryGetSprites(path, out var sprites)) return;
                    

            
            var clipPath = Path.ChangeExtension(path, "anim");
            var loadedClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(clipPath);
            var animationClipGenerator = new AnimationClipGenerator();
            
            if (loadedClip != null)
            {
                var result = EditorUtility.DisplayDialogComplex(clipPath,
                    "既に同名のアニメーションクリップが存在します、上書きしますか？", "上書きする", "中止", "別ファイルとして保存");
                if (result == 0)
                {
                    animationClipGenerator.ModifyAnimationClip(loadedClip, sprites, document);
                    AssetDatabase.ImportAsset(clipPath);
                }
                else if (result == 2)
                {
                    var clip = animationClipGenerator.CreateAnimationClip(sprites, document);
                    AssetDatabase.CreateAsset(clip, AssetDatabase.GenerateUniqueAssetPath(clipPath));
                }
            }
            else
            {
                var clip = animationClipGenerator.CreateAnimationClip(sprites, document);
                AssetDatabase.CreateAsset(clip, clipPath);
            }
        }
        /// <summary>
        /// Edge2のpadファイルの場合、テクスチャの設定をSpriteに変更する
        /// </summary>
        private void OnPreprocessTexture()
        {
            var textureImporter = assetImporter as TextureImporter;
            var pageDataPath = Path.ChangeExtension(textureImporter.assetPath, "xml");
            var guid = AssetDatabase.AssetPathToGUID(pageDataPath);
            if (guid != "")
            {
                var spriteSettings = SpriteSettings.GetOrCreate();
                var spriteSettingWriter = new SpriteSettingsWriter(textureImporter,
                    spriteSettings.TextureImporterCompression, spriteSettings.FilterMode, spriteSettings.PixelPerUnit, spriteSettings.MaxTextureSize);
                spriteSettingWriter.WriteSpriteSetting();
            }
        }
    }
}