using System.IO;
using System.Linq;
using System.Xml.Linq;
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
            return document.Root?.Name == "ExportedPages";
        }

        internal bool TryGetSprites(string xmlPath,out Sprite[] sprites)
        {
            var spritePath = Path.ChangeExtension(xmlPath, "png");
            sprites = AssetDatabase.LoadAllAssetsAtPath(spritePath).Cast<Sprite>().ToArray();
            return sprites != null && sprites.Length != 0;
        }
    }
}