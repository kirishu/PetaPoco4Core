﻿------------------------------------------------------------------------------------
【T4Template（テーブルEntity自動生成ツール）の使い方】

① ここにあるファイル「Database.tt」をテキストエディタで開きます。
② 接続文字列や名前空間を適切なものに変更て保存します。
③ コマンドプロンプトを開いて、このフォルダに移動します。
④ ↓のコマンドを貼り付けて実行します。※最後のパラメータは出力ファイル名。[DB名.cs]にしましょう。

	"C:\Program Files (x86)\Microsoft Visual Studio\2017\Professional\Common7\IDE\TextTransform.exe" Database.tt -o Database.cs

	"C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\Common7\IDE\TextTransform.exe" Database.tt -o Database.cs

※テーブルのレイアウトが変更となったら、このコマンドを実行してcsファイルを再生成しましょう。

------------------------------------------------------------------------------------
【条件】
	必要なDBクライアントドライバがGACにインストールされていないとダメです。
	
	動作確認が取れているドライバとバージョン
		MySQL: MySQL Connector NET 6.9.9
		PostgreSQL: Npgsql 4.0.3 https://github.com/npgsql/npgsql/releases/tag/v4.0.3   (4.1.3はダメでした。どうもドライバがSystem.Runtime.CompilerServices.Unsafeを必要とするみたい。)

------------------------------------------------------------------------------------
2020/02/03 機能追加
	PostgreSQLを複数スキーマに対応しました。
	
	ログインアカウントが所有するスキーマ内のテーブルすべてを出力対象とします。
	デフォルトスキーマのテーブルには、スキーマ名のPrefixはつけません。
	
	【注意】
	デフォルトスキーマ以外のテーブルにクラス名にはスキーマ名が付与されます。(スキーマ名.クラス名)
	
2020/02/07 機能削除
	Recordオブジェクト自身に持たせるオペレーション群の生成は廃止した。
	（個人的に全く使っていないため）
