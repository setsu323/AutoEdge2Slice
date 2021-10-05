using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace AutoEdge2Slice.Editor
{
    public class GenerateAnimationClipWindow : EditorWindow
    {
        public static void ShowWindow()
        {
            CreateWindow<GenerateAnimationClipWindow>();
        }

        private void OnEnable()
        {
            var dataProvider = new AnimationClipDataProvider();
            
            //パス用のフィールド
            var pathListField = new ListView();
            
            var pathField = new ObjectField() { objectType = typeof(TextAsset[]), label = "path" };
            pathField.RegisterValueChangedCallback(changeEvent =>
            {
                var path = AssetDatabase.GetAssetPath(changeEvent.newValue);
                if (dataProvider.TryGetPageDocument(path, out var document))
                {
                    //取得成功
                    //spriteの表示を行う
                    if(dataProvider.TryGetSprites(path,out var sprites))
                    {
                        //imageの表示とbuttonの表示を行う
                        var image = new Image() { image = sprites[0].texture };
                        var generateAnimationClipButton = new Button(() =>
                        {

                        });
                    }
                }
            });
            var image = new Image() { };

            EditorUtility.DisplayDialog("title", "Animationが既に存在します。上書きしますか？", "Ok", "Skip");
        }
    }
}