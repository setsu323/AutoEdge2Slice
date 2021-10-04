using AutoEdge2Slice.Editor;
using UnityEditor.U2D.Sprites;
using UnityEngine;

namespace SpriteFoldingExtension
{
    [CreateAssetMenu(fileName = "OutlineGeneratorFactory", menuName = "AutoEdge2Slice/VerticalSplitOutlineGeneratorFactory", order = 0)]
    public class VerticalSplitOutlineGeneratorFactory : BaseOutlineGeneratorFactory
    {
        [SerializeField] private float _detail;
        public override IOutlineGenerator Create(ISpriteEditorDataProvider spriteEditorDataProvider)
        {
            return new VerticalSplitOutlineGenerator(spriteEditorDataProvider, _detail);
        }
    }
}