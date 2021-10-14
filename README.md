# AutoEdge2Slice
Edge2のページを並べて書き出す際に、
Spriteの設定を自動化するツールです。

Edge2のVersion 1.16 dev020以降に対応しています。

## 使用方法
### Spriteへの変換
Edge2において
ページウィンドウから
[ファイル]-[ページを一枚に並べて書き出す]を選びます。<br>

この時[ページ情報をXMLで出力する]にチェックを入れます。<br>

Unityのプロジェクト内のフォルダを選び保存すると、
分割されたSpriteAssetが生成されます。

また、AutoEdge2Slice/Editor/SpriteSettingsの値を変更することで、
インポート時の設定を一部変更できます。

### AnimationClipの生成
UnityにおいてTools/AnimationGeneratorからウィンドウを開きます。<br>
Edge2XMLファイルをフィールドにセットするとSpriteとButtonが表示されます。<br>
AnimationClipを生成するButtonを押すと、AnimationClipが生成されます。<br>
既に同名のAnimationClipがある場合は、<br>
上書きするか、別ファイルとして保存するか、<br>
を選ぶことが出来ます。<br>
