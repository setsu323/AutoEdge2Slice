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
                            if (iter.name == "_outlineGeneratorFactoryType")
                            {
                                continue;
                            }
                            EditorGUILayout.PropertyField(iter, true);
                        }

                        var factoryTypeNameProp = so.FindProperty("_outlineGeneratorFactoryType");
                        var factoryTypeName = factoryTypeNameProp.stringValue;

                        var methods = TypeCache.GetMethodsWithAttribute<OutlineGeneratorFactoryMethodAttribute>();
                        var objects = methods.Select(m => m.Invoke(null, null) as Object).ToList();
                        
                        var index = objects.FindIndex(s => s.GetType().Name == factoryTypeName);
                        index = EditorGUILayout.Popup("_factories", index,
                            objects.Select(s => s.GetType().Name).ToArray());

                        if (index != -1)
                        {
                            factoryTypeNameProp.stringValue = objects[index].GetType().Name;
                        }


                        if (index != -1)
                        {
                            using (var fscope = new EditorGUI.ChangeCheckScope())
                            {
                                var fso = new SerializedObject(objects[index]);
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
                                }
                                (objects[index] as IOutlineGeneratorFactory).Save();
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