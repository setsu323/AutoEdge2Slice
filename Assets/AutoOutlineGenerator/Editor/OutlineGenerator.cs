using UnityEngine;

namespace AutoOutlineGenerator.Editor
{
    /// <summary>
    /// Spriteの情報からOutlineを生成するGenerator
    /// </summary>
    public class OutlineGenerator
    {
        private Sprite _sprite;

        //まず、そもそもSpriteを読み込みたい……
        
        public void GenerateOutline()
        {
            Texture2D texture = _sprite.texture;
            Rect rect = _sprite.rect;
            //pivotの位置を取得
            //pivotの位置を確認しつつ、Outlineを生成する
            //Outlineは
            //textureを取得することで、何とか出来ないかなぁ……
            //pivotよりも上の領域内で
        }
        
    }
}