# QuickMenuEx
VRCのクイックメニューと連動して拡張メニューを表示するためのアセットです。<br>
このアセットには拡張メニューは機能としては含まれていません。クイックメニューと連動してちょうどいい位置にUIのCanvasが表示されるのみです。<br>
ワールドに設置する場合はそのCanvas内にいい感じにメニューを作って利用してください。

## 注意
本アセットはU#1.x(1.1.6)で書かれています。旧バージョン0.20.3では動作しません。<br>
VCC (VRChat Creator Companion)を使用して、Migration(移行)、またはUdonsharpテンプレートでワールドを作成してください。<br>
また、このギミックはVRCのアップデートで利用できなくなる可能性があります。

## パッケージのダウンロード
以下から最新バージョンのunitypackageをダウンロードしてください。<br>
https://github.com/irucaVRC/QuickMenuEx/releases

## 導入方法
1. VCCのプロジェクトを用意
2. 本アセットをインポート
3. Asset内の`iruca/MenuExtension`にあるPrefabをシーンに配置する

## 拡張メニューを作成する際の注意事項
プレハブ内の`ExtensionMenu/Menu/Canvas`内にメニューを作成してください。<br>
その中に配置した要素にマテリアルを指定する場所があれば、それらすべてにAsset内の`iruca/MenuExtension/OverlayMat`を入れてください。
