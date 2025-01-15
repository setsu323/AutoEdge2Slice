using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace AutoEdge2Slice.Editor.Main
{
    internal class EdgePageDataPostProcessor : AssetPostprocessor
    {
        internal static string ExportedMark = "ExportedPages";
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            if (!SpriteSettings.instance.UseSpriteAutoImport) return;
            var shouldExportAnimations = new List<(string,Type)>();
            
            Type defaultTargetComponentType;
            switch (SpriteSettings.instance.TargetComponent)
            {
                case SpriteSettings.TargetComponentType.SpriteRenderer:
                    defaultTargetComponentType = typeof(SpriteRenderer);
                    break;
                case SpriteSettings.TargetComponentType.Image:
                    defaultTargetComponentType = typeof(Image);
                    break;
                default:
                    defaultTargetComponentType = typeof(SpriteRenderer);
                    break;
            }
            
            
            foreach (var importedAssetPath in importedAssets)
            {
                if (Path.GetExtension(importedAssetPath) == ".xml")
                {
                    //TargetPathsに含まれるパスが含まれているかを確認し、
                    //含まれていない場合は処理をスキップする。
                    var isTarget = false;
                    foreach (var targetPath in SpriteSettings.instance.TargetPaths)
                    {
                        if (importedAssetPath.Contains(targetPath))
                        {
                            isTarget = true;
                            break;
                        }
                    }
                    if (!isTarget) continue;
                    
                    //Edgeのページデータかを判定する
                    var document = XDocument.Parse(File.ReadAllText(importedAssetPath));
                    if (document.Root != null && document.Root.Name == ExportedMark)
                    {
                        var currentType = defaultTargetComponentType;
                        if (importedAssetPath.Contains("CloseUp")) currentType = typeof(Image);
                        

                        SpriteAssetImporter.ImportSpriteAsset(importedAssetPath, currentType != typeof(Image));
                        
                        //上で分割を行うため、Postprocessの段階ではまだSpriteアセットの設定が為されていない。
                        //そのためDelayCallを使ってAnimationを生成するタイミングをずらしている。
                        shouldExportAnimations.Add(new(importedAssetPath, currentType));
                    }
                }
            }
            
            if (shouldExportAnimations.Count > 0)
            {
                EditorApplication.delayCall += () => CreateAnimations(shouldExportAnimations);
            }
        }

        private static void CreateAnimations(List<(string,Type)> pathAndTargetComponents)
        {
            var count = pathAndTargetComponents.Count - 1;
            var autoOverride = false;
            foreach (var path in pathAndTargetComponents)
            {
                CreateAnimation(path.Item1, count, ref autoOverride, path.Item2);
                count -= 1;
            }
        }

        private static void CreateAnimation(string path,int rest,ref bool autoOverride,Type targetComponentType)
        {
            var dataProvider = new AnimationClipDataProvider();
            if (!dataProvider.TryGetPageDocument(path, out var document)) return;
            if (!dataProvider.TryGetSprites(path, out var sprites)) return;
            
            var clipPath = Path.ChangeExtension(path, "anim");
            var loadedClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(clipPath);
            var animationClipGenerator = new AnimationClipGenerator();
            var containsLoopName = clipPath.Contains("Loop");

            if (loadedClip != null)
            {
                int result;
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
                    animationClipGenerator.ModifyAnimationClip(loadedClip, sprites, document, false, targetComponentType, "m_Sprite");
                    AssetDatabase.ImportAsset(clipPath);
                }
                //別ファイルとして保存
                else if (result == 2)
                {
                    var clip = animationClipGenerator.CreateAnimationClip(sprites, document,containsLoopName,targetComponentType,"m_Sprite");
                    AssetDatabase.CreateAsset(clip, AssetDatabase.GenerateUniqueAssetPath(clipPath));
                }
            }
            else
            {
                var clip = animationClipGenerator.CreateAnimationClip(sprites, document, containsLoopName,
                    targetComponentType, "m_Sprite");
                AssetDatabase.CreateAsset(clip, clipPath);
            }
        }
    }
}