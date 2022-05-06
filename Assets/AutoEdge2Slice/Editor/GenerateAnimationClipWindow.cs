using System;
using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace AutoEdge2Slice.Editor
{
    public class GenerateAnimationClipWindow : EditorWindow
    {
        [MenuItem("Tools/AnimationGenerator")]
        public static void ShowWindow()
        {
            CreateWindow<GenerateAnimationClipWindow>();
        }

        private Action _preAction;
        private void OnEnable()
        {
            var dataProvider = new AnimationClipDataProvider();
            var animationClipGenerator = new AnimationClipGenerator();

            var pathField = new ObjectField() { objectType = typeof(TextAsset), label = "Edge2XMLファイル" };
            pathField.RegisterValueChangedCallback(changeEvent =>
            {
                var path = AssetDatabase.GetAssetPath(changeEvent.newValue);
                if (dataProvider.TryGetPageDocument(path, out var document))
                {
                    //取得成功
                    //spriteの表示を行う
                    if(dataProvider.TryGetSprites(path,out var sprites))
                    {

                        var spriteImage = rootVisualElement.Query<Image>(name: "spriteImage").First();
                        if (spriteImage == null)
                        {
                            spriteImage = new Image() { name = "spriteImage"};
                            rootVisualElement.Add(spriteImage);
                        }
                        spriteImage.image = sprites[0].texture;

                        var button = rootVisualElement.Query<Button>("generateButton").First();
                        if (button == null)
                        {
                            button = new Button() { name = "generateButton", text = "AnimationClipを生成する"};
                            rootVisualElement.Add(button);
                        }

                        if (_preAction != null)
                        {
                            button.clicked -= _preAction;
                        }
                        _preAction = new Action(() =>
                        {
                            var clipPath = Path.ChangeExtension(path, "anim"); 
                            var loadedClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(clipPath);
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

                        });
                        button.clicked += _preAction;

                    }
                }
            });
            rootVisualElement.Add(pathField);
        }
    }
}