<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="Game Frame X Logo" width="160" />

# Game Frame X Google 登錄

[![License](https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.login.google)](https://github.com/GameFrameX/com.gameframex.unity.login.google/blob/main/LICENSE.md)
[![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.login.google)](https://github.com/GameFrameX/com.gameframex.unity.login.google/releases)
[![Unity Version](https://img.shields.io/badge/Unity-2019.4-black?logo=unity)](https://unity.com/)
[![Documentation](https://img.shields.io/badge/Documentation-docs-blue)](https://gameframex.doc.alianblank.com)

獨立遊戲前後端一體化解決方案 · 獨立遊戲開發者的圓夢大使

<br />

[文檔](https://gameframex.doc.alianblank.com) · [快速開始](#快速開始) · QQ群: 467608841 / 233840761

<br />

[English](README.md) | [简体中文](README.zh-CN.md) | **繁體中文** | [日本語](README.ja.md) | [한국어](README.ko.md)

</div>

## 項目簡介

Game Frame X Google 登錄是 GameFrameX 框架的 Google 登錄組件，提供初始化、登錄和登出功能。

## 快速開始

### 安裝

任選以下方式之一：

1. 直接在 `manifest.json` 的文件中的 `dependencies` 節點下添加以下內容：
   ```json
   {"com.gameframex.unity.login.google": "https://github.com/AlianBlank/com.gameframex.unity.login.google.git"}
   ```

2. 在 Unity 的 `Packages Manager` 中使用 `Git URL` 的方式添加庫，地址為：
   ```
   https://github.com/AlianBlank/com.gameframex.unity.login.google.git
   ```

3. 直接下載倉庫放置到 Unity 項目的 `Packages` 目錄下，會自動加載識別。

## 使用範例

1. 在 `GameEntry` 遊戲入口對象上掛載 `GoogleLoginComponent` 組件。
2. 在 `GoogleLoginComponent` 組件上設置 `ProjectId`。
3. 調用方法：

```csharp
// 獲取 Google 登錄組件
var googleLoginComponent = GameEntry.GetComponent<GoogleLoginComponent>();

// 初始化
googleLoginComponent.Init();

// 登錄
googleLoginComponent.Login(
    (googleLoginSuccess) =>
    {
        Debug.Log($"登錄成功! {JsonUtility.ToJson(googleLoginSuccess)}");
    },
    (code) =>
    {
        Debug.LogError($"登錄失敗! {code}");
    });

// 登出
googleLoginComponent.LogOut();
```

## 平台配置

### Android

1. 在項目 `res/values/strings.xml` 文件中添加 `game_services_project_id` 字符串資源：
   ```xml
   <?xml version="1.0" encoding="utf-8"?>
   <resources>
       <string name="game_services_project_id">YOUR_PROJECT_ID</string>
   </resources>
   ```

2. 在 `AndroidManifest.xml` 文件的 `application` 節點下添加 `meta-data`：
   ```xml
   <meta-data
       android:name="com.google.android.gms.games.APP_ID"
       android:value="@string/game_services_project_id"/>
   ```

3. 在 `build.gradle` 文件中添加庫引用：
   ```groovy
   implementation 'com.google.android.gms:play-services-games-v2:+'
   implementation 'com.google.android.gms:play-services-auth:19.0.0'
   ```

## 依賴項

- `com.gameframex.unity`: GameFrameX 核心框架
- `com.gameframex.unity.getchannel`: 渠道管理

## 文檔與資源

- 文檔地址: https://gameframex.doc.alianblank.com
- 倉庫地址: https://github.com/GameFrameX/com.gameframex.unity.login.google
- 問題反饋: https://github.com/GameFrameX/com.gameframex.unity.login.google/issues

## 開源協議

本項目遵循 MIT 許可證。詳細信息請查看 [LICENSE](LICENSE.md) 文件。
