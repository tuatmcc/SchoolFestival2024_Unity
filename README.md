# 2024年度農部祭のUnity側レポジトリ

## バージョン: 2022.3.45f1

## その他
個々でテスト用に開発する際は、Assetsフォルダの下にファルダを作って作業してください


#### ML-Agentsにおける対照型シューティングゲーム学習

再生を押せば事前に学習されたモデルに従ってUnityChanが動きます。

#### 注意
基本的に動かすこと第一で実装したのと試行錯誤による代償として内部はスパゲッティです。名前の定義も遵守していません。とりあえずこのリポジトリを拡張して何かをやろうとすることは非推奨です。
私(ru322)もよくわかっていない箇所が多数あります。というか報酬だったりの設定を著しく間違えており、順調に学習が進行することはありません(たぶん)。あんまり学習させてないからよくわかんない。
とりあえず動きます。それだけです。

#### 学習させてい方へ(別に学習させる必要性はないです)
##### 環境構築

次のページに従ってML-Agentsの環境をつくってください(windowsの場合はNvidia製のGPUが必要です)
[ML-Agentsインストールガイド](https://unity-technologies.github.io/ml-agents/Installation/)

なお公式のインストールに従った場合つぎのコマンドの実行に失敗する場合があります。(numpy関連)
```
cd /path/to/ml-agents
python -m pip install ./ml-agents-envs
python -m pip install ./ml-agents
```
その際は次の指示に従ってください(なお実際の行番号とずれが存在します)
```
In ml-agents/setup.py:
python_requires=">=3.8.13,<3.11.0" (line 82)

In ml-agents-envs/setup.py:
"numpy>=1.14.1,<1.24" (line 54)
and
python_requires=">=3.8.13,<3.11.0" (line 63)

and delete "numpy==1.21.2" (line 60)
```
https://github.com/Unity-Technologies/ml-agents/issues/5826　を参照


##### 学習の実行
```
mlagents-learn config/poca/UnityChan3vs3 --run-id=<任意の文字列>
```