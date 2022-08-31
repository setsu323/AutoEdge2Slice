using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AutoEdge2Slice.Editor
{
    public class SpriteSettingsPreferences
    {

        internal static SpriteSettings spriteSettings
        {
            get
            {
                if (_spriteSettings == null)
                    _spriteSettings = Resources.Load("SpriteSettings") as SpriteSettings;
                return _spriteSettings;
            }
        }
        private static SpriteSettings _spriteSettings;
        
        [SettingsProvider]
        public static SettingsProvider CreateExampleProvider()
        {
            var provider = new SettingsProvider("SpriteSettings/", SettingsScope.User)
            {
                // タイトル
                label = "Example",
                // GUI描画
                guiHandler = searchContext =>
                {
                    using (var scope = new EditorGUI.ChangeCheckScope())
                    {
                        var so = new SerializedObject(spriteSettings);
                        so.Update();

                        var iter = so.GetIterator();
                        iter.NextVisible(true);
                        while (iter.NextVisible(false))
                        {
                            EditorGUILayout.PropertyField(iter, true);
                        }
                        
                        var factory = so.FindProperty("_outlineGeneratorFactory").objectReferenceValue as BaseOutlineGeneratorFactory;
                        var fso = new SerializedObject(factory);
                        fso.Update();
                        
                        var fiter = fso.GetIterator();
                        fiter.NextVisible(true);
                        while (fiter.NextVisible(false))
                        {
                            EditorGUILayout.PropertyField(fiter, true);
                        }
                        
                        if (scope.changed)
                        {
                            so.ApplyModifiedProperties();
                            fso.ApplyModifiedProperties();
                        }
                    }
                },
                // 検索時のキーワード
                keywords = new HashSet<string>(new[] { "Example" })
            };

            return provider;
        }
    }
}