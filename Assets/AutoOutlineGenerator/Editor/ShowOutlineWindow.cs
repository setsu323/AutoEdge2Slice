using System;
using System.Collections.Generic;
using System.Linq;
using AutoOutlineGenerator.SpriteExtension;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace AutoOutlineGenerator.Editor
{
    public class ShowOutlineWindow : EditorWindow
    {
        [MenuItem("Tools/ShowOutlineValue")]
        public static void ShowWindow()
        {
            GetWindow<ShowOutlineWindow>();
        }

        private TextField _textField;
        private void OnEnable()
        {
            var root = rootVisualElement;
            var label = new Label("アウトライン");
            _textField = new TextField("objectNameBinding");
            var button = new Button(OnClick);
            
            var field = new ObjectField() {allowSceneObjects = false};
            field.objectType = typeof(Texture2D);
            var outlineRoot = new ScrollView();
            field.RegisterValueChangedCallback(x => OnChangeTexture(x.newValue as Texture2D, outlineRoot));

            var applyButton = new Button(() => Apply(field.value as Texture2D));
            

            root.Add(label);
            root.Add(_textField);
            root.Add(button);
            root.Add(field);
            root.Add(outlineRoot);
            root.Add(applyButton);
        }

        void OnClick()
        {
            rootVisualElement.Add(new Label(_textField.value));
        }

        private List<Vector2Field[]> _outlineFields = new List<Vector2Field[]>();
        /// <summary>
        /// Texture2Dが切り替わった際に呼ぶ
        /// </summary>
        private void OnChangeTexture(Texture2D texture2D,VisualElement outlineRootElement)
        {
            outlineRootElement.Clear();
            _outlineFields.Clear();
            
            var textureImporter =  AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(texture2D)) as TextureImporter;
            var outlines = OutlineSerialization.LoadOutline(textureImporter);
            for (var i = 0; i < outlines.Count; i++)
            {
                var outlineElement = new Box();
                outlineRootElement.Add(outlineElement);
                
                outlineElement.Add(new Label("_________"));
                
                var outline = outlines[i];
                var outlineField = new Vector2Field[outline.Length];
                for(var j=0;j<outline.Length;j++)
                {
                    var field = new Vector2Field() {value = outline[j]};
                    outlineField[j] = field;
                    outlineElement.Add(field);
                }

                _outlineFields.Add(outlineField);
            }
            //texture2D内のアウトラインの情報を列挙するためのUIElementsを登録する
        }

        private void Apply(Texture2D texture2D)
        {
            var textureImporter =  AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(texture2D)) as TextureImporter;
            OutlineSerialization.ApplyOutline(textureImporter,
                _outlineFields.Select(f => f.Select(x => x.value).ToArray()).ToList());
        }
    }
}