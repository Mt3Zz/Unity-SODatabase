# Unity-SODatabase

[![license](https://img.shields.io/badge/LICENSE-MIT-green.svg)](LICENSE.md)
[English version here](README.md) | [日本語版はこちら](README_JA.md)

## 概要
SODatabaseはScriptableObjectを管理するためのUnityライブラリです。
以下の特徴があります。
- 最小限のデータベースシステム
- GUIで完結するCRUD操作
- 拡張性

## セットアップ
**要件**
- Unity 2022.3以上

**インストール**
1. Package Managerを開きます。
2. 「+」ボタンをクリックし、「Add package from git URL」を選択します。
3. 以下のURLを入力してインストールします：
`https://github.com/Mt3Zz/Unity-SODatabase.git?path=/Assets/SODatabase`

## 使い方
1. Window > SODatabase Editorを開きます。
2. 「Preference」ボタンをクリックしてパッケージ設定を開きます。
3. 「Create New Storage」ボタンをクリックし、「Managed Storages」に新しいストレージを作成します。
4. SODatabase Editorウィンドウに戻ります。
5. サイドバーに作成したストレージが表示されます。「Append Record」から新しいオブジェクトを作成します。

## 拡張
1. 任意のフォルダーに新しいアセンブリ定義ファイルを作成します。
2. 作成したファイルのアセンブリ定義に「SODatabase」をアタッチします。ただし、SODatabase.EditorやSODatabase.Editor.Tests、SODatabase.Tests.PlayModeはアタッチしないでください。
3. フォルダーにC#コードを作成します。以下のサンプルコードを参照してください。
4. 定義したオブジェクトがウィンドウに追加されます。「Append Record」から定義したオブジェクトを作成して使用してください。

```csharp
// SampleObject.cs

using UnityEngine;

// SODatabase.DataObjectを追加してください
using SODatabase.DataObject;

// BaseObjectを継承してください
public sealed class SampleObject : BaseObject
{
    [SerializeField]
    private int _sampleIntValue;
    [SerializeField]
    private string _sampleStringValue;
}
```
