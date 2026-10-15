<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="Game Frame X Logo" width="160" />

# Game Frame X Google Login

[![License](https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.login.google)](https://github.com/GameFrameX/com.gameframex.unity.login.google/blob/main/LICENSE.md)
[![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.login.google)](https://github.com/GameFrameX/com.gameframex.unity.login.google/releases)
[![Unity Version](https://img.shields.io/badge/Unity-2019.4-black?logo=unity)](https://unity.com/)
[![Documentation](https://img.shields.io/badge/Documentation-docs-blue)](https://gameframex.doc.alianblank.com)

独立游戏前后端一体化解决方案 · 独立游戏开发者的圆梦大使

<br />

[文档](https://gameframex.doc.alianblank.com) · [快速开始](#快速开始) · QQ群: 467608841 / 233840761

<br />

[English](../../README.md) | **简体中文** | [繁體中文](README.zh-TW.md) | [日本語](README.ja.md) | [한국어](README.ko.md)

</div>

## 项目简介

**Google 登录渠道** - GameFrameX 登录渠道注册表的 Google 渠道包。以 `GoogleLoginManager` 实现 `ILoginManager`，启动时经 `GoogleLoginBootstrap` 自注册进 `LoginChannelRegistry`（装包即生效），返回 `GoogleLoginResult`（`LoginResult` 子类；IdToken 由统一 `Token` 字段承载）。

### 功能特性

- 以 `GoogleLoginManager` 实现 `ILoginManager` Google 渠道；经 `GoogleLoginBootstrap` 在 `BeforeSceneLoad` 自注册进 `LoginChannelRegistry`（渠道标识 `"Google"`）——装包即生效，场景与 entry 包零改动
- `GoogleLoginResult : LoginResult` 保留 `FamilyName` / `GivenName` / `PlayerId`；IdToken 由统一 `Token` 字段承载，`PlayerId` 同时写入 `OpenId`
- 渠道配置由渠道包自读：经 BlankGetChannel 读取 `googleProjectId`（必须是 Web 端 ClientId）
- Google 是 `LoginComponent` 的 Android / iOS 平台默认管理器；Editor 下返回桩数据供流程验证

## 快速开始

### 安装

选择以下任意一种安装方式：

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

   `scopes` 控制哪些包会通过该注册表解析。只有名称以 `com.gameframex` 开头的包才会从该注册表获取。

2. Add to `manifest.json` dependencies:
   ```json
   {
      "com.gameframex.unity.login.google": "https://github.com/gameframex/com.gameframex.unity.login.google.git"
   }
   ```
3. Use **Package Manager** in Unity with **Git URL**: `https://github.com/gameframex/com.gameframex.unity.login.google.git`
4. Clone the repository into your Unity project's `Packages` directory. It will be loaded automatically.

## 使用示例

```csharp
using GameFrameX.Login.Runtime;
using UnityEngine;

public class GoogleLoginDemo : MonoBehaviour
{
    void Start()
    {
        // 安装本包后 Google 在 BeforeSceneLoad 自动注册进 LoginChannelRegistry；
        // 统一经 LoginComponent 门面访问（不再有 per-channel 组件）
        var login = GameEntry.GetComponent<LoginComponent>(); // 或 GameApp.Login

        login.OnLoginSuccess += (sender, e) =>
        {
            var google = e.Result as GoogleLoginResult; // 需要渠道特有字段时向下转型
            Debug.Log($"token={e.Result.Token} openId={e.Result.OpenId} playerId={google?.PlayerId}");
        };

        login.Init();                          // 内部经 BlankGetChannel 读取 googleProjectId
        login.Login(new LoginParams("Google")); // 显式路由（留空则走平台默认渠道）
    }
}
```

**渠道配置**：`googleProjectId`（必须是 Web 端 ClientId）由渠道包经 BlankGetChannel 自读——与其他渠道值的配置方式一致（如 `BlankGetChannel.GetChannelName("googleProjectId")`）。

## 架构概览

### 自注册语义

`GoogleLoginBootstrap` 在 `RuntimeInitializeOnLoadMethod(BeforeSceneLoad)`（早于 `LoginComponent.Awake`）将 `"Google"` 渠道（工厂 `() => new GoogleLoginManager()`）注册进 `LoginChannelRegistry`。未安装本包时代码被裁剪、注册不会发生；装包 / 不装包两种工程形态均可编译运行。

### 与核心包的关系

- 只依赖 `com.gameframex.unity.login`（外加 `com.gameframex.unity` 与 BlankGetChannel）。
- Google 是 `LoginComponent` 上 Android / iOS 平台默认管理器类型（序列化类型名字符串；实现缺失时回落 `DefaultLoginManager`）。
- `LoginParams.ChannelId = "Google"` 显式路由；为空回落平台默认（同一实例，无双实例）。

### 迁移说明（2.x -> 3.0.0）

| 旧形态 | 新形态 |
|--------|--------|
| `GoogleLoginComponent` | `LoginComponent`（核心包；删除场景上的旧组件） |
| `GameApp.GoogleLogin` | `GameApp.Login`（宏 → `ENABLE_GAME_FRAME_X_LOGIN`） |
| `GoogleLoginSuccess.IdToken` | `GoogleLoginResult.Token`（服务器校验方改读 `Token`） |
| `googleLoginComponent.Login(onSuccess, onFail)` | `login.Login(onSuccess, onFailed)` 一次性重载或聚合事件 |
| 组件 `Init()` | `GameApp.Login.Init()`（`googleProjectId` 读取方式不变） |

## 平台支持

| 平台 | 支持 |
|------|------|
| Android | 支持（native 桥） |
| iOS | 支持（native 桥） |
| Editor | 支持（桩数据） |

## 文档与资源

- [文档](https://gameframex.doc.alianblank.com)
- [GitHub 仓库](https://github.com/GameFrameX/com.gameframex.unity.login.google)

## 社区与支持

- QQ 群：通过[二维码](https://qm.qq.com/cgi-bin/qm/qr?k=ikT9gA5m2sKwOyNOfYmQvSAPK_c3GmD6)加入

## 依赖

| 包 | 版本 |
|-----|------|
| `com.gameframex.unity` | 2.5.4 |
| `com.gameframex.unity.getchannel` | 1.3.2 |
| `com.gameframex.unity.login` | 1.0.0 |

## 更新日志

详见 [Releases](https://github.com/gameframex/com.gameframex.unity.login.google/releases) 更新日志。

## 开源协议

详见 [LICENSE.md](LICENSE.md) 文件。
