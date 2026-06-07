<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="Game Frame X Logo" width="160" />

# Game Frame X Google ログイン

[![License](https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.login.google)](https://github.com/GameFrameX/com.gameframex.unity.login.google/blob/main/LICENSE.md)
[![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.login.google)](https://github.com/GameFrameX/com.gameframex.unity.login.google/releases)
[![Documentation](https://img.shields.io/badge/Documentation-docs-blue)](https://gameframex.doc.alianblank.com)

インディゲーム開発者向けオールインワンソリューション · インディ開発者の夢を支援

<br />

[ドキュメント](https://gameframex.doc.alianblank.com) · [クイックスタート](#クイックスタート) · QQグループ: 467608841 / 233840761

<br />

[English](README.md) | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | **日本語** | [한국어](README.ko.md)

</div>
## プロジェクト概要

Game Frame X Google ログインは、GameFrameX フレームワークの Google ログインコンポーネントで、初期化、ログイン、ログアウト機能を提供します。

## クイックスタート

### インストール

以下のいずれかの方法をお選びください：

1. プロジェクトの `manifest.json` の `dependencies` セクションに以下を追加：
   ```json
   {"com.gameframex.unity.login.google": "https://github.com/AlianBlank/com.gameframex.unity.login.google.git"}
   ```

2. Unity の Package Manager で `Git URL` を使用：
   ```
   https://github.com/AlianBlank/com.gameframex.unity.login.google.git
   ```

3. リポジトリをダウンロードして Unity プロジェクトの `Packages` ディレクトリに配置。自動的にロードされます。

## 使用例

1. `GameEntry` ゲームオブジェクトに `GoogleLoginComponent` コンポーネントをアタッチ。
2. `GoogleLoginComponent` コンポーネントに `ProjectId` を設定。
3. メソッドを呼び出し：

```csharp
// Google ログインコンポーネントの取得
var googleLoginComponent = GameEntry.GetComponent<GoogleLoginComponent>();

// 初期化
googleLoginComponent.Init();

// ログイン
googleLoginComponent.Login(
    (googleLoginSuccess) =>
    {
        Debug.Log($"ログイン成功! {JsonUtility.ToJson(googleLoginSuccess)}");
    },
    (code) =>
    {
        Debug.LogError($"ログイン失敗! {code}");
    });

// ログアウト
googleLoginComponent.LogOut();
```

## プラットフォーム設定

### Android

1. `res/values/strings.xml` に `game_services_project_id` 文字列リソースを追加：
   ```xml
   <?xml version="1.0" encoding="utf-8"?>
   <resources>
       <string name="game_services_project_id">YOUR_PROJECT_ID</string>
   </resources>
   ```

2. `AndroidManifest.xml` の `application` ノードに `meta-data` を追加：
   ```xml
   <meta-data
       android:name="com.google.android.gms.games.APP_ID"
       android:value="@string/game_services_project_id"/>
   ```

3. `build.gradle` にライブラリ参照を追加：
   ```groovy
   implementation 'com.google.android.gms:play-services-games-v2:+'
   implementation 'com.google.android.gms:play-services-auth:19.0.0'
   ```

## 依存関係

- `com.gameframex.unity`: GameFrameX コアフレームワーク
- `com.gameframex.unity.getchannel`: チャネル管理

## ドキュメントとリソース

- ドキュメント: https://gameframex.doc.alianblank.com
- リポジトリ: https://github.com/GameFrameX/com.gameframex.unity.login.google
- Issues: https://github.com/GameFrameX/com.gameframex.unity.login.google/issues

## ライセンス

このプロジェクトは MIT ライセンスの下で公開されています。詳細は [LICENSE](LICENSE.md) ファイルを参照してください。
