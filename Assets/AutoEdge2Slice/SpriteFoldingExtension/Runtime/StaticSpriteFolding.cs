using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.U2D;

namespace AutoEdge2Slice.SpriteFoldingExtension.Runtime
{
    public class StaticSpriteFolding : MonoBehaviour
    {
#if UNITY_EDITOR
        private void Reset()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
#endif
        
        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        private void Start()
        {
            var sprite = _spriteRenderer.sprite;
            if (SpriteFoldingManager.IsModified(sprite)) return;
            
            ModifyTexCoord(sprite);
            ModifyVertex(sprite);
            SpriteFoldingManager.Add(sprite);
        }

        private void ModifyTexCoord(Sprite sprite)
        {
            var texCoord = sprite.GetVertexAttribute<Vector2>(VertexAttribute.TexCoord0);
            var texCoordArray = new NativeArray<Vector2>(texCoord.ToArray(), Allocator.Temp);
            sprite.SetVertexAttribute<Vector2>(VertexAttribute.TexCoord0, texCoordArray);
        }

        private void ModifyVertex(Sprite sprite)
        {
            var s = sprite.GetVertexAttribute<Vector3>(VertexAttribute.Position);
            var set = ConvertVertex(s);
            sprite.SetVertexAttribute<Vector3>(VertexAttribute.Position,set);
        }

        private NativeArray<Vector3> ConvertVertex(NativeSlice<Vector3> native)
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