using AutoOutlineGenerator.SpriteExtension;
using UnityEditor.U2D.Sprites;
using UnityEngine;

namespace AutoOutlineGenerator.Editor
{
    /// <summary>
    /// Spriteの情報からOutlineを生成するGenerator
    /// </summary>
    public class OutlineGenerator
    {
        //まず、そもそもSpriteを読み込みたい……
        private ISpriteEditorDataProvider _spriteEditorDataProvider;
        private ISpriteOutlineDataProvider _spriteOutlineDataProvider;
        private ITextureDataProvider _textureDataProvider;

        public void Set(ISpriteEditorDataProvider spriteEditorDataProvider)
        {
            _spriteEditorDataProvider = spriteEditorDataProvider;
            _spriteOutlineDataProvider = spriteEditorDataProvider.GetDataProvider<ISpriteOutlineDataProvider>();
            _textureDataProvider = spriteEditorDataProvider.GetDataProvider<ITextureDataProvider>();
        }

        public void GenerateOutline()
        {
            var spriteRects = _spriteEditorDataProvider.GetSpriteRects();
            foreach (var spriteRect in spriteRects)
            {
                var outlines = ShapeEditorExtension.GenerateSplitRectOutline(spriteRect,0,1,_textureDataProvider);
                _spriteOutlineDataProvider.SetOutlines(spriteRect.spriteID, outlines);
            }
            _spriteEditorDataProvider.Apply();
        }
        
    }
}