using System;
using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace AutoEdge2Slice.Editor
{
    public class SplitSpriteOutlineWindow : EditorWindow
    {
        [MenuItem("Tools/SplitSpriteOutline")]
        public static void ShowWindow()
        {
            CreateWindow<SplitSpriteOutlineWindow>();
        }

        private void OnEnable()
        {
            var dataProvider = new AnimationClipDataProvider();
            var animationClipGenerator = new AnimationClipGenerator();

            var spriteField = new ObjectField() { objectType = typeof(Texture2D), label = "Edge2XMLファイル" };
            spriteField.RegisterValueChangedCallback(changeEvent =>
            {
                var path = AssetDatabase.GetAssetPath(changeEvent.newValue);

                var spriteEditorDataProvider =
                    SpriteAssetImporter.GetSpriteEditorDataProvider((Texture2D)changeEvent.newValue);
                var outlineGenerator =
                    SpriteAssetImporter.GetOutlineGeneratorFactory().Create(spriteEditorDataProvider);
                outlineGenerator.GenerateOutline();
            });
            rootVisualElement.Add(spriteField);
        }
    }
}