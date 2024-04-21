using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine;

namespace AutoEdge2Slice.Editor
{
    internal class EdgePageDataPostProcessor : AssetPostprocessor
    {
        internal static string ExportedMark = "ExportedPages";
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            if (!SpriteSettings.instance.UseSpriteAutoImport) return;
            var shouldExportAnimations = new List<string>();
            foreach (var importedAssetPath in importedAssets)
            {
                if (Path.GetExtension(importedAssetPath) == ".xml")
                {
                    //Edgeのページデータかを判定する
                    var document = XDocument.Parse(File.ReadAllText(importedAssetPath));
                    if (document.Root.Name == ExportedMark)
                    {
                        SpriteAssetImporter.ImportSpriteAsset(importedAssetPath);
                        //上で分割を行うため、Postprocessの段階ではまだSpriteアセットの設定が為されていない。
                        //そのためDelayCallを使ってAnimationを生成するタイミングをずらしている。
                        shouldExportAnimations.Add(importedAssetPath);
                    }
                }
            }
            
            if (shouldExportAnimations.Count > 0)
            {
                EditorApplication.delayCall += () => CreateAnimations(shouldExportAnimations);
            }
        }

        private static void CreateAnimations(List<string> paths)
        {
            var count = paths.Count - 1;
            var autoOverride = false;
            foreach (var path in paths)
            {
                CreateAnimation(path,count,ref autoOverride);
                count -= 1;
            }
        }

        private static void CreateAnimation(string path,int rest,ref bool autoOverride)
        {
            var dataProvider = new AnimationClipDataProvider();
            if (!dataProvider.TryGetPageDocument(path, out var document)) return;
            if (!dataProvider.TryGetSprites(path, out var sprites)) return;

            var clipPath = Path.ChangeExtension(path, "anim");
            var loadedClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(clipPath);
            var animationClipGenerator = new AnimationClipGenerator();
            var useLoop = clipPath.Contains("Loop");

            if (loadedClip != null)
            {
                var result = 0;
                if (autoOverride)
                {
                    result = 0;
                }
                else
                {
                    result = EditorUtility.DisplayDialogComplex(clipPath,
                        "既に同名のアニメーションクリップが存在します、上書きしますか？", "上書きする", "中止", "別ファイルとして保存");

                    if (result == 0 && rest > 0)
                    {
                        autoOverride = EditorUtility.DisplayDialog("残りクリップ数" + rest, "残りクリップも同様に上書きしますか？", "上書きする", "しない");
                    }
                }

                
                //上書きする場合
                if (result == 0)
                {
                    animationClipGenerator.ModifyAnimationClip(loadedClip, sprites, document,useLoop);
                    AssetDatabase.ImportAsset(clipPath);
                }
                //別ファイルとして保存
                else if (result == 2)
                {
                    var clip = animationClipGenerator.CreateAnimationClip(sprites, document,useLoop);
                    AssetDatabase.CreateAsset(clip, AssetDatabase.GenerateUniqueAssetPath(clipPath));
                }
            }
            else
            {
                var clip = animationClipGenerator.CreateAnimationClip(sprites, document,useLoop);
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
                var spriteSettings = SpriteSettings.instance;
                var spriteSettingWriter = new SpriteSettingsWriter(textureImporter,
                    spriteSettings.TextureImporterCompression, spriteSettings.FilterMode, spriteSettings.PixelPerUnit, spriteSettings.MaxTextureSize);
                spriteSettingWriter.WriteSpriteSetting();
            }
        }
    }
}