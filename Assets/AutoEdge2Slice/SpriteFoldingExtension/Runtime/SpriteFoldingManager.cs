using System.Collections.Generic;
using UnityEngine;

namespace AutoEdge2Slice.SpriteFoldingExtension.Runtime
{
    public class SpriteFoldingManager
    {
        private static readonly HashSet<Sprite> ModifiedHashSets = new HashSet<Sprite>();

        public static bool IsModified(Sprite sprite)
        {
            return ModifiedHashSets.Contains(sprite);
        }

        public static void Add(Sprite sprite)
        {
            ModifiedHashSets.Add(sprite);
        }
    }
}