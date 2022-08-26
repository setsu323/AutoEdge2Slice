
using UnityEditor.U2D.Sprites;
using UnityEngine;

namespace AutoEdge2Slice.Editor
{
    public abstract class BaseOutlineGeneratorFactory : ScriptableObject
    {
         public abstract IOutlineGenerator Create(ISpriteEditorDataProvider spriteEditorDataProvider);
    }
}