------------------------------------------------------------------------------------
【T4Template（テーブルEntity自動生成ツール）の使い方】

① このフォルダ（Database）の直下にあるファイル「DevSupport.tt」をテキストエディタで開きます。
② 接続文字列や名前空間を適切なものに変更て保存します。
③ コマンドプロンプトを開いて、このフォルダに移動します。
④ ↓のコマンドを貼り付けて実行します。※最後のパラメータは出力ファイル名。[DB名.cs]にしましょう。

	"C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\Common7\IDE\TextTransform.exe" DevSupport.tt -o DevSupport.cs

※テーブルのレイアウトが変更となったら、このコマンドを実行してcsファイルを再生成しましょう。

