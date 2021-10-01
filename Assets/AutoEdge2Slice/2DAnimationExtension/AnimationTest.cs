using System.Collections;
using System.Collections.Generic;
using System.Text;
using Codice.Client.Common;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Plastic.Newtonsoft.Json;
using UnityEditor.AssetImporters;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.U2D;

namespace DefaultNamespace
{
    public class AnimationTest : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        private static readonly HashSet<Sprite> ModifiedSprites = new HashSet<Sprite>();
        private void Update()
        {
            var sprite = _spriteRenderer.sprite;

            if (ModifiedSprites.Contains(sprite)) return;
            ModifiedSprites.Add(sprite);

            var texCoord = sprite.GetVertexAttribute<Vector2>(VertexAttribute.TexCoord0);
            var texCoordArray = new NativeArray<Vector2>(texCoord.ToArray(), Allocator.Temp);
            sprite.SetVertexAttribute<Vector2>(VertexAttribute.TexCoord0, texCoordArray);
            
            var s = sprite.GetVertexAttribute<Vector3>(UnityEngine.Rendering.VertexAttribute.Position);
            var builder = new StringBuilder();
            for (var i = 0; i < s.Length; i++)
            {
                builder.Append(s[i].ToString());
            }
            
            var set = ConvertVertex(s);
            sprite.SetVertexAttribute<Vector3>(UnityEngine.Rendering.VertexAttribute.Position,set);

            Debug.Log(builder);
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