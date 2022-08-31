using AutoEdge2Slice.Editor;
using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;

namespace SpriteFoldingExtension
{
    [FilePath("ProjectSettings/VerticalSplitOutlineGeneratorFactory.asset", FilePathAttribute.Location.ProjectFolder)]
    public class VerticalSplitOutlineGeneratorFactory : ScriptableSingleton<VerticalSplitOutlineGeneratorFactory>,IOutlineGeneratorFactory
    {
        [OutlineGeneratorFactoryMethod]
        public static Object GetInstance()
        {
            return instance;
        }
        
        [SerializeField] private float _detail;
        public IOutlineGenerator Create(ISpriteEditorDataProvider spriteEditorDataProvider)
        {
            return new VerticalSplitOutlineGenerator(spriteEditorDataProvider, _detail);
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
}