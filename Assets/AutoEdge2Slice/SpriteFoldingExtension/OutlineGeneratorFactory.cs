using AutoEdge2Slice.Editor;
using UnityEditor.U2D.Sprites;
using UnityEngine;

namespace SpriteFoldingExtension
{
    public class OutlineGeneratorFactory : BaseOutlineGeneratorFactory
    {
        [SerializeField] private float _detail;
        public override IOutlineGenerator Create(ISpriteEditorDataProvider spriteEditorDataProvider)
        {
            return new OutlineGenerator(spriteEditorDataProvider, _detail);
        }
    }
}