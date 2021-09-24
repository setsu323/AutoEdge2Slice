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
            var outlineRoot = new VisualElement();
            field.RegisterValueChangedCallback(x => OnChangeTexture(x.newValue as Texture2D, outlineRoot));
            
            root.Add(label);
            root.Add(_textField);
            root.Add(button);
            root.Add(field);
            root.Add(outlineRoot);
        }

        void OnClick()
        {
            rootVisualElement.Add(new Label(_textField.value));
        }

        /// <summary>
        /// Texture2Dが切り替わった際に呼ぶ
        /// </summary>
        private void OnChangeTexture(Texture2D texture2D,VisualElement outlineRootElement)
        {
            outlineRootElement.Clear();
            
            var textureImporter =  AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(texture2D)) as TextureImporter;
            var outlines = OutlineSerialization.LoadOutline(textureImporter);
            for (var i = 0; i < outlines.Count; i++)
            {
                var outlineElement = new Box();
                outlineRootElement.Add(outlineElement);
                
                outlineElement.Add(new Label("_________"));
                var outline = outlines[i];
                for(var j=0;j<outline.Length;j++)
                {
                    outlineElement.Add(new Label() {text = outline[j].ToString()});
                }
            }

            //texture2D内のアウトラインの情報を列挙するためのUIElementsを登録する
        }
    }
}