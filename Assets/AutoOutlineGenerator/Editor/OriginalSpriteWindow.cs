using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;

namespace AutoOutlineGenerator.Editor
{
    public class OriginalSpriteWindow
    {
        private ISpriteOutlineDataProvider _spriteOutlineDataProvider;
        private ISpriteEditorDataProvider _spriteEditorDataProvider;
        
        private SpriteDataProviderFactories _spriteDataProviderFactories;

        //private SpriteDataProviderBase _spriteDataProviderBase;
        //private SpriteOutlineDataTransfer _spriteOutlineDataTransfer;
        
        void OnEnable()
        {
            _spriteDataProviderFactories.Init();
            _spriteDataProviderFactories.GetSpriteEditorDataProviderFromObject(null);
            //ISpriteEditorDataProviderがあれば、ISpriteOutlineDataProvider系は実装可能……
            //問題は……
            //問題はSpriteDataProviderFactoryの依存性の注入はどうするか……
            ISpriteDataProviderFactory<Texture2D> spriteDataProviderFactory;
            
            
            _spriteEditorDataProvider = spriteDataProviderFactory.CreateDataProvider(null);
            _spriteOutlineDataProvider = _spriteEditorDataProvider.GetDataProvider<ISpriteOutlineDataProvider>();
            //おそらくはこれでいける……
            var spriteRects = _spriteEditorDataProvider.GetSpriteRects();
            _spriteOutlineDataProvider.GetOutlines(spriteRects[0].spriteID);
            
            //問題はインターフェースにどうやって依存性を注入するか……


        }
    }
}