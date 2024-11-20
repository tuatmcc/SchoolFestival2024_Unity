# 2024年度農部祭のUnity側レポジトリ

## 開発環境
 - OS: Windows, Mac, Linux
 - Unity Editor 2022.3.45f1
 - JetBrains Rider or VisualStudio

※ Intel or AMD の Linux 版 Unity は OpenGL 4.2 で起動することを推奨

## 動作方法

ゲーム起動直後にサーバーモードとクライアントモードを選択できる。クライアントのみではプレイできない。

複数デバイスでマルチプレイする場合の注意
- 同一LANに接続する
- サーバーにするマシンの7777番ポートが解放されていること
- 通信量が多いため、有線接続が望ましい

## 技術スタック

- マルチプレイ: NetCode for GameObject
- DI: Zenject(Extenject)
- データベース: Supabase (supabase-csharp)
- 非同期処理: UniTask
- Rx: R3

## 使用したアセット

- BGM
  - 
- SE
  - [効果音ラボ](https://soundeffect-lab.info/)
- フォント
  - [Dela Gothic One](https://fonts.google.com/specimen/Dela+Gothic+One?query=dela+gothic)

## その他

個々でテスト用に開発する際は、Assetsフォルダの下にファルダを作って作業してください
