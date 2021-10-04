using AutoOutlineGenerator.SpriteExtension;
using UnityEditor.U2D.Sprites;

namespace AutoEdge2Slice.Editor
{
    internal class OutlineGenerator : IOutlineGenerator
    {
        //outline
        //まず、そもそもSpriteを読み込みたい……
        private readonly ISpriteEditorDataProvider _spriteEditorDataProvider;
        private readonly ISpriteOutlineDataProvider _spriteOutlineDataProvider;
        private readonly ITextureDataProvider _textureDataProvider;
        private readonly float _detail;

        public OutlineGenerator(ISpriteEditorDataProvider spriteEditorDataProvider, float detail)
        {
            _spriteEditorDataProvider = spriteEditorDataProvider;
            _detail = detail;
            _spriteOutlineDataProvider = spriteEditorDataProvider.GetDataProvider<ISpriteOutlineDataProvider>();
            _textureDataProvider = spriteEditorDataProvider.GetDataProvider<ITextureDataProvider>();
        }

        public void GenerateOutline()
        {
            var spriteRects = _spriteEditorDataProvider.GetSpriteRects();
            foreach (var spriteRect in spriteRects)
            {
                var outlines =
                    ShapeEditorExtension.GenerateOutline(spriteRect, _detail, 0, _textureDataProvider);
                _spriteOutlineDataProvider.SetOutlines(spriteRect.spriteID, outlines);
            }
            _spriteEditorDataProvider.Apply();
        }
    }
}