using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AutoEdge2Slice.Editor
{
    public class SpriteSettingsPreferences
    {
        [SettingsProvider]
        public static SettingsProvider CreateExampleProvider()
        {
            var provider = new SettingsProvider("2D/", SettingsScope.Project)
            {
                // タイトル
                label = "AutoEdge2Slice",
                // GUI描画
                guiHandler = searchContext =>
                {
                    using (var scope = new EditorGUI.ChangeCheckScope())
                    {
                        var so = new SerializedObject(SpriteSettings.instance);
                        so.Update();


                        var iter = so.GetIterator();
                        iter.NextVisible(true);
                        while (iter.NextVisible(false))
                        {
                            if (iter.name == "_outlineGeneratorFactory")
                            {
                                continue;
                            }
                            EditorGUILayout.PropertyField(iter, true);
                        }

                        var factoryProp = so.FindProperty("_outlineGeneratorFactory");
                        var factory = factoryProp
                            .objectReferenceValue;
                        
                        var methods = TypeCache.GetMethodsWithAttribute<OutlineGeneratorFactoryMethodAttribute>();
                        var objects = methods.Select(m => m.Invoke(null, null) as Object).ToList();
                        var index = objects.FindIndex(s => s == factory);

                        index = EditorGUILayout.Popup("_factories", index,
                            objects.Select(s => s.GetType().Name).ToArray());

                        if (index != -1)
                        {
                            factoryProp.objectReferenceValue = objects[index];
                        }


                        if (factory != null)
                        {
                            using (var fscope = new EditorGUI.ChangeCheckScope())
                            {
                                var fso = new SerializedObject(factory);
                                fso.Update();
                        
                                var fiter = fso.GetIterator();
                                fiter.NextVisible(true);
                                while (fiter.NextVisible(false))
                                {
                                    EditorGUILayout.PropertyField(fiter, true);
                                }
                        
                                if (fscope.changed)
                                {
                                    fso.ApplyModifiedProperties();
                                    (factory as IOutlineGeneratorFactory).Save();
                                }
                            }
                        }

                        if (scope.changed)
                        {
                            so.ApplyModifiedProperties();
                            SpriteSettings.instance.Save();
                        }
                    }
                },
                // 検索時のキーワード
                keywords = new HashSet<string>(new[] { "AutoEdge2Slice","Edge2","Sprite" })
            };

            return provider;
        }
    }
}