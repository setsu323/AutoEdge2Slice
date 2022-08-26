using System.Linq;
using UnityEditor;

namespace AutoEdge2Slice.Editor
{
    [CustomEditor(typeof(SpriteSettings))]
    public class SpriteSettingsEditor : UnityEditor.Editor
    {
        //重要なのはSpriteSettingsの内部にデータを作成すること
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            //TODO 子となっているFactoryの中から選んで、このインスペクタ上で値を変更できるようにしたい。
            // if (GUILayout.Button("全部消す"))
            // {
            //     var assetPath = AssetDatabase.GetAssetPath(target);
            //     foreach (var asset in AssetDatabase.LoadAllAssetsAtPath(assetPath))
            //     {
            //         if (AssetDatabase.IsSubAsset(asset))
            //         {
            //             DestroyImmediate(asset, true);
            //         }
            //     }
            //
            //     AssetDatabase.Refresh();
            //     AssetDatabase.SaveAssets();
            // }
        }

        private TypeCache.TypeCollection _typeCollection;
        public void OnEnable()
        {
            if (_typeCollection.Count == 0)
            {
                _typeCollection = TypeCache.GetTypesDerivedFrom<BaseOutlineGeneratorFactory>();
            }
            
            //AssetをSpriteSettingsの直下に配置します。
            var path = AssetDatabase.GetAssetPath(target);
            var assets = AssetDatabase.LoadAllAssetsAtPath(path).Where(x => AssetDatabase.IsSubAsset(x))
                .Where(x => x.GetType().IsSubclassOf(typeof(BaseOutlineGeneratorFactory)));

            var isChanged = false;
            foreach (var factoryType in _typeCollection)
            {
                if (!assets.Any(x => x.GetType() == factoryType))
                {
                    //存在しなければアセットを生成する。
                    var instance = CreateInstance(factoryType);
                    instance.name = factoryType.Name;
                    AssetDatabase.AddObjectToAsset(instance, path);
                    isChanged = true;
                };
            }

            if (isChanged)
            {
                AssetDatabase.ImportAsset(path);
            }
        }
    }
}