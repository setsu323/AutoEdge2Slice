using AutoEdge2Slice.Editor;
using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SpriteFoldingExtension
{
    [FilePath("ProjectSettings/OutlineGeneratorFactory.asset", FilePathAttribute.Location.ProjectFolder)]
    public class OutlineGeneratorFactory : ScriptableSingleton<OutlineGeneratorFactory>,IOutlineGeneratorFactory
    {
        [OutlineGeneratorFactoryMethod]
        public static Object GetInstance()
        {
            return instance;
        }
        
        [SerializeField] private float _detail;
        public IOutlineGenerator Create(ISpriteEditorDataProvider spriteEditorDataProvider)
        {
            return new OutlineGenerator(spriteEditorDataProvider, _detail);
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