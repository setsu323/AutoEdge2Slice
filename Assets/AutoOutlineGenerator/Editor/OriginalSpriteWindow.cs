using AutoOutlineGenerator.SpriteExtension;
using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace AutoOutlineGenerator.Editor
{
    public class OriginalSpriteWindow : EditorWindow
    {
        private SpriteDataProviderFactories SpriteDataProviderFactories
        {
            get
            {
                if (_spriteDataProviderFactories == null)
                {
                    _spriteDataProviderFactories = new SpriteDataProviderFactories();
                    _spriteDataProviderFactories.Init();
                }
                return _spriteDataProviderFactories;
            }
        }

        private SpriteDataProviderFactories _spriteDataProviderFactories;
        
        [MenuItem("Tools/TextureDivider")]
        public static void ShowWindow()
        {
            GetWindow<OriginalSpriteWindow>();
        }

        private ISpriteEditorDataProvider CreateEditorDataProvider(Texture2D texture)
        {
            var spriteEditorDataProvider = SpriteDataProviderFactories.GetSpriteEditorDataProviderFromObject(texture);
            spriteEditorDataProvider.InitSpriteEditorDataProvider();
            return spriteEditorDataProvider;
        }
        void OnEnable()
        {
            var autoSpriteDivider = new AutoSpriteDivider();
            var outlineOptimizer = new OutlineGenerator();
            
            var field = new ObjectField() {allowSceneObjects = false};
            field.objectType = typeof(Texture2D);
            field.RegisterValueChangedCallback(x =>
            {
                var spriteEditorDataProvider = CreateEditorDataProvider(x.newValue as Texture2D);
                autoSpriteDivider.SetSpriteEditorDataProvider(spriteEditorDataProvider);
                outlineOptimizer.Set(spriteEditorDataProvider);
            });
            var applyButton = new Button(() =>
                {
                    autoSpriteDivider.DivideSprite(new Vector2Int(100, 100), 3, new Vector2(0.4f, 0.4f));
                    outlineOptimizer.GenerateOutline();
                }
            );
            
            var root = rootVisualElement;
            root.Add(field);
            root.Add(applyButton);
        }
    }
}