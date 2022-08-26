using UnityEditor.U2D.Sprites;
using UnityEngine;

namespace AutoEdge2Slice.Editor
{
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