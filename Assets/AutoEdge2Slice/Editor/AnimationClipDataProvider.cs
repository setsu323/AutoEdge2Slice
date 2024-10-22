using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using AutoEdge2Slice.Editor.Main;
using UnityEditor;
using UnityEngine;

namespace AutoEdge2Slice.Editor
{
    /// <summary>
    /// Pathからアニメーションクリップを作成するのに必要な情報を取得するためのクラス
    /// </summary>
    public class AnimationClipDataProvider
    {
        public bool TryGetPageDocument(string xmlPath,out XDocument document)
        {
            //xmlPathから取得する
            var textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(xmlPath);
            document = XDocument.Parse(textAsset.text);
            return document.Root?.Name == EdgePageDataPostProcessor.ExportedMark;
        }

        internal bool TryGetSprites(string xmlPath,out Sprite[] sprites)
        {
            var spritePath = Path.ChangeExtension(xmlPath, "png");
            //spritePathからSpriteの名前を取得する
            var parentSpriteName = Path.GetFileNameWithoutExtension(spritePath);
            sprites = AssetDatabase.LoadAllAssetsAtPath(spritePath).OfType<Sprite>().ToArray();

            var regex = new Regex(parentSpriteName + "_" +"([0-9]+)", RegexOptions.Compiled);
            sprites = sprites.Where(x => regex.IsMatch(x.name))
                .OrderBy(s => int.Parse(regex.Match(s.name).Groups[1].Value)).ToArray();
            
            //Spriteの順番をちゃんとする必要がある。
            //名前でのソートが必要。
            return sprites != null && sprites.Length != 0;
        }
    }
}