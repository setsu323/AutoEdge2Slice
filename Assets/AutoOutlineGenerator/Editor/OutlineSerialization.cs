using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AutoOutlineGenerator.SpriteExtension
{
    public class OutlineSerialization
    {
        public static void ApplyOutline(TextureImporter textureImporter,List<Vector2[]> outline)
        {
            //SpriteのRectと、Pivotも欲しいな
            var mode = SpriteImportMode.Multiple;
            var importer = new SerializedObject(textureImporter);
            
            var outlineSP = mode == SpriteImportMode.Multiple ?
                importer.FindProperty("m_SpriteSheet.m_Sprites").GetArrayElementAtIndex(0).FindPropertyRelative("m_Outline") :
                importer.FindProperty("m_SpriteSheet.m_Outline");

            if (mode == SpriteImportMode.Multiple)
            {
                var spriteProperty = importer.FindProperty("m_SpriteSheet.m_Sprites");
                for (var i = 0; i < spriteProperty.arraySize; i++)
                {
                    var prop = spriteProperty.GetArrayElementAtIndex(i).FindPropertyRelative("m_Outline");
                    Apply(prop, new List<Vector2[]> {outline[0]});
                }
            }
            else
            {
                Apply(importer.FindProperty("m_SpriteSheet.m_Outline"), outline);
            }
            
            
            importer.ApplyModifiedProperties();
        }

        private static void Apply(SerializedProperty outlineSP,List<Vector2[]> outline)
        {
            outlineSP.ClearArray();
            for (int j = 0; j < outline.Count; ++j)
            {
                outlineSP.InsertArrayElementAtIndex(j);
                var o = outline[j];
                SerializedProperty outlinePathSP = outlineSP.GetArrayElementAtIndex(j);
                outlinePathSP.ClearArray();
                for (int k = 0; k < o.Length; ++k)
                {
                    outlinePathSP.InsertArrayElementAtIndex(k);
                    outlinePathSP.GetArrayElementAtIndex(k).vector2Value = o[k];
                }
            }
        }
        
        public static List<Vector2[]> LoadOutline(TextureImporter textureImporter)
        {
            var mode = textureImporter.spriteImportMode;
            var importer = new SerializedObject(textureImporter);
            
            if (mode == SpriteImportMode.Multiple)
            {
                var retValue = new List<Vector2[]>();
                var spriteProperties = importer.FindProperty("m_SpriteSheet.m_Sprites");
                for (var i = 0; i < spriteProperties.arraySize; i++)
                {
                    retValue.AddRange(LoadOutline(spriteProperties.GetArrayElementAtIndex(i)
                        .FindPropertyRelative("m_Outline")));
                }

                return retValue;
            }
            else
            {
                return LoadOutline(importer.FindProperty("m_SpriteSheet.m_Outline"));
            }
        }

        private static List<Vector2[]> LoadOutline(SerializedProperty outlineSp)
        {
            var outline = new List<Vector2[]>();
            for (int j = 0; j < outlineSp.arraySize; ++j)
            {
                SerializedProperty outlinePathSP = outlineSp.GetArrayElementAtIndex(j);
                var o = new Vector2[outlinePathSP.arraySize];
                for (int k = 0; k < outlinePathSP.arraySize; ++k)
                {
                    o[k] = outlinePathSP.GetArrayElementAtIndex(k).vector2Value;
                }
                outline.Add(o);
            }

            return outline;
        } 
    }
}