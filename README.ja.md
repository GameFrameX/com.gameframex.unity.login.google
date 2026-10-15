<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="Game Frame X Logo" width="160" />

# Game Frame X Google Login

[![License](https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.login.google)](https://github.com/GameFrameX/com.gameframex.unity.login.google/blob/main/LICENSE.md)
[![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.login.google)](https://github.com/GameFrameX/com.gameframex.unity.login.google/releases)
[![Unity Version](https://img.shields.io/badge/Unity-2019.4-black?logo=unity)](https://unity.com/)
[![Documentation](https://img.shields.io/badge/Documentation-docs-blue)](https://gameframex.doc.alianblank.com)

インディゲーム開発者向けオールインワンソリューション · インディ開発者の夢を支援

<br />

[ドキュメント](https://gameframex.doc.alianblank.com) · [クイックスタート](#クイックスタート) · QQグループ: 467608841 / 233840761

<br />

[English](../../README.md) | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | **日本語** | [한국어](README.ko.md)

</div>

## プロジェクト概要

**Google ログインチャネル** - GameFrameX ログインチャネルレジストリの Google チャネルパッケージ。`GoogleLoginManager` として `ILoginManager` を実装し、起動時に `GoogleLoginBootstrap` 経由で `LoginChannelRegistry` へ自己登録します（パッケージを導入するだけで有効）。`GoogleLoginResult`（`LoginResult` のサブクラス、IdToken は統一 `Token` フィールドで保持）を返します。

### 機能概要

- `GoogleLoginManager` として `ILoginManager` の Google チャネルを実装。`GoogleLoginBootstrap` が `BeforeSceneLoad` で `LoginChannelRegistry` へ自己登録（チャネル ID `"Google"`）。パッケージ導入だけで有効で、シーンと entry パッケージの変更は不要
- `GoogleLoginResult : LoginResult` は `FamilyName` / `GivenName` / `PlayerId` を保持。IdToken は統一 `Token` フィールドで運ばれ、`PlayerId` は `OpenId` にも書き込まれます
- チャネル設定はチャネルパッケージが自分で読み取ります：BlankGetChannel 経由で `googleProjectId`（Web クライアント ID である必要あり）
- Google は `LoginComponent` の Android / iOS プラットフォーム既定マネージャー。Editor ではスタブデータを返してフロー検証が可能

## クイックスタート

### インストール

以下のいずれかの方法を選択してください：

1. Edit your Unity project's `Packages/manifest.json` and add the `scopedRegistries` section:
   ```json
   {
     "scopedRegistries": [
       {
         "name": "GameFrameX",
         "url": "https://gameframex.upm.alianblank.uk",
         "scopes": [
           "com.gameframex"
         ]
       }
     ],
     "dependencies": {
       "com.gameframex.unity.login.google": "3.0.0"
     }
   }
   ```

   `scopes` はこのレジストリから解決されるパッケージを制御します。`com.gameframex` で始まる名前のパッケージのみがこのレジストリから取得されます。

2. Add to `manifest.json` dependencies:
   ```json
   {
      "com.gameframex.unity.login.google": "https://github.com/gameframex/com.gameframex.unity.login.google.git"
   }
   ```
3. Use **Package Manager** in Unity with **Git URL**: `https://github.com/gameframex/com.gameframex.unity.login.google.git`
4. Clone the repository into your Unity project's `Packages` directory. It will be loaded automatically.

## 使用例

```csharp
using GameFrameX.Login.Runtime;
using UnityEngine;

public class GoogleLoginDemo : MonoBehaviour
{
    void Start()
    {
        // 本パッケージ導入後、Google は BeforeSceneLoad で LoginChannelRegistry に自動登録されます。
        // LoginComponent ファサード経由で統一的にアクセス（per-channel コンポーネントは廃止）
        var login = GameEntry.GetComponent<LoginComponent>(); // または GameApp.Login

        login.OnLoginSuccess += (sender, e) =>
        {
            var google = e.Result as GoogleLoginResult; // チャネル固有フィールドが必要な場合はダウンキャスト
            Debug.Log($"token={e.Result.Token} openId={e.Result.OpenId} playerId={google?.PlayerId}");
        };

        login.Init();                          // 内部で BlankGetChannel 経由により googleProjectId を読み取り
        login.Login(new LoginParams("Google")); // 明示的ルーティング（空ならプラットフォーム既定チャネル）
    }
}
```

**チャネル設定**：`googleProjectId`（Web クライアント ID である必要あり）はチャネルパッケージが BlankGetChannel 経由で自読します。他のチャネル値と同じ方法で設定します（例：`BlankGetChannel.GetChannelName("googleProjectId")`）。

## アーキテクチャ

### 自己登録セマンティクス

`GoogleLoginBootstrap` は `RuntimeInitializeOnLoadMethod(BeforeSceneLoad)`（`LoginComponent.Awake` より早い段階）で `"Google"` チャネル（ファクトリ `() => new GoogleLoginManager()`）を `LoginChannelRegistry` に登録します。本パッケージが未導入の場合はコードがストリップされ、登録は発生しません。導入 / 未導入のどちらのプロジェクト形態でもコンパイル・実行できます。

### 中核パッケージとの関係

- `com.gameframex.unity.login` のみに依存（`com.gameframex.unity` と BlankGetChannel を含む）。
- Google は `LoginComponent` の Android / iOS プラットフォーム既定マネージャー型（シリアライズされた型名文字列。実装が無い場合は `DefaultLoginManager` へフォールバック）。
- `LoginParams.ChannelId = "Google"` で明示的ルーティング。空ならプラットフォーム既定へ（同一インスタンス、二重化なし）。

### 移行ガイド（2.x -> 3.0.0）

| 旧 | 新 |
|----|----|
| `GoogleLoginComponent` | `LoginComponent`（中核パッケージ。シーン上の旧コンポーネントは削除） |
| `GameApp.GoogleLogin` | `GameApp.Login`（定義 -> `ENABLE_GAME_FRAME_X_LOGIN`） |
| `GoogleLoginSuccess.IdToken` | `GoogleLoginResult.Token`（サーバー検証は `Token` を読み取り） |
| `googleLoginComponent.Login(onSuccess, onFail)` | `login.Login(onSuccess, onFailed)` 一回限りオーバーロードまたは集約イベント |
| コンポーネント `Init()` | `GameApp.Login.Init()`（`googleProjectId` の読み取り方法は不変） |

## プラットフォーム対応

| プラットフォーム | 対応 |
|----------|-----------|
| Android | 対応（ネイティブブリッジ） |
| iOS | 対応（ネイティブブリッジ） |
| Editor | 対応（スタブ） |

## ドキュメントとリソース

- [ドキュメント](https://gameframex.doc.alianblank.com)
- [GitHub リポジトリ](https://github.com/GameFrameX/com.gameframex.unity.login.google)

## コミュニティとサポート

- QQ グループ：[QR コード](https://qm.qq.com/cgi-bin/qm/qr?k=ikT9gA5m2sKwOyNOfYmQvSAPK_c3GmD6)から参加

## 依存関係

| パッケージ | バージョン |
|---------|---------|
| `com.gameframex.unity` | 2.5.4 |
| `com.gameframex.unity.getchannel` | 1.3.2 |
| `com.gameframex.unity.login` | 1.0.0 |

## 変更履歴

変更履歴は [Releases](https://github.com/gameframex/com.gameframex.unity.login.google/releases) を参照してください。

## ライセンス

詳しくは [LICENSE.md](LICENSE.md) をご参照ください。
