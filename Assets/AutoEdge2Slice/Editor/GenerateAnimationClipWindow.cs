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

        private void OnEnable()
        {
            var dataProvider = new AnimationClipDataProvider();
            var animationClipGenerator = new AnimationClipGenerator();

            var pathField = new ObjectField() { objectType = typeof(TextAsset), label = "path" };
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
                            button = new Button() { name = "generateButton" };
                            rootVisualElement.Add(button);
                        }

                        button.clicked += () =>
                        {
                            var clip = animationClipGenerator.CreateAnimationClip(sprites, document);
                            AssetDatabase.CreateAsset(clip, Path.ChangeExtension(path, "anim"));
                            AssetDatabase.Refresh();
                        };
                    }
                }
            });
            rootVisualElement.Add(pathField);
        }
    }
}