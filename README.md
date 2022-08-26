# AutoEdge2Slice
Edge2のページを並べて書き出す際に、
Spriteの設定を自動化するツールです。

## Requirement
- Edge2 1.16 dev020 or higher  
- Unity 2020.3 or higher


## Git Path(Unity Package Manger)
https://github.com/setsu323/AutoEdge2Slice.git?path=Assets/AutoEdge2Slice


## Usage
Edge2において
ページウィンドウから
[ファイル]-[ページを一枚に並べて書き出す]を選びます。<br>

この時[ページ情報をXMLで出力する]にチェックを入れます。<br>

Unityのプロジェクト内のフォルダを選び保存すると、
分割されたSpriteAsset、AnimationClipが生成されます。

また、AutoEdge2Slice/Editor/SpriteSettingsの値を変更することで、
インポート時の設定を一部変更できます。
