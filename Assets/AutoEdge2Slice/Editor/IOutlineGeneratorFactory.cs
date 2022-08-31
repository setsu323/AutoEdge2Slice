using UnityEditor.U2D.Sprites;

namespace AutoEdge2Slice.Editor
{
    public interface IOutlineGeneratorFactory
    {
        public IOutlineGenerator Create(ISpriteEditorDataProvider spriteEditorDataProvider);
        public void Save();
    }
}