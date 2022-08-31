using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;

namespace AutoEdge2Slice.Editor
{
    [FilePath("ProjectSettings/NullOutlineGeneratorFactory.asset", FilePathAttribute.Location.ProjectFolder)]
    public class NullOutlineGeneratorFactory : ScriptableSingleton<NullOutlineGeneratorFactory>,IOutlineGeneratorFactory
    {
        [OutlineGeneratorFactoryMethod]
        public static Object GetInstance()
        {
            return instance;
        }
        
        public IOutlineGenerator Create(ISpriteEditorDataProvider spriteEditorDataProvider)
        {
            return new NullOutlineGenerator();
        }

        public void Save()
        {
            Save(true);
        }

        private void OnEnable()
        {
            hideFlags &= ~HideFlags.NotEditable;
        }
    }

    internal class NullOutlineGenerator : IOutlineGenerator
    {
        public void GenerateOutline() { }
    }
    
    
}