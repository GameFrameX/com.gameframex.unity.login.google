<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="Game Frame X Logo" width="160" />

# Game Frame X Google 登录

[![License](https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.login.google)](https://github.com/GameFrameX/com.gameframex.unity.login.google/blob/main/LICENSE.md)
[![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.login.google)](https://github.com/GameFrameX/com.gameframex.unity.login.google/releases)
[![Unity Version](https://img.shields.io/badge/Unity-2019.4-black?logo=unity)](https://unity.com/)
[![Documentation](https://img.shields.io/badge/Documentation-docs-blue)](https://gameframex.doc.alianblank.com)

独立游戏前后端一体化解决方案 · 独立游戏开发者的圆梦大使

<br />

[文档](https://gameframex.doc.alianblank.com) · [快速开始](#快速开始) · QQ群: 467608841 / 233840761

<br />

[English](README.md) | **简体中文** | [繁體中文](README.zh-TW.md) | [日本語](README.ja.md) | [한국어](README.ko.md)

</div>

## 项目简介

Game Frame X Google 登录是 GameFrameX 框架的 Google 登录组件，提供初始化、登录和登出功能。

## 快速开始

### 安装

选择以下任一方式：

1. 编辑 Unity 项目的 `Packages/manifest.json`，添加 `scopedRegistries` 部分：
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
       "com.gameframex.unity.login.google": "1.1.0"
     }
   }
   ```

   `scopes` 控制哪些包通过此注册表解析。只有以 `com.gameframex` 开头的包才会从这个注册表获取。

2. 直接在 `manifest.json` 的 `dependencies` 节点下添加以下内容：
   ```json
   {
      "com.gameframex.unity.login.google": "https://github.com/gameframex/com.gameframex.unity.login.google.git"
   }
   ```
3. 在 Unity 的 `Package Manager` 中使用 `Git URL` 的方式添加库，地址为：`https://github.com/gameframex/com.gameframex.unity.login.google.git`
4. 直接下载仓库放置到 Unity 项目的 `Packages` 目录下，会自动加载识别。
## 使用示例

1. 在 `GameEntry` 游戏入口对象上挂载 `GoogleLoginComponent` 组件。
2. 在 `GoogleLoginComponent` 组件上设置 `ProjectId`。
3. 调用方法：

```csharp
// 获取 Google 登录组件
var googleLoginComponent = GameEntry.GetComponent<GoogleLoginComponent>();

// 初始化
googleLoginComponent.Init();

// 登录
googleLoginComponent.Login(
    (googleLoginSuccess) =>
    {
        Debug.Log($"登录成功! {JsonUtility.ToJson(googleLoginSuccess)}");
    },
    (code) =>
    {
        Debug.LogError($"登录失败! {code}");
    });

// 登出
googleLoginComponent.LogOut();
```

## 平台配置

### Android

1. 在项目 `res/values/strings.xml` 文件中添加 `game_services_project_id` 字符串资源：
   ```xml
   <?xml version="1.0" encoding="utf-8"?>
   <resources>
       <string name="game_services_project_id">YOUR_PROJECT_ID</string>
   </resources>
   ```

2. 在 `AndroidManifest.xml` 文件的 `application` 节点下添加 `meta-data`：
   ```xml
   <meta-data
       android:name="com.google.android.gms.games.APP_ID"
       android:value="@string/game_services_project_id"/>
   ```

3. 在 `build.gradle` 文件中添加库引用：
   ```groovy
   implementation 'com.google.android.gms:play-services-games-v2:+'
   implementation 'com.google.android.gms:play-services-auth:19.0.0'
   ```

## 依赖

- `com.gameframex.unity`: GameFrameX 核心框架
- `com.gameframex.unity.getchannel`: 渠道管理

## 文档与资源

- 文档地址: https://gameframex.doc.alianblank.com
- 仓库地址: https://github.com/GameFrameX/com.gameframex.unity.login.google
- 问题反馈: https://github.com/GameFrameX/com.gameframex.unity.login.google/issues


## 社区与支持

- QQ群: 467608841 / 233840761

## 更新日志

查看 [Releases](https://github.com/GameFrameX/gameframex/com.gameframex.unity.login.google/releases) 了解更新日志。
## 开源协议

本项目遵循 MIT 许可证。详细信息请查看 [LICENSE](LICENSE.md) 文件。
