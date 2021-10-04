using UnityEditor.U2D.Sprites;
using UnityEngine;

namespace AutoEdge2Slice.Editor
{
    [CreateAssetMenu(fileName = "OutlineGeneratorFactory", menuName = "AutoEdge2Slice/NullOutlineGeneratorFactory", order = 0)]
    public class NullOutlineGeneratorFactory : BaseOutlineGeneratorFactory
    {
        public override IOutlineGenerator Create(ISpriteEditorDataProvider spriteEditorDataProvider)
        {
            return new NullOutlineGenerator();
        }
    }

    internal class NullOutlineGenerator : IOutlineGenerator
    {
        public void GenerateOutline() { }
    }
}