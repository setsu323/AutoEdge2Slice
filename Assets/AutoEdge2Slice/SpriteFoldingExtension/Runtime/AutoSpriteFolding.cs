using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.U2D;

namespace AutoEdge2Slice.SpriteFoldingExtension.Runtime
{
    public class AutoSpriteFolding : MonoBehaviour
    {
        
#if UNITY_EDITOR
        private void Reset()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
#endif
        
        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        private static readonly HashSet<Sprite> ModifiedSprites = new HashSet<Sprite>();

        private void LateUpdate()
        {
            var sprite = _spriteRenderer.sprite;
            if (ModifiedSprites.Contains(sprite)) return;
            
            
            //そのままだと、頂点の座標のxy座標がUV座標として利用されてしまうため上書きする
            var texCoord = sprite.GetVertexAttribute<Vector2>(VertexAttribute.TexCoord0);
            var texCoordArray = new NativeArray<Vector2>(texCoord.ToArray(), Allocator.Temp);
            sprite.SetVertexAttribute(VertexAttribute.TexCoord0, texCoordArray);
            
            //頂点座標をpivotにあわせて曲げる処理
            var vertexPosition = sprite.GetVertexAttribute<Vector3>(VertexAttribute.Position);
            var modifiedPosition = CalculateVerticesToBendSprite(vertexPosition);
            sprite.SetVertexAttribute(VertexAttribute.Position,modifiedPosition);

            
            ModifiedSprites.Add(sprite);
        }

        private NativeArray<Vector3> CalculateVerticesToBendSprite(NativeSlice<Vector3> native)
        {
            var retValue = new NativeArray<Vector3>(native.Length,Allocator.Temp);
            for (var i = 0; i < native.Length; i++)
            {
                var objectPos = native[i];
                if (objectPos.y < 0)
                {
                    objectPos.z = objectPos.y;
                    objectPos.y = 0;
                }
                retValue[i] = objectPos;
            }
            return retValue;
        }
    }
}