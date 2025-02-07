# 2024年度農部祭のUnity側レポジトリ

## 開発環境
 - OS: Windows, Mac, Linux
 - Unity Editor 2022.3.45f1
 - JetBrains Rider or VisualStudio

※ Intel or AMD の Linux 版 Unity は OpenGL 4.2 で起動することを推奨

## 動作方法

`release` ブランチをビルドする。ゲーム起動直後にサーバーモードとクライアントモードを選択できる。クライアントのみではプレイできない。

**複数デバイスでマルチプレイする場合の注意**
- 同一LANに接続すること
- サーバーにするマシンの7777番ポートが解放されていること
- 通信量が多いため、有線接続が望ましい

## 主な技術スタック

- マルチプレイ: [NetCode for GameObjects](https://docs-multiplayer.unity3d.com/netcode/current/about/)
- DI: [Zenject(Extenject)](https://github.com/Mathijs-Bakker/Extenject)
- データベース: [Supabase](https://supabase.com/) ([supabase-csharp](https://github.com/supabase-community/supabase-csharp))
- 非同期処理: [UniTask](https://github.com/Cysharp/UniTask)
- Rx: [R3](https://github.com/Cysharp/R3)
- 
## 使用したアセット等

- BGM
- SE: [効果音ラボ](https://soundeffect-lab.info/)
- フォント
  - [Dela Gothic One](https://fonts.google.com/specimen/Dela+Gothic+One?query=dela+gothic)
  - [AB-countryroad](https://fonts.adobe.com/fonts/ab-countryroad#about-section)
- シェーダー: [lilToon](https://lilxyzw.github.io/lilToon/)
- フィールド, 背景オブジェクト, キャラモデル, モーション, Skybox, エフェクトなど
  - 開発メンバーによるお手製(Blender, Illustrator, iBisPaint, etc...)

## その他

個々でテスト用に開発する際は、Assetsフォルダの下にファルダを作って作業してください
