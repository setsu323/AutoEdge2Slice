using System.Collections.Generic;
using UnityEngine;

namespace SpriteFoldingExtension.Runtime
{
    public class SpriteFoldingManager
    {
        private HashSet<Sprite> _modifiedHashSets = new HashSet<Sprite>();

        public bool IsModified(Sprite sprite)
        {
            return _modifiedHashSets.Contains(sprite);
        }
    }
}