<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="Game Frame X Logo" width="160" />

# Game Frame X Google Login

[![License](https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.login.google)](https://github.com/GameFrameX/com.gameframex.unity.login.google/blob/main/LICENSE.md)
[![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.login.google)](https://github.com/GameFrameX/com.gameframex.unity.login.google/releases)
[![Unity Version](https://img.shields.io/badge/Unity-2019.4-black?logo=unity)](https://unity.com/)
[![Documentation](https://img.shields.io/badge/Documentation-docs-blue)](https://gameframex.doc.alianblank.com)

獨立遊戲前後端一體化解決方案 · 獨立遊戲開發者的圓夢大使

<br />

[文檔](https://gameframex.doc.alianblank.com) · [快速開始](#快速開始) · QQ群: 467608841 / 233840761

<br />

[English](../../README.md) | [简体中文](README.zh-CN.md) | **繁體中文** | [日本語](README.ja.md) | [한국어](README.ko.md)

</div>

## 項目簡介

**Google 登入渠道** - GameFrameX 登入渠道註冊表的 Google 渠道包。以 `GoogleLoginManager` 實現 `ILoginManager`，啟動時經 `GoogleLoginBootstrap` 自註冊進 `LoginChannelRegistry`（裝包即生效），返回 `GoogleLoginResult`（`LoginResult` 子類；IdToken 由統一 `Token` 欄位承載）。

### 功能特性

- 以 `GoogleLoginManager` 實現 `ILoginManager` Google 渠道；經 `GoogleLoginBootstrap` 在 `BeforeSceneLoad` 自註冊進 `LoginChannelRegistry`（渠道標識 `"Google"`）——裝包即生效，場景與 entry 包零改動
- `GoogleLoginResult : LoginResult` 保留 `FamilyName` / `GivenName` / `PlayerId`；IdToken 由統一 `Token` 欄位承載，`PlayerId` 同時寫入 `OpenId`
- 渠道配置由渠道包自讀：經 BlankGetChannel 讀取 `googleProjectId`（必須是 Web 端 ClientId）
- Google 是 `LoginComponent` 的 Android / iOS 平台預設管理器；Editor 下返回樁資料供流程驗證

## 快速開始

### 安裝

選擇以下任意一種安裝方式：

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

   `scopes` 控制哪些包會通過該註冊表解析。只有名稱以 `com.gameframex` 開頭的包才會從該註冊表獲取。

2. Add to `manifest.json` dependencies:
   ```json
   {
      "com.gameframex.unity.login.google": "https://github.com/gameframex/com.gameframex.unity.login.google.git"
   }
   ```
3. Use **Package Manager** in Unity with **Git URL**: `https://github.com/gameframex/com.gameframex.unity.login.google.git`
4. Clone the repository into your Unity project's `Packages` directory. It will be loaded automatically.

## 使用範例

```csharp
using GameFrameX.Login.Runtime;
using UnityEngine;

public class GoogleLoginDemo : MonoBehaviour
{
    void Start()
    {
        // 安裝本包後 Google 在 BeforeSceneLoad 自動註冊進 LoginChannelRegistry；
        // 統一經 LoginComponent 門面存取（不再有 per-channel 組件）
        var login = GameEntry.GetComponent<LoginComponent>(); // 或 GameApp.Login

        login.OnLoginSuccess += (sender, e) =>
        {
            var google = e.Result as GoogleLoginResult; // 需要渠道特有欄位時向下轉型
            Debug.Log($"token={e.Result.Token} openId={e.Result.OpenId} playerId={google?.PlayerId}");
        };

        login.Init();                          // 內部經 BlankGetChannel 讀取 googleProjectId
        login.Login(new LoginParams("Google")); // 顯式路由（留空則走平台預設渠道）
    }
}
```

**渠道配置**：`googleProjectId`（必須是 Web 端 ClientId）由渠道包經 BlankGetChannel 自讀——與其他渠道值的配置方式一致（如 `BlankGetChannel.GetChannelName("googleProjectId")`）。

## 架構概覽

### 自註冊語義

`GoogleLoginBootstrap` 在 `RuntimeInitializeOnLoadMethod(BeforeSceneLoad)`（早於 `LoginComponent.Awake`）將 `"Google"` 渠道（工廠 `() => new GoogleLoginManager()`）註冊進 `LoginChannelRegistry`。未安裝本包時代碼被裁剪、註冊不會發生；裝包 / 不裝包兩種工程形態均可編譯執行。

### 與核心包的關係

- 只依賴 `com.gameframex.unity.login`（外加 `com.gameframex.unity` 與 BlankGetChannel）。
- Google 是 `LoginComponent` 上 Android / iOS 平台預設管理器類型（序列化類型名字串；實現缺失時回落 `DefaultLoginManager`）。
- `LoginParams.ChannelId = "Google"` 顯式路由；為空回落平台預設（同一實例，無雙實例）。

### 遷移說明（2.x -> 3.0.0）

| 舊形態 | 新形態 |
|--------|--------|
| `GoogleLoginComponent` | `LoginComponent`（核心包；刪除場景上的舊組件） |
| `GameApp.GoogleLogin` | `GameApp.Login`（宏 → `ENABLE_GAME_FRAME_X_LOGIN`） |
| `GoogleLoginSuccess.IdToken` | `GoogleLoginResult.Token`（伺服器校驗方改讀 `Token`） |
| `googleLoginComponent.Login(onSuccess, onFail)` | `login.Login(onSuccess, onFailed)` 一次性重載或聚合事件 |
| 組件 `Init()` | `GameApp.Login.Init()`（`googleProjectId` 讀取方式不變） |

## 平台支援

| 平台 | 支援 |
|------|------|
| Android | 支援（native 橋） |
| iOS | 支援（native 橋） |
| Editor | 支援（樁資料） |

## 文檔與資源

- [文檔](https://gameframex.doc.alianblank.com)
- [GitHub 倉庫](https://github.com/GameFrameX/com.gameframex.unity.login.google)

## 社區與支援

- QQ 群：透過[二維碼](https://qm.qq.com/cgi-bin/qm/qr?k=ikT9gA5m2sKwOyNOfYmQvSAPK_c3GmD6)加入

## 依賴

| 包 | 版本 |
|-----|------|
| `com.gameframex.unity` | 2.5.4 |
| `com.gameframex.unity.getchannel` | 1.3.2 |
| `com.gameframex.unity.login` | 1.0.0 |

## 更新日誌

詳見 [Releases](https://github.com/gameframex/com.gameframex.unity.login.google/releases) 更新日誌。

## 開源協議

詳見 [LICENSE.md](LICENSE.md) 檔案。
