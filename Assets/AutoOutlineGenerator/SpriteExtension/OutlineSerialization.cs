using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AutoOutlineGenerator.SpriteExtension
{
    public class OutlineSerialization
    {

        void ApplyOutline(TextureImporter textureImporter,List<Vector2[]> outline)
        {
            var mode = SpriteImportMode.Multiple;
            var importer = new SerializedObject(textureImporter);
            
            var outlineSP = mode == SpriteImportMode.Multiple ?
                importer.FindProperty("m_SpriteSheet.m_Sprites").GetArrayElementAtIndex(0).FindPropertyRelative("m_Outline") :
                importer.FindProperty("m_SpriteSheet.m_Outline");
            
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
        
        void LoadOutline(TextureImporter textureImporter)
        {
            var mode = SpriteImportMode.Multiple;
            var importer = new SerializedObject(textureImporter);
            
            //modeに応じて必要なアウトラインの情報が異なる
            var outlineSP = mode == SpriteImportMode.Multiple ?
                importer.FindProperty("m_SpriteSheet.m_Sprites").GetArrayElementAtIndex(0).FindPropertyRelative("m_Outline") :
                importer.FindProperty("m_SpriteSheet.m_Outline");
            
            //複数のSpriteに複数のOutlineが入っている。
            //Outlineは複数のVector2の集合
            
            var outline = new List<Vector2[]>();
            for (int j = 0; j < outlineSP.arraySize; ++j)
            {
                SerializedProperty outlinePathSP = outlineSP.GetArrayElementAtIndex(j);
                var o = new Vector2[outlinePathSP.arraySize];
                for (int k = 0; k < outlinePathSP.arraySize; ++k)
                {
                    o[k] = outlinePathSP.GetArrayElementAtIndex(k).vector2Value;
                }
                outline.Add(o);
            }
        }
    }
}