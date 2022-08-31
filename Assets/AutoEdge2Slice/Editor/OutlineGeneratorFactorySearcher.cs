using UnityEditor;

namespace AutoEdge2Slice.Editor
{
    public class OutlineGeneratorFactorySearcher
    {
        public static void Search()
        {
            var outlineTypes = TypeCache.GetTypesDerivedFrom<IOutlineGeneratorFactory>();
            //outlineTypesの中から検索したい
            //outlineTypesの中からインスタンスを検索したいけど……
            
            //outlineTypesを継承したScriptableSingletonを
            //Attributeを使うのはどうだろう
            //Instanceを作成するメソッドに対してAttributeを付ける。
            //そうするとFactoryクラスだと断定するみたいな。
        }
    }
}